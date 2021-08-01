using System.Collections;
using System.Collections.Generic;
using UI;
using UI.Portrait;
using UnityEngine;

public static class PortraitManager {

	private static GameObject _portraitTemplate;

	public static void InstantiateActorPortrait(Actor actor) {
		if (_portraitTemplate == null) {
			_portraitTemplate = Resources.Load<GameObject>("Prefabs/Portrait");
			if (_portraitTemplate == null) {
				throw new UnityException("Place the Portrait prefab under Assets/Resources/Prefabs");
			}
		}

		GameObject actorPortraitObj = GameObject.Instantiate(_portraitTemplate);
		actorPortraitObj.transform.SetParent(UserInterface.Instance.Portraits);
		actorPortraitObj.transform.name = actor.Guid.ToString();
		ActorPortrait actorPortrait = actorPortraitObj.GetComponent<ActorPortrait>();
		actorPortrait.Initialize(actor);
	}

	public static void DeleteActorPortrait(Actor actor) {
		GameObject actorPortraitObj = UserInterface.Instance.Portraits.Find(actor.Guid.ToString()).gameObject;
		ActorPortrait actorPortrait = actorPortraitObj.GetComponent<ActorPortrait>();
		actorPortrait.OnDelete(actor);
		Object.Destroy(actorPortraitObj);
	}
}
