using System.Collections.Generic;
using UnityEngine;
using TaskSystem;

public static class Squad {

	private static readonly List<Actor> _actors = new List<Actor>();

	static Squad() {
		UserInputs.Instance.Subscribe("Terrain", MoveSquad);
	}

	public static void AddToSquad(Actor actor) {
		int index = _actors.FindIndex(x => x.Guid == actor.Guid);
		if (index == -1) {
			_actors.Add(actor);
		} else {
			throw new UnityException("This actor has already been added to the Squad, something is wrong.");
		}

		PortraitManager.InstantiateActorPortrait(actor);
		UserInputs.Instance.Subscribe(actor.Guid.ToString(), SelectActor);
	}

	public static void RemoveFromSquad(Actor actor) {
		_actors.RemoveAll(x => x.Guid == actor.Guid);
		PortraitManager.DeleteActorPortrait(actor);
		UserInputs.Instance.Unsubscribe(actor.Guid.ToString(), SelectActor);
	}

	/// <summary>
	/// Move the selected actors to the position.
	/// </summary>
	/// <param name="mouseButton">Which mouse button it is used.</param>
	/// <param name="raycastHit">The RaycastHit information</param>
	private static void MoveSquad(MouseButton mouseButton, RaycastHit raycastHit) {
		if (mouseButton == MouseButton.LEFT_MOUSE_BUTTON) {
			MoveArguments moveArguments = new MoveArguments(raycastHit.point);
			_actors.ForEach(x => {
				if (x.Selected) {
					x.TaskScheduler.CreateTask<Move>(moveArguments);
				}
			});
		}
	}

	/// <summary>
	/// Event triggered when the user clicks on this actor.
	/// </summary>
	/// <param name="mouseButton">Which Mouse button it used.</param>
	/// <param name="raycastHit">The RaycastHit information</param>
	private static void SelectActor(MouseButton mouseButton, RaycastHit raycastHit) {
		if (mouseButton == MouseButton.LEFT_MOUSE_BUTTON) {
			if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl)) {
				_actors.ForEach(x => x.Selected = false);
			}
			_actors.Find(x => x.Guid.ToString() == raycastHit.collider.name).Selected = true;
		}
	}
}
