﻿using UnityEngine;

namespace CameraSystem {
	public class CameraTarget : MonoBehaviour {

		private Transform _target;

		private Ray _ray;
		private RaycastHit _hit;
		private Vector3 _buffer;
		private LayerMask _layerMask;
		private Vector3 _offset = new Vector3(0, 15, 0);

		public void Initialize(LayerMask layerMask) {
			_layerMask = layerMask;
			_ray = new Ray(transform.position + _offset, Vector3.down);
		}

		public void StartRoutine(Transform target) {
			_target = target;
		}

		private void LateUpdate() {
			if (!ReferenceEquals(_target, null)) {
				transform.position = _target.position;
			} else {
				_ray.origin = transform.position + _offset;
				if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _layerMask)) {
					_buffer = transform.position;
					_buffer.y = _hit.point.y;
					transform.position = _buffer;
				}
			}
		}

		public void CancelRoutine() {
			_target = null;
		}
	}
}
