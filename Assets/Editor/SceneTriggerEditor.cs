using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuestSystem {

	[ExecuteInEditMode]
	[CanEditMultipleObjects]
	[CustomEditor(typeof(SceneTrigger))]
	public class SceneTriggerEditor : Editor {

		private static GameObject _marker;

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			if (!ReferenceEquals(_marker, null)) {
				_marker.transform.position = ((SceneTrigger)target).Position;
			}
		}

		public void OnEnable() {
			SceneTrigger sceneTrigger = (SceneTrigger) target;

			if (SceneManager.GetActiveScene().name == sceneTrigger.SceneReference.SceneName) {

				// Find the SceneMarker if available.
				SceneMarker sceneMarker = FindSceneMarkerByName(target.name);
				if (ReferenceEquals(sceneMarker, null)) {

					// Else create it.
					_marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					_marker.transform.position = sceneTrigger.Position;

					DestroyImmediate(_marker.GetComponent<MeshRenderer>());

					sceneMarker = _marker.AddComponent<SceneMarker>();
					sceneMarker.Initialize(sceneTrigger);
				}
			}
		}

		private SceneMarker FindSceneMarkerByName(string name) {
			GameObject sceneMarkerObject = GameObject.Find(name);
			if (!ReferenceEquals(sceneMarkerObject, null)) {
				return sceneMarkerObject.GetComponent<SceneMarker>();
			}

			return null;
		}
	}
}
