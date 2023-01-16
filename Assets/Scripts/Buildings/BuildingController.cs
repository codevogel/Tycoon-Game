using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Buildings
{
    public class BuildingController : MonoBehaviour
    {
        /// <summary>
        /// Current tick
        /// </summary>
        public static int Tick { get; private set; }
        /// <summary>
        /// Event for production cycle
        /// </summary>
        public static UnityEvent Produce = new();
        /// <summary>
        /// Event for transport cycle
        /// </summary>
        public static UnityEvent Transport = new();
        public static UnityEvent Refresh = new();

        private static List<Building> _buildings = new();

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

        internal static void SubscribeBuilding(Building building, bool produce, bool transport)
        {
            _buildings.Add(building);
            if (produce)
            {
                Produce.AddListener(building.Produce);
            }
            if (transport)
            {
                Transport.AddListener(building.Transport);
            }
            Refresh.AddListener(building.RefreshRecipients);
        }

        internal static void UnsubscribeBuilding(Building building)
        {
            _buildings.Remove(building);
            Produce.RemoveListener(building.Produce);
            Transport.RemoveListener(building.Transport);
            Refresh.RemoveListener(building.RefreshRecipients);
        }
    }
}
