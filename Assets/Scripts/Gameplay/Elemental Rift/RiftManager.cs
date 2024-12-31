using System.Collections.Generic;
using UnityEngine;

namespace ElementalRift {
    /// <summary>
    /// The Rift Manager is responsible for managing all the Rifts in the game.
    /// </summary>
    public class RiftManager : MonoBehaviour {

        public static RiftManager Instance;

        private List<RiftBehaviour> _riftBehaviours = new List<RiftBehaviour>();

        private void Awake() {
            Instance = this;

            InvokeRepeating("Routine", 0.0f, Constant.RIFT_TICK_RATE);
        }

        /// <summary>
        /// Subscribe to the Rift Manager.
        /// </summary>
        /// <param name="riftBehaviour"></param>
        public void SubscribeRift(RiftBehaviour riftBehaviour) {
            // Subscribe to the Rift
            _riftBehaviours.Add(riftBehaviour);
        }

        /// <summary>
        /// Unsubscribe from the Rift Manager.
        /// </summary>
        /// <param name="riftBehaviour"></param>
        public void UnsubscribeRift(RiftBehaviour riftBehaviour) {
            // Unsubscribe from the Rift
            _riftBehaviours.Remove(riftBehaviour);
        }

        /// <summary>
        /// Routine that will be called every X seconds to increase the health of all the Rifts.
        /// </summary>
        private void Routine() {
            for (int i = _riftBehaviours.Count - 1; i >= 0; i--) {
                _riftBehaviours[i].IncreaseHealth();
            }
        }
    }
}