using System.Collections.Generic;
using UI;
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

		/// <summary>
		/// Swap two items between different IContainer
		/// </summary>
		/// <param name="item">The item to be swapped.</param>
		/// <param name="source">The source container</param>
		/// <param name="destination">The destination container</param>
		/// <param name="sourceIndex">The index in the source container</param>
		/// <param name="destinationIndex">The index in the destination container</param>
		/// <returns>Whether or not the operation was successful.</returns>
		public static bool SwapItemsBetweenContainers(Item item, IContainer source, IContainer destination, int sourceIndex, int destinationIndex) {
			// Verify if the Condition at the Source & Destination is respected for this item.
			if (!source.IfCondition(item) || !destination.IfCondition(item)) {
				return false;
			}

			// Verify if the Condition at the Source & Destination for the existing item at destination, if any, is respected.
			Item existingItem = destination.GetItemAtIndex(destinationIndex);
			if (!ReferenceEquals(existingItem, null)) {
				if (!source.IfCondition(existingItem) || !destination.IfCondition(existingItem)) {
					return false;
				}
			}

			// Take a copy of the item itself.
			Item copy = new Item(item);

			// Remove it in the Source.
			if (source.RemoveItemAtSource(item, sourceIndex)) {
				Item _ = null;

				// Add it in the destination.
				if (destination.AddItemAtDestination(copy, destinationIndex, ref existingItem)) {
					if (!ReferenceEquals(existingItem, null)) {
						// Replace the existing item at source, if any.
						source.AddItemAtDestination(existingItem, sourceIndex, ref _);
					}
				} else {
					// If somehow, the operation failed. Rollback.
					source.AddItemAtDestination(copy, sourceIndex, ref _);
					return false;
				}

				if (!ReferenceEquals(_, null)) {
					throw new UnityException("Something happened here, please verify.");
				}
			}
			return true;
		}

		/// <summary>
		/// Group the Array of Items by ID
		/// </summary>
		/// <param name="items">The Array of Items to be grouped.</param>
		/// <returns>A dictionary of items where the key is the ID and the value is the Amount.</returns>
		public static Dictionary<string, int> GroupItemsByID(Item[] items) {
			Dictionary<string, int> groupedItems = new Dictionary<string, int>();
			foreach (Item item in items) {
				if (!ReferenceEquals(item, null)) {
					string key = item.ItemData.ID;
					if (groupedItems.ContainsKey(key)) {
						groupedItems[key] += item.Amount;
					} else {
						groupedItems.Add(key, item.Amount);
					}
				}
			}
			return groupedItems;
		}

		public static bool CheckIfItemsCanBeInserted(Item[] sources, Item[] destinations) {
			Dictionary<string, int> currentOutputs = GroupItemsByID(destinations);

			// Verify if you will be able to add the outputs once the operation is completed.
			foreach (Item item in sources) {
				if (currentOutputs.ContainsKey(item.ItemData.ID)) {
					currentOutputs[item.ItemData.ID] += item.Amount;
					if (currentOutputs[item.ItemData.ID] > Inventory.MAX_STACK) {
						return false;
					}
				} else {
					currentOutputs.Add(item.ItemData.ID, item.Amount);

					// If you have added more key in the dictionary then there is destinations, return false.
					if (currentOutputs.Count > destinations.Length) {
						return false;
					}
				}
			}

			return true;
		}
	}
}
