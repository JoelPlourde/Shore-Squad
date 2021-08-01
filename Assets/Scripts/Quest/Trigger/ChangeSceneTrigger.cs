using GameSystem;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuestSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "ChangeSceneTrigger", menuName = "ScriptableObjects/Quest/Trigger/Change Scene")]
	public class ChangeSceneTrigger : Trigger {

		[SerializeField]
		[Tooltip("[Required] Scene name to change to.")]
		public string Scene;

		[SerializeField]
		[Tooltip("Position where to teleport the Actors to in the new scene.")]
		public Vector3 Position;

		public override void Execute() {
			SceneController.Instance.OnSceneLoadedEvent += OnSceneLoaded;
			SceneController.Instance.LoadScene(Scene);
		}

		private void OnSceneLoaded() {
			SceneController.Instance.OnSceneLoadedEvent -= OnSceneLoaded;

			Squad.TeleportSquad(Position);

			if (Squad.FirstSelected(out Actor actor)) {
				CameraSystem.CameraController.Instance.FollowTarget(actor.transform);
			}
		}
	}
}
