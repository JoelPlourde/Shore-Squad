using UnityEngine;
using System.Collections.Generic;

namespace ItemSystem {
	namespace EffectSystem {
		public class Process : IItemEffect {

			public void Activate(Actor actor, Item item, ItemEffect itemEffect) {
                if (ReferenceEquals(itemEffect.RecipeData, null)) {
                    Debug.LogError("To Process an item, you must provide its Recipe!");
                    return;
                }

                Item[] items = new Item[] { item };
                if (!itemEffect.RecipeData.CheckIfRecipeIsPossible(items)) {
                    Debug.LogError("This recipe is not possible.");
                    return;
                }

                if (!itemEffect.RecipeData.CheckIfRecipeIsValid(items)) {
                    Debug.LogError("This recipe is not valid.");
                    return;
                }

				actor.Emotion.PlayEmote(EmoteSystem.EmoteType.PROCESS);

                List<Item> itemList = new List<Item> { item };
                actor.Inventory.RemoveItemsFromInventory(itemList, out List<Item> removedItems);
                if (removedItems.Count > 0) {
                    Debug.Log("We could not remove the item!!");
                    return;
                }

                actor.Inventory.AddItemsToInventory(itemEffect.RecipeData.Outputs, out List<Item> addedItems);
                if (addedItems.Count > 0) {
                    Debug.Log("We could not add the item!!");
                    return;
                }
			}
		}
	}
}