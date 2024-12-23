using UnityEngine;
using DropSystem;
using System.Collections.Generic;
using ItemSystem;

namespace NodeSystem {
	public class NodeBehaviour : InteractableBehavior, IInteractable {

		[Tooltip("The radius at which the player should stopped at.")]
		public float InteractionRadius;

		[SerializeField]
		protected NodeData _nodeData;

		protected Node _node;

		public void Start() {
			// Register the particle system if any.
			if (!ReferenceEquals(_nodeData.OnHit, null)) {
				ParticleSystemManager.Instance.RegisterParticleSystem(_nodeData.OnHit.ParticleSystem?.name, _nodeData.OnHit.ParticleSystem);
			}

			OnStart();
		}

		/// <summary>
		/// On Interact Enter, starts the gathering process.
		/// </summary>
		/// <param name="actor">The actor that interacted with this node.</param>
		public void OnInteractEnter(Actor actor) {

			if (ReferenceEquals(_node, null)) {
				_node = new Node(_nodeData);
				_node.Initialize(OnHarvest);
			}

			if (_node.IsDepleted()) {
				actor.Emotion.PlayEmote(EmoteSystem.EmoteType.SHRUG);
				actor.TaskScheduler.CancelTask();
				return;
			}

			if (actor.Skills.GetLevel(_nodeData.SkillType).Value < _nodeData.Requirement) {
				actor.Emotion.PlayEmote(EmoteSystem.EmoteType.WONDERING);
				actor.TaskScheduler.CancelTask();
				return;
			}

			if (!actor.Armory.HasWeaponEquipped(_nodeData.WeaponType)) {
				actor.Emotion.PlayEmote(EmoteSystem.EmoteType.WONDERING);
				actor.TaskScheduler.CancelTask();
				return;
			}

			actor.Animator.SetFloat("Harvest Speed", GameplayUtils.CalculateRepeatRateBasedOnSpeed(actor.Statistics.GetStatistic(_nodeData.SpeedStatistic)));
			actor.Animator.SetBool("Harvest", true);
			actor.OnHarvestEvent += OnHit;
		}

		/// <summary>
		/// Callback that will be called whenever the Actor.OnHarvestEvent is triggered.
		/// </summary>
		/// <param name="actor">The actor the event is called on.</param>
		protected virtual void OnHit(Actor actor) {
			if (!ReferenceEquals(_nodeData.OnHit.ParticleSystem, null)) {
				ParticleSystemManager.Instance.SpawnParticleSystem(_nodeData.OnHit.ParticleSystem.name, actor.transform.position + _nodeData.OnHit.RelativePosition);
			}

			if (!ReferenceEquals(_nodeData.OnHit.Sound, null)) {
				actor.AudioPlayer.PlayOneShot(_nodeData.OnHit.Sound);
			}

			if (_node.ReduceHealth(actor, actor.Statistics.GetStatistic(_nodeData.DamageStatistic)) <= 0) {
				actor.TaskScheduler.CancelTask();
			}

			OnResponse(actor);
		}

		/// <summary>
		/// On Interact Exit, stops the gathering process.
		/// </summary>
		/// <param name="actor"></param>
		public void OnInteractExit(Actor actor) {
			actor.Animator.SetBool("Harvest", false);
			actor.OnHarvestEvent -= OnHarvest;
		}

		public virtual void OnStart() {}

		public virtual void OnResponse(Actor actor) {}

		private void OnHarvest(Actor actor) {
			Drop drop = _nodeData.DropTable.GetRandomDrop();

			actor.Inventory.AddItemsToInventory(new List<Item> { drop.ToItem() }, out List<Item> remainingItems);
			if (remainingItems.Count > 0) {
				Debug.Log("Do something with the exceeding items!");
			}

			actor.Skills.GainExperience(_nodeData.SkillType, _nodeData.Experience);
		}

		public float GetInteractionRadius() {
			return InteractionRadius;
		}

		protected override OutlineType GetOutlineType() {
			return OutlineType.INTERACTABLE;
		}

		public void OnDrawGizmosSelected() {
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, InteractionRadius);
		}
	}
}
