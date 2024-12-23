using UnityEngine;
using System;

namespace TaskSystem {
    public class ProcessArguments : ITaskArguments {

        public float ProcessingTime;
        public Action OnProcessingDone;

        public ProcessArguments(float processingTime, Action onProcessingDone) {
            ProcessingTime = processingTime;
            OnProcessingDone = onProcessingDone;
        }

        public TaskType GetTaskType() {
			return TaskType.PROCESS;
		}
    }
}