using QuestSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem {
	public class SceneController : MonoBehaviour {

		public static SceneController Instance;

		public event Action OnSceneLoadedEvent;

		private Queue<QuestAction> _questActions = new Queue<QuestAction>();

		private void Awake() {
			Instance = this;
		}

		/// <summary>
		/// Load the scene.
		/// </summary>
		/// <param name="scene">The identifier of the scene as defined in the build settings.</param>
		public void LoadScene(string scene) {
			IsLoading = true;

			// TODO disable user inputs.
			UserInputs.Instance.DisableInput();

			UserInputs.Instance.UnsubscribeAll();

			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.LoadScene(scene, LoadSceneMode.Single);
		}

		/// <summary>
		/// Event triggered when the scene has finished loading. This will dequeue all pending quest actions and triggers anyone listening to this event.
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="mode"></param>
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			DequeueActions();

			OnSceneLoadedEvent?.Invoke();

			UserInputs.Instance.EnableInput();

			IsLoading = false;
		}

		/// <summary>
		/// Queue quest-related action to be executed after the scene has been loaded.
		/// </summary>
		/// <param name="action"></param>
		/// <param name="quest"></param>
		public void QueueAction(Action<Quest> action, Quest quest) {
			_questActions.Enqueue(new QuestAction(action, quest));
		}

		/// <summary>
		/// Dequeue all the actions that have been queued up before the scene is loaded.
		/// </summary>
		private void DequeueActions() {
			while (_questActions.Count > 0) {
				QuestAction questAction = _questActions.Dequeue();
				questAction.Action?.Invoke(questAction.Quest);
			}
		}

		public bool IsLoading { get; private set; }
	}

	public class QuestAction {

		public QuestAction(Action<Quest> action, Quest quest) {
			Action = action;
			Quest = quest;
		}

		public Action<Quest> Action { get; private set; }
		public Quest Quest { get; private set; }
	}
}
