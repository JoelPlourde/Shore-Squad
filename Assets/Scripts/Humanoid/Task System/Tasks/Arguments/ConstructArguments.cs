using ConstructionSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaskSystem {
	public class ConstructArguments : ITaskArguments {

		public ConstructionBehaviour ConstructionBehaviour;

		public ConstructArguments(ConstructionBehaviour constructionBehaviour) {
			ConstructionBehaviour = constructionBehaviour;
		}

		TaskType ITaskArguments.GetTaskType() {
			return TaskType.CONSTRUCT;
		}
	}
}
