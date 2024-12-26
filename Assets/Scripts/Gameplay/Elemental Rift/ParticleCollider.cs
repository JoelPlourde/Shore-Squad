using UnityEngine;

namespace ElementalRift {
    /**
    * This class is used to detect when a particle collides with a rune.
    **/
    public class ParticleCollider : MonoBehaviour {

        private OrbBehaviour _orbBehaviour;

        public void Initialize(OrbBehaviour orbBehaviour) {
            _orbBehaviour = orbBehaviour;
        }

        public void OnParticleCollision(GameObject other) {
            if (other.transform.childCount == 0) {
                return;
            }

            if (ReferenceEquals(_orbBehaviour, null)) {
                Debug.LogError("OrbBehaviour is not initialized!");
                return;
            }

            _orbBehaviour.OnParticleCollision(other.transform.GetChild(0).gameObject);
        }
    }
}