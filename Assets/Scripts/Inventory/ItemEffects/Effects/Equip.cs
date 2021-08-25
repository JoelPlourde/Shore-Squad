using GamePlay;
using ItemSystem.EquipmentSystem;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace ItemSystem {
	namespace EffectSystem {
		public class Equip : IItemEffect {

			public void Activate(Actor actor, Item item, ItemEffect _) {
				if (MenuBar.Instance.CurrentMenu == MenuType.INVENTORY) {
					OnEquip(actor, item);
				} else if (MenuBar.Instance.CurrentMenu == MenuType.EQUIPMENT){
					OnUnequip(actor, item);
				}
			}

			public void OnEquip(Actor actor, Item item) {
				Equipment equipment = new Equipment((EquipmentData)item.ItemData, 1);

				int requiredSpace = 0;
				if (actor.Armory.Equipments[equipment.EquipmentData.SlotType].IsAttached) {
					requiredSpace = actor.Armory.Equipments[equipment.EquipmentData.SlotType].Equipment.Amount;
				}

				if (requiredSpace > actor.Inventory.GetNumberOfAvailableSlotForItem(item.ItemData)) {
					FeedbackManager.Instance.DisplayError(actor, string.Format("You need at least {0} slot in your inventory to equip this", requiredSpace));
					return;
				}

				// TODO ADD SOUND HERE.
				if (actor.Armory.Equip(equipment, out Equipment previousEquipment)) {
					actor.Inventory.RemoveItemsFromInventory(new List<Item>() { item }, out List<Item> _);
					if (!ReferenceEquals(previousEquipment, null)) {
						actor.Inventory.AddItemsToInventory(new List<Item>() { new Item(previousEquipment.EquipmentData, previousEquipment.Amount) }, out List<Item> _);
					}
				}
			}

			public void OnUnequip(Actor actor, Item item) {
				Equipment equipment = new Equipment((EquipmentData)item.ItemData, 1);

				int requiredSpace = actor.Armory.Equipments[equipment.EquipmentData.SlotType].Equipment.Amount;
				if (requiredSpace > actor.Inventory.GetNumberOfAvailableSlotForItem(item.ItemData)) {
					FeedbackManager.Instance.DisplayError(actor, string.Format("You need at least {0} slot in your inventory to unequip this", requiredSpace));
					return;
				}

				// TODO ADD SOUND HERE.
				if (actor.Armory.Unequip(equipment.EquipmentData.SlotType, out Equipment previousEquipment)) {
					actor.Inventory.AddItemsToInventory(new List<Item>() { new Item(previousEquipment.EquipmentData, previousEquipment.Amount) }, out List<Item> remainingItems);
					if (remainingItems.Count > 0) {
						throw new UnityException("You shouldn't be able to unequip this equipment normally, please verify the logic.");
					}
				}
			}
		}
	}
}
