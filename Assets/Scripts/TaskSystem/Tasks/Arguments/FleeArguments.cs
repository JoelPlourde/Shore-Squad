using System.Collections;
using System.Collections.Generic;
using TaskSystem;
using UnityEngine;

namespace TaskSystem {
	public class FleeArguments : ITaskArguments {

		public Vector3 Direction;

		public FleeArguments(Vector3 direction) {
			Direction = direction;
		}

		TaskType ITaskArguments.GetTaskType() {
			return TaskType.FLEE;
		}
	}
}
