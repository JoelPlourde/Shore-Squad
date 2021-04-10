using UnityEngine;

namespace CameraSystem {
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(SphereCollider))]
	public class CameraTarget : MonoBehaviour, IUpdatable {

		private Transform _target;
		private Collider _collider;

		private Ray _ray;
		private RaycastHit _hit;
		private Vector3 _buffer;
		private LayerMask _layerMask;
		private Vector3 _offset = new Vector3(0, 15, 0);

		private void Start() {
			Rigidbody rigidbody = GetComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;

			SphereCollider sphereCollider = GetComponent<SphereCollider>();
			sphereCollider.isTrigger = true;

			GameController.Instance.RegisterLateUpdatable(this);
		}

		public void Initialize(LayerMask layerMask) {
			_layerMask = layerMask;
			_ray = new Ray(transform.position + _offset, Vector3.down);
		}

		public void FollowTarget(Transform target) {
			_target = target;
		}

		public void LockPositionInsideCollider(Collider collider) {
			_collider = collider;
		}

		public void OnUpdate() {
			if (!ReferenceEquals(_target, null)) {
				transform.position = _target.position;
			} else if (!ReferenceEquals(_collider, null)) {
				if (!_collider.bounds.Contains(transform.position)) {
					transform.position = _collider.ClosestPoint(transform.position);
				}
			} else {
				_ray.origin = transform.position + _offset;
				if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerMask)) {
					_buffer = transform.position;
					_buffer.y = _hit.point.y;
					transform.position = _buffer;
				}
			}
		}

		public void CancelFollow() {
			_target = null;
		}

		public void UnlockPosition() {
			_collider = null;
		}

		private void OnDestroy() {
			if (GameController.Instance.Alive) {
				GameController.Instance.DeregisterLateUpdatable(this);
			}
		}
	}
}
