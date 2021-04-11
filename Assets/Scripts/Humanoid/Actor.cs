using FactionSystem;
using StatusEffectSystem;
using System;
using TaskSystem;
using UI.Portrait;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(TaskScheduler))]
[RequireComponent(typeof(StatusEffectScheduler))]
public class Actor : MonoBehaviour {
	public FactionType FactionType;

	public bool Playable = false;

	[SerializeField]
	private Attributes _attributes;

	[SerializeField]
	private Status _status;

	public event Action<float> OnUpdateHealthEvent;
	public event Action<float> OnUpdateFoodEvent;
	public event Action<bool> OnSelectionEvent;

	public virtual void Awake() {
		Animator = GetComponent<Animator>();
		TaskScheduler = GetComponent<TaskScheduler>();
		StatusEffectScheduler = GetComponent<StatusEffectScheduler>();
		NavMeshAgent = GetComponent<NavMeshAgent>();

		gameObject.name = Guid.ToString();
		gameObject.tag = FactionType.ToString();
		Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;

		NavMeshAgent.autoBraking = true;
		NavMeshAgent.stoppingDistance = 0.1f;

		SphereCollider collider = gameObject.AddComponent<SphereCollider>();
		collider.radius = 1f;
		collider.isTrigger = true;

		TaskScheduler.Initialize(Guid);
		StatusEffectScheduler.Initialize(Guid);
	}

	public virtual void Start() {
		StatusEffectScheduler.Instance(Guid).AddStatusEffect(new StatusEffectSystem.Status(this, 1f, StatusEffectManager.GetStatusEffectData(Constant.HUNGRY)));

		if (Playable) {
			Squad.AddToSquad(this);
		}
	}

	/// <summary>
	/// Suffer X amount of damage. If health is empty, call the virtual OnDeath event.
	/// </summary>
	/// <param name="damage">Amount of damage to suffer.</param>
	public void SufferDamage(float damage) {
		Health -= damage;

		OnUpdateHealthEvent?.Invoke(Health / MaxHealth);

		if (Health <= 0) {
			StatusEffectScheduler.Instance(Guid).AddStatusEffect(new StatusEffectSystem.Status(this, 1f, StatusEffectManager.GetStatusEffectData(Constant.DEATH)));
		}
	}

	/// <summary>
	/// Increase the health value by X. The health value is capped at maximum health of the actor.
	/// </summary>
	/// <param name="value">Amount of health to increase.</param>
	public void IncreaseHealth(float value) {
		if (!Dead) {
			Health += value;

			if (Health > MaxHealth) {
				Health = MaxHealth;
			}

			OnUpdateHealthEvent?.Invoke(Health / MaxHealth);
		}
	}

	/// <summary>
	/// Increase the hunger rate by X.
	/// </summary>
	/// <param name="value">Amount of hunger rate to increase</param>
	public void IncreaseHungerRate(float value) {
		HungerRate += value;
	}

	/// <summary>
	/// Reduce the hunger rate by X.
	/// </summary>
	/// <param name="value">Amount of hunger to deduct.</param>
	public void ReduceHungerRate(float value) {
		HungerRate -= value;
	}

	/// <summary>
	/// Increase the food bar by X. Remove any Hungry status effects.
	/// </summary>
	/// <param name="value">Amount of food to restore.</param>
	public void IncreaseFood(float value) {
		Food += value;

		StatusEffectScheduler.Instance(Guid).RemoveStatusEffect(Constant.STARVING);

		if (Food > Constant.ACTOR_BASE_FOOD) {
			Food = Constant.ACTOR_BASE_FOOD;
		}

		OnUpdateFoodEvent?.Invoke(Food);
	}

	/// <summary>
	/// Reduce the food by X. Apply a Hungry status effect if the food equals to 0.
	/// </summary>
	/// <param name="value">Amount of food to deduct.</param>
	public void ReduceFood(float value) {
		Food -= value;

		if (Food <= 0) {
			Food = 0;
			StatusEffectScheduler.Instance(Guid).AddStatusEffect(new StatusEffectSystem.Status(this, 0.5f, StatusEffectManager.GetStatusEffectData(Constant.STARVING)));
		}

		OnUpdateFoodEvent?.Invoke(Food);
	}

	/// <summary>
	/// Reduce speed by a percentage of the base speed.
	/// </summary>
	/// <param name="value">A value in %</param>
	public void ReduceSpeed(float value) {
		Speed = Constant.ACTOR_BASE_SPEED - (Constant.ACTOR_BASE_SPEED * value);
	}

	/// <summary>
	/// Reset the speed to its original speed before the speed bonus applied.
	/// </summary>
	/// <param name="value">A value in % of the speed bonus applied before.</param>
	public void ResetSpeed(float value) {
		Speed = Speed + (Constant.ACTOR_BASE_SPEED * value);
	}

	/// <summary>
	/// Increase speed by a percentage of the base speed.
	/// </summary>
	/// <param name="value">A value in %</param>
	public void IncreaseSpeed(float value) {
		Speed = Constant.ACTOR_BASE_SPEED + (Constant.ACTOR_BASE_SPEED * value);
	}

	/// <summary>
	/// Increase temperature by a value.
	/// </summary>
	/// <param name="value">A value to increment by.</param>
	public void IncreaseTemperature(float value) {
		Temperature += value;
	}

	/// <summary>
	/// Decrease temperature by a value.
	/// </summary>
	/// <param name="value">A value to decrease by.</param>
	public void DecreaseTemperature(float value) {
		Temperature -= value;
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

	public float MaxHealth { get => _attributes.MaxHealth; private set => _attributes.MaxHealth = value; }
	public float Health { get => _attributes.Health; private set => _attributes.Health = value; }
	public float Speed { get => _attributes.Speed; private set => _attributes.Speed = value; }
	public float Damage { get => _attributes.Damage; private set => _attributes.Damage = value; }
	public float HealthRegeneration { get => _attributes.HealthRegeneration; private set => _attributes.HealthRegeneration = value; }
	public float HungerRate { get => _attributes.HungerRate; private set => _attributes.HungerRate = value; }
	public float Food { get => _attributes.Food; private set => _attributes.Food = value; }
	public float Temperature { get => _attributes.Temperature; private set => _attributes.Temperature = value; }

	public bool Dead { get => _status.Dead; set => _status.Dead = value; }
	public bool Fleeing { get => _status.Fleeing; set => _status.Fleeing = value; }
	public bool Sheltered { get => _status.Sheltered; set => _status.Sheltered = value; }
	public bool Stunned { get => _status.Stunned; set => _status.Stunned = value; }

	public bool Selected { get; private set; }

	public Animator Animator { get; private set; }
	public TaskScheduler TaskScheduler { get; private set; }
	public StatusEffectScheduler StatusEffectScheduler { get; private set; }
	public NavMeshAgent NavMeshAgent { get; private set; }

	public Guid Guid { get; } = Guid.NewGuid();
}
