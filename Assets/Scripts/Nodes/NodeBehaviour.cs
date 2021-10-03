using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NodeSystem {
	public class NodeBehaviour : MonoBehaviour, IInteractable {

		[Tooltip("The radius at which the player should stopped at.")]
		public float InteractionRadius;

		[SerializeField]
		private NodeData _nodeData;

		private Node _node;

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
		private void OnHit(Actor actor) {
			if (_node.ReduceHealth(actor, actor.Statistics.GetStatistic(_nodeData.DamageStatistic)) <= 0) {
				actor.TaskScheduler.CancelTask();
			}

			actor.Skills.GainExperience(_nodeData.SkillType, _nodeData.Experience);
		}

		/// <summary>
		/// On Interact Exit, stops the gathering process.
		/// </summary>
		/// <param name="actor"></param>
		public void OnInteractExit(Actor actor) {
			actor.Animator.SetBool("Harvest", false);
			actor.OnHarvestEvent -= OnHarvest;
		}

		private void OnHarvest(Actor actor) {
			// TODO Output the Loot.
			Debug.Log("LOOT!!");
		}

		public float GetInteractionRadius() {
			return InteractionRadius;
		}
	}
}
