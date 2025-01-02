using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TaskSystem {
	public class Interact : TaskBehaviour {

		private InteractArguments _interactArguments;
		private Trigger trigger;

		public override void Execute() {
			// Validate the arguments you've received is of the correct type.
			_interactArguments = TaskArguments as InteractArguments;

			TriggerManager.CreateTrigger(_interactArguments.Position, _interactArguments.Interactable.GetInteractionRadius(), OnTriggerEnterCondition, AtDestination);

			if (Vector3.Distance(actor.transform.position, _interactArguments.Position) > (_interactArguments.Interactable.GetInteractionRadius() * 2)) {
				actor.NavMeshAgent.SetDestination(_interactArguments.Position);
				actor.NavMeshAgent.isStopped = false;

				actor.Animator.SetFloat("Speed", actor.NavMeshAgent.speed);
				actor.Animator.SetBool("Move", true);
			} else {
				actor.transform.LookAt(_interactArguments.Position, Vector3.up);
			}
		}

		public override void Combine(ITaskArguments taskArguments) {
			base.Initialize(taskArguments, TaskPriority);
			if (trigger) {
				trigger.Destroy();
			}
			Execute();
		}

		private void AtDestination() {
			actor.transform.LookAt(_interactArguments.Position, Vector3.up);

			_interactArguments.Interactable.OnInteractEnter(actor);
			actor.NavMeshAgent.isStopped = true;
			actor.Animator.SetBool("Move", false);
			if (_interactArguments.Interactable.IsPickup()) {
				actor.Animator.SetTrigger("Pickup");
			}
		}

		public override void OnEnd() {
			_interactArguments.Interactable.OnInteractExit(actor);
			base.OnEnd();
			if (trigger) {
				trigger.Destroy();
			}
			if (actor.NavMeshAgent.isOnNavMesh) {
				actor.NavMeshAgent.isStopped = true;
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
