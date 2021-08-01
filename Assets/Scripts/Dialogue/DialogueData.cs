using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/Dialogue Data")]
	public class DialogueData : ScriptableObject {

		[SerializeField]
		[Tooltip("If any ACTION, this node will trigger an ObjectiveType.ACTIVATE with this value.")]
		public string Identifier;

		[SerializeField]
		public Node Entry;
	}
}
