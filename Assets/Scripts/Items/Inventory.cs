using System.Collections;
using System.Collections.Generic;
using TaskSystem;
using UnityEngine;
using System.Linq;
using System;

namespace ItemSystem {
	public class Inventory {

		public static readonly int MAX_STACK = 20;

		public event Action OnRedrawEvent;
		public event Action<List<Item>> OnItemsAddedEvent;
		public event Action<List<Item>> OnItemsRemovedEvent;

		private Item[] _items;

		public Inventory(int capacity) {
			_items = new Item[capacity];
		}

		/// <summary>
		/// Add items to the inventory.
		/// </summary>
		/// <param name="items">Items to add to the inventory.</param>
		/// <param name="remainingItems">Items that couldn't be added to the inventory.</param>
		/// /// <returns>Returns the items that have been added.</returns>
		public List<Item> AddItemsToInventory(List<Item> items, out List<Item> remainingItems) {
			remainingItems = new List<Item>();
			List<Item> addedItems = new List<Item>();
			foreach (Item item in items) {
				int availableAmount = GetNumberOfAvailableSlotForItem(item.ItemData);
				if (availableAmount >= item.Amount && AddItemToInventory(item.ItemData, item.Amount)) {
					addedItems.Add(new Item(item.ItemData, item.Amount));
				} else {
					if (availableAmount > 0 && AddItemToInventory(item.ItemData, availableAmount)) {
						addedItems.Add(new Item(item.ItemData, availableAmount));
					}

					int diff = item.Amount - availableAmount;
					if (diff > 0) {
						remainingItems.Add(new Item(item.ItemData, diff));
					}
				}
			}

			OnItemsAddedEvent?.Invoke(addedItems);
			return addedItems;
		}

		/// <summary>
		/// Remove items from the inventory.
		/// </summary>
		/// <param name="items">Items to remove from the inventory.</param>
		/// <param name="remainingItems">Items that couldn't be removed from the inventory.</param>
		/// <returns>Returns the items that have been removed.</returns>
		public List<Item> RemoveItemsFromInventory(List<Item> items, out List<Item> remainingItems) {
			remainingItems = new List<Item>();
			List<Item> removedItems = new List<Item>();
			foreach (Item item in items) {
				CheckIfItemExistsInInventory(item.ItemData, out int availableAmount);
				int amount = item.Amount;
				if (availableAmount >= item.Amount && RemoveItemFromInventory(item.ItemData, item.Amount)) {
					removedItems.Add(new Item(item.ItemData, amount));
				} else {
					amount = availableAmount;
					if (availableAmount > 0 && RemoveItemFromInventory(item.ItemData, availableAmount)) {
						removedItems.Add(new Item(item.ItemData, amount));
					}
					int diff = item.Amount - availableAmount;
					if (diff > 0) {
						remainingItems.Add(new Item(item.ItemData, diff));
					}
				}
			}

			OnItemsRemovedEvent?.Invoke(removedItems);
			return removedItems;
		}

