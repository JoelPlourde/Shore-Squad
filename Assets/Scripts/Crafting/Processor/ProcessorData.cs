using System;
using System.Collections.Generic;
using UnityEngine;

namespace CraftingSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "ProcessorData", menuName = "ScriptableObjects/Processor Data")]
	public class ProcessorData : ScriptableObject {

		[SerializeField]
		public List<RecipeData> Recipes;

		[SerializeField]
		[Tooltip("Whether or not the character has to operate the processor to process the recipes.")]
		public bool Manual;

		[SerializeField]
		[Tooltip("Whether or not the processor requires power or not.")]
		public bool RequiresPower;

		[SerializeField]
		[Tooltip("Percentage value of the efficiency (%)")]
		[Range(0, 2)]
		public float Efficiency;

		[SerializeField]
		[Tooltip("Number of Input slot(s)")]
		public int InputCount = 1;

		[SerializeField]
		[Tooltip("Number of Output slot(s)")]
		public int OutputCount = 1;

		[SerializeField]
		[Tooltip("Number of Fuel slot(s)")]
		public int FuelCount = 1;
	}
}
