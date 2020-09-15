using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TaskSystem;
using System;

namespace TaskSystem {
	public class Sleep : TaskBehaviour {

		public override void Execute() {
			// Validate the arguments you've received is of the correct type.
			SleepArguments sleepArguments = TaskArguments as SleepArguments;

			// Start the coroutine with this seconds.
			StartCoroutine(WaitForSeconds(sleepArguments.Seconds));
		}

		public override void Combine(ITaskArguments taskArguments) {
			base.Initialize(taskArguments, TaskPriority);
			StopAllCoroutines();
			Execute();
		}

		public IEnumerator WaitForSeconds(float seconds) {
			yield return new WaitForSeconds(seconds);
			OnEnd();
		}
	}
}
