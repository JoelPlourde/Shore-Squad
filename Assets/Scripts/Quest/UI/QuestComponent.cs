using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem {
	[RequireComponent(typeof(RectTransform))]
	public class QuestComponent : MonoBehaviour {

		private Text _title;
		private TaskComponent[] _taskComponents;

		private RectTransform _rectTransform;
		private RectTransform _parentTransform;

		private int _currentTask = 0;

		private void Awake() {
			_title = transform.Find("Title").GetComponent<Text>();
			if (ReferenceEquals(_title, null)) {
				throw new UnityException("Please verify the hierarchy of this QuestComponent: Missing Title");
			}

			_rectTransform = GetComponent<RectTransform>();
		}

		/// <summary>
		/// Initialize the quest component.
		/// </summary>
		/// <param name="quest">The quest.</param>
		public void Initialize(Quest quest) {
			Quest = quest;

			InitializeComponent();

			int count = 0;
			foreach (var objectives in quest.QuestData.objectiveDatas) {
				if (objectives.TaskDatas.Length > count) {
					count = objectives.TaskDatas.Length;
				}
			}

			_taskComponents = new TaskComponent[count];
			for (int i = 0; i < count; i++) {
				_taskComponents[i] = CreateTaskComponent();
			}

			ResizeComponent(_taskComponents[_currentTask].Initialize(_parentTransform, quest));
		}

		/// <summary>
		/// Callback called whenever a task progressed.
		/// </summary>
		public void OnTaskProgress() {
			_taskComponents[_currentTask].UpdateDisplay(Quest);
		}

		/// <summary>
		/// Callback called whenever a task has been completed.
		/// </summary>
		public void OnTaskCompleted() {
			_taskComponents[_currentTask].MarkCompleted();
			_currentTask++;
			if (_currentTask < _taskComponents.Length) {
				ResizeComponent(_taskComponents[_currentTask].Initialize(_parentTransform, Quest));
			}
		}

		/// <summary>
		/// Callback called whenever an objective has been completed.
		/// </summary>
		public void OnObjectiveCompleted() {
			_currentTask = 0;
			for (int i = 1; i < _taskComponents.Length; i++) {
				if (!ReferenceEquals(_taskComponents[i], null)) {
					ResizeComponent(-_taskComponents[i].Disable());
				}
			}
			_taskComponents[_currentTask].UpdateDisplay(Quest);
			_taskComponents[_currentTask].gameObject.SetActive(true);
		}

		/// <summary>
		/// Callback called whenever the quest has been completed.
		/// </summary>
		public void OnQuestCompleted() {
			for (int i = 0; i < _taskComponents.Length; i++) {
				if (!ReferenceEquals(_taskComponents[i], null)) {
					ResizeComponent(-_taskComponents[i].Disable());
					_taskComponents[i].Delete();
				}
			}
			_title.text += " ✔";

			_taskComponents = null;
		}

		private TaskComponent CreateTaskComponent() {
			GameObject taskComponentObj = Instantiate(QuestHandler.Instance.taskComponentTemplate, _title.transform);
			TaskComponent taskComponent = taskComponentObj.GetComponent<TaskComponent>();
			taskComponentObj.SetActive(false);
			return taskComponent;
		}

		private void InitializeComponent() {
			_parentTransform = transform.parent.GetComponent<RectTransform>();
			_title.rectTransform.localPosition = Vector2.zero;
			_title.rectTransform.sizeDelta = new Vector2(_parentTransform.sizeDelta.x, _title.rectTransform.sizeDelta.y);
			_title.text = Quest.QuestData.Title;

			ResizeComponent(_title.rectTransform.sizeDelta.y);
		}

		private void ResizeComponent(float height) {
			_rectTransform.sizeDelta = new Vector2(_parentTransform.sizeDelta.x, _rectTransform.sizeDelta.y + height);
		}

		public Quest Quest { get; private set; }
	}
}
