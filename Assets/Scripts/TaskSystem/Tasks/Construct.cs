using NavigationSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TaskSystem {
	public class Construct : TaskBehaviour {

		private Trigger _trigger;
		private NavMeshAgent navMeshAgent;
		private ConstructArguments constructArguments;

		public override void Combine(ITaskArguments taskArguments) {
			base.Initialize(taskArguments, TaskPriority);
			Execute();
		}

		public override void Execute() {
			// Validate the arguments you've received is of the correct type.
			constructArguments = TaskArguments as ConstructArguments;

			if (constructArguments.ConstructionBehaviour == null) {
				Debug.Log("Construction Behaviour is null, stop it.");
				OnEnd();
				return;
			}

			// Create a trigger.
			GameObject triggerObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			triggerObject.transform.position = constructArguments.ConstructionBehaviour.transform.position;
			_trigger = triggerObject.AddComponent<Trigger>();
			_trigger.Initialize(constructArguments.ConstructionBehaviour.GetComponent<Obstacle>().Radius, OnTriggerEnterCondition);
			_trigger.OnTriggerEnterEvent += AtDestination;

			// Start the NavMeshAgent.
			navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
			navMeshAgent.SetDestination(constructArguments.ConstructionBehaviour.transform.position);
			navMeshAgent.isStopped = false;

			actor.Animator.SetBool("Move", true);
		}

		public void AtDestination() {
			navMeshAgent.isStopped = true;
			actor.Animator.SetBool("Move", false);

			if (!IsInvoking()) {
				InvokeRepeating(nameof(Routine), 0f, Constant.TICK_RATE / 1000f);
			}
		}

		private void Routine() {
			if (constructArguments.ConstructionBehaviour.Done || constructArguments.ConstructionBehaviour.Progress()) {
				OnEnd();
			}
		}

		public override void OnEnd() {
			base.OnEnd();
			CancelInvoke();
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
