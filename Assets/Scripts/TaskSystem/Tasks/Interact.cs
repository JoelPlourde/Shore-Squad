using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TaskSystem {
	public class Interact : TaskBehaviour {

		private InteractArguments _interactArguments;
		private NavMeshAgent navMeshAgent;
		private Trigger trigger;

		public override void Execute() {
			// Validate the arguments you've received is of the correct type.
			_interactArguments = TaskArguments as InteractArguments;

			TriggerManager.CreateTrigger(_interactArguments.Position, _interactArguments.Interactable.GetInteractionRadius(), OnTriggerEnterCondition, AtDestination);

			// Start the NavMeshAgent.
			navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
			navMeshAgent.SetDestination(_interactArguments.Position);
			navMeshAgent.isStopped = false;

			actor.Animator.SetBool("Move", true);
		}

		public override void Combine(ITaskArguments taskArguments) {
			base.Initialize(taskArguments, TaskPriority);
			if (trigger) {
				trigger.Destroy();
			}
			Execute();
		}

		private void AtDestination() {
			_interactArguments.Interactable.OnInteractEnter(actor);
			navMeshAgent.isStopped = true;
			actor.Animator.SetBool("Move", false);
		}

		public override void OnEnd() {
			_interactArguments.Interactable.OnInteractExit(actor);
			base.OnEnd();
			if (trigger) {
				trigger.Destroy();
			}
			if (navMeshAgent.isOnNavMesh) {
				navMeshAgent.isStopped = true;
			}
			actor.Animator.SetBool("Move", false);
		}

		public void OnDestroy() {
			if (trigger) {
				trigger.OnTriggerEnterEvent -= AtDestination;
			}
		}

		private bool OnTriggerEnterCondition(Collider collider) {
			if (this) {
				return collider.gameObject.name == name;
			} else {
				return false;
			}
		}
	}
}
