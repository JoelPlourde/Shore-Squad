using UnityEngine;
using DialogueSystem;
using TaskSystem;

public class NPC : MonoBehaviour, IInteractable
{
	[Tooltip("The identifier for this NPC, this is how the dialogue will be instantiated by.")]
	public string Identifier;

	[Tooltip("The current dialogue when the player interacts with this NPC.")]
	public DialogueData DialogueData;

	private Actor _actor;

	public void Start() {

		if (string.IsNullOrWhiteSpace(Identifier)) {
			throw new UnityException("Please provide the Identifier for this NPC: " + transform.name);
		}

		if (ReferenceEquals(DialogueData, null)) {
			throw new UnityException("Please assign a DialogueData unto this NPC!");
		}

		transform.name = Identifier;

		UserInputs.Instance.Subscribe(Identifier, Interact);
	}

	public void Interact(MouseButton mouseButton, RaycastHit raycastHit) {
		if (Squad.First(out Actor actor)) {
			_actor = actor;
			InteractArguments interactArguments = new InteractArguments(1f, this.transform.position, this);
			_actor.TaskScheduler.CreateTask<Interact>(interactArguments);
		}
	}

	public void OnInteractEnter() {
		Debug.Log("Start interaction !");

		Vector3 position = (_actor.transform.position + transform.position) / 2;

		CameraSystem.CameraTarget.Instance.ZoomIn(position);

		DialogueHandler.Instance.OnDialogueEnd += OnDialogueEnded;
		DialogueHandler.Instance.Initialize(DialogueData);
	}

	public void OnInteractExit() {
		Debug.Log("Exit interaction@!");

		CameraSystem.CameraTarget.Instance.ZoomOut();
	}

	private void OnDialogueEnded() {
		if (!ReferenceEquals(_actor, null)) {
			_actor.TaskScheduler.CancelTask(TaskType.INTERACT);
		}
	}
}
