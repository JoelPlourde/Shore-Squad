using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

namespace TaskSystem {
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(NavMeshAgent))]
	public class TaskScheduler : MonoBehaviour {

		public static Dictionary<TaskType, Type> TaskTypes = new Dictionary<TaskType, Type>() {
			{ TaskType.ATTACK, typeof(Attack) },
			{ TaskType.CONSTRUCT, typeof(Construct) },
			{ TaskType.FLEE, typeof(Flee) },
			{ TaskType.MOVE, typeof(Move) },
			{ TaskType.SLEEP, typeof(Sleep) }
		};
		private static readonly Dictionary<Guid, TaskScheduler> _instances = new Dictionary<Guid, TaskScheduler>();

		public event Action<TaskBehaviour> OnAddTaskEvent;
		public event Action<TaskBehaviour> OnEndTaskEvent;

		private Guid _guid;

		private bool _isExecuting = false;
		private readonly Queue<TaskBehaviour> _taskBehaviours = new Queue<TaskBehaviour>();

		private TaskBehaviour _currentTask;

		public void Initialize(Guid guid) {
			_guid = guid;
			_instances.Add(guid, this);
		}

		public static TaskScheduler Instance(Guid guid) {
			if (_instances.TryGetValue(guid, out TaskScheduler taskScheduler)) {
				return taskScheduler;
			}
			throw new UnityException("This instance of TaskScheduler does not exists by Guid: " + guid.ToString());
		}

		public void CreateTask(ITaskArguments taskArguments, TaskPriority taskPriority = TaskPriority.NORMAL) {
			MethodInfo methodInfo = typeof(TaskScheduler).GetMethods().Where(x => x.Name == "CreateTask").Where(x => x.IsGenericMethod).First();
			methodInfo = methodInfo.MakeGenericMethod(TaskTypes[taskArguments.GetTaskType()]);
			object[] args = { taskArguments, taskPriority };
			methodInfo.Invoke(this, args);
		}

		public void CreateTask<T>(ITaskArguments taskArguments, TaskPriority taskPriority = TaskPriority.NORMAL) where T : TaskBehaviour {
			if (!ReferenceEquals(_currentTask, null) && taskPriority >= _currentTask.TaskPriority) {
				CancelTask();
				TaskBehaviour taskBehaviour = gameObject.AddComponent<T>();
				taskBehaviour.Initialize(taskArguments, taskPriority);
				AddTask(taskBehaviour);
				return;
			}

			// Verify if the new Task is the same as an existing task, if so combine them.
			if (!ReferenceEquals(_currentTask, null) && _currentTask.TaskArguments.GetTaskType() == taskArguments.GetTaskType()) {
				_currentTask.Combine(taskArguments);
				return;
			}

			if (_taskBehaviours.Count > 0 && _taskBehaviours.Peek().TaskArguments.GetTaskType() == taskArguments.GetTaskType()) {
				// It is the same as the next one, therefore don't recreate a task for nothing.
				_taskBehaviours.Peek().Initialize(taskArguments, taskPriority);
			} else {
				TaskBehaviour taskBehaviour = gameObject.AddComponent<T>();
				taskBehaviour.Initialize(taskArguments, taskPriority);
				AddTask(taskBehaviour);
			}
		}

		private void AddTask(TaskBehaviour taskBehaviour) {
			_taskBehaviours.Enqueue(taskBehaviour);

			// Invoke on Add Task event.
			OnAddTaskEvent?.Invoke(taskBehaviour);

			if (!_isExecuting) {
				ScheduleTask();
			}
		}

		private void ScheduleTask() {
			// Task is executing;
			_isExecuting = true;

			// Fetch the new task.
			_currentTask = _taskBehaviours.Dequeue();

			// Subscribe to the event.
			_currentTask.OnEndCallback += OnTaskEnd;

			// Start executing the task.
			_currentTask.Execute();
		}

		public void CancelTask() {
			if (!ReferenceEquals(_currentTask, null)) {
				_currentTask.OnEnd();
				Destroy(_currentTask);
			}
		}

		public void CancelTask(TaskType taskType) {
			if (!ReferenceEquals(_currentTask, null) && _currentTask.TaskArguments.GetTaskType() == taskType) {
				_currentTask.OnEnd();
				Destroy(_currentTask);
			}
		}

		private void OnTaskEnd(TaskBehaviour taskBehaviour) {
			// Unsubscribe to the event.
			taskBehaviour.OnEndCallback -= OnTaskEnd;

			Destroy(taskBehaviour);

			_currentTask = null;

			// Invoke On End Task Event;
			OnEndTaskEvent?.Invoke(taskBehaviour);

			_isExecuting = false;
			if (_taskBehaviours.Count > 0) {
				ScheduleTask();
			}
		}

		public bool Busy { get { return _currentTask != null; } }
	}
}
