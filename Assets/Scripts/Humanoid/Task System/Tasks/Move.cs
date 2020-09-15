using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TaskSystem {
	public class Move : TaskBehaviour {

		private NavMeshAgent navMeshAgent;
		private Trigger trigger;

		public override void Execute() {
			// Validate the arguments you've received is of the correct type.
			MoveArguments moveArguments = TaskArguments as MoveArguments;

			// Create a trigger at destination.
			GameObject triggerObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			triggerObject.transform.position = moveArguments.Position;
			trigger = triggerObject.AddComponent<Trigger>();
			trigger.Initialize(moveArguments.Radius, OnTriggerEnterCondition);
			trigger.OnTriggerEnterEvent += base.OnEnd;

			// Start the NavMeshAgent.
			navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
			navMeshAgent.SetDestination(moveArguments.Position);
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

		public override void OnEnd() {
			base.OnEnd();
			if (trigger) {
				trigger.Destroy();
			}
			navMeshAgent.isStopped = true;
			actor.Animator.SetBool("Move", false);
		}

		public void OnDestroy() {
			// Unsubscribe to the event.
			if (trigger) {
				trigger.OnTriggerEnterEvent -= base.OnEnd;
			}
		}

		private bool OnTriggerEnterCondition(Collider collider) {
			if (this) {
				// Here is the condition to trigger the trigger.
				return collider.gameObject.name == name;
			} else {
				return false;
			}
		}
	}
}
