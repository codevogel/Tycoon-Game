using Architect.Placeables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Buildings
{
    /// <summary>
    /// Singleton behaviour that handles the production cycle for buildings.
    /// Provides hooks for the buildings to subscribe on.
    /// </summary>
    public class BuildingController : SingletonBehaviour<BuildingController>
    {
        /// <summary>
        /// Current tick
        /// </summary>
        public static int Tick { get; private set; }
        /// <summary>
        /// Event for production cycle
        /// </summary>
        public UnityEvent Produce = new();
        /// <summary>
        /// Event for transport cycle
        /// </summary>
        public UnityEvent Transport = new();
        public UnityEvent Refresh = new();

        private List<Building> _buildings = new();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Refresh.Invoke();
            }
        }

        public void FixedUpdate()
        {
            // Increase tick and invoke events
            Tick++;
            Produce.Invoke();
            Transport.Invoke();
        }

        /// <summary>
        /// Subscribes a building to the BuildingController hooks.
        /// </summary>
        /// <param name="building">the building</param>
        /// <param name="produce">this building produces</param>
        /// <param name="transport">this building transports</param>
        internal void SubscribeBuilding(Building building, bool produce, bool transport)
        {
            _buildings.Add(building);
            if (produce)
            {
                Produce.AddListener(building.Produce);
            }
            if (transport)
            {
                building.IsTransporting = true;
                Transport.AddListener(building.Transport);
                Refresh.AddListener(building.RefreshRecipients);
            }
        }

        /// <summary>
        /// Unsubscribes a building from the BuildingController hooks.
        /// </summary>
        /// <param name="building">the building</param>
        internal void UnsubscribeBuilding(Building building)
        {
            _buildings.Remove(building);
            Produce.RemoveListener(building.Produce);
            Transport.RemoveListener(building.Transport);
            Refresh.RemoveListener(building.RefreshRecipients);
        }
    }
}
