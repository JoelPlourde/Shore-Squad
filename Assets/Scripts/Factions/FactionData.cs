using System;
using UnityEngine;

namespace FactionSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "FactionData", menuName = "ScriptableObjects/Faction Data")]
	public class FactionData : ScriptableObject {

		[SerializeField]
		[Tooltip("Main color representing this faction.")]
		public Color MainColor;

		[SerializeField]
		[Tooltip("Secondary color representing this faction.")]
		public Color SecondaryColor;

		[SerializeField]
		[Tooltip("Flag's material associated to this faction.")]
		public Material Flag;
	}
}
