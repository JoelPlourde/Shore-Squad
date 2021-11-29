using System;
using System.Collections.Generic;
using System.Linq;
using ItemSystem;
using UnityEngine;

namespace CraftingSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "RecipeData", menuName = "ScriptableObjects/Recipe Data")]
	public class RecipeData : ScriptableObject {

		[SerializeField]
		public List<Item> Inputs;

		[SerializeField]
		public List<Item> Outputs;

		[SerializeField]
		public int RequiredProgress = 100;

		public Dictionary<string, RecipeData> ToDictionary() {
			return Inputs.ToDictionary(x => x.ItemData.ID, x => this);
		}

		public bool CheckIfRecipeIsPossible(Item[] items) {
			HashSet<string> ids = new HashSet<string>(Inputs.Select(x => x.ItemData.ID));

			foreach (Item item in items) {
				if (!ReferenceEquals(item, null) && ids.Contains(item.ItemData.ID)) {
					return true;
				}
			}

			return false;
		}

		public bool CheckIfRecipeIsValid(Item[] items) {
			Dictionary<string, int> actual = InventoryUtils.GroupItemsByID(items);
			Dictionary<string, int> expected = Inputs.ToDictionary(x => x.ItemData.ID, x => x.Amount);

			foreach (var key in expected.Keys) {
				if (!actual.ContainsKey(key)) return false;
				if (actual[key] < expected[key]) return false;
			}

			return true;
		}
	}
}
