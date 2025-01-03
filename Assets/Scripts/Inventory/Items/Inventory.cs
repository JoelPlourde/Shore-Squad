using System.Collections;
using System.Collections.Generic;
using TaskSystem;
using UnityEngine;
using System.Linq;
using System;
using SaveSystem;
using UI;

namespace ItemSystem {
	public class Inventory {

		public static readonly int MAX_STACK = 20;

		public event Action<List<int>, Item[]> OnDirtyItemsEvent;
		public event Action<Item[]> OnRedrawEvent;

		public Inventory(int capacity) {
			Items = new Item[capacity];
		}

		/// <summary>
		/// Initialize the Inventory with a saved inventory.
		/// </summary>
		/// <param name="inventoryDto">The saved inventory to load.</param>
		public void Initialize(InventoryDto inventoryDto) {
			for (int i = 0; i < Items.Length; i++) {
				if (!ReferenceEquals(inventoryDto.ItemDtos[i], null) && inventoryDto.ItemDtos[i].ID != "-1") {
					Items[i] = new Item(ItemManager.Instance.GetItemData(inventoryDto.ItemDtos[i].ID), inventoryDto.ItemDtos[i].Amount, i);
				} else {
					Items[i] = null;
				}
			}
		}

		/// <summary>
		/// Add items to the inventory.
		/// </summary>
		/// <param name="items">Items to add to the inventory.</param>
		/// <param name="remainingItems">Items that couldn't be added to the inventory.</param>
		/// /// <returns>Returns the items that have been added.</returns>
		public void AddItemsToInventory(List<Item> items, out List<Item> remainingItems) {
			remainingItems = new List<Item>();
			List<int> indexes = new List<int>();
			foreach (Item item in items) {
				int availableAmount = GetNumberOfAvailableSlotForItem(item.ItemData);

				if (availableAmount == 0) {
					remainingItems.Add(new Item(item.ItemData, item.Amount));
					continue;
				}

				if (availableAmount >= item.Amount) {
					AddItemToInventory(item.ItemData, item.Amount, ref indexes);
				} else {
					AddItemToInventory(item.ItemData, availableAmount, ref indexes);
					int diff = item.Amount - availableAmount;
					if (diff > 0) {
						remainingItems.Add(new Item(item.ItemData, diff));
					}
				}
			}

			OnDirtyItemsEvent?.Invoke(indexes, Items);
		}

		/// <summary>
		/// Remove items from the inventory.
		/// </summary>
		/// <param name="items">Items to remove from the inventory.</param>
		/// <param name="remainingItems">Items that couldn't be removed from the inventory.</param>
		/// <returns>Returns the items that have been removed.</returns>
		public void RemoveItemsFromInventory(List<Item> items, out List<Item> remainingItems) {
			remainingItems = new List<Item>();
			List<int> indexes = new List<int>();
			foreach (Item item in items) {
				CheckIfItemExistsInInventory(item.ItemData, out int availableAmount);
				if (availableAmount >= item.Amount) {
					RemoveItemFromInventory(item.ItemData, item.Amount, ref indexes);
				} else {
					RemoveItemFromInventory(item.ItemData, availableAmount, ref indexes);
					int diff = item.Amount - availableAmount;
					if (diff > 0) {
						remainingItems.Add(new Item(item.ItemData, diff));
					}
				}
			}

			OnDirtyItemsEvent?.Invoke(indexes, Items);
		}

