using NavMesh;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : Placeable
{
    public Storage input;
    public Storage output;
    private int range; 

    public Resource[] productionCost;
    public Resource[] produces;

    public BuildingPreset LocalPreset { get; private set; }

    public Tile Tile { get; set; }


    /// <summary>
    /// List of providers that this building imports resources from.
    /// </summary>
    public List<Building> providers = new();
    /// <summary>
    /// List of recipients that this building exports to.
    /// </summary>
    public List<Building> recipients = new();
    private AgentSpawner agentSpawner;

    /// <summary>
    /// Creates a new building with a given BuildingPreset.
    /// </summary>
    /// <param name="preset">The preset for this building.</param>
    public Building(BuildingPreset preset)
    {
        Preset = preset;
        LocalPreset = preset;
        input = new Storage(preset.InitialStorage);
        output = new Storage(Array.Empty<Resource>());
        productionCost = LocalPreset.ProductionCost;
        produces = LocalPreset.Produces;
        range = LocalPreset.range;
    }

    public void InitializeAfterInstantiation(Tile hostingTile)
    {
        Tile = hostingTile;
        BuildingController.SubscribeBuilding(this);
        RefreshRecipients();
        agentSpawner = Tile.PlaceableHolder.GetComponentInChildren<AgentSpawner>();
    }

    public void RefreshRecipients()
    {
        recipients = new();
        Building[] buildingsInRange = GetBuildingsInRange();
        foreach (Building building in buildingsInRange)
        {
            InsertAtFrontOfQueue(building);
        }
    }

    private Building GetClosestBuilding()
    {
        Building[] buildings = GetBuildingsInRange();
        float minDistance = float.PositiveInfinity;
        int maxDistanceIndex = -1;
        for (int i = 0; i < buildings.Length; i++)
        {
            float currentDistance = Vector3.Distance(buildings[i].Tile.PlaceableHolder.transform.position, Tile.PlaceableHolder.transform.position);
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

    private Building[] GetBuildingsInRange()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(Tile.PlaceableHolder.position, range, LayerMask.GetMask("Building"));
        List<(Building building, float dist)> buildingsByDistance = new();
        foreach (Collider overlap in overlappedColliders)
        {
            Building other = overlap.GetComponent<Tile>().Content as Building;
            if (other == this)
               continue;

            bool addedBuildings = false;
            foreach (Resource needed in other.productionCost)
            {
                if (addedBuildings)
                    break;
                foreach (Resource resource in produces)
                {
                    if (needed.Type == resource.Type)
                    {
                        buildingsByDistance.Add((other, Vector3.Distance(other.Tile.PlaceableHolder.transform.position, Tile.PlaceableHolder.transform.position)));
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
    /// <param name="building">the recipient to add.</param>
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

    ///// <summary>
    ///// Remove recipient from this building
    ///// </summary>
    ///// <param name="building">the recipient to remove.</param>
    //public void RemoveRecipient(Building building)
    //{
    //    recipients.Remove(building);
    //}

    /// <summary>
    /// Production cycle for this building.
    /// </summary>
    public void Produce()
    {
        if (BuildingController.Tick % LocalPreset.ProductionTime == 0)
        {
            Fabricate();
        }
    }

    /// <summary>
    /// Fabricates the resources from productionCost into the resources from produces.
    /// </summary>
    private void Fabricate()
    {
        if (!input.HasResourcesRequired(productionCost))
        {
            Debug.Log("Did not have enough resources to produce!");
            return;
        }
        RemoveFromStorage(input, productionCost);
        AddToStorage(output, produces);
    }

    public void AddToStorage(Storage storage, Resource[] resources)
    {
        foreach (Resource resource in resources)
        {
            storage.Add(resource);
        }
    }

    public void RemoveFromStorage(Storage storage, Resource[] resources)
    {
        foreach (Resource resource in resources)
        {
            storage.Remove(resource);
        }
    }

    public void Transport()
    {
        if (BuildingController.Tick % LocalPreset.TransportTime == 0)
        {
            TransportToRecipients();
        }
    }

    private void TransportToRecipients()
    {
        if (recipients.Count > 0)
        {
            Building recipient = DequeueRecipient();
            List<Resource> resourcesToSend = new();
            // For each requested resource
            foreach (Resource requestedResource in recipient.productionCost)
            {
                // If this building's output contains that requested resource
                if (output.HasResourceRequired(requestedResource))
                {
                    resourcesToSend.Add(requestedResource);
                }
                else
                {
                    // Did not have that resource
                }
            }
            // Send resources that were requested by recipient
            Resource[] resourcesToSendArray = resourcesToSend.ToArray();

            if (resourcesToSendArray.Length == 0)
            {
                InsertAtFrontOfQueue(recipient);
                return;
            }
            RemoveFromStorage(output, resourcesToSendArray);
            AgentBehaviour agent = agentSpawner.AgentPool.Get();
            (agent as DeliveryAgent).SetDeliveryTarget(resourcesToSendArray, recipient);
            // Put recipient back into queue
            EnqueueRecipient(recipient);
        }
    }

    public override void OnDestroy()
    {
        BuildingController.UnsubscribeBuilding(this);
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
        public Dictionary<ResourceType, int> Contents { get; set; } = new();

        /// <summary>
        /// Creates a storage, and adds the contents of initialStorage to it.
        /// </summary>
        /// <param name="initialStorage">The initial resources in this storage.</param>
        public Storage(Resource[] initialStorage)
        {
            foreach (Resource resource in initialStorage)
            {
                Add(resource);
            }
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

        internal bool HasResourceRequired(Resource resource)
        {
            if (Contents.ContainsKey(resource.Type))
            {
                if (Contents[resource.Type] - resource.Amount < 0)
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
            if (Contents.ContainsKey(resource.Type))
            {
                Contents[resource.Type] += resource.Amount;
            }
            else
            {
                Contents[resource.Type] = resource.Amount;
            }
        }

        /// <summary>
        /// Removes a resource item from the storage.
        /// </summary>
        /// <param name="resource">Removes a resource to the storage.</param>
        public void Remove(Resource resource) { Contents[resource.Type] -= resource.Amount; }

        public int Get(ResourceType type)
        {
            if (Contents.ContainsKey(type))
            {
                return Contents[type];
            }
            return 0;
        }
    }
}
