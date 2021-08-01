using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaskSystem {
	public class MoveArguments : ITaskArguments {

		public float Radius { get; }
		public Vector3 Position { get; }

		public MoveArguments(Vector3 position) {
			Position = position;
			Radius = 0.5f;
		}

		public MoveArguments(Vector3 position, float radius) {
			Position = position;
			Radius = radius;
		}

		public TaskType GetTaskType() {
			return TaskType.MOVE;
		}
	}
}
