using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DropSystem;

namespace NodeSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "NodeData", menuName = "ScriptableObjects/Node Data")]
	public class NodeData : ScriptableObject {

		[Header("Harvest Parameters")]
		[Tooltip("Maximum capacity of harvest before the node as a chance to be depleted.")]
		public uint HarvestCapacity = 1;

		[Range(5, 100)]
		[Tooltip("Probability that a node will be depleted after reaching its maximum capacity.")]
		public byte Probability = 5;

		[Header("Drops")]
		[Tooltip("Drop table that will rolled on whenever a node is harvested.")]
		public DropTable DropTable;

		// TODO REQUIREMENTS
	}
}
