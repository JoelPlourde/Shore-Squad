using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem {
	public static class InventoryUtils {

		/// <summary>
		/// Transfer an item from a source inventory to a destination inventory. If the operation cannot be completed, no action is done.
		/// </summary>
		/// <param name="source">Source inventory</param>
		/// <param name="destination">Destination inventory</param>
		/// <param name="itemData">The ItemData of the item to transfer.</param>
		/// <param name="amount">The amount of the item to transfer.</param>
		/// <returns>Returns if the operation could be completed or not.</returns>
		public static bool TransferItem(Inventory source, Inventory destination, ItemData itemData, int amount) {
			if (source.CheckIfItemExistsInInventory(itemData, out int count) && count >= amount) {
				if (destination.GetNumberOfAvailableSlotForItem(itemData) >= amount) {
					List<Item> item = new List<Item>() { new Item(itemData, amount )};
					// TODO optimize this
					List<int> i = new List<int>();
					List<int> j = new List<int>();
					if (source.RemoveItemFromInventory(itemData, amount, ref i) && destination.AddItemToInventory(itemData, amount, ref j)) {
						List<Item> modifiedItems = new List<Item> { new Item(itemData, amount) };
						source.Redraw();
						destination.Redraw();
						return true;
					} else {
						return false;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Transfer the items from a source inventory to a destination inventory. If the operation cannot be completed, the items that couldn't be
		/// transferred are populated in remaining items list.
		/// This method triggers the Redraw event.
		/// </summary>
		/// <param name="source">Source inventory.</param>
		/// <param name="destination">Destination inventory.</param>
		/// <param name="Items">Items to transfer</param>
		/// <param name="remainingItems">Remaining items that couldn't be added.</param>
		public static void TransferItems(Inventory source, Inventory destination, List<Item> Items, out List<Item> remainingItems) {
			remainingItems = new List<Item>();
			foreach (var item in Items) {
				if (!TransferItem(source, destination, item.ItemData, item.Amount)) {
					remainingItems.Add(new Item(item.ItemData, item.Amount));
				}
			}

			source.Redraw();
			destination.Redraw();
		}
	}
}
