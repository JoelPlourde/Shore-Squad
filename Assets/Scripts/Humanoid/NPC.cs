using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using TaskSystem;

public class NPC : Actor, IInteractable
{
	public DialogueData DialogueData;
	private Actor _actor;

	public override void Start() {
		base.Start();

		Playable = false;

		if (!ReferenceEquals(DialogueData, null)) {
			UserInputs.Instance.Subscribe(Guid.ToString(), Interact);
		} else {
			throw new UnityException("Please assign a DialogueData unto this NPC!");
		}
	}

	public void Interact(MouseButton mouseButton, RaycastHit raycastHit) {
		if (Squad.First(out Actor actor)) {
			_actor = actor;
			InteractArguments interactArguments = new InteractArguments(1f, this.transform.position, this);
			_actor.TaskScheduler.CreateTask<Interact>(interactArguments, TaskPriority.LOW);
		}
	}

	public void OnInteractEnter() {
		Debug.Log("Start interaction !");

		CameraSystem.CameraTarget.Instance.ZoomIn(transform.position);

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
