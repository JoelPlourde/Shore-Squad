using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TriggerManager
{
	public static Trigger CreateTrigger(Vector3 position, float radius, Func<Collider, bool> onTriggerEnterCondition, Action atDestination) {
		GameObject @object = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		@object.transform.position = position;
		Trigger trigger = @object.AddComponent<Trigger>();
		trigger.Initialize(radius, onTriggerEnterCondition);
		trigger.OnTriggerEnterEvent += atDestination;
		return trigger;
	}
}
