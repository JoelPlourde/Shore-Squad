using UnityEngine;
using GameSystem;
using EncampmentSystem.AI;
using System.Collections.Generic;

namespace EncampmentSystem {
	[RequireComponent(typeof(ZoneBehaviour))]
	[RequireComponent(typeof(Rigidbody))]
	public class Building : ObjectBehaviour {

		private Rigidbody _rigidbody;

		private Map _interiorMap;

		public bool _enabled = false;

		// TODO DELETE TIS;
		private List<Area> areas = new List<Area>();

		private void Awake() {
			base.Initialize();

			// RigidBody of this building to allow Trigger detection.
			_rigidbody = GetComponent<Rigidbody>();
			_rigidbody.useGravity = false;
			_rigidbody.isKinematic = true;

			// Interior map to place object in.
			Obstacle.CalculateBoundingBox();
			_interiorMap = new Map(Mathf.RoundToInt(Obstacle.Size.x), Mathf.RoundToInt(Obstacle.Size.z));  // The size is based on the size of the obstacle
		}

		public override void Enable() {
			base.Enable();
			_enabled = true;

			// Register a new area defined by the Obstacle of this building.
			if (Map.GetObstacleOriginRelativeToMap(ZoneBehaviour.Map, ZoneBehaviour.transform.position, Obstacle, out Vector2Int relativePosition)) {
				areas = Map.GetAreasFromMap(_interiorMap, Color.white);

				// if the object has to be rotated, rotate the area.
				bool rotate = false;
				if (Mathf.Approximately(transform.eulerAngles.y, 90f) || Mathf.Approximately(transform.eulerAngles.y, 270f)) {
					rotate = true;
				}

				foreach (var area in areas) {
					area.Origin += relativePosition;
					if (rotate) {
						var tmp = area.Size.x;
						area.Size.x = area.Size.y;
						area.Size.y = tmp;
					}
					Debug.Log("registering: " + area);
				}
				ZoneBehaviour.gameObject.GetComponent<EncampmentAI>()?.RegisterAreas(areas);
			} else {
				Debug.LogError("Please verify your behaviour.");
			}
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
