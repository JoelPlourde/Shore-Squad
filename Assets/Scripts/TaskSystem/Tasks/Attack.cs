using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AI;

namespace TaskSystem {

	public class Attack : TaskBehaviour {

		NavMeshAgent navMeshAgent;
		AttackArguments attackArguments;

		private bool _canAttack = true;

		private DelayedAction _checkStateAction;
		private DelayedAction _resetAttackAction;

		public override void Combine(ITaskArguments taskArguments) {
			base.Initialize(taskArguments, TaskPriority);
			Execute();
		}

		public override void Execute() {
			// Validate the arguments you've received is of the correct type.
			attackArguments = TaskArguments as AttackArguments;

			// First of all, verify if you or the target is dead.
			if (attackArguments.Target.Dead || actor.Dead) {
				OnEnd();
				return;
			}
			navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
			navMeshAgent.SetDestination(attackArguments.Target.transform.position);
			navMeshAgent.isStopped = false;

			_checkStateAction = new DelayedAction(CheckState, 0.5f);
			_resetAttackAction = new DelayedAction(ResetAttack, actor.Attributes.Speed);
			TimerManager.Instance.Enqueue(_checkStateAction);
		}

		private void CheckState() {
			if (!this) {
				return;
			}

			if (actor.Dead || attackArguments.Target.Dead || actor.Fleeing) {
				OnEnd();
				return;
			}

			if (CheckIfCloseToTarget()) {
				AttackState();
			} else {
				MoveState();
			}

			TimerManager.Instance.Enqueue(_checkStateAction);
		}

		private void MoveState() {
			if (actor.NavMeshAgent.isPathStale) {
				OnEnd();
				return;
			}

			navMeshAgent.destination = attackArguments.Target.transform.position;
			navMeshAgent.isStopped = false;
			actor.Animator.SetBool("Move", true);
		}

		private void AttackState() {
			actor.Animator.SetBool("Move", false);
			navMeshAgent.isStopped = true;
			if (_canAttack) {
				_canAttack = false;
				AttackNow();
			}
		}

		private void AttackNow() {
			actor.Animator.SetTrigger("Attack");
			attackArguments.Target.Attributes.SufferDamage(actor.Attributes.Damage);
			TimerManager.Instance.Enqueue(_resetAttackAction);
		}

		public override void OnEnd() {
			if (navMeshAgent.isActiveAndEnabled) {
				navMeshAgent.isStopped = true;
			}
			base.OnEnd();
		}

		private void ResetAttack() {
			_canAttack = true;
		}

		private bool CheckIfCloseToTarget() {
			return Vector3.Distance(attackArguments.Target.transform.position, transform.position) < 
				(navMeshAgent.radius * 4);
		}
	}
}
