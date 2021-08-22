using BodySystem;
using FactionSystem;
using ItemSystem;
using ItemSystem.EquipmentSystem;
using SaveSystem;
using SkillSystem;
using StatusEffectSystem;
using System;
using TaskSystem;
using UI.Portrait;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TaskScheduler))]
[RequireComponent(typeof(StatusEffectScheduler))]
public class Actor : MonoBehaviour {

	public FactionType FactionType;
	public bool Playable = false;

	public event Action<bool> OnSelectionEvent;

	public virtual void Awake() {
		Animator = GetComponent<Animator>();
		TaskScheduler = GetComponent<TaskScheduler>();
		StatusEffectScheduler = GetComponent<StatusEffectScheduler>();
		NavMeshAgent = GetComponent<NavMeshAgent>();
		Armory = GetComponentInChildren<Armory>();

		Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;

		NavMeshAgent.autoBraking = true;
		NavMeshAgent.stoppingDistance = 0.1f;
		NavMeshAgent.angularSpeed = 360f;
		NavMeshAgent.speed = 1.5f;

		SphereCollider collider = gameObject.AddComponent<SphereCollider>();
		collider.radius = 1f;
		collider.isTrigger = true;
	}

	public void Initialize(ActorDto actorDto) {
		Guid = Guid.Parse(actorDto.Guid);
		gameObject.name = Guid.ToString();
		// gameObject.tag = FactionType.ToString();

		FactionType = actorDto.FactionType;
		Playable = actorDto.Playable;

		TaskScheduler.Initialize(Guid);
		StatusEffectScheduler.Initialize(Guid);
		Attributes.Initialize(this, actorDto.AttributesDto);
		Status.Initialize(actorDto.StatusDto);

		Inventory.Initialize(actorDto.InventoryDto);
		Armory.Initialize(this, actorDto.ArmoryDto);
		Skills.Initialize(this, actorDto.SkillsDto);

		SkinnedMeshRenderer skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
		Face.Initialize(skinnedMeshRenderer, actorDto.FeaturesDto);
		Body.Initialize(skinnedMeshRenderer, actorDto.FeaturesDto);

		if (Playable) {
			Squad.AddToSquad(this);
		}
	}

	/// <summary>
	/// On Death event triggered by the Status Effect: Dead
	/// </summary>
	public virtual void OnDeath() {
		gameObject.GetComponent<Rigidbody>().isKinematic = true;
		gameObject.GetComponent<Collider>().enabled = false;
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
		NavMeshAgent.enabled = false;
		TaskScheduler.CancelTask();
	}

	/// <summary>
	/// On Resurrection event triggered by the Status Effect: Resurrect
	/// </summary>
	public virtual void OnResurrection() {
		gameObject.GetComponent<Rigidbody>().isKinematic = false;
		gameObject.GetComponent<Collider>().enabled = true;
		gameObject.layer = LayerMask.NameToLayer("Default");
		NavMeshAgent.enabled = true;
	}

	/// <summary>
	/// Set Selected on this Actor.
	/// </summary>
	/// <param name="value"></param>
	public void SetSelected(bool value) {
		Selected = value;
		OnSelectionEvent?.Invoke(value);
	}

	public bool Dead { get => Status.Dead; set => Status.Dead = value; }
	public bool Fleeing { get => Status.Fleeing; set => Status.Fleeing = value; }
	public bool Sheltered { get => Status.Sheltered; set => Status.Sheltered = value; }
	public bool Stunned { get => Status.Stunned; set => Status.Stunned = value; }

	public bool Selected { get; private set; }

	public Animator Animator { get; private set; }
	public TaskScheduler TaskScheduler { get; private set; }
	public StatusEffectScheduler StatusEffectScheduler { get; private set; }
	public NavMeshAgent NavMeshAgent { get; private set; }
	public Armory Armory { get; private set; } = new Armory();
	public Inventory Inventory { get; private set; } = new Inventory(Inventory.MAX_STACK);
	public Skills Skills { get; private set; } = new Skills();

	public Attributes Attributes { get; private set; } = new Attributes();
	public Statistics Statistics { get; private set; } = new Statistics();
	public Status Status { get; private set; } = new Status();
	public Face Face { get; private set; } = new Face();
	public Body Body { get; private set; } = new Body();

	public Guid Guid { get; private set; }
}
