using QuestSystem;
using System;
using UnityEngine;

namespace SaveSystem {
	[Serializable]
	public class QuestDto {

		[SerializeField]
		public string ID;

		[SerializeField]
		public int CurrentObjective;

		[SerializeField]
		public int CurrentTask;

		[SerializeField]
		public int CurrentProgress;

		[SerializeField]
		public QuestState QuestState;
	}
}
