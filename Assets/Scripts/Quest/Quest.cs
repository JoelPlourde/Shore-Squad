using GameSystem;
using SaveSystem;
using System;
using UnityEngine;

namespace QuestSystem {
	public class Quest {

		#region Event
		public event Action<string> OnTaskProgressed;
		public event Action<string> OnTaskCompleted;
		public event Action<string> OnObjectiveCompleted;
		public event Action<string> OnQuestCompleted;
		#endregion

		public void Initialize(QuestDto questDto, QuestData questData) {
			QuestData = questData;
			ID = questDto.ID;
			CurrentObjective = questDto.CurrentObjective;
			CurrentTask = questDto.CurrentTask;
			CurrentProgress = questDto.CurrentProgress;
			State = questDto.QuestState;

			SpawnDependencies();

			QuestManager.Instance.Subscribe(questData.objectiveDatas[CurrentObjective].TaskDatas[CurrentTask], UpdateTaskProgress);
			QuestHandler.Instance.AddQuest(this);
		}

		private void SpawnDependencies() {
			if (!ReferenceEquals(Task.Dependencies, null)) {
				if (SceneController.Instance.IsLoading) {
					SceneController.Instance.QueueAction(Task.Dependencies.Spawn, this);
				} else {
					Task.Dependencies.Spawn(this);
				}
			}
		}

		private void DespawnDepencencies() {
			if (!ReferenceEquals(Task.Dependencies, null)) {
				Task.Dependencies.Despawn(this);
			}
		}

		private void Trigger() {
			if (!ReferenceEquals(Task.Triggers, null) && Task.Triggers.Length > 0) {
				foreach (Trigger trigger in Task.Triggers) {
					trigger.Execute();
				}
			}
		}

		#region Progress
		private void UpdateTaskProgress(int value) {
			if (State == QuestState.IN_PROGRESS) {
				CurrentProgress += value;

				OnTaskProgressed?.Invoke(ID);

				CheckTaskProgress();
			}
		}

		private void CheckTaskProgress() {
			if (CurrentProgress >= QuestData.objectiveDatas[CurrentObjective].TaskDatas[CurrentTask].Amount) {

				Trigger();

				QuestManager.Instance.Unsubscribe(QuestData.objectiveDatas[CurrentObjective].TaskDatas[CurrentTask], UpdateTaskProgress);

				DespawnDepencencies();

				CurrentTask++;
				CurrentProgress = 0;

				if (!CheckObjectiveProgress()) {
					OnTaskCompleted?.Invoke(ID);

					QuestManager.Instance.Subscribe(QuestData.objectiveDatas[CurrentObjective].TaskDatas[CurrentTask], UpdateTaskProgress);
				} else {
					SpawnDependencies();
				}
			}
		}

		private bool CheckObjectiveProgress() {
			if (CurrentTask >= QuestData.objectiveDatas[CurrentObjective].TaskDatas.Length) {

				CurrentObjective++;
				CurrentTask = 0;

				if (!CheckQuestProgress()) {
					OnObjectiveCompleted?.Invoke(ID);

					QuestManager.Instance.Subscribe(QuestData.objectiveDatas[CurrentObjective].TaskDatas[CurrentTask], UpdateTaskProgress);
				}
				return true;
			}
			return false;
		}

		private bool CheckQuestProgress() {
			if (CurrentObjective >= QuestData.objectiveDatas.Length) {
				State = QuestState.COMPLETED;
				OnQuestCompleted?.Invoke(ID);
				return true;
			}
			return false;
		}
		#endregion

		public int CurrentObjective { get; private set; }
		public int CurrentTask { get; private set; }
		public int CurrentProgress { get; private set; }

		public TaskData Task { get { return QuestData.objectiveDatas[CurrentObjective].TaskDatas[CurrentTask]; } }

		public string ID { get; private set; }
		public QuestState State { get; private set; }
		public QuestData QuestData { get; private set; }
	}
}