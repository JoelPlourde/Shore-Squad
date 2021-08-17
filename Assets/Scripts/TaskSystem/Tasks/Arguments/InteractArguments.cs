using UnityEngine;

namespace TaskSystem {
	public class InteractArguments : ITaskArguments {

		public IInteractable Interactable;
		public Vector3 Position;

		public InteractArguments(Vector3 position, IInteractable interactable) {
			Position = position;
			Interactable = interactable;
		}

		public TaskType GetTaskType() {
			return TaskType.INTERACT;
		}
	}
}
