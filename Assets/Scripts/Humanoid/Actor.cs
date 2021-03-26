using UnityEngine;
using UnityEngine.AI;
using TaskSystem;
using System;
using FactionSystem;
using StatusEffectSystem;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TaskScheduler))]
[RequireComponent(typeof(StatusEffectScheduler))]
public class Actor : MonoBehaviour {
	private Guid guid = Guid.NewGuid();

	public FactionType FactionType;

	[SerializeField]
	private Attributes _attributes;

	[SerializeField]
	private Status _status;

	public virtual void Awake() {
		Animator = GetComponent<Animator>();
		TaskScheduler = GetComponent<TaskScheduler>();
		StatusEffectScheduler = GetComponent<StatusEffectScheduler>();
		NavMeshAgent = GetComponent<NavMeshAgent>();

		gameObject.name = guid.ToString();
		gameObject.tag = FactionType.ToString();
		Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;

		NavMeshAgent.autoBraking = true;
		NavMeshAgent.stoppingDistance = 0.1f;

		SphereCollider collider = gameObject.AddComponent<SphereCollider>();
		collider.radius = 1f;
		collider.isTrigger = true;

		TaskScheduler.Initialize(guid);
		StatusEffectScheduler.Initialize(guid);
	}

	/// <summary>
	/// Suffer X amount of damage. If health is empty, call the virtual OnDeath event.
	/// </summary>
	/// <param name="damage">Amount of damage to suffer.</param>
	public void SufferDamage(float damage) {
		Health -= damage;
		if (Health <= 0) {
			Dead = true;
			OnDeath();
		}
	}

	public virtual void OnDeath() {
		Debug.Log("On Death !!");
		Animator.SetTrigger("Dead");
		gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
		gameObject.GetComponent<Rigidbody>().isKinematic = true;
		gameObject.GetComponent<Collider>().enabled = false;
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		NavMeshAgent.enabled = false;
		TaskScheduler.CancelTask();
	}

	public float Health { get => _attributes.Health; private set => _attributes.Health = value; }
	public float Speed { get => _attributes.Speed; private set => _attributes.Speed = value; }
	public float Damage { get => _attributes.Damage; private set => _attributes.Damage = value; }
	public bool Dead { get => _status.Dead; private set => _status.Dead = value; }
	public bool Fleeing { get => _status.Fleeing; set => _status.Fleeing = value; }
	public bool Sheltered { get => _status.Sheltered; set => _status.Sheltered = value; }
	public Guid Guid { get => guid; }

	public Animator Animator { get; private set; }
	public TaskScheduler TaskScheduler { get; private set; }
	public StatusEffectScheduler StatusEffectScheduler { get; private set; }
	public NavMeshAgent NavMeshAgent { get; private set; }
}
