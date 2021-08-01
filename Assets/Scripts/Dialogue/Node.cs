using System;
using UnityEngine;

namespace DialogueSystem {
	[Serializable]
	public class Node {

		[SerializeField]
		[Tooltip("Indicates how to handle the following node.")]
		public NodeType NodeType = NodeType.DEFAULT;

		[SerializeField]
		[Tooltip("Content to display in the dialogue box")]
		[TextArea(3, 5)]
		public string Content;

		[Tooltip("Indicates the next node to proceed to.")]
		public Node[] Nodes;

		public Node Next(int option) {
			return Nodes[option];
		}
	}
}
