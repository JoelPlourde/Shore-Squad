using System;
using UnityEngine;

namespace ConstructionSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "ConstructionData", menuName = "ScriptableObjects/Construction/Construction Data")]
	public class ConstructionData : ScriptableObject {

		/*
		[SerializeField]
		[Tooltip("Resources cost to build this building")]
		public List<Resource> Costs;
		*/

		[SerializeField]
		[Tooltip("Time (in seconds realtime) taken to construct this construction.")]
		public int ConstructionTime;

		[SerializeField]
		[Tooltip("Image of this construction")]
		public Sprite Sprite;

		[SerializeField]
		[Tooltip("Prefab of this construction")]
		public GameObject Prefab;

		[SerializeField]
		[TextArea(3, 15)]
		[Tooltip("Tooltip to be displayed.")]
		public string Tooltip;
	}
}