		/// <summary>
		/// Add an item to the inventory. WARNING DO NOT USE THIS METHOD DIRECTLY, USE ADDITEMSTOINVENTORY
		/// </summary>
		/// <param name="itemData">The ItemData to add.</param>
		/// <param name="amount">The amount to add.</param>
		/// <returns>Returns whether the operation was successful or not.</returns>
		public bool AddItemToInventory(ItemData itemData, int amount, ref List<int> indexes) {
			if (GetNumberOfAvailableSlotForItem(itemData) >= amount) {
				KeyValuePair<int, int> keyValuePair = GetNextIndexToAdd(itemData, amount);
				if (keyValuePair.Key >= 0) {
					indexes.Add(keyValuePair.Key);
					if (keyValuePair.Value == 0) {
						CreateItem(keyValuePair.Key, itemData, Items[keyValuePair.Key]?.Amount, amount);
					} else {
						CreateItem(keyValuePair.Key, itemData, 0, (itemData.Stackable) ? MAX_STACK : 1);
						return AddItemToInventory(itemData, keyValuePair.Value, ref indexes);
					}
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Remove an item from the inventory.
		/// </summary>
		/// <param name="itemData">The ItemData to remove.</param>
		/// <param name="amount">The amount to remove.</param>
		/// <returns>Returns whether the operation was successful or not.</returns>
		public bool RemoveItemFromInventory(ItemData itemData, int amount, ref List<int> indexes) {
			if (CheckIfItemExistsInInventory(itemData, out int count)) {
				if (count >= amount) {
					KeyValuePair<int, int> keyValuePair = GetNextIndexToRemove(itemData, amount);
					if (keyValuePair.Key >= 0) {
						indexes.Add(keyValuePair.Key);
						Items[keyValuePair.Key].Amount = (keyValuePair.Value == 0) ? Items[keyValuePair.Key].Amount - amount : 0;
						if (Items[keyValuePair.Key].Amount == 0) {
							Items[keyValuePair.Key] = null;
						}
						if (keyValuePair.Value > 0) {
							return RemoveItemFromInventory(itemData, keyValuePair.Value, ref indexes);
						}
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Swap two items inside the same inventory
		/// </summary>
		/// <param name="sourceIndex">The source index</param>
		/// <param name="destinationIndex">The destination index</param>
		/// <returns>Returns true if the operation was successful, else false.</returns>
		public bool SwapItems(int sourceIndex, int destinationIndex) {
			if (Items.Length <= sourceIndex || Items.Length <= destinationIndex || sourceIndex < 0 || destinationIndex < 0) {
				return false;
			}

			if (!CombineItems(sourceIndex, destinationIndex)) {
				var buffer = Items[sourceIndex];
				Items[sourceIndex] = Items[destinationIndex];
				Items[destinationIndex] = buffer;

				Items[sourceIndex]?.UpdateIndex(sourceIndex);
				Items[destinationIndex]?.UpdateIndex(destinationIndex);
			}

			OnDirtyItemsEvent?.Invoke(new List<int>() { sourceIndex, destinationIndex }, Items);
			return true;
		}

		/// <summary>
		/// Combine Item at Source to Destination.
		///	WARNING; This method does not trigger a redraw at the indexes.
		/// </summary>
		/// <param name="sourceIndex">The index of the source item</param>
		/// <param name="destinationIndex">The index of the destination item</param>
		/// <returns>True if the combination worked, else false.</returns>
		public bool CombineItems(int sourceIndex, int destinationIndex) {
			if (ReferenceEquals(Items[sourceIndex], null) || ReferenceEquals(Items[destinationIndex], null)) {
				return false;
			}

			if (!Items[sourceIndex].ItemData.Stackable || !Items[destinationIndex].ItemData.Stackable) {
				return false;
			}

			if (Items[destinationIndex].ItemData.ID == Items[sourceIndex].ItemData.ID) {
				int diff = MAX_STACK - Items[destinationIndex].Amount;
				if (diff > 0) {
					if (Items[sourceIndex].Amount <= diff) {
						Items[destinationIndex].Amount += Items[sourceIndex].Amount;
						Items[sourceIndex] = null;
					} else if (Items[sourceIndex].Amount > diff) {
						Items[destinationIndex].Amount += diff;
						Items[sourceIndex].Amount -= diff;
					}
					return true;
				} else {
					return false;
				}
			}

			return false;
		}

		/// <summary>
		/// Add an item in the inventory at position.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <param name="index">The index where to add the item</param>
		/// <param name="remainingItem">The remaining item, if any</param>
		/// <param name="callback"></param>
		/// <returns></returns>
		public bool AddItemInInventoryAtPosition(Item item, int index, ref Item remainingItem) {
			if (ReferenceEquals(Items[index], null)) {
				Items[index] = new Item(item.ItemData, item.Amount, index);
				OnDirtyItemsEvent?.Invoke(new List<int>() { index }, Items);
				return true;
			}

			// There is an item in the slot, check if the Item is the same to combine them.
			if (item.ItemData.ID == Items[index].ItemData.ID) {
				Items[index].Amount += item.Amount;
				int difference = Items[index].Amount - MAX_STACK;
				if (difference > 0) {
					// Adjust the remaining quantity.
					remainingItem = new Item(Items[index].ItemData, difference);
				}
				Items[index].Amount = Mathf.Clamp(Items[index].Amount, 0, MAX_STACK);
				OnDirtyItemsEvent?.Invoke(new List<int>() { index }, Items);
				return true;
			}

			// There is an item in the slot, but its not the same, swap.
			if (item.ItemData.ID != Items[index].ItemData.ID) {
				remainingItem = Items[index];
				Items[index] = new Item(item.ItemData, item.Amount, index);
				OnDirtyItemsEvent?.Invoke(new List<int>() { index }, Items);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Remove an item from the inventory at the specific position.
		/// </summary>
		/// <param name="index">The index has to be between 0 and MAX_STACK</param>
		/// <param name="amount">The amount to remove.</param>
		/// <returns>True if the operation was successful else, false.</returns>
		public bool RemoveItemFromInventoryAtPosition(int index, int amount) {
			if (Items.Length <= index || index < 0 || amount < 0 || amount > MAX_STACK) {
				return false;
			}

			if (ReferenceEquals(Items[index], null)) {
				return false;
			}

			if (Items[index].Amount < amount) {
				return false;
			}

			Items[index].Amount -= amount;
			if (Items[index].Amount == 0) {
				Items[index] = null;
			}

			OnDirtyItemsEvent?.Invoke(new List<int>() { index }, Items);
			return true;
		}

		/// <summary>
		/// Get the next index where to add the item.
		/// </summary>
		/// <param name="itemData">The ItemData to add.</param>
		/// <param name="amount">The amount to try to add from the array.</param>
		/// <returns>Returns the index where the item to add is located and the remainder amount after potentially adding the item.</returns>
		public KeyValuePair<int, int> GetNextIndexToAdd(ItemData itemData, int amount) {
			ValidateNegativeAmountCondition(amount);
			for (int i = 0; i < Items.Length; i++) {
				if (Items[i] == null) {
					int diff = amount - ((itemData.Stackable) ? MAX_STACK : 1);
					return new KeyValuePair<int, int>(i, (diff > 0) ? diff : 0);
				}

				if (Items[i].Amount == MAX_STACK || !Items[i].ItemData.Stackable) {
					if (CheckIfIndexIsLast(i)) {
						return GetInvalidIndex();
					}
					continue;
				}

				if (Items[i].ItemData.ID == itemData.ID && Items[i].ItemData.Stackable) {
					if (Items[i].Amount + amount > MAX_STACK) {
						return (CheckIfIndexIsLast(i)) ? GetInvalidIndex() : new KeyValuePair<int, int>(i, Items[i].Amount + amount - MAX_STACK);
					} else {
						return new KeyValuePair<int, int>(i, 0);
					}
				}
			}
			return GetInvalidIndex();
		}

		/// <summary>
		/// Get the next index where to remove the item.
		/// </summary>
		/// <param name="itemData">The ItemData to remove.</param>
		/// <param name="amount">The amount to try to remove from the array.</param>
		/// <returns>Return the index where the item to remove is located and the remainder amount after potentially removing the item.</returns>
		public KeyValuePair<int, int> GetNextIndexToRemove(ItemData itemData, int amount) {
			ValidateNegativeAmountCondition(amount);
			for (int i = 0; i < Items.Length; i++) {
				if (Items[i] != null && Items[i].ItemData.ID == itemData.ID) {
					if (Items[i].Amount - amount >= 0) {
						return new KeyValuePair<int, int>(i, 0);
					} else {
						amount -= Items[i].Amount;
						return new KeyValuePair<int, int>(i, amount);
					}
				}
			}
			return GetInvalidIndex();
		}

		/// <summary>
		/// Check if the item exists in the inventory.
		/// </summary>
		/// <param name="itemData">The Item to look for.</param>
		/// <returns>Return true if the item exists in the inventory else false.</returns>
		public bool CheckIfItemExistsInInventory(ItemData itemData) {
			return Array.Exists(Items, x => x != null && x.ItemData.ID == itemData.ID);
		}

		/// <summary>
		/// Check if the item exists in the inventory. Returns the amount if so.
		/// </summary>
		/// <param name="itemData">The Item to look for.</param>
		/// <param name="amount">The cumulated amount</param>
		/// <returns>Return true if the item exists in the inventory else false.</returns>
		public bool CheckIfItemExistsInInventory(ItemData itemData, out int amount) {
			amount = Array.FindAll(Items, x => x != null && x.ItemData.ID == itemData.ID).Sum(x => x.Amount);
			return amount > 0;
		}

		/// <summary>
		/// Get the number of available slot for the item.
		/// </summary>
		/// <param name="itemData">The Item to verify for.</param>
		/// <returns>Return the number of available slot.</returns>
		public int GetNumberOfAvailableSlotForItem(ItemData itemData) {
			int count = 0;
			Array.ForEach(Items, x => {
				if (itemData.Stackable) {
					if (x == null) {
						count += MAX_STACK;
					} else if (x.ItemData.ID == itemData.ID) {
						count += MAX_STACK - x.Amount;
					}
				} else {
					if (x == null) {
						count++;
					}
				}
			});
			return count;
		}

		/// <summary>
		/// Get Items group by item ID and amount as value.
		/// </summary>
		/// <returns>A group of key: item Id and value: Amount</returns>
		public Dictionary<string, int> GetGroupedItems() {
			return Items
				.Where(item => item != null)
				.GroupBy(x => x.ItemData.ID)
				.Select(g => new { g.Key, Value = g.Sum(s => s.Amount) })
				.ToDictionary(x => x.Key, x => x.Value);
		}

		/// <summary>
		/// Get Item at the index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns></returns>
		public Item GetItemAtIndex(int index) {
			if (index >= Items.Length) {
				throw new UnityException("Provided index is out of range of the items length: " + index + " out of : " + Items.Length);
			}
			return Items[index];
		}

		/// <summary>
		/// Get the non-null items in the inventory.
		/// </summary>
		/// <returns>Returns the list of non-null items in the inventory.</returns>
		public List<Item> GetItems() {
			return Items.Where(x => x != null).ToList();
		}

		/// <summary>
		/// Trigger the OnRedrawEvent manually.
		/// </summary>
		public void Redraw() {
			OnRedrawEvent?.Invoke(Items);
		}

		/// <summary>
		/// Convert the inventory to a string.
		/// </summary>
		/// <returns>Returns the non-null items in the inventory.</returns>
		public override string ToString() {
			string res = "";
			foreach (var item in Items) {
				res += item?.ToString() + "\n";
			}
			return res;
		}

		/// <summary>
		/// Validate if the amount received in parameters is at least higher than zero.
		/// </summary>
		/// <param name="amount"></param>
		private void ValidateNegativeAmountCondition(int amount) {
			if (amount <= 0) {
				throw new UnityException("You cannot retrieved the index of a negative amount or zero.");
			}
		}

		/// <summary>
		/// Create an item object to be stored into the Items array.
		/// </summary>
		/// <param name="index">The index of the array to create the item.</param>
		/// <param name="itemData">The Item Data of the item.</param>
		/// <param name="previousAmount">The previous amount of this item, if any.</param>
		/// <param name="amount">The amount to register for this item.</param>
		private void CreateItem(int index, ItemData itemData, int? previousAmount, int amount) {
			if (Items[index] == null) {
				Items[index] = new Item(itemData, amount, index);
			} else {
				Items[index].Amount = (previousAmount ?? 0) + amount;
			}
		}

		/// <summary>
		/// Check if the index correspond to the last index of the Items array.
		/// </summary>
		/// <param name="index">The index</param>
		/// <returns>Return true if so, else false.</returns>
		private bool CheckIfIndexIsLast(int index) {
			return index + 1 == Items.Length;
		}

		/// <summary>
		/// Get an invalid index.
		/// </summary>
		/// <returns>Return the definition of an invalid index.</returns>
		private KeyValuePair<int, int> GetInvalidIndex() {
			return new KeyValuePair<int, int>(-1, -1);
		}

		public Item[] Items { get; }
	}
}
