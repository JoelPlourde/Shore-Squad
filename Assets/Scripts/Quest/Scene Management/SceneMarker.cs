using UnityEngine;

namespace QuestSystem {
	[ExecuteInEditMode]
	public class SceneMarker : MonoBehaviour {

		private SceneTrigger _sceneTrigger;

#if UNITY_EDITOR
		private void Awake() {
			if (Application.isPlaying) {
				Destroy(gameObject.GetComponent<MeshRenderer>());
			}

			_sceneTrigger = Resources.Load<SceneTrigger>("Scriptable Objects/Quests/Triggers/" + gameObject.name);
			if (ReferenceEquals(_sceneTrigger, null) && gameObject.name != "Cylinder") {
				Destroy(gameObject);
			}
		}

		void Update() {
			if (transform.hasChanged) {
				transform.hasChanged = false;

				if (!ReferenceEquals(_sceneTrigger, null)) {
					_sceneTrigger.Position = transform.position;
				}
			}
		}

		void OnDrawGizmos() {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, 2);
		}
#endif

		public void Initialize(SceneTrigger sceneTrigger) {
			_sceneTrigger = sceneTrigger;

			gameObject.name = _sceneTrigger.name;
			gameObject.tag = "EditorOnly";
		}
	}
}
