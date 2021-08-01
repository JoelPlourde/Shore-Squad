﻿using System;
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

			if (trigger == null) {
				// Create a trigger at destination.
				trigger = TriggerManager.CreateTrigger(moveArguments.Position, moveArguments.Radius, OnTriggerEnterCondition, OnEnd);
			} else {
				// Re-use the same trigger, simply move the position of it.
				trigger.transform.position = moveArguments.Position;
			}

			// Start the NavMeshAgent.
			actor.NavMeshAgent.SetDestination(moveArguments.Position);
			actor.NavMeshAgent.isStopped = false;

			/*
			 * TODO: Take into account any previous speed modifier into consideration.
			// Calculate the distance, it if above a threshold run, if not walk.
			float distance = (moveArguments.Position - actor.NavMeshAgent.transform.position).magnitude;
			Debug.Log(distance);
			if (distance > 5f) {
				actor.Animator.SetInteger("Movement", 1);
				actor.NavMeshAgent.speed = Constant.AGENT_BASE_RUN_SPEED;
			} else {
				actor.Animator.SetInteger("Movement", 0);
				actor.NavMeshAgent.speed = Constant.AGENT_BASE_WALK_SPEED;
			}
			*/

			actor.Animator.SetBool("Move", true);
		}

		public override void Combine(ITaskArguments taskArguments) {
			base.Initialize(taskArguments, TaskPriority);
			Execute();
		}

		public override void OnEnd() {
			base.OnEnd();
			trigger?.Destroy();
			actor.NavMeshAgent.isStopped = true;
			actor.Animator.SetBool("Move", false);
		}

		public void OnDestroy() {
			// Unsubscribe to the event.
			if (trigger) {
				trigger.OnTriggerEnterEvent -= OnEnd;
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
