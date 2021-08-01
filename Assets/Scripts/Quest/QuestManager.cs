using SaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem {
	public class QuestManager : MonoBehaviour, ISaveable {
		public static QuestManager Instance;

		private Dictionary<(ObjectiveType, string), Action<int>> _callbacks = new Dictionary<(ObjectiveType, string), Action<int>>();

		private List<QuestDto> _questDtos = new List<QuestDto>();
		private Dictionary<string, Quest> _quests = new Dictionary<string, Quest>();

		private void Awake() {
			Instance = this;

			QuestionMarkTemplate = Resources.Load<GameObject>("Prefabs/question_mark");
			if (ReferenceEquals(QuestionMarkTemplate, null)) {
				throw new UnityException("Please define the prefab question_mark under Resources/Prefabs/question_mark");
			}

			InterrogationMarkTemplate = Resources.Load<GameObject>("Prefabs/interrogation_mark");
			if (ReferenceEquals(InterrogationMarkTemplate, null)) {
				throw new UnityException("Please define the prefab interrogation_mark under Resources/Prefabs/interrogation_mark");
			}
		}

		/// <summary>
		/// Subscribe to an event.
		/// </summary>
		/// <param name="topic">The topic to subscribe to.</param>
		/// <param name="action">The action to be triggered.</param>
		public void Subscribe(TaskData taskData, Action<int> action) {
			var key = (taskData.objectiveType, taskData.Identifier);
			if (_callbacks.ContainsKey(key)) {
				_callbacks[key] += action;
			} else {
				_callbacks.Add(key, action);
			}
		}

		/// <summary>
		/// Unsubscribe from an event.
		/// </summary>
		/// <param name="topic">The topic to subscribe to.</param>
		/// <param name="action">The action to be triggered.</param>
		public void Unsubscribe(TaskData taskData, Action<int> action) {
			var key = (taskData.objectiveType, taskData.Identifier);
			if (_callbacks.ContainsKey(key)) {
				_callbacks[key] -= action;
				if (_callbacks[key] == null) {
					_callbacks.Remove(key);
				}
			}
		}

		/// <summary>
		/// Trigger an event. Propagates this event to all quests that listens to this event.
		/// 
		/// Ex: An entity died: 
		///		ObjectiveType = KILL,
		///		topic = <name-of-the-entity>,
		///		amount = 1
		///		
		///	Ex: A dialogue has ended:
		///		ObjectiveType = TALK,
		///		topic = <name-of-the-entity>,
		///		amount = 1
		/// </summary>
		/// <param name="objectiveType">Objective type</param>
		/// <param name="topic">The topic</param>
		/// <param name="amount">The amount to progress the task</param>
		public void TriggerEvent(ObjectiveType objectiveType, string topic, int amount = 1) {
			if (_callbacks.TryGetValue((objectiveType, topic), out Action<int> value)) {
				value?.Invoke(amount);
			}
		}

		#region Save/Load
		/// <summary>
		/// Load all the quests from a save file.
		/// </summary>
		/// <param name="t"></param>
		public void Load(Save save) {
			_questDtos = save.QuestDtos;
			foreach (QuestDto questDto in _questDtos) {
				_quests.Add(questDto.ID, LoadQuest(questDto));
			}
		}

		/// <summary>
		/// On Save, return the current list of quest dto.
		/// </summary>
		/// <returns></returns>
		public void Save(Save save) {
			save.QuestDtos = _questDtos;
		}

		/// <summary>
		/// Load the quest from Resources based on its ID. Throws an exception if the quest isnt found in Resources folder.
		/// </summary>
		/// <param name="ID">The ID of the quest</param>
		/// <returns>Returns a fully-initialized quest.</returns>
		private Quest LoadQuest(QuestDto questDto) {
			Quest quest = new Quest();

			QuestData questData = Resources.Load<QuestData>("Scriptable Objects/Quests/" + questDto.ID);
			if (ReferenceEquals(questData, null)) {
				throw new UnityException("The following quest cannot be found by its ID: " + questDto.ID);
			}

			quest.Initialize(questDto, questData);
			return quest;
		}
		#endregion

		public GameObject QuestionMarkTemplate { get; private set; }
		public GameObject InterrogationMarkTemplate { get; private set; }
	}
}
