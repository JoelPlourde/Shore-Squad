using UnityEngine;

namespace CameraSystem {
	public class CameraController : MonoBehaviour {

		public static CameraController Instance;

		[Header("LayerMask")]
		public LayerMask ignoreLayer;
		public LayerMask blockedByLayer;

		[Header("Sensitivity Parameters")]
		public int CameraSensitivity = 300;
		public int ScrollSensitivity = 2000;
		public float SmoothSensitivity = 0.5f;
		public int MovingSpeed = 3;

		[Header("Zoom Parameters")]
		public int MinZoom = 5;
		public int MaxZoom = 30;

		private RaycastHit _hit;
		protected private Vector3 _localRotation;

		private void Awake() {
			Instance = this;
			Camera = GetComponent<Camera>();
			Distance = (MinZoom + MaxZoom) / 2;

			Target = FindObjectOfType<CameraTarget>();
			if (ReferenceEquals(Target, null)) {
				GameObject targetObj = new GameObject("CameraTarget");
				Target = targetObj.AddComponent<CameraTarget>();
			}
			Target.Initialize(blockedByLayer);
		}

		void LateUpdate() {
			Distance -= Input.GetAxis("Mouse ScrollWheel") * Time.smoothDeltaTime * ScrollSensitivity;
			Distance = Mathf.Clamp(Distance, MinZoom, MaxZoom);

			if (Input.GetMouseButton(2)) {
				_localRotation.x += Input.GetAxis("Mouse X") * Time.smoothDeltaTime * CameraSensitivity;
				_localRotation.y += Input.GetAxis("Mouse Y") * Time.smoothDeltaTime * CameraSensitivity;
				_localRotation.y = Mathf.Clamp(_localRotation.y, -60, 60);
			}

			Target.transform.Translate(Input.GetAxis("Horizontal") * transform.right * (MovingSpeed * ((int)Distance >> 1)) * Time.deltaTime, Space.World);
			Target.transform.Translate(Input.GetAxis("Vertical") * transform.forward * (MovingSpeed * ((int)Distance >> 1)) * Time.deltaTime, Space.World);

			Vector3 smoothedPosition = Vector3.Lerp(transform.position, Target.transform.position + Quaternion.Euler(_localRotation.y, _localRotation.x, 0f) * (Distance * -Vector3.back), SmoothSensitivity);
			transform.position = smoothedPosition;
			if (Physics.Raycast(transform.position, (Target.transform.position - transform.position), out _hit, Mathf.Infinity, ignoreLayer, QueryTriggerInteraction.UseGlobal)) {
				transform.position = _hit.point;
			}
			transform.LookAt(Target.transform.position, Vector3.up);
		}

		public void FollowTarget(Transform target) {
			Target.StartRoutine(target);
		}

		public void FocusOnTarget(Transform target) {
			Target.transform.position = target.transform.position;
		}

		public CameraTarget Target { get; private set; }
		public Camera Camera { get; private set; }
		public float Distance { get; private set; } = 20;
	}
}

