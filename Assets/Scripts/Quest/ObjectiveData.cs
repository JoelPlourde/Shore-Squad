using System;
using UnityEngine;

namespace QuestSystem {
	[Serializable]
	public class ObjectiveData {

		[SerializeField]
		[Tooltip("The title of the Objective.")]
		public string Title;

		[SerializeField]
		public TaskData[] TaskDatas;
	}
}
