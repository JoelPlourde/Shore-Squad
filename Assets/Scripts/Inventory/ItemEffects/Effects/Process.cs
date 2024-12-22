using UnityEngine;
using System.Collections.Generic;
using UI;
using CraftingSystem;

namespace ItemSystem {
	namespace EffectSystem {
		public class Process : IItemEffect {

            private Actor _actor;
            private Item _item;

			public void Activate(Actor actor, Item item, ItemEffect itemEffect) {
                if (ReferenceEquals(itemEffect.RecipeDatas, null)) {
                    Debug.LogError("To Process an item, you must provide its Recipe!");
                    return;
                }

                _actor = actor;
                _item = item;
                
                RecipeHandler.Instance.Show(itemEffect.RecipeDatas, OnRecipeClicked, item.ItemData.Sprite);
			}

            public void OnRecipeClicked(RecipeData recipeData) {
                Item[] itemsArray = _actor.Inventory.GetItems().ToArray();

                if (!recipeData.CheckIfRecipeIsValid(itemsArray)) {
                    Debug.LogWarning("This recipe is not valid.");
                    return;
                }

                _actor.Emotion.PlayEmote(EmoteSystem.EmoteType.PROCESS);

                _actor.Inventory.RemoveItemsFromInventory(recipeData.Inputs, out List<Item> removedItems);
                if (removedItems.Count > 0) {
                    Debug.Log("We could not remove the item!!");
                    return;
                }

                _actor.Inventory.AddItemsToInventory(recipeData.Outputs, out List<Item> addedItems);
                if (addedItems.Count > 0) {
                    Debug.Log("We could not add the item!!");
                    return;
                }
            }
		}
	}
}