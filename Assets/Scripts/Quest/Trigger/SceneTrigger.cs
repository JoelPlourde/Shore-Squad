using GameSystem;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuestSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "ChangeSceneTrigger", menuName = "ScriptableObjects/Quest/Trigger/Change Scene")]
	public class SceneTrigger : Trigger {

		[SerializeField]
		[Tooltip("Scene to change to.")]
		public SceneReference SceneReference;

		[SerializeField]
		[Tooltip("Position where to teleport the Actors to in the new scene.")]
		public Vector3 Position;

		public override void Execute() {
			if (SceneReference.SceneName != SceneManager.GetActiveScene().name) {
				// Subscribe
				SceneController.Instance.OnSceneLoadedEvent += OnSceneLoaded;
				SceneController.Instance.LoadScene(SceneReference.SceneName);

			} else {
				TeleportSquad(Position);
			}
		}

		private void OnSceneLoaded() {
			// Unsubscribe
			SceneController.Instance.OnSceneLoadedEvent -= OnSceneLoaded;

			TeleportSquad(Position);
		}

		private void TeleportSquad(Vector3 position) {
			Squad.TeleportSquad(Position);

			if (Squad.FirstSelected(out Actor actor)) {
				CameraSystem.CameraController.Instance.FollowTarget(actor.transform);
			}
		}
	}
}
