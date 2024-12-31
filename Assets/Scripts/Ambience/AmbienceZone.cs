using UI;
using UnityEngine;

namespace Ambience {
    [RequireComponent(typeof(Collider))]
    public class AmbienceZone : MonoBehaviour, IUpdatable {

        public AmbienceZoneData AmbienceZoneData;

        public float InfluenceRadius = 10.0f;

        // Collider
        private Collider _collider;

		// Camera Tint
        private Transform _cameraTarget;
        private float _cameraTintAlpha;

        private void Awake() {
            _collider = GetComponent<Collider>();
            SetupCollider(InfluenceRadius);
        }

        /// <summary>
        /// Setup the collider for the Ambience Zone.
        /// </summary>
        /// <param name="influenceRadius"></param>
        private void SetupCollider(float influenceRadius) {
            if (_collider is SphereCollider sphereCollider) {
                sphereCollider.isTrigger = true;
                sphereCollider.radius = influenceRadius;
            } else if (_collider is BoxCollider boxCollider) {
                boxCollider.isTrigger = true;
                boxCollider.size = new Vector3(influenceRadius, influenceRadius, influenceRadius);
            }
        }

        /// <summary>
        /// Initialize the Ambience Zone
        /// </summary>
        /// <param name="influenceRadius"></param>
        public void Initialize(AmbienceZoneData ambienceZoneData, float influenceRadius) {
            AmbienceZoneData = ambienceZoneData;
            InfluenceRadius = influenceRadius;
            SetupCollider(InfluenceRadius);
        }

        public void OnUpdate() {
            if (ReferenceEquals(_cameraTarget, null)) {
                return;
            }

            if (ReferenceEquals(AmbienceZoneData, null)) {
                Debug.LogWarning("AmbienceZoneData is not set for " + gameObject.name);
                GameController.Instance.DeregisterUpdatable(this);
                return;
            }

            // Calculate the distance between the _cameraTarget and the Rift
            float distance = Vector3.Distance(_cameraTarget.position, transform.position);

            // Calculate a percentage of the distance and the influence radius
            _cameraTintAlpha = 1 - (distance / InfluenceRadius);

            CameraTint.Instance.UpdateTint(AmbienceZoneData.Tint, _cameraTintAlpha);
        }

        protected void OnTriggerEnter(Collider other) {
			if (other.tag == "CameraTarget") {
                _cameraTarget = other.transform;
                GameController.Instance.RegisterUpdatable(this);
            }
        }

        protected void OnTriggerExit(Collider other) {
            if (other.tag == "CameraTarget") {
                GameController.Instance.DeregisterUpdatable(this);
                _cameraTarget = null;

                CameraTint.Instance.ResetTint();
            }
        }
    }
}