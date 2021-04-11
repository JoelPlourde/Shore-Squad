using System.Collections.Generic;
using UnityEngine;
using TaskSystem;
using System;

public static class Squad {

	private static readonly List<Unit> _units = new List<Unit>();

	private static readonly GameObject _selectorTemplate;

	static Squad() {
		UserInputs.Instance.Subscribe("Terrain", MoveSquad);

		_selectorTemplate = Resources.Load<GameObject>("Prefabs/Selector");
		if (_selectorTemplate == null) {
			throw new UnityException("Please define a projector gameobject at: Assets/Resources/Prefabs/Selector");
		}
	}

	/// <summary>
	/// Add an actor from the squad.
	/// </summary>
	/// <param name="actor">An actor.</param>
	public static void AddToSquad(Actor actor) {
		int index = _units.FindIndex(x => x.Actor.Guid == actor.Guid);
		if (index == -1) {
			_units.Add(new Unit(actor, CreateSelector(actor)));
		} else {
			throw new UnityException("This actor has already been added to the Squad, something is wrong.");
		}

		PortraitManager.InstantiateActorPortrait(actor);
		UserInputs.Instance.Subscribe(actor.Guid.ToString(), SelectActor);
	}

	/// <summary>
	/// Remove an actor from the squad.
	/// </summary>
	/// <param name="actor">An actor.</param>
	public static void RemoveFromSquad(Actor actor) {
		_units.RemoveAll(x => x.Actor.Guid == actor.Guid);
		DeleteSelector(actor);
		PortraitManager.DeleteActorPortrait(actor);
		UserInputs.Instance.Unsubscribe(actor.Guid.ToString(), SelectActor);
	}

	/// <summary>
	/// Return any Actor in the squad.
	/// </summary>
	/// <param name="actor"></param>
	/// <returns></returns>
	public static bool First(out Actor actor) {
		actor = null;
		if (_units.Count > 0) {
			actor = _units[0].Actor;
			_units[0].EnableSelector(true);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Move the selected actors to the position.
	/// </summary>
	/// <param name="mouseButton">Which mouse button it is used.</param>
	/// <param name="raycastHit">The RaycastHit information</param>
	private static void MoveSquad(MouseButton mouseButton, RaycastHit raycastHit) {
		if (mouseButton == MouseButton.LEFT_MOUSE_BUTTON) {
			MoveArguments moveArguments = new MoveArguments(raycastHit.point);
			_units.ForEach(x => {
				if (x.Actor.Selected) {
					x.Actor.TaskScheduler.CreateTask<Move>(moveArguments);
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
				_units.ForEach(x => {
					x.EnableSelector(false);
				});

			}
			Unit selectedUnit = _units.Find(x => x.Actor.Guid.ToString() == raycastHit.collider.name);
			selectedUnit.EnableSelector(true);
		}
	}

	#region Selector
	private static GameObject CreateSelector(Actor actor) {
		GameObject selectorObj = GameObject.Instantiate(_selectorTemplate);
		selectorObj.SetActive(false);
		selectorObj.name = "Selector";
		selectorObj.transform.SetParent(actor.transform);
		selectorObj.transform.localPosition = new Vector3(0, 5, 0);
		return selectorObj;
	}

	private static void DeleteSelector(Actor actor) {
		UnityEngine.Object.Destroy(actor.transform.Find("Selector").gameObject);
	}
	#endregion

	public class Unit {

		public Actor Actor { get; private set; }
		public GameObject Selector { get; private set; }

		public Unit(Actor actor, GameObject selector) {
			Actor = actor;
			Selector = selector;
		}

		public void EnableSelector(bool value) {
			Selector.SetActive(value);
			Actor.SetSelected(value);
		}
	}
}
