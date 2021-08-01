using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem {
	public class QuestHandler : MonoBehaviour {

		public static QuestHandler Instance;

		public GameObject questComponentTemplate;
		public GameObject taskComponentTemplate;

		private Dictionary<string, QuestComponent> _questComponents = new Dictionary<string, QuestComponent>();

		private void Awake() {
			if (ReferenceEquals(Instance, null)) {
				Instance = this;
			}
		}

		public void AddQuest(Quest quest) {
			GameObject questComponentObj = GameObject.Instantiate(questComponentTemplate, transform);
			questComponentObj.transform.SetAsFirstSibling();
			QuestComponent questComponent = questComponentObj.GetComponent<QuestComponent>();
			questComponent.Initialize(quest);

			quest.OnTaskProgressed += OnTaskProgress;
			quest.OnTaskCompleted += OnTaskCompleted;
			quest.OnObjectiveCompleted += OnObjectiveCompleted;
			quest.OnQuestCompleted += OnQuestCompleted;

			_questComponents.Add(quest.ID, questComponent);
		}

		public void RemoveQuest(Quest quest) {
			// TODO implement this.
		}

		private void OnTaskProgress(string id) {
			if (_questComponents.ContainsKey(id)) {
				_questComponents[id].OnTaskProgress();
			}
		}

		private void OnTaskCompleted(string id) {
			if (_questComponents.ContainsKey(id)) {
				_questComponents[id].OnTaskCompleted();
			}
		}

		private void OnObjectiveCompleted(string id) {
			if (_questComponents.ContainsKey(id)) {
				_questComponents[id].OnObjectiveCompleted();
			}
		}

		private void OnQuestCompleted(string id) {
			if (_questComponents.ContainsKey(id)) {
				_questComponents[id].OnQuestCompleted();
			}
		}

		private void OnDestroy() {
			foreach (var pair in _questComponents) {
				pair.Value.Quest.OnTaskProgressed -= OnTaskProgress;
				pair.Value.Quest.OnTaskCompleted -= OnTaskCompleted;
				pair.Value.Quest.OnObjectiveCompleted -= OnObjectiveCompleted;
				pair.Value.Quest.OnQuestCompleted -= OnQuestCompleted;
			}
		}
	}
}
