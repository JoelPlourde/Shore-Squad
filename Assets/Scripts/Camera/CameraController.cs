using UnityEngine;

namespace CameraSystem {
	public class CameraController : MonoBehaviour, IUpdatable {

		public static CameraController Instance;

		public CameraTarget Target;

		[Header("LayerMask")]
		[Tooltip("Layer(s) on which the Camera be blocked by. Thus, the camera will move forward whenever behind an object assigned to any of the layer(s).")]
		public LayerMask BlockingLayer;

		[Tooltip("Layer(s) on which the Camera Target will navigate on.")]
		public LayerMask CameraTargetLayer;

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
		protected private Vector3 _smoothedPosition;
		protected private float _desiredDistance = 20f;

		protected private Vector3 _direction;

		private void Awake() {
			Instance = this;

			DontDestroyOnLoad(this.gameObject);

			Camera = GetComponent<Camera>();
			Distance = (MinZoom + MaxZoom) / 2;

			if (ReferenceEquals(Target, null)) {
				throw new UnityException("Please assign a CameraTarget to the CameraController object.");
			}
			Target.Initialize(CameraTargetLayer);
		}

		private void Start() {
			GameController.Instance.RegisterLateUpdatable(this);
		}

		public void OnUpdate() {
			_desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.smoothDeltaTime * ScrollSensitivity;

			_direction = (transform.position - Target.transform.position).normalized;
			if (Physics.Raycast(Target.transform.position, _direction * _desiredDistance, out _hit, _desiredDistance, BlockingLayer, QueryTriggerInteraction.UseGlobal)) {
				Distance = (_hit.point - Target.transform.position).magnitude;
			} else {
				Distance = _desiredDistance;
			}
			Distance = Mathf.Clamp(Distance, MinZoom, MaxZoom);

			if (Input.GetMouseButton(2)) {
				_localRotation.x += Input.GetAxis("Mouse X") * Time.smoothDeltaTime * CameraSensitivity;
				_localRotation.y += Input.GetAxis("Mouse Y") * Time.smoothDeltaTime * CameraSensitivity;
				_localRotation.y = Mathf.Clamp(_localRotation.y, -60, -5);
			}

			Target.transform.Translate(Input.GetAxis("Horizontal") * transform.right * (MovingSpeed * ((int)Distance >> 1)) * Time.deltaTime, Space.World);
			Target.transform.Translate(Input.GetAxis("Vertical") * transform.forward * (MovingSpeed * ((int)Distance >> 1)) * Time.deltaTime, Space.World);

			transform.position = Vector3.Lerp(transform.position, Target.transform.position + Quaternion.Euler(_localRotation.y, _localRotation.x, 0f) * (Distance * -Vector3.back), SmoothSensitivity);
			transform.LookAt(Target.transform.position, Vector3.up);
		}

		public void FollowTarget(Transform target) {
			Target.FollowTarget(target);
		}

		public void StopFollow() {
			Target.CancelFollow();
		}

		public void FocusOnTarget(Transform target) {
			Target.transform.position = target.transform.position;
		}

		public void OnDestroy() {
			if (GameController.Instance.Alive) {
				GameController.Instance.DeregisterLateUpdatable(this);
			}
		}

		public Camera Camera { get; private set; }
		public float Distance { get; private set; } = 20;
	}
}

