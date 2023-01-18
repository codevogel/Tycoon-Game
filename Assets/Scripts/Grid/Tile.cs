using System;
using Agency;
using Architect;
using Architect.Placeables;
using UnityEngine;
using static Grid.GridManager;
using static Architect.Placeables.Road;

namespace Grid
{
    public class Tile : MonoBehaviour
    {
        #region fields
        public Transform allowContentPlacement;
        public Transform blockContentPlacement;
        /// <summary>
        /// Parent transform of the Placeable object
        /// </summary>
        [SerializeField] private Transform placeableHolder;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider tileCollider;
        private Vector2Int _gridPosition;
        private TilePreset _preset;
        #endregion

        #region Properties

        /// <summary>
        /// Indices of this tile in the grid
        /// </summary>
        public Vector2Int GridPosition => _gridPosition;

        /// <summary>
        /// The scriptable object containing the sprite and the resources in the Tile.
        /// </summary>
        public TilePreset TilePreset => _preset;

        /// <summary>
        /// Reference to the Placeable content this tile hosts.
        /// </summary>
        public Placeable Content { get; private set; }

        /// <summary>
        /// Reference to the collider for this tile.
        /// </summary>
        public Collider TileCollider => tileCollider;

        /// <summary>
        /// Parent transform of the Placeable object
        /// </summary>
        public Transform PlaceableHolder => placeableHolder;

        /// <summary>
        /// Determines whether the tile hosts content
        /// </summary>
        public bool HasContent => Content != null;

        /// <summary>
        /// Gets the neighbours for this tile from the gridmanager.
        /// </summary>
        public Neighbour[] Neighbours { get => GridManager.Instance.GetNeighboursFor(this); }
        #endregion

        #region Methods
        public void Initialize(Vector2Int pos, TilePreset preset, Vector2 tileSize)
        {
            spriteRenderer.sprite = preset.sprite;
            _gridPosition = pos;
            _preset = preset;
            transform.localScale = new Vector3(tileSize.x * transform.localScale.x, (tileSize.x + tileSize.y) / 2 * transform.localScale.y, tileSize.y * transform.localScale.z);
            //if (preset.Obstacle != null)
            //{
            //    PlaceContent(new Placeable(preset.Obstacle), 0);
            //    AllowContentPlacement.gameObject.SetActive(false);
            //    BlockContentPlacement.gameObject.SetActive(true);
            //}
        }

        internal void OnSelect()
        {
            if (HasContent && Content is Building b)
            {
                b.BuildingConnectionsRenderer.ShowLines(true);
            }
        }

        internal void OnDeselect()
        {
            if (HasContent && Content is Building b)
            {
                b.BuildingConnectionsRenderer.ShowLines(false);
            }
        }

        /// <summary>
        /// Updates the model to reflect the Content.
        /// </summary>
        /// <param name="rotation">Rotates by rotation * 90 degrees.</param>
        private void UpdateModel(int rotation)
        {
            if (PlaceableHolder.childCount > 0)
            {
                Destroy(PlaceableHolder.GetChild(0).gameObject);
                PlaceableHolder.localEulerAngles = new Vector3(0, 0, 0);
            }

            if (Content != null && Content.Preset != null)
            {
                Instantiate(Content.Preset.Prefab, PlaceableHolder);
                PlaceableHolder.localEulerAngles = new Vector3(0, rotation * 90, 0);
            }

            RuntimeNavBaker.Instance.DelayedBake();
        }

        /// <summary>
        /// Places content as this tile with a rotation.
        /// </summary>
        /// <param name="toBePlaced">Placeable to be placed.</param>
        /// <param name="rotation">Rotates by rotation * 90 degrees.</param>
        public void PlaceContent(Placeable toBePlaced, int rotation)
        {
            Content = toBePlaced;
            switch (Content)
            {
                case Road road:
                    TileCollider.gameObject.layer = LayerMask.NameToLayer("Road");
                    PickRoad();
                    PickRoadForNeighbours();
                    break;
                case Building building:
                    TileCollider.gameObject.layer = LayerMask.NameToLayer("Building");
                    UpdateModel(rotation);
                    building.InitializeAfterInstantiation(this);
                    break;
            }
            PlaceableHolder.parent.name = Content.Preset.name;
            Buildings.BuildingController.Refresh.Invoke(); // TODO: not performance friendly
        }

        /// <summary>
        /// Picks the right road piece for this tile determined by looking at it's neighbours.
        /// </summary>
        private void PickRoad()
        {
            byte roadConnectionFlag = GetRoadConnectionFlag();
            //Debug.Log(Convert.ToString(roadConnectionFlag, 2));
            (RoadType type, int rotation) = GetFittingPiece(roadConnectionFlag);
            Content.Preset = ArchitectController.Instance.Roads[(int)type];
            UpdateModel(rotation);
        }

        /// <summary>
        /// Picks the right road piece for the neighbours of this tile.
        /// </summary>
        private void PickRoadForNeighbours()
        {
            foreach (Neighbour neighbour in Neighbours)
            {
                if (neighbour.Tile.HasContent && neighbour.Tile.Content is Road)
                {
                    neighbour.Tile.PickRoad();
                }
            }
        }

        /// <summary>
        /// Get the road connection bitflag.
        /// </summary>
        /// <returns>The road connection bitflag.</returns>
        private byte GetRoadConnectionFlag()
        {
            int bitwiseFlag = 0;
            foreach (Neighbour neighbour in Neighbours)
            {
                if (neighbour.Tile.HasContent && neighbour.Tile.Content is Road)
                {
                    bitwiseFlag += (int)neighbour.InDirection;
                }
            }

            return (byte)bitwiseFlag;
        }

        /// <summary>
        /// Get the RoadType and Rotation for this bitflag.
        /// </summary>
        /// <param name="roadConnectionFlag"></param>
        /// <returns>the RoadType and Rotation for this road connection flag.</returns>
        /// <exception cref="NotImplementedException">Throws NotImplemented when unsupported case is reached.</exception>
        private (RoadType road, int rotation) GetFittingPiece(byte roadConnectionFlag) => roadConnectionFlag switch
        {
            0b00000000 => (RoadType.CROSS, 0),
            0b00000001 => (RoadType.END, 0),
            0b00000010 => (RoadType.END, 1),
            0b00000011 => (RoadType.CORNER, 0),
            0b00000100 => (RoadType.END, 2),
            0b00000101 => (RoadType.STRAIGHT, 0),
            0b00000110 => (RoadType.CORNER, 1),
            0b00000111 => (RoadType.TJUNC, 0),
            0b00001000 => (RoadType.END, 3),
            0b00001001 => (RoadType.CORNER, 3),
            0b00001010 => (RoadType.STRAIGHT, 1),
            0b00001011 => (RoadType.TJUNC, 3),
            0b00001100 => (RoadType.CORNER, 2),
            0b00001101 => (RoadType.TJUNC, 2),
            0b00001110 => (RoadType.TJUNC, 1),
            0b00001111 => (RoadType.CROSS, 0),
            _ => throw new NotImplementedException("Invalid road connection flag: " +
                                                   Convert.ToString(roadConnectionFlag, 2))
        };

        /// <summary>
        /// Removes the content from this tile and updates the model and neighbours to reflect that.
        /// </summary>
        internal void RemoveContent()
        {
            Content.OnDestroy();
            Content = null;
            UpdateModel(0);
            PickRoadForNeighbours();
            TileCollider.gameObject.layer = LayerMask.NameToLayer("Default");
        }

        #endregion
    }
}
