using System;
using UnityEngine;

namespace QuestSystem {
	[Serializable]
	public abstract class Trigger : ScriptableObject {

		public abstract void Execute();
	}
}
