using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaskSystem {
	public class SleepArguments : ITaskArguments {

		public float Seconds { get; }

		public SleepArguments(float seconds) {
			Seconds = seconds;
		}

		public TaskType GetTaskType() {
			return TaskType.SLEEP;
		}
	}
}