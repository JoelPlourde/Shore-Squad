using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "QuestData", menuName = "ScriptableObjects/Quest/Quest Data")]
	public class QuestData : ScriptableObject {

		[SerializeField]
		[Tooltip("The name of the Quest")]
		public string Title;

		[SerializeField]
		[TextArea(3, 15)]
		[Tooltip("A short prologue of the quest")]
		public string Prologue;

		[SerializeField]
		[Tooltip("The objectives that composes the Quest")]
		public ObjectiveData[] objectiveDatas;

		[SerializeField]
		[Tooltip("The IDs of the quest required to start this one.")]
		public string[] Requirements;

		[SerializeField]
		[Tooltip("The IDs of the quest that is unlocked whenever this one is completed.")]
		public string[] Unlocks;
	}
}
