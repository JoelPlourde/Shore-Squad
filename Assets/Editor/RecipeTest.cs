using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using CraftingSystem;
using ItemSystem;
using UnityEngine.TestTools;

namespace UnitTest {
	public class RecipeTest : IPrebuildSetup {

		public void Setup() {
			var gameObject = new GameObject();
			ItemManager itemManager = gameObject.AddComponent<ItemManager>();
		}

		[Test]
		[PrebuildSetup(typeof(RecipeTest))]
		public void CheckIfRecipeIsPossible_with_not_enough_items_test() {
			RecipeData recipeData = GetRecipeDataFixture_Branches_to_Log();

			Item branch = new Item(GetItemDataFixture(0), 2);

			Item[] items = new Item[2];
			items[0] = branch;

			Assert.True(recipeData.CheckIfRecipeIsPossible(items));
		}

		[Test]
		[PrebuildSetup(typeof(RecipeTest))]
		public void CheckIfRecipeIsPossible_with_enough_items_in_different_slot_test() {
			RecipeData recipeData = GetRecipeDataFixture_Branches_to_Log();

			Item branch1 = new Item(GetItemDataFixture(0), 2);
			Item branch2 = new Item(GetItemDataFixture(0), 1);

			Item[] items = new Item[2];
			items[0] = branch1;
			items[1] = branch2;

			Assert.True(recipeData.CheckIfRecipeIsPossible(items));
		}

		[Test]
		[PrebuildSetup(typeof(RecipeTest))]
		public void CheckIfRecipeIsPossible_with_different_items_test() {
			RecipeData recipeData = GetRecipeDataFixture_Branches_to_Log();

			Item stone = new Item(GetItemDataFixture(2), 3);

			Item[] items = new Item[2];
			items[0] = stone;

			Assert.False(recipeData.CheckIfRecipeIsPossible(items));
		}

		[Test]
		[PrebuildSetup(typeof(RecipeTest))]
		public void CheckIfRecipeIsPossible_with_enough_items_test() {
			RecipeData recipeData = GetRecipeDataFixture_Log_to_Branches();

			Item log = new Item(GetItemDataFixture(1), 1);

			Item[] items = new Item[2];
			items[0] = log;

			Assert.True(recipeData.CheckIfRecipeIsPossible(items));
		}

		[Test]
		[PrebuildSetup(typeof(RecipeTest))]
		public void CheckIfRecipeIsValid_test() {
			RecipeData recipeData = GetRecipeDataFixture_Branches_to_Log();

			Item branch = new Item(GetItemDataFixture(0), 3);
			Item[] items = new Item[2];
			items[0] = branch;

			Assert.True(recipeData.CheckIfRecipeIsValid(items));
		}

		[Test]
		[PrebuildSetup(typeof(RecipeTest))]
		public void CheckIfRecipeIsValid_with_not_enough_items_test() {
			RecipeData recipeData = GetRecipeDataFixture_Branches_to_Log();

			Item branch = new Item(GetItemDataFixture(0), 2);
			Item[] items = new Item[2];
			items[0] = branch;

			Assert.False(recipeData.CheckIfRecipeIsValid(items));
		}

		[Test]
		[PrebuildSetup(typeof(RecipeTest))]
		public void CheckIfRecipeIsValid_with_empty_items_test() {
			RecipeData recipeData = GetRecipeDataFixture_Branches_to_Log();
			Assert.False(recipeData.CheckIfRecipeIsValid(new Item[2]));
		}

		[Test]
		[PrebuildSetup(typeof(RecipeTest))]
		public void CheckIfRecipeIsValid_complex_test() {
			RecipeData recipeData = GetComplexRecipeDataFixture();

			Item log = new Item(GetItemDataFixture(1), 1);
			Item stone = new Item(GetItemDataFixture(2), 4);
			Item[] items = new Item[2];
			items[0] = log;
			items[1] = stone;

			Assert.True(recipeData.CheckIfRecipeIsValid(items));
		}

		[Test]
		[PrebuildSetup(typeof(RecipeTest))]
		public void CheckIfRecipeIsValid_complex_misordered_test() {
			RecipeData recipeData = GetComplexRecipeDataFixture();

			Item log = new Item(GetItemDataFixture(1), 1);
			Item stone = new Item(GetItemDataFixture(2), 4);
			Item[] items = new Item[2];
			items[0] = stone;
			items[1] = log;

			Assert.True(recipeData.CheckIfRecipeIsValid(items));
		}

		[Test]
		[PrebuildSetup(typeof(RecipeTest))]
		public void CheckIfRecipeIsValid_complex_with_unfilled_slot_test() {
			RecipeData recipeData = GetComplexRecipeDataFixture();

			Item log = new Item(GetItemDataFixture(1), 1);
			Item stone = new Item(GetItemDataFixture(2), 4);
			Item[] items = new Item[4];
			items[0] = stone;
			items[1] = log;

			Assert.True(recipeData.CheckIfRecipeIsValid(items));
		}

		[Test]
		[PrebuildSetup(typeof(RecipeTest))]
		public void Equals_test() {
			RecipeData recipeData1 = GetRecipeDataFixture_Branches_to_Log();
			RecipeData recipeData2 = GetRecipeDataFixture_Log_to_Branches();
			Assert.False(recipeData1.Equals(recipeData2));
		}

		private RecipeData GetRecipeDataFixture_Log_to_Branches() {
			return new RecipeData {
				Inputs = new List<Item> {
					new Item (GetItemDataFixture(1), 1)
				},
				Outputs = new List<Item> {
					new Item(GetItemDataFixture(0), 3)
				}
			};
		}

		private RecipeData GetRecipeDataFixture_Branches_to_Log() {
			RecipeData recipeData = GetRecipeDataFixture_Log_to_Branches();
			var tmp = recipeData.Inputs;
			recipeData.Inputs = recipeData.Outputs;
			recipeData.Outputs = tmp;
			return recipeData;
		}

		private RecipeData GetComplexRecipeDataFixture() {
			return new RecipeData {
				Inputs = new List<Item> {
					new Item (GetItemDataFixture(1), 1),
					new Item (GetItemDataFixture(2), 4)
				},
				Outputs = new List<Item> {
					new Item (GetItemDataFixture(0), 3)
				}
			};
		}

		private ItemData GetItemDataFixture(int index) {
			switch (index) {
				case 0:
					return ItemManager.Instance.GetItemData("branch");
				case 1:
					return ItemManager.Instance.GetItemData("log");
				case 2:
					return ItemManager.Instance.GetItemData("stone");
				default:
					return ItemManager.Instance.GetItemData("branch");
			}
		}
	}
}
