using System;
using System.Collections.Generic;
using System.Linq;
using ItemSystem;
using UnityEngine;
using SkillSystem;

namespace CraftingSystem {
	[Serializable]
	[CreateAssetMenu(fileName = "RecipeData", menuName = "ScriptableObjects/Recipe Data")]
	public class RecipeData : ScriptableObject {

		[SerializeField]
		public List<Item> Inputs;

		[SerializeField]
		public List<Item> Outputs;

		[SerializeField]
		[Header("Crafting Time")]
		public int RequiredProgress = 100;

		[SerializeField]
		[Header("Requirements")]
		public List<SkillRequirement> RequiredSkills;

		[SerializeField]
		[Header("Experience")]
		public List<ExperienceGain> ExperienceGains;

		public Dictionary<string, RecipeData> ToDictionary() {
			return Inputs.ToDictionary(x => x.ItemData.ID, x => this);
		}

		public bool CheckIfActorHasRequirements(Actor actor) {
			for (int i = 0; i < RequiredSkills.Count; i++) {
				if (actor.Skills.GetLevel( RequiredSkills[i].SkillType).Value <  RequiredSkills[i].Level) {
					Debug.Log("The actor does not have the required skill level.");
					return false;
				}
			}
			return true;
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
