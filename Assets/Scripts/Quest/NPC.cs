using UnityEngine;
using DialogueSystem;
using TaskSystem;
using UnityEngine.SceneManagement;
using PointerSystem;

namespace QuestSystem {
	public class NPC : MonoBehaviour, IInteractable {

		private DialogueData _dialogueData;
		private Actor _actor;

		private GameObject _questMarker;

		public void Initialize(DialogueData dialogueData) {
			_dialogueData = dialogueData;
			UserInputs.Instance.Subscribe(transform.name, Interact);
		}

		#region Interactable
		public void Interact(MouseButton mouseButton, RaycastHit raycastHit) {
			if (Squad.FirstSelected(out Actor actor)) {
				_actor = actor;
				_actor.TaskScheduler.CreateTask<Interact>(new InteractArguments(transform.position, this));
			}
		}

		public void OnInteractEnter(Actor actor) {
			CameraSystem.CameraTarget.Instance.ZoomIn((_actor.transform.position + transform.position) / 2);
			DialogueHandler.Instance.OnDialogueEnd += OnDialogueEnded;
			DialogueHandler.Instance.Initialize(_dialogueData);
		}

		public void OnInteractExit(Actor actor) {
			CameraSystem.CameraTarget.Instance.ZoomOut();
		}

		public float GetInteractionRadius() {
			return 1f;
		}
		#endregion

		private void OnDialogueEnded() {
			DialogueHandler.Instance.OnDialogueEnd -= OnDialogueEnded;
			if (!ReferenceEquals(_actor, null)) {
				_actor.TaskScheduler.CancelTask(TaskType.INTERACT);
			}
		}

		#region Quest Marker
		public void SetQuestMarker(GameObject questMarker) {
			_questMarker = questMarker;
		}

		public void ResetQuestMarker() {
			Destroy(_questMarker);
		}
		#endregion

		#region Mouse Event
		public void OnMouseEnter() {
			PointerManager.Instance.SetPointer(PointerMode.TALK);
		}

		public void OnMouseExit() {
			PointerManager.Instance.SetPointer(PointerMode.DEFAULT);
		}
		#endregion
	}
}
