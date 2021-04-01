using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskSystem;

/// <summary>
/// The Trigger class can be added to any GameObject to enable the OnTriggerEnter behaviour.
/// Ex: GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<Trigger>();
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class Trigger : MonoBehaviour
{
	private SphereCollider _collider;

	public Func<Collider, bool> _onTriggerEnterCondition;
	public event Action OnTriggerEnterEvent;

	private void Awake() {
		// TODO OPTIMIZE THIS
		Destroy(gameObject.GetComponent<MeshFilter>());
		Destroy(gameObject.GetComponent<MeshRenderer>());
		_collider = gameObject.GetComponent<SphereCollider>();
		_collider.isTrigger = true;
	}

	public void Initialize(float radius, Func<Collider, bool> onTriggerEnterCondition) {
		_collider.radius = radius;
		_onTriggerEnterCondition = onTriggerEnterCondition;
	}

	public void Destroy() {
		if (OnTriggerEnterEvent != null) {
			foreach (var d in OnTriggerEnterEvent.GetInvocationList()) {
				OnTriggerEnterEvent -= (d as Action);
			}
		}
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider other) {
		if (_onTriggerEnterCondition(other)) {
			OnTriggerEnterEvent?.Invoke();
			Destroy();
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, _collider.radius);
	}
}
