using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using GameSystem;

namespace NavigationSystem {
	public class Obstacle : MonoBehaviour {
		[SerializeField]
		public List<Box> Boxes = new List<Box>();

		private List<NavMeshObstacle> _navMeshObstacles = new List<NavMeshObstacle>();
		private Box _box = new Box();

		private void Awake() {
			if (Boxes != null) {
				foreach (var box in Boxes) {
					var child = new GameObject("obstacle");
					child.transform.SetParent(transform);
					child.transform.localPosition = Vector3.zero;
					var obstacle = child.AddComponent<NavMeshObstacle>();
					obstacle.center = box.Center;
					box.Size.y = 1;
					obstacle.size = box.Size;
					obstacle.carving = false;
					obstacle.enabled = false;
					_navMeshObstacles.Add(obstacle);
				}

				CalculateBox();
			}
		}

		public void RegisterBox(Box box) {
			box.Size.y = 1;
			Boxes.Add(box);
			CalculateBox();
		}

		public void Enable() {
			_navMeshObstacles.ForEach(x => {
				x.enabled = true;
				x.carving = true;
			});
		}

		public void Disable() {
			_navMeshObstacles.ForEach(x => {
				x.enabled = false;
				x.carving = false;
			});
		}

		private void CalculateBox() {
			float Xmin = float.MaxValue;
			float Xmax = float.MinValue;
			float Zmin = float.MaxValue;
			float Zmax = float.MinValue;

			// Calculate the average position of the box.
			Vector3 origin = new Vector3();
			foreach (var item in Boxes) {
				origin += item.Center;
			}
			origin /= Boxes.Count;


			foreach (var box in Boxes) {
				float minx = (box.Center.x - origin.x) - (box.Size.x / 2);
				if (Xmin > minx) {
					Xmin = minx;
				}

				float maxx = (box.Center.x - origin.x) + (box.Size.x / 2);
				if (Xmax < maxx) {
					Xmax = maxx;
				}

				float minz = (box.Center.z - origin.z) - (box.Size.z / 2);
				if (Zmin > minz) {
					Zmin = minz;
				}

				float maxz = (box.Center.z - origin.z) + (box.Size.z / 2);
				if (Zmax < maxz) {
					Zmax = maxz;
				}
			}

			_box.Center = origin + transform.position;
			_box.Size = new Vector3(Mathf.Abs(Xmin) + Mathf.Abs(Xmax), 0, Mathf.Abs(Zmin) + Mathf.Abs(Zmax));
		}

		void OnDrawGizmosSelected() {
			if (Boxes != null) {
				foreach (var box in Boxes) {
					Gizmos.color = Color.red;
					Gizmos.DrawWireCube(transform.position + box.Center, box.Size);
				}

				CalculateBox();
				Gizmos.color = Color.blue;
				Gizmos.DrawWireCube(_box.Center, _box.Size);
			}
		}

		/// <summary>
		/// Size of the obstacle in World unit.
		/// </summary>
		public Vector3 Size { get { return _box.Size; } }

		/// <summary>
		/// Center of the obstacle in World position.
		/// </summary>
		public Vector3 Center { get { return _box.Center; } }
	}

	[System.Serializable]
	public class Box {

		[Header("Parameters")]
		[Tooltip("The center of the box.")]
		public Vector3 Center;

		[Tooltip("The size of the box.")]
		public Vector3 Size;

		public Box() {
		}

		public Box(Vector3 center, Vector3 size) {
			Center = center;
			Size = size;
		}
	}
}
