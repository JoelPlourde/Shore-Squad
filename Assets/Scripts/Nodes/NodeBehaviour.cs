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
		private Dictionary<string, Coroutine> _routines = new Dictionary<string, Coroutine>();

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
				// TODO output error message to the player.
				Debug.Log("This node has been completed depleted.");
				actor.TaskScheduler.CancelTask();
				return;
			}

			if (actor.Skills.GetLevel(_nodeData.SkillType).Value < _nodeData.Requirement) {
				// TODO output error message to the player.
				Debug.Log("The Character is not skilled enough to harvest this node.");
				actor.TaskScheduler.CancelTask();
				return;
			}

			if (!actor.Armory.HasWeaponEquipped(_nodeData.WeaponType)) {
				// TODO output error message to the player.
				Debug.Log("The Character has not the correct tool equipped!");
				actor.TaskScheduler.CancelTask();
				return;
			}

			_routines.Add(actor.name, StartCoroutine(Routine(actor, GameplayUtils.CalculateRepeatRateBasedOnSpeed(actor.Statistics.GetStatistic(_nodeData.SpeedStatistic)))));
		}

		/// <summary>
		/// Routine that reduces health of this node every X seconds.
		/// </summary>
		/// <param name="actor">The actor that reducing the health.</param>
		/// <param name="repeatRate">The rate at which the actor should strike the node.</param>
		/// <returns>The IEnumerator</returns>
		IEnumerator Routine(Actor actor, float repeatRate) {
			while (true) {
				yield return new WaitForSeconds(repeatRate);
				if (_node.ReduceHealth(actor, actor.Statistics.GetStatistic(_nodeData.DamageStatistic)) <= 0) {
					actor.TaskScheduler.CancelTask();
				}
			}
		}

		/// <summary>
		/// On Interact Exit, stops the gathering process.
		/// </summary>
		/// <param name="actor"></param>
		public void OnInteractExit(Actor actor) {
			if (_routines.TryGetValue(actor.name, out Coroutine coroutine)) {
				StopCoroutine(coroutine);
				_routines.Remove(actor.name);
			}
		}

		private void OnHarvest(Actor actor) {
			// TODO Output the Loot.
			Debug.Log("LOOT!!");

			actor.Skills.GainExperience(_nodeData.SkillType, _nodeData.Experience);
		}

		public float GetInteractionRadius() {
			return InteractionRadius;
		}
	}
}
