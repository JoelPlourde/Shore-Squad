using UnityEngine;
using System.Collections;

namespace TaskSystem {
    public class Process : TaskBehaviour {
        
        private IEnumerator _coroutine;
		private ProcessArguments _processArguments;

        public override void Execute() {
			// Validate the arguments you've received is of the correct type.
			_processArguments = TaskArguments as ProcessArguments;

            Debug.Log("Processing task started in the task itself");

            actor.Animator.SetBool("Processing", true);
            actor.Animator.SetFloat("Processing Speed", 1);

            // Start a timer with the processing time.
            _coroutine = waitForProcessingTime();

            StartCoroutine(_coroutine);
        }

        public override void Combine(ITaskArguments taskArguments) {
			base.Initialize(taskArguments, TaskPriority);
            StopCoroutine(_coroutine);
            Execute();
		}

        IEnumerator waitForProcessingTime() {
            yield return new WaitForSecondsRealtime(_processArguments.ProcessingTime);

            OnEnd();

            _processArguments.OnProcessingDone();
        }

        public override void OnEnd() {
            base.OnEnd();

            actor.Animator.SetBool("Processing", false);
        }
    }
}