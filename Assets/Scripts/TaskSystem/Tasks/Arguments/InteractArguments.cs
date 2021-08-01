using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaskSystem {
	public class InteractArguments : ITaskArguments {

		public Actor Actor;
		public IInteractable Interactable;
		public Vector3 Position;
		public float Radius;

		public InteractArguments(float radius, Vector3 position, IInteractable interactable) {
			Radius = radius;
			Position = position;
			Interactable = interactable;
		}

		public TaskType GetTaskType() {
			return TaskType.INTERACT;
		}
	}
}
