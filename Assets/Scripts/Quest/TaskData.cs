using System;
using UnityEngine;
using DialogueSystem;

namespace QuestSystem {
	[Serializable]
	public class TaskData {

		[SerializeField]
		[Tooltip("The title of the task.")]
		public string Title;

		[SerializeField]
		public ObjectiveType objectiveType;

		[SerializeField]
		[Tooltip("How many X do you need to complete the task.")]
		public int Amount;

		[SerializeField]
		[Tooltip("What is the identifier of the task ? Ex: Kill 10 Goblins, the identifier is 'goblin', or Gather 5 berries, the identifier is 'berries'")]
		public string Identifier;

		[SerializeField]
		[Tooltip("Dependencies of the task. Ex: The player has to TALK to a NPC located at X,Y,Z with W dialogue.")]
		public Dependencies Dependencies;

		[SerializeField]
		[Tooltip("Action(s) occuring when this task get completed. Ex: Change the scene, trigger a cinematic, etc.")]
		public Trigger[] Triggers;
	}
}
