﻿using System.Collections;
using System.Collections.Generic;
using TaskSystem;
using UnityEngine;
using System.Linq;
using System;

namespace ItemSystem {
	public class Inventory {

		public static readonly int MAX_STACK = 20;

		public event Action<List<int>, Item[]> OnDirtyItemsEvent;
		public event Action<Item[]> OnRedrawEvent;

		public Inventory(int capacity) {
			Items = new Item[capacity];
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
		/// Add an item to the inventory.
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