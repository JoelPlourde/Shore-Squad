using GameSystem;
using System;
using System.IO;
using UnityEngine;

namespace EncampmentSystem {
	[ExecuteInEditMode]
	[RequireComponent(typeof(BoxCollider))]
	public class ZoneBehaviour : MonoBehaviour {

		public Map Map { get; set; }

		[SerializeField]
		private int _influenceRadius = 5;           // The influence radius where objects can be placed around the encampment.

		private BoxCollider _boxCollider;

		private void Awake() {
			_boxCollider = GetComponent<BoxCollider>();
			_boxCollider.size = new Vector3Int(_influenceRadius, _influenceRadius, _influenceRadius);
			_boxCollider.center = new Vector3Int(0, _influenceRadius >> 1, 0);
			_boxCollider.isTrigger = true;

			if (TerrainScanner.GetTerrainAtWorldPosition(transform.position, out Terrain terrain)) {
				if (TerrainScanner.GetTerrainMap(terrain, transform.position, InfluenceRadius, out Map map)) {
					Map = map;
				}
			} else {
				throw new UnityException("The ZoneBehaviour has to be located unto a terrain.");
			}
		}

		public void Update() {
			if (Input.GetKeyUp(KeyCode.A)) {
				string filename = Application.persistentDataPath + "/" + Guid.NewGuid() + ".png";
				Debug.Log(filename);
				File.WriteAllBytes(filename, Map.Texture.EncodeToPNG());
			}
		}

		public void OnTriggerEnter(Collider other) {
			ObjectBehaviour objectBehaviour = other.gameObject.GetComponent<ObjectBehaviour>();
			if (objectBehaviour != null) {
				objectBehaviour.RegisterZone(this);
			}
		}

		public void OnTriggerExit(Collider other) {
			ObjectBehaviour objectBehaviour = other.gameObject.GetComponent<ObjectBehaviour>();
			if (objectBehaviour != null) {
				objectBehaviour.UnregisterZone();
			}
		}

		#region Editor
		private void OnDrawGizmosSelected() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(transform.position + new Vector3Int(0, _influenceRadius >> 1, 0), new Vector3Int(_influenceRadius, _influenceRadius, _influenceRadius));
		}
		#endregion

		public int InfluenceRadius { get { return _influenceRadius; } protected set { _influenceRadius = value;  } }
	}
}
