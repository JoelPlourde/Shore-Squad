using System;
using UnityEngine;

namespace NodeSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "TreeData", menuName = "ScriptableObjects/Node Data/Tree")]
	public class TreeData : NodeData {

		[Tooltip("The particle system to be played to determine how the node reacts to being hit.")]
		public ParticleSystemData OnResponse;
	}
}
