using UnityEngine;
using GameSystem;

namespace EncampmentSystem {
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(ZoneBehaviour))]
	public class Building : ObjectBehaviour {

		public Rigidbody Rigidbody;
		public ZoneBehaviour InnerZoneBehaviour;

		public bool _enabled = false;

		private void Awake() {
			Rigidbody = GetComponent<Rigidbody>();
			Rigidbody.useGravity = false;
			Rigidbody.isKinematic = true;
			InnerZoneBehaviour = GetComponent<ZoneBehaviour>();
			InnerZoneBehaviour.Map = new Map(InnerZoneBehaviour.InfluenceRadius);

			Initialize();
			// Obstacle.Initialize(new Vector3(InnerZoneBehaviour.InfluenceRadius, InnerZoneBehaviour.InfluenceRadius, InnerZoneBehaviour.InfluenceRadius));
		}

		public override void Initialize() {
			base.Initialize();
		}

		public override void Enable() {
			base.Enable();
			_enabled = true;
		}

		public override void Disable() {
			base.Disable();
			_enabled = false;
		}

		public void OnTriggerEnter(Collider other) {
			if (_enabled) {
				Actor actor = other.gameObject.GetComponent<Actor>();
				if (actor != null) {
					Debug.Log("Actor: " + actor.name + " is sheltered !");
					actor.Sheltered = true;
				}
			}
		}

		public void OnTriggerExit(Collider other) {
			if (_enabled) {
				Actor actor = other.gameObject.GetComponent<Actor>();
				if (actor != null) {
					Debug.Log("Actor: " + actor.name + " is NOT sheltered !");
					actor.Sheltered = false;
				}
			}
		}
	}
}
