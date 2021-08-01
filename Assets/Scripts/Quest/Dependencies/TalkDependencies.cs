using UnityEngine;
using System;
using DialogueSystem;

namespace QuestSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "TalkDependencies", menuName = "ScriptableObjects/Quest/Dependencies/Talk")]
	public class TalkDependencies : Dependencies {

		[SerializeField]
		[Tooltip("The Prefab of the NPC to talk to.")]
		public GameObject NPC;

		[SerializeField]
		[Tooltip("Where is located the NPC for this particular task.")]
		public Vector3 Position;

		[SerializeField]
		[Tooltip("What the NPC is going to say when the player interacts with it.")]
		public DialogueData DialogueData;

		public override void Spawn(Quest quest) {
			Debug.Log("Spawning: " + NPC.name);
			GameObject npcObject = GameObject.Find(NPC.name);
			if (ReferenceEquals(npcObject, null)) {
				npcObject = Instantiate(NPC, Position, Quaternion.identity);
				npcObject.name = npcObject.name.Replace("(Clone)", "");
			}

			NPC npc = npcObject.GetComponent<NPC>();
			if (ReferenceEquals(npc, null)) {
				npc = npcObject.AddComponent<NPC>();
			}

			npc.Initialize(DialogueData);
			if (quest.State == QuestState.NOT_STARTED) {
				npc.SetQuestMarker(Instantiate(QuestManager.Instance.InterrogationMarkTemplate, npc.transform.position + Vector3.up, Quaternion.identity, npc.transform));
			} else if (quest.State == QuestState.IN_PROGRESS) {
				npc.SetQuestMarker(Instantiate(QuestManager.Instance.QuestionMarkTemplate, npc.transform.position + Vector3.up, Quaternion.identity, npc.transform));
			}
		}

		public override void Despawn(Quest quest) {
			GameObject.Find(NPC.name).GetComponent<NPC>().ResetQuestMarker();
		}
	}
}
