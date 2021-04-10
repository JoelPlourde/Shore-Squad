using UnityEngine;

namespace CameraSystem {
	[RequireComponent(typeof(Collider))]
	public class CameraRestrictedArea : MonoBehaviour {

		private Collider _collider;

		private void Awake() {
			_collider = GetComponent<Collider>();
		}

		private void OnTriggerEnter(Collider other) {
			CameraTarget cameraTarget = other.GetComponent<CameraTarget>();
			if (!ReferenceEquals(cameraTarget, null)) {
				cameraTarget.LockPositionInsideCollider(_collider);
			}
		}
	}
}