		/// <summary>
		/// Add an item to the inventory.
		/// </summary>
		/// <param name="itemData">The ItemData to add.</param>
		/// <param name="amount">The amount to add.</param>
		/// <returns>Returns whether the operation was successful or not.</returns>
		public bool AddItemToInventory(ItemData itemData, int amount) {
			if (GetNumberOfAvailableSlotForItem(itemData) >= amount) {
				KeyValuePair<int, int> index = GetNextIndexToAdd(itemData, amount);
				if (index.Key >= 0) {
					if (index.Value == 0) {
						CreateItem(index.Key, itemData, _items[index.Key]?.Amount, amount);
					} else {
						CreateItem(index.Key, itemData, 0, MAX_STACK);
						return AddItemToInventory(itemData, index.Value);
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
		public bool RemoveItemFromInventory(ItemData itemData, int amount) {
			if (CheckIfItemExistsInInventory(itemData, out int count)) {
				if (count >= amount) {
					KeyValuePair<int, int> index = GetNextIndexToRemove(itemData, amount);
					if (index.Key >= 0) {
						_items[index.Key].Amount = (index.Value == 0) ? _items[index.Key].Amount - amount : 0;
						if (_items[index.Key].Amount == 0) {
							_items[index.Key] = null;
						}
						if (index.Value > 0) {
							return RemoveItemFromInventory(itemData, index.Value);
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
			for (int i = 0; i < _items.Length; i++) {
				if (_items[i] == null) {
					int diff = amount - MAX_STACK;
					return new KeyValuePair<int, int>(i, (diff > 0) ? diff : 0);
				}

				if (_items[i].Amount == MAX_STACK) {
					if (CheckIfIndexIsLast(i)) {
						return GetInvalidIndex();
					}
					continue;
				}

				if (_items[i].ItemData.ID == itemData.ID) {
					if (_items[i].Amount + amount > MAX_STACK) {
						return (CheckIfIndexIsLast(i)) ? GetInvalidIndex() : new KeyValuePair<int, int>(i, _items[i].Amount + amount - MAX_STACK);
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
			for (int i = 0; i < _items.Length; i++) {
				if (_items[i] != null && _items[i].ItemData.ID == itemData.ID) {
					if (_items[i].Amount - amount >= 0) {
						return new KeyValuePair<int, int>(i, 0);
					} else {
						amount -= _items[i].Amount;
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
			return Array.Exists(_items, x => x != null && x.ItemData.ID == itemData.ID);
		}

		/// <summary>
		/// Check if the item exists in the inventory. Returns the amount if so.
		/// </summary>
		/// <param name="itemData">The Item to look for.</param>
		/// <param name="amount">The cumulated amount</param>
		/// <returns>Return true if the item exists in the inventory else false.</returns>
		public bool CheckIfItemExistsInInventory(ItemData itemData, out int amount) {
			amount = Array.FindAll(_items, x => x != null && x.ItemData.ID == itemData.ID).Sum(x => x.Amount);
			return amount > 0;
		}

		/// <summary>
		/// Get the number of available slot for the item.
		/// </summary>
		/// <param name="itemData">The Item to verify for.</param>
		/// <returns>Return the number of available slot.</returns>
		public int GetNumberOfAvailableSlotForItem(ItemData itemData) {
			int count = 0;
			Array.ForEach(_items, x => {
				if (x == null) {
					count += MAX_STACK;
				} else if (x.ItemData.ID == itemData.ID){
					count += MAX_STACK - x.Amount;
				}
			});
			return count;
		}

		/// <summary>
		/// Get Items group by item ID and amount as value.
		/// </summary>
		/// <returns>A group of key: item Id and value: Amount</returns>
		public Dictionary<string, int> GetGroupedItems() {
			return _items
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
			if (index >= _items.Length) {
				throw new UnityException("Provided index is out of range of the items length: " + index + " out of : " + _items.Length);
			}
			return _items[index];
		}

		/// <summary>
		/// Get the non-null items in the inventory.
		/// </summary>
		/// <returns>Returns the list of non-null items in the inventory.</returns>
		public List<Item> GetItems() {
			return _items.Where(x => x != null).ToList();
		}

		/// <summary>
		/// Trigger the OnItemsAddedEvent manually.
		/// </summary>
		/// <param name="addedItems">The list of items that have been added.</param>
		public void ItemsAddedEvent(List<Item> addedItems) {
			OnItemsAddedEvent?.Invoke(addedItems);
		}

		/// <summary>
		/// Trigger the OnItemsRemovedEvent manually.
		/// </summary>
		/// <param name="removedItems">The list of items that have been removed.</param>
		public void ItemsRemovedEvent(List<Item> removedItems) {
			OnItemsRemovedEvent?.Invoke(removedItems);
		}

		/// <summary>
		/// Trigger the OnRedrawEvent manually.
		/// </summary>
		public void Redraw() {
			OnRedrawEvent?.Invoke();
		}

		/// <summary>
		/// Convert the inventory to a string.
		/// </summary>
		/// <returns>Returns the non-null items in the inventory.</returns>
		public override string ToString() {
			string res = "";
			foreach (var item in _items) {
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
			if (_items[index] == null) {
				_items[index] = new Item(itemData, amount, index);
			} else {
				_items[index].Amount = (previousAmount ?? 0) + amount;
			}
		}

		/// <summary>
		/// Check if the index correspond to the last index of the Items array.
		/// </summary>
		/// <param name="index">The index</param>
		/// <returns>Return true if so, else false.</returns>
		private bool CheckIfIndexIsLast(int index) {
			return index + 1 == _items.Length;
		}

		/// <summary>
		/// Get an invalid index.
		/// </summary>
		/// <returns>Return the definition of an invalid index.</returns>
		private KeyValuePair<int, int> GetInvalidIndex() {
			return new KeyValuePair<int, int>(-1, -1);
		}
	}
}
