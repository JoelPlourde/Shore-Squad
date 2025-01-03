using UnityEngine;
using System.Collections.Generic;
using UI;
using CraftingSystem;
using TaskSystem;
using SkillSystem;

namespace ItemSystem {
	namespace EffectSystem {
		public class Process : IItemEffect {

            private Actor _actor;
            private Item _item;
            private RecipeData _recipeData;

			public void Activate(Actor actor, Item item, ItemEffect itemEffect) {
                if (ReferenceEquals(itemEffect.RecipeDatas, null)) {
                    Debug.LogError("To Process an item, you must provide its Recipe!");
                    return;
                }

                _actor = actor;
                _item = item;
                
                if (itemEffect.IsImmediate) {
                    StartProcessingTask(itemEffect.RecipeDatas[0]);
                    return;
                } else {
                    RecipeHandler.Instance.Show(itemEffect.RecipeDatas, StartProcessingTask, item.ItemData.Sprite);
                }
			}

            public void StartProcessingTask(RecipeData recipeData) {
                _recipeData = recipeData;

                if (!CheckIfRecipeIsValid()) {
                    Debug.LogWarning("This recipe is not valid.");
                    return;
                }

                ProcessArguments processArguments = new ProcessArguments(recipeData.RequiredProgress, OnProcessingDone);

                _actor.TaskScheduler.CreateTask(processArguments, TaskPriority.HIGH);
            }

            public void OnProcessingDone() {
                if (!CheckIfRecipeIsValid()) {
                    Debug.LogWarning("This recipe is not valid.");
                    return;
                }

                _actor.Inventory.RemoveItemsFromInventory(_recipeData.Inputs, out List<Item> removedItems);
                if (removedItems.Count > 0) {
                    Debug.Log("We could not remove the item!!");
                    return;
                }

                _actor.Inventory.AddItemsToInventory(_recipeData.Outputs, out List<Item> addedItems);
                if (addedItems.Count > 0) {
                    Debug.Log("We could not add the item!!");
                    return;
                }

                for (int i = 0; i < _recipeData.ExperienceGains.Count; i++) {
                    ExperienceGain experienceGain = _recipeData.ExperienceGains[i];
                    _actor.Skills.GainExperience(experienceGain.SkillType, experienceGain.Experience);
                }
            }

            private bool CheckIfRecipeIsValid() {
                if (!_recipeData.CheckIfActorHasRequirements(_actor)) {
                    return false;
                }

                Item[] itemsArray = _actor.Inventory.GetItems().ToArray();
                return _recipeData.CheckIfRecipeIsValid(itemsArray);
            }
		}
	}
}