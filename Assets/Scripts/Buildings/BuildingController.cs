using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Buildings
{
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

        internal void UnsubscribeBuilding(Building building)
        {
            _buildings.Remove(building);
            Produce.RemoveListener(building.Produce);
            Transport.RemoveListener(building.Transport);
            Refresh.RemoveListener(building.RefreshRecipients);
        }
    }
}
