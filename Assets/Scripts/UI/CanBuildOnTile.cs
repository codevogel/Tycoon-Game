using Grid;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Provides behaviour for build-preview cube.
    /// </summary>
    public class CanBuildOnTile : MonoBehaviour
    {
        private Tile _oldTile;
    
        private void Update()
        {
            DisplayBuildableTile();
        }

        /// <summary>
        /// This method places a red blocker on a tile that is already populated with an building or road.
        /// When you move off of a blocked tile the red blocker will be removed
        /// </summary>
        private void DisplayBuildableTile()
        {
            Tile targetTile = GridManager.Instance.HoverTile;

            if (_oldTile != null)
            {
                _oldTile.allowContentPlacement.gameObject.SetActive(false);
                _oldTile.blockContentPlacement.gameObject.SetActive(false);
                _oldTile = null;
            }

            if (targetTile == null || targetTile == _oldTile) return;

            if (targetTile.HasContent)
            {
                targetTile.blockContentPlacement.gameObject.SetActive(true);
            }
            else
            {
                targetTile.allowContentPlacement.gameObject.SetActive(true);
            }

            _oldTile = targetTile;
        }
    }
}
