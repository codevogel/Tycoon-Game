using System;
using System.Collections.Generic;
using Agency;
using Architect.Placeables.Presets;
using Buildings;
using Buildings.Resources;
using Grid;
using UI;
using UnityEngine;
using static Grid.GridManager;

namespace Architect.Placeables
{
    /// <summary>
    /// Base class for Buildings.
    /// Extends from Placeable.
    /// </summary>
    [Serializable]
    public class Building : Placeable
    {
        public Storage Input;
        public Storage Output;

        public Resource[] ProductionCost;
        public Resource[] Produces;

        public BuildingPreset LocalPreset { get; private set; }

        public Tile Tile { get; set; }

        public BuildingConnectionsRenderer BuildingConnectionsRenderer { get; protected set; }

        public float Range { get; set; }

        private Tile b_entrance, b_exit;
        /// <summary>
        /// Holds the entrance tile of a building this is where agents enter. 
        /// The default value is the Tile this building is placed on
        /// </summary>
        public Tile Entrance
        {
            get { return b_entrance == null ? Tile : b_entrance; }
            private set { b_entrance = value; }
        }
        /// <summary>
        /// Holds the exit tile of a building this is where agents exit. 
        /// The default value is the Tile this building is placed on
        /// </summary>
        public Tile Exit
        {
            get { return b_exit == null ? Tile : b_exit; }
            private set { b_exit = value; }
        }

        public bool HasExitAndEntrance => b_entrance != null && Exit != null;


        /// <summary>
        /// List of providers that this building imports resources from.
        /// </summary>
        public List<Building> providers = new();

        /// <summary>
        /// List of recipients that this building exports to.
        /// </summary>
        public List<Building> recipients = new();

        public delegate void Event(bool b);
        public Event OnRoadCheck, OnFabricate, OnTransport;

        public bool IsTransporting { get; set; }

        private AgentSpawner _agentSpawner;

        /// <summary>
        /// Creates a new building with a given BuildingPreset.
        /// </summary>
        /// <param name="preset">The preset for this building.</param>
        public Building(BuildingPreset preset)
        {
            Preset = preset;
            LocalPreset = preset;
            Input = new Storage(preset.inputStorageCap);
            Input.Add(preset.initialStorage);
            Output = new Storage(preset.outputStorageCap);
            ProductionCost = LocalPreset.productionCost;
            Produces = LocalPreset.produces;
            Range = LocalPreset.range;
        }

        /// <summary>
        /// Set entrance tile for this building.
        /// </summary>
        /// <param name="tile">The new entrance.</param>
        /// <returns>Whether an entrance could be set.</returns>
        internal bool SetEntrance(Tile tile)
        {
            if (!tile.HasContent || tile.Content is not Road)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Could not set entrance because there is no road there!");
#endif
                return false;
            }
            Entrance = tile;
#if UNITY_EDITOR
            Debug.Log("setting entrance at " + tile);
#endif
            return true;
        }

