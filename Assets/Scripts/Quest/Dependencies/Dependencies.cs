using System;
using UnityEngine;

namespace QuestSystem {
	[Serializable]
	public abstract class Dependencies : ScriptableObject {

		public abstract void Spawn(Quest quest);

		public abstract void Despawn(Quest quest);
	}
}