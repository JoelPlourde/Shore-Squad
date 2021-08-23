using ItemSystem.EquipmentSystem;
using ItemSystem.UI;
using System.Collections.Generic;

namespace ItemSystem {
	namespace EffectSystem {
		public class Equip : IItemEffect {

			public void Activate(Actor actor, Item item, ItemEffect _) {

				Equipment equipment = new Equipment((EquipmentData) item.ItemData, 1);

				if (actor.Armory.Equip(equipment, out Equipment previousEquipment)) {
					actor.Inventory.RemoveItemsFromInventory(new List<Item>() { item }, out List<Item> _);

					// TODO ADD SOUND HERE.

					if (!ReferenceEquals(previousEquipment, null)) {
						actor.Inventory.AddItemsToInventory(new List<Item>() { new Item(previousEquipment.EquipmentData, previousEquipment.Amount) }, out List<Item> _);
					}
				}
			}
		}
	}
}