        /// <summary>
        /// Set exit tile for this building.
        /// </summary>
        /// <param name="tile">The new exit.</param>
        /// <returns>Whether an exit could be set.</returns>
        internal bool SetExit(Tile tile)
        {
            if (!tile.HasContent || tile.Content is not Road)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Could not set exit because there is no road there!");
#endif
                return false;
            }
            Exit = tile;
#if UNITY_EDITOR
            Debug.Log("setting exit at " + tile);
#endif
            return true;
        }

        /// <summary>
        /// Initializes a buildings fields after it has been instantiated.
        /// </summary>
        /// <param name="hostingTile">The tile this building is hosted on.</param>
        public virtual void InitializeAfterInstantiation(Tile hostingTile)
        {
            Tile = hostingTile;
            bool producesItems = Produces.Length > 0;
            BuildingController.Instance.SubscribeBuilding(this, producesItems, producesItems);
            _agentSpawner = Tile.PlaceableHolder.GetComponentInChildren<AgentSpawner>();
            BuildingConnectionsRenderer =
                Tile.transform.Find("Recipient Lines").GetComponent<BuildingConnectionsRenderer>();
        }

        /// <summary>
        /// Refreshes the resource recipeints for this building.
        /// </summary>
        public void RefreshRecipients()
        {
            this.recipients = new();

            //Check if there is a road in the surrounding tiles
            Neighbour[] neighbours = GridManager.Instance.GetNeighboursFor(Tile);
            bool hasRoadNeighbour = false;
            foreach (Neighbour neighbour in neighbours)
            {
                if (neighbour.Tile.Content is Road)
                {
                    hasRoadNeighbour = true;
                    break;
                }
            }

            OnRoadCheck?.Invoke(hasRoadNeighbour);
            if (!hasRoadNeighbour) return;

            //Get all the buildings in range
            Building[] recipientsInRange = GetBuildingsInRange();
            foreach (Building recipient in recipientsInRange)
            {
                InsertAtFrontOfQueue(recipient);
                recipient.AddProvider(this);
            }

            BuildingConnectionsRenderer.SetRecipients(recipientsInRange);
        }

        /// <summary>
        /// Adds a building that provides resources to this building.
        /// </summary>
        /// <param name="newProvider">the new provider</param>
        private void AddProvider(Building newProvider)
        {
            if (!providers.Contains(newProvider))
            {
                providers.Add(newProvider);
            }

            BuildingConnectionsRenderer.SetProviders(providers.ToArray());
        }

        /// <summary>
        /// Remove a provider from this building.
        /// </summary>
        /// <param name="newProvider">the provider to remove</param>
        private void RemoveProvider(Building building)
        {
            providers.Remove(building);
            BuildingConnectionsRenderer.SetProviders(providers.ToArray());
        }

        /// <summary>
        /// Gets the closest building to this building.
        /// </summary>
        /// <returns>the closest building to this building</returns>
        private Building GetClosestBuilding()
        {
            Building[] buildings = GetBuildingsInRange();
            float minDistance = float.PositiveInfinity;
            int maxDistanceIndex = -1;
            for (int i = 0; i < buildings.Length; i++)
            {
                float currentDistance = Vector3.Distance(buildings[i].Tile.PlaceableHolder.transform.position,
                    Tile.PlaceableHolder.transform.position);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    maxDistanceIndex = i;
                }
            }

            if (maxDistanceIndex < 0)
            {
                return null;
            }

            return buildings[maxDistanceIndex];
        }

        /// <summary>
        /// Gets the buildings that are in range of this building.
        /// </summary>
        /// <returns>the buildings that are in range of this building</returns>
        private Building[] GetBuildingsInRange()
        {
            Collider[] overlappedColliders =
                Physics.OverlapSphere(Tile.PlaceableHolder.position, Range * GridManager.Instance.AverageTileWidth, LayerMask.GetMask("Building"));
            List<(Building building, float dist)> buildingsByDistance = new();
            foreach (Collider overlap in overlappedColliders)
            {
                Building other = overlap.GetComponent<Tile>().Content as Building;
                if (other == this)
                    continue;

                bool addedBuildings = false;
                foreach (Resource needed in other.ProductionCost)
                {
                    if (addedBuildings)
                        break;
                    foreach (Resource resource in Produces)
                    {
                        if (needed.type == resource.type)
                        {
                            buildingsByDistance.Add((other,
                                Vector3.Distance(other.Tile.PlaceableHolder.transform.position,
                                    Tile.PlaceableHolder.transform.position)));
                            addedBuildings = true;
                            break;
                        }
                    }
                }
            }

            buildingsByDistance.Sort(new BuildingByDistanceComparer());

            Building[] buildings = new Building[buildingsByDistance.Count];
            for (int i = 0; i < buildings.Length; i++)
            {
                buildings[i] = buildingsByDistance[i].building;
            }

            return buildings;
        }

        /// <summary>
        /// Add recipient to this building
        /// </summary>
        /// <param name="building">the recipient to add.</param>
        public void InsertAtFrontOfQueue(Building building)
        {
            recipients.Insert(0, building);
        }

        /// <summary>
        /// Enqueue recipient add end of list
        /// </summary>
        /// <param name="recipient">the recipient to add.</param>
        public void EnqueueRecipient(Building recipient)
        {
            recipients.Add(recipient);
        }

        /// <summary>
        /// Dequeue recipient.
        /// </summary>
        public Building DequeueRecipient()
        {
            Building firstInQueue = recipients[0];
            recipients.RemoveAt(0);
            return firstInQueue;
        }

        /// <summary>
        /// Production cycle for this building.
        /// </summary>
        public void Produce()
        {
            if (BuildingController.Tick % LocalPreset.productionTime == 0)
            {
                Fabricate();
            }
        }

        /// <summary>
        /// Fabricates the resources from productionCost into the resources from produces.
        /// </summary>
        protected virtual void Fabricate()
        {
            bool HasRequiredResources = Input.HasResourcesRequired(ProductionCost) && Output.HasSpaceFor(Produces);
            OnFabricate?.Invoke(HasRequiredResources);
            if (!HasRequiredResources)
            {
                //Debug.Log("Did not have enough resources to produce!");
                return;
            }

            RemoveFromStorage(Input, ProductionCost);
            AddToStorage(Output, Produces);
        }

        /// <summary>
        /// Adds resources to this buildings' storage
        /// </summary>
        /// <param name="storage">the storage to add to</param>
        /// <param name="resources">the resources to add</param>
        public void AddToStorage(Storage storage, Resource[] resources)
        {
            foreach (Resource resource in resources)
            {
                storage.Add(resource);
            }
        }

        /// <summary>
        /// Removes resources to this buildings' storage
        /// </summary>
        /// <param name="storage">the storage to remove from</param>
        /// <param name="resources">the resources to remove</param>
        public void RemoveFromStorage(Storage storage, Resource[] resources)
        {
            foreach (Resource resource in resources)
            {
                storage.Remove(resource);
            }
        }

        /// <summary>
        /// Transport hook for the Building Controller's events.
        /// </summary>
        public void Transport()
        {
            if (BuildingController.Tick % LocalPreset.transportTime == 0)
            {
                TransportToRecipients();
            }
        }

        /// <summary>
        /// Attempts transporting resources to recipients.
        /// </summary>
        private void TransportToRecipients()
        {
            if (recipients.Count > 0)
            {
                Building recipient = DequeueRecipient();
                List<Resource> resourcesToSend = new();
                int totalAmount = 0;
                // For each requested resource
                foreach (Resource requestedResource in recipient.ProductionCost)
                {
                    // If this building's output contains that requested resource
                    if (Output.HasResourceRequired(requestedResource))
                    {
                        resourcesToSend.Add(requestedResource);
                        totalAmount += requestedResource.amount;
                    }
                    else
                    {
                        // Did not have that resource
                    }
                }

                // Send resources that were requested by recipient
                Resource[] resourcesToSendArray = resourcesToSend.ToArray();

                if (recipient is ConstructionSite c && c.isReceiving)
                {
                    TransportToRecipients();
                    return;
                }

                if (resourcesToSendArray.Length == 0)
                {
                    InsertAtFrontOfQueue(recipient);
                    return;
                }

                if (!recipient.Input.HasSpaceForAmount(totalAmount))
                {
                    EnqueueRecipient(recipient);
                    return;
                }
                //Check if it can spawn an agent

                //TODO: why is this nullcheck nessecary
                if (_agentSpawner == null)
                {
                    _agentSpawner = Tile.PlaceableHolder.GetComponentInChildren<AgentSpawner>();
                }

                AgentBehaviour agent = _agentSpawner.SpawnAgent();
                bool gotAgent = agent != null;
                OnTransport?.Invoke(gotAgent);
                if (gotAgent)
                {
                    agent.transform.position = Exit.transform.position;
                    RemoveFromStorage(Output, resourcesToSendArray);
                    (agent as DeliveryAgent).SetDeliveryTarget(resourcesToSendArray, recipient);
                    //(agent as DeliveryAgent).AddTarget(this);
                    if (recipient is ConstructionSite c2) c2.isReceiving = true;
                }

                // Put recipient back into queue
                EnqueueRecipient(recipient);
            }
        }

        public override void OnDestroy()
        {
            BuildingController.Instance.UnsubscribeBuilding(this);
            providers.Clear();
            BuildingConnectionsRenderer.SetProviders(providers.ToArray());
            foreach (Building recipient in recipients)
            {
                recipient.RemoveProvider(this);
            }

            BuildingConnectionsRenderer.ShowLines(false);
            GameObject.Destroy(_agentSpawner);
        }

        public class BuildingByDistanceComparer : Comparer<(Building val, float dist)>
        {
            public override int Compare((Building val, float dist) a, (Building val, float dist) b)
            {
                return (a.dist > b.dist) ? 1 : (a.dist == b.dist) ? 0 : -1;
            }
        }


        /// <summary>
        /// A storage holds a collection of resources.
        /// </summary>
        [Serializable]
        public class Storage
        {
            private Dictionary<ResourceType, int> _contents;
            private int _cap;
            private int _totalAmount;
            public Dictionary<ResourceType, int> Contents => _contents;
            public int Cap => _cap;

            /// <summary>
            /// Creates a storage, and sets the cap.
            /// </summary>
            public Storage(int cap)
            {
                _contents = new Dictionary<ResourceType, int>();
                _cap = cap;
                _totalAmount = 0;
            }

            /// <summary>
            /// Does the storage contain the required resources for this production cycle?
            /// </summary>
            /// <param name="required">The required resources for this production cycle.</param>
            /// <returns>Whether enough resources are contained in the storage.</returns>
            public bool HasResourcesRequired(Resource[] required)
            {
                foreach (Resource resource in required)
                {
                    if (!HasResourceRequired(resource))
                    {
                        return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// Does the storage contain the required resource for this production cycle?
            /// </summary>
            /// <param name="required">The required resource for this production cycle.</param>
            /// <returns>Whether the resource is contained in the storage.</returns>
            internal bool HasResourceRequired(Resource resource)
            {
                if (_contents.ContainsKey(resource.type))
                {
                    if (_contents[resource.type] - resource.amount < 0)
                        return false;
                }
                else
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Adds a resource item to the storage
            /// </summary>
            /// <param name="resource">Adds a resource to the storage.</param>
            public void Add(Resource resource)
            {
                //int amount = (_totalAmount + resource.amount > _cap) ? _cap - _totalAmount : resource.amount;
                int amount = resource.amount;
                _totalAmount += amount;
                if (_contents.ContainsKey(resource.type))
                {
                    _contents[resource.type] += amount;
                }
                else
                {
                    _contents[resource.type] = amount;
                }
            }

            /// <summary>
            /// Adds resources to the storage
            /// </summary>
            /// <param name="resource">Adds a resource to the storage.</param>
            public void Add(Resource[] resources)
            {
                foreach (Resource resource in resources)
                {
                    Add(resource);
                }
            }

            /// <summary>
            /// Removes a resource item from the storage.
            /// </summary>
            /// <param name="resource">Removes a resource to the storage.</param>
            public void Remove(Resource resource)
            {
                _contents[resource.type] -= resource.amount;
                _totalAmount -= resource.amount;
            }

            /// <summary>
            /// Gets the number of resources of given type.
            /// </summary>
            /// <param name="type">the resource type</param>
            /// <returns>The the number of resources of given type.</returns>
            public int Get(ResourceType type)
            {
                if (_contents.ContainsKey(type))
                {
                    return _contents[type];
                }

                return 0;
            }

            public bool HasSpaceForAmount(int amount)
            {
                return _totalAmount + amount <= _cap;
            }

            public bool HasSpaceFor(Resource[] resources)
            {
                int amount = 0;
                foreach(Resource resource in resources)
                {
                    amount += resource.amount;
                }
                return HasSpaceForAmount(amount);
            }
        }
    }
}