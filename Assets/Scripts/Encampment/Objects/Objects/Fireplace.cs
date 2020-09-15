using ConstructionSystem;
using EncampmentSystem;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(SphereCollider))]
public class Fireplace : ObjectBehaviour {

	public float Heat;
	public float Radius;

	private Light _light;
	private bool _enabled = false;

	private void Awake() {
		_light = GetComponent<Light>();
		SphereCollider sphereCollider = GetComponent<SphereCollider>();
		sphereCollider.radius = Radius;
		sphereCollider.isTrigger = true;
		Initialize();
		Disable();
	}

	public override void Initialize() {
		base.Initialize();
	}

	public override void Enable() {
		base.Enable();
		_light.enabled = true;
		_enabled = true;
	}

	public override void Disable() {
		base.Disable();
		_light.enabled = false;
		_enabled = false;
	}

	public void OnTriggerEnter(Collider other) {
		if (_enabled) {
			Debug.Log("Something entered into the fire radius. Apply heat to it.");
		}
	}

	public void OnTriggerExit(Collider other) {
		if (_enabled) {
			Debug.Log("Something left the fire radius. Apply heat to it.");
		}
	}
}
