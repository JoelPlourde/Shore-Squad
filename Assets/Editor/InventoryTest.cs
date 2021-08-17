using ItemSystem;
using NUnit.Framework;
using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnitTest {
	public class InventoryTest : IPrebuildSetup {

		public void Setup() {
			var gameObject = new GameObject();
			ItemManager itemManager = gameObject.AddComponent<ItemManager>();
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void Setup_test() {
			Assert.That(ItemManager.Instance, !Is.EqualTo(null));
			ItemData itemData = GetItemDataFixture();
			Assert.That(itemData, !Is.EqualTo(null));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNextIndexToRemoveWithEmptyInventory_test() {
			Inventory inventory = new Inventory(1);
			Assert.That(inventory.GetNextIndexToRemove(GetItemDataFixture(), 5), Is.EqualTo(new KeyValuePair<int, int>(-1, -1)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNextIndexToRemoveWithNegativeAmount_test() {
			Assert.Throws<UnityException>(delegate () {
				Inventory inventory = new Inventory(1);
				inventory.GetNextIndexToRemove(GetItemDataFixture(), -1);
			});
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNextIndexToAddWithEmptyInventory_test() {
			Inventory inventory = new Inventory(1);
			Assert.That(inventory.GetNextIndexToAdd(GetItemDataFixture(), 5), Is.EqualTo(new KeyValuePair<int, int>(0, 0)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNextIndexToAddWithEmptyInventory_non_stackable_test() {
			Inventory inventory = new Inventory(1);
			Assert.That(inventory.GetNextIndexToAdd(GetNonStackableItemData(), 1), Is.EqualTo(new KeyValuePair<int, int>(0, 0)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNextIndexToAddWithNegativeAmount_test() {
			Assert.Throws<UnityException>(delegate () {
				Inventory inventory = new Inventory(1);
				inventory.GetNextIndexToAdd(GetItemDataFixture(), -1);
			});
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNextIndexToAddMaxStack_test() {
			Inventory inventory = new Inventory(1);
			Assert.That(inventory.GetNextIndexToAdd(GetItemDataFixture(), 25), Is.EqualTo(new KeyValuePair<int, int>(0, 5)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNextIndexToAdd_non_stackable_test() {
			Inventory inventory = new Inventory(1);
			Assert.That(inventory.GetNextIndexToAdd(GetNonStackableItemData(), 2), Is.EqualTo(new KeyValuePair<int, int>(0, 1)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNextIndexToAddSameItem_non_stackable_test() {
			Inventory inventory = new Inventory(1);
			ItemData nonStackableItem = GetNonStackableItemData();

			Assert.That(inventory.GetNextIndexToAdd(nonStackableItem, 1), Is.EqualTo(new KeyValuePair<int, int>(0, 0)));
			List<int> indexes = new List<int>();
			inventory.AddItemToInventory(nonStackableItem, 1, ref indexes);
			Assert.That(inventory.GetNextIndexToAdd(nonStackableItem, 1), Is.EqualTo(new KeyValuePair<int, int>(-1, -1)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNextIndexToAddWithFullInventoryWithCombine_test() {
			Inventory inventory = new Inventory(1);
			ItemData itemData = GetItemDataFixture();
			List<int> indexes = new List<int>();
			inventory.AddItemToInventory(itemData, 15, ref indexes);
			Assert.That(indexes[0], Is.EqualTo(0));
			Assert.That(inventory.GetNextIndexToAdd(itemData, 5), Is.EqualTo(new KeyValuePair<int, int>(0, 0)));
			Assert.That(inventory.GetNextIndexToAdd(itemData, 6), Is.EqualTo(new KeyValuePair<int, int>(-1, -1)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNextIndexToAddWithFullInventory_test() {
			Inventory inventory = new Inventory(1);
			ItemData itemData = GetItemDataFixture();
			List<int> indexes = new List<int>();
			inventory.AddItemToInventory(itemData, Inventory.MAX_STACK, ref indexes);
			Assert.That(indexes[0], Is.EqualTo(0));
			Assert.That(inventory.GetNextIndexToAdd(itemData, 1), Is.EqualTo(new KeyValuePair<int, int>(-1, -1)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddItemToInventoryWithMultipleItem_test() {
			Inventory inventory = new Inventory(3);
			List<int> indexes = new List<int>();
			for (int i = 0; i < 3; i++) {
				inventory.AddItemToInventory(GetItemDataFixture(i), 5 + i, ref indexes);
			}

			for (int i = 0; i < 3; i++) {
				Assert.That(inventory.CheckIfItemExistsInInventory(GetItemDataFixture(i), out int amount));
				Assert.That(amount, Is.EqualTo(5 + i));
			}
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddItemToInventoryCombine_test() {
			Inventory inventory = new Inventory(1);
			ItemData itemData = GetItemDataFixture();
			List<int> indexes1 = new List<int>();
			List<int> indexes2 = new List<int>();
			Assert.That(inventory.AddItemToInventory(itemData, 10, ref indexes1), Is.True);
			Assert.That(indexes1[0], Is.EqualTo(0));
			Assert.That(inventory.AddItemToInventory(itemData, 10, ref indexes2), Is.True);
			Assert.That(indexes2[0], Is.EqualTo(0));
			Assert.That(inventory.CheckIfItemExistsInInventory(itemData, out int amount), Is.True);
			Assert.That(amount, Is.EqualTo(Inventory.MAX_STACK));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddItemToInventoryWithFullInventory_test() {
			Inventory inventory = new Inventory(1);
			ItemData itemData = GetItemDataFixture();
			List<int> indexes1 = new List<int>();
			List<int> indexes2 = new List<int>();
			Assert.That(inventory.AddItemToInventory(itemData, Inventory.MAX_STACK, ref indexes1), Is.True);
			Assert.That(indexes1[0], Is.EqualTo(0));
			Assert.That(inventory.AddItemToInventory(itemData, 1, ref indexes2), Is.False);
			Assert.That(indexes2.Count, Is.EqualTo(0));
			Assert.That(inventory.GetItemAtIndex(0), Is.EqualTo(new Item(itemData, Inventory.MAX_STACK)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddItemToInventoryWithNegativeAmount_test() {
			Assert.Throws<UnityException>(delegate () {
				Inventory inventory = new Inventory(1);
				List<int> indexes = new List<int>();
				inventory.AddItemToInventory(GetItemDataFixture(), -1, ref indexes);
			});
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void RemoveItemFromInventoryWithEmptyInventory_test() {
			Inventory inventory = new Inventory(1);
			List<int> indexes = new List<int>();
			Assert.That(inventory.RemoveItemFromInventory(GetItemDataFixture(), 1, ref indexes), Is.False);
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void RemoveItemFromInventoryWithFullInventory_test() {
			Inventory inventory = new Inventory(1);
			ItemData itemData = GetItemDataFixture();
			List<int> indexes1 = new List<int>();
			List<int> indexes2 = new List<int>();
			inventory.AddItemToInventory(itemData, Inventory.MAX_STACK, ref indexes1);
			Assert.That(indexes1[0], Is.EqualTo(0));
			Assert.That(inventory.RemoveItemFromInventory(itemData, 10, ref indexes2), Is.True);
			Assert.That(indexes2[0], Is.EqualTo(0));
			Assert.That(inventory.CheckIfItemExistsInInventory(itemData, out int amount), Is.True);
			Assert.That(amount, Is.EqualTo(10));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void RemoveItemFromInventory_test() {
			Inventory inventory = new Inventory(2);
			ItemData itemData = GetItemDataFixture();
			List<int> indexes1 = new List<int>();
			List<int> indexes2 = new List<int>();
			inventory.AddItemToInventory(itemData, 15, ref indexes1);
			Assert.That(indexes1[0], Is.EqualTo(0));
			inventory.AddItemToInventory(itemData, 8, ref indexes2);
			Assert.That(indexes2[0], Is.EqualTo(0));
			Assert.That(inventory.CheckIfItemExistsInInventory(itemData, out int before), Is.True);
			Assert.That(before, Is.EqualTo(23));

			List<int> indexes3 = new List<int>();
			Assert.That(inventory.RemoveItemFromInventory(itemData, Inventory.MAX_STACK, ref indexes3), Is.True);
			Assert.That(indexes3[0], Is.EqualTo(0));
			Assert.That(inventory.CheckIfItemExistsInInventory(itemData, out int after), Is.True);
			Assert.That(after, Is.EqualTo(3));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void RemoveItemFromInventoryWhereNotEnoughItems_test() {
			Inventory inventory = new Inventory(2);
			ItemData itemData = GetItemDataFixture();
			List<int> indexes1 = new List<int>();
			inventory.AddItemToInventory(itemData, Inventory.MAX_STACK, ref indexes1);
			Assert.That(indexes1[0], Is.EqualTo(0));
			List<int> indexes2 = new List<int>();
			inventory.AddItemToInventory(itemData, 4, ref indexes2);
			Assert.That(indexes2[0], Is.EqualTo(1));
			List<int> indexes3 = new List<int>();
			Assert.That(inventory.RemoveItemFromInventory(itemData, 30, ref indexes3), Is.False);
			Assert.That(indexes3.Count, Is.EqualTo(0));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNumberOfAvailableSlot_test() {
			Inventory inventory = new Inventory(1);
			ItemData itemData = GetItemDataFixture();
			List<int> indexes1 = new List<int>();
			Assert.That(inventory.AddItemToInventory(itemData, 15, ref indexes1), Is.True);
			Assert.That(indexes1[0], Is.EqualTo(0));
			Assert.That(inventory.GetNumberOfAvailableSlotForItem(itemData), Is.EqualTo(5));
			List<int> indexes2 = new List<int>();
			Assert.That(inventory.AddItemToInventory(itemData, 5, ref indexes2), Is.True);
			Assert.That(indexes2[0], Is.EqualTo(0));
			Assert.That(inventory.GetNumberOfAvailableSlotForItem(itemData), Is.EqualTo(0));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNumberOfAvailableSlotWithNotStackable_test() {
			Inventory inventory = new Inventory(2);
			ItemData itemData = GetNonStackableItemData();
			Assert.That(inventory.GetNumberOfAvailableSlotForItem(itemData), Is.EqualTo(2));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNumberOfAvailableSlotWithEmptyInventory_test() {
			Inventory inventory = new Inventory(5);
			Assert.That(inventory.GetNumberOfAvailableSlotForItem(GetItemDataFixture()), Is.EqualTo(Inventory.MAX_STACK * 5));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetNumberOfAvailableSlotWithFullInventory_test() {
			Inventory inventory = new Inventory(1);
			ItemData itemData = GetItemDataFixture();
			List<int> indexes = new List<int>();
			Assert.That(inventory.AddItemToInventory(itemData, Inventory.MAX_STACK, ref indexes), Is.True);
			Assert.That(indexes[0], Is.EqualTo(0));
			Assert.That(inventory.GetNumberOfAvailableSlotForItem(GetItemDataFixture()), Is.EqualTo(0));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void TransferItem_test() {
			Inventory source = new Inventory(1);
			Inventory destination = new Inventory(1);

			List<int> indexes = new List<int>();
			ItemData itemData = GetItemDataFixture();
			Assert.That(source.AddItemToInventory(itemData, Inventory.MAX_STACK, ref indexes), Is.True);
			Assert.That(indexes[0], Is.EqualTo(0));

			Assert.That(InventoryUtils.TransferItem(source, destination, itemData, 15));

			Assert.That(source.CheckIfItemExistsInInventory(itemData, out int srcAmount));
			Assert.That(srcAmount, Is.EqualTo(5));

			Assert.That(destination.CheckIfItemExistsInInventory(itemData, out int dstAmount));
			Assert.That(dstAmount, Is.EqualTo(15));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void TransferItemWithEmptySource_test() {
			Inventory source = new Inventory(1);
			Inventory destination = new Inventory(1);

			ItemData itemData = GetItemDataFixture();
			Assert.That(InventoryUtils.TransferItem(source, destination, itemData, 15), Is.False);

			Assert.That(source.CheckIfItemExistsInInventory(itemData), Is.False);
			Assert.That(destination.CheckIfItemExistsInInventory(itemData), Is.False);
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void TransferItemWithFullDestination_test() {
			Inventory source = new Inventory(1);
			Inventory destination = new Inventory(1);

			ItemData itemData = GetItemDataFixture();

			List<int> indexes = new List<int>();
			Assert.That(destination.AddItemToInventory(itemData, Inventory.MAX_STACK, ref indexes), Is.True);
			Assert.That(indexes[0], Is.EqualTo(0));

			Assert.That(InventoryUtils.TransferItem(source, destination, itemData, 15), Is.False);

			Assert.That(source.CheckIfItemExistsInInventory(itemData), Is.False);
			Assert.That(destination.CheckIfItemExistsInInventory(itemData, out int amount), Is.True);
			Assert.That(amount, Is.EqualTo(Inventory.MAX_STACK));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void TransferItemWithNegativeAmount_test() {
			Inventory source = new Inventory(1);
			Inventory destination = new Inventory(1);
			ItemData itemData = GetItemDataFixture();
			Assert.That(InventoryUtils.TransferItem(source, destination, itemData, -1), Is.False);
			Assert.That(source.CheckIfItemExistsInInventory(itemData), Is.False);
			Assert.That(destination.CheckIfItemExistsInInventory(itemData), Is.False);
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetGroupedItems_test() {
			Inventory inventory = new Inventory(5);
			List<int> indexes = new List<int>();
			for (int i = 0; i < 3; i++) {
				inventory.AddItemToInventory(GetItemDataFixture(i), Random.Range(1, Inventory.MAX_STACK), ref indexes);
			}
			Dictionary<string, int> groupedItems = inventory.GetGroupedItems();
			for (int i = 0; i < 3; i++) {
				Assert.That(groupedItems[GetItemDataFixture(i).ID] > 0);
			}
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void GetItemAtIndex_test() {
			Inventory inventory = new Inventory(2);
			Debug.Log(inventory);
			Debug.Log(inventory.Items);

			Assert.That(inventory.GetItemAtIndex(0), Is.EqualTo(null));
			Debug.Log("Here !");
			List<int> indexes = new List<int>();
			Assert.That(inventory.AddItemToInventory(GetItemDataFixture(), 5, ref indexes));
			Debug.Log(indexes);
			Assert.That(indexes[0], Is.EqualTo(0));
			Assert.That(inventory.GetItemAtIndex(0), Is.EqualTo(new Item(GetItemDataFixture(), 5)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddItemsToInventory_test() {
			Inventory inventory = new Inventory(10);

			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(0), 5),
			new Item(GetItemDataFixture(1), 15),
			new Item(GetItemDataFixture(2), 7),
			new Item(GetItemDataFixture(0), 16)
		};

			inventory.AddItemsToInventory(items, out List<Item> remainingItems);
			Assert.That(inventory.CheckIfItemExistsInInventory(GetItemDataFixture(0), out int branch));
			Assert.That(inventory.CheckIfItemExistsInInventory(GetItemDataFixture(1), out int log));
			Assert.That(inventory.CheckIfItemExistsInInventory(GetItemDataFixture(2), out int stone));
			Assert.That(branch, Is.EqualTo(21));
			Assert.That(log, Is.EqualTo(15));
			Assert.That(stone, Is.EqualTo(7));
			Assert.That(remainingItems.Count, Is.EqualTo(0));

			Assert.That(inventory.GetItemAtIndex(0), Is.EqualTo(new Item(GetItemDataFixture(0), 20)));
			Assert.That(inventory.GetItemAtIndex(1), Is.EqualTo(new Item(GetItemDataFixture(1), 15)));
			Assert.That(inventory.GetItemAtIndex(2), Is.EqualTo(new Item(GetItemDataFixture(2), 7)));
			Assert.That(inventory.GetItemAtIndex(3), Is.EqualTo(new Item(GetItemDataFixture(0), 1)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddItemsToInventoryWithFullInventory_test() {
			Inventory inventory = new Inventory(3);
			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(0), 5),
			new Item(GetItemDataFixture(1), 15),
			new Item(GetItemDataFixture(2), 7),
			new Item(GetItemDataFixture(0), 16),
			new Item(GetItemDataFixture(1), 10)
		};
			inventory.AddItemsToInventory(items, out List<Item> remainingItems);
			Assert.That(remainingItems[0], Is.EqualTo(new Item(GetItemDataFixture(0), 1)));
			Assert.That(remainingItems[1], Is.EqualTo(new Item(GetItemDataFixture(1), 5)));
			Assert.That(remainingItems.Count, Is.EqualTo(2));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddAllItemsToInventory_test() {
			Inventory inventory = new Inventory(5);
			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(0), 2 * Inventory.MAX_STACK),
			new Item(GetItemDataFixture(1), Inventory.MAX_STACK),
			new Item(GetItemDataFixture(2), 2* Inventory.MAX_STACK)
		};

			inventory.AddItemsToInventory(items, out List<Item> remainingItemsToBeAdded);
			Assert.That(remainingItemsToBeAdded.Count, Is.EqualTo(0));
			Assert.That(inventory.GetNumberOfAvailableSlotForItem(GetItemDataFixture(0)), Is.EqualTo(0));
			Assert.That(inventory.GetItemAtIndex(0), Is.EqualTo(new Item(GetItemDataFixture(0), Inventory.MAX_STACK)));
			Assert.That(inventory.GetItemAtIndex(1), Is.EqualTo(new Item(GetItemDataFixture(0), Inventory.MAX_STACK)));
			Assert.That(inventory.GetItemAtIndex(2), Is.EqualTo(new Item(GetItemDataFixture(1), Inventory.MAX_STACK)));
			Assert.That(inventory.GetItemAtIndex(3), Is.EqualTo(new Item(GetItemDataFixture(2), Inventory.MAX_STACK)));
			Assert.That(inventory.GetItemAtIndex(4), Is.EqualTo(new Item(GetItemDataFixture(2), Inventory.MAX_STACK)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void RemoveItemsFromInventory_test() {
			Inventory inventory = new Inventory(3);
			List<Item> initialItems = new List<Item> {
			new Item(GetItemDataFixture(0), 5),
			new Item(GetItemDataFixture(1), 15),
			new Item(GetItemDataFixture(2), 7)
		};
			List<Item> itemsToRemove = new List<Item> {
			new Item(GetItemDataFixture(0), 5),
			new Item(GetItemDataFixture(1), 12)
		};

			inventory.AddItemsToInventory(initialItems, out List<Item> remainingItemsAfterAddOp);
			inventory.RemoveItemsFromInventory(itemsToRemove, out List<Item> remainingItemsAfterRemoveOp);

			Assert.That(inventory.CheckIfItemExistsInInventory(GetItemDataFixture(0)), Is.False);
			Assert.That(inventory.CheckIfItemExistsInInventory(GetItemDataFixture(1), out int itemAmount1));
			Assert.That(inventory.CheckIfItemExistsInInventory(GetItemDataFixture(2), out int itemAmount2));
			Assert.That(itemAmount1, Is.EqualTo(3));
			Assert.That(itemAmount2, Is.EqualTo(7));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void RemoveItemsFromInventoryWithNotEnoughItemsInInventory_test() {
			Inventory inventory = new Inventory(3);
			List<Item> itemsToRemove = new List<Item> {
			new Item(GetItemDataFixture(0), 5),
			new Item(GetItemDataFixture(1), 12)
		};

			List<int> indexes1 = new List<int>();
			inventory.AddItemToInventory(GetItemDataFixture(0), 3, ref indexes1);
			Assert.That(indexes1[0], Is.EqualTo(0));
			List<int> indexes2 = new List<int>();
			inventory.AddItemToInventory(GetItemDataFixture(1), 4, ref indexes2);
			Assert.That(indexes2[0], Is.EqualTo(1));

			inventory.RemoveItemsFromInventory(itemsToRemove, out List<Item> remainingItems);
			Assert.That(remainingItems[0], Is.EqualTo(new Item(GetItemDataFixture(0), 2)));
			Assert.That(remainingItems[1], Is.EqualTo(new Item(GetItemDataFixture(1), 8)));
			Assert.That(remainingItems.Count, Is.EqualTo(2));

			Assert.That(inventory.GetItemAtIndex(0), Is.Null);
			Assert.That(inventory.GetItemAtIndex(1), Is.Null);
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void RemoveItemsFromInventoryWithEmptyInventory_test() {
			Inventory inventory = new Inventory(3);
			List<Item> itemsToRemove = new List<Item> {
			new Item(GetItemDataFixture(0), 5),
			new Item(GetItemDataFixture(1), 12)
		};

			inventory.RemoveItemsFromInventory(itemsToRemove, out List<Item> remainingItems);
			Assert.That(remainingItems[0], Is.EqualTo(new Item(GetItemDataFixture(0), 5)));
			Assert.That(remainingItems[1], Is.EqualTo(new Item(GetItemDataFixture(1), 12)));
			Assert.That(remainingItems.Count, Is.EqualTo(2));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void RemoveAllItemsFromInventory_test() {
			Inventory inventory = new Inventory(3);
			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(0), 2 * Inventory.MAX_STACK),
			new Item(GetItemDataFixture(1), Inventory.MAX_STACK)
		};

			inventory.AddItemsToInventory(items, out List<Item> remainingItemsToBeAdded);
			Assert.That(inventory.GetNumberOfAvailableSlotForItem(GetItemDataFixture(0)), Is.EqualTo(0));
			inventory.RemoveItemsFromInventory(items, out List<Item> remainingItemsToBeRemoved);
			Assert.That(inventory.GetNumberOfAvailableSlotForItem(GetItemDataFixture(0)), Is.EqualTo(3 * Inventory.MAX_STACK));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void ItemEquals_test() {
			Item item1 = new Item(GetItemDataFixture(), 5);
			Item item2 = new Item(GetItemDataFixture(), 5);
			Item item3 = new Item(GetItemDataFixture(), 6);

			Assert.That(item1.Equals(item2));
			Assert.That(item1.Equals(item3), Is.False);
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void TransferItems_test() {
			Inventory source = new Inventory(5);
			Inventory destination = new Inventory(10);
			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(0), 2 * Inventory.MAX_STACK),
			new Item(GetItemDataFixture(1), Inventory.MAX_STACK),
			new Item(GetItemDataFixture(2), 2 * Inventory.MAX_STACK)
		};

			source.AddItemsToInventory(items, out List<Item> remainingItemsToBeAdded);
			Assert.That(remainingItemsToBeAdded.Count, Is.EqualTo(0));

			InventoryUtils.TransferItems(source, destination, items, out List<Item> remainingItemsToTransfer);
			Assert.That(remainingItemsToTransfer.Count, Is.EqualTo(0));

			Assert.That(destination.GetNumberOfAvailableSlotForItem(GetItemDataFixture(0)), Is.EqualTo(5 * Inventory.MAX_STACK));
			Assert.That(destination.GetItemAtIndex(0), Is.EqualTo(new Item(GetItemDataFixture(0), Inventory.MAX_STACK)));
			Assert.That(destination.GetItemAtIndex(1), Is.EqualTo(new Item(GetItemDataFixture(0), Inventory.MAX_STACK)));
			Assert.That(destination.GetItemAtIndex(2), Is.EqualTo(new Item(GetItemDataFixture(1), Inventory.MAX_STACK)));
			Assert.That(destination.GetItemAtIndex(3), Is.EqualTo(new Item(GetItemDataFixture(2), Inventory.MAX_STACK)));
			Assert.That(destination.GetItemAtIndex(4), Is.EqualTo(new Item(GetItemDataFixture(2), Inventory.MAX_STACK)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void TransferItemsWithFullDestinationInventory_test() {
			Inventory source = new Inventory(5);
			Inventory destination = new Inventory(4);
			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(0), 2 * Inventory.MAX_STACK),
			new Item(GetItemDataFixture(1), Inventory.MAX_STACK),
			new Item(GetItemDataFixture(2), 2 * Inventory.MAX_STACK)
		};

			source.AddItemsToInventory(items, out List<Item> remainingItemsToBeAdded);
			Assert.That(remainingItemsToBeAdded.Count, Is.EqualTo(0));

			InventoryUtils.TransferItems(source, destination, source.GetItems(), out List<Item> remainingItemsToTransfer);
			Assert.That(remainingItemsToTransfer.Count, Is.EqualTo(1));

			Assert.That(source.GetNumberOfAvailableSlotForItem(GetItemDataFixture(0)), Is.EqualTo(4 * Inventory.MAX_STACK));
			Assert.That(source.GetItemAtIndex(4), Is.EqualTo(new Item(GetItemDataFixture(2), Inventory.MAX_STACK)));

			Assert.That(destination.GetNumberOfAvailableSlotForItem(GetItemDataFixture(0)), Is.EqualTo(0));
			Assert.That(destination.GetItemAtIndex(0), Is.EqualTo(new Item(GetItemDataFixture(0), Inventory.MAX_STACK)));
			Assert.That(destination.GetItemAtIndex(1), Is.EqualTo(new Item(GetItemDataFixture(0), Inventory.MAX_STACK)));
			Assert.That(destination.GetItemAtIndex(2), Is.EqualTo(new Item(GetItemDataFixture(1), Inventory.MAX_STACK)));
			Assert.That(destination.GetItemAtIndex(3), Is.EqualTo(new Item(GetItemDataFixture(2), Inventory.MAX_STACK)));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void TransferItemsWithEmptySourceInventory_test() {
			Inventory source = new Inventory(5);
			Inventory destination = new Inventory(5);

			InventoryUtils.TransferItems(source, destination, source.GetItems(), out List<Item> remainingItemsToTransfer);
			Assert.That(remainingItemsToTransfer.Count, Is.EqualTo(0));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddAllNonStackableItemsToEmptyInventory_test() {
			Inventory inventory = new Inventory(3);

			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.AddItemsToInventory(items, out List<Item> remainingItems);
			Assert.That(remainingItems.Count, Is.EqualTo(0));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddAllNonStackableItemsToPartialInventory_test() {
			Inventory inventory = new Inventory(3);

			List<Item> initial = new List<Item> {
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.AddItemsToInventory(initial, out _);

			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.AddItemsToInventory(items, out List<Item> remainingItems);
			Assert.That(remainingItems.Count, Is.EqualTo(0));
			Assert.That(inventory.Items.Count, Is.EqualTo(3));
			foreach (Item item in inventory.Items) {
				Assert.That(item.Amount, Is.EqualTo(1));
			}
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddAllNonStackableItemsToFullInventory_test() {
			Inventory inventory = new Inventory(3);

			List<Item> initial = new List<Item> {
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.AddItemsToInventory(initial, out _);

			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(2), 15)
		};

			inventory.AddItemsToInventory(items, out List<Item> remainingItems);
			Assert.That(remainingItems.Count, Is.EqualTo(2));
			Assert.That(remainingItems[0].Amount, Is.EqualTo(1));
			Assert.That(remainingItems[1].Amount, Is.EqualTo(15));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddMixedStackableItemsToEmptyInventory() {
			Inventory inventory = new Inventory(3);

			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(1), 15),
			new Item(GetItemDataFixture(2), 18),
			new Item(GetItemDataFixture(2), 7)
		};

			inventory.AddItemsToInventory(items, out List<Item> remainingItems);
			Assert.That(remainingItems.Count, Is.EqualTo(1));
			Assert.That(remainingItems[0].Amount, Is.EqualTo(5));
		}


		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddMixedStackableItemsToPartialInventory() {
			Inventory inventory = new Inventory(3);

			List<Item> initial = new List<Item> {
			new Item(GetItemDataFixture(1), 15),
			new Item(GetItemDataFixture(2), 18),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.AddItemsToInventory(initial, out _);

			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(1), 5),
			new Item(GetItemDataFixture(2), 2),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.AddItemsToInventory(items, out List<Item> remainingItems);
			Assert.That(remainingItems.Count, Is.EqualTo(1));
			Assert.That(remainingItems[0].Amount, Is.EqualTo(1));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void AddMixedStackableItemsToFullInventory() {
			Inventory inventory = new Inventory(3);

			List<Item> initial = new List<Item> {
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(2), 20),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.AddItemsToInventory(initial, out _);

			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(0), 4),
			new Item(GetItemDataFixture(1), 3),
			new Item(GetItemDataFixture(2), 2),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.AddItemsToInventory(items, out List<Item> remainingItems);
			Assert.That(remainingItems.Count, Is.EqualTo(4));
			for (int i = 0; i < remainingItems.Count; i++) {
				Assert.That(remainingItems[i].Amount, Is.EqualTo(remainingItems.Count - i));
			}
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void RemoveStackableItemsFromEmptyInventory() {
			Inventory inventory = new Inventory(3);

			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.RemoveItemsFromInventory(items, out List<Item> remainingItems);
			Assert.That(remainingItems.Count, Is.EqualTo(2));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void RemoveStackableItemsFromPartialInventory() {
			Inventory inventory = new Inventory(3);

			List<Item> initial = new List<Item> {
			new Item(GetItemDataFixture(1), 15),
			new Item(GetItemDataFixture(2), 5),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.AddItemsToInventory(initial, out _);

			List<Item> items = new List<Item> {
			new Item(GetItemDataFixture(2), 4),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.RemoveItemsFromInventory(items, out List<Item> remainingItems);
			Assert.That(remainingItems.Count, Is.EqualTo(0));
			Assert.That(inventory.Items[0].Amount, Is.EqualTo(15));
			Assert.That(inventory.Items[1].Amount, Is.EqualTo(1));
			Assert.That(inventory.Items[2], Is.EqualTo(null));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void RemoveStackableItemsFromFullInventory_test() {
			Inventory inventory = new Inventory(3);

			List<Item> initial = new List<Item> {
			new Item(GetItemDataFixture(1), 20),
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.AddItemsToInventory(initial, out _);

			inventory.RemoveItemsFromInventory(initial, out List<Item> remainingItems);
			Assert.That(remainingItems.Count, Is.EqualTo(0));
			foreach (var item in inventory.Items) {
				Assert.That(item, Is.EqualTo(null));
			}
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void SaveInventory_test() {
			Inventory inventory = new Inventory(3);

			List<Item> initial = new List<Item> {
			new Item(GetItemDataFixture(1), 20),
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(3), 1)
		};

			inventory.AddItemsToInventory(initial, out _);

			InventoryDto inventoryDto = new InventoryDto(inventory);

			Assert.That(inventoryDto.ItemDtos[0].Amount, Is.EqualTo(20));
			Assert.That(inventoryDto.ItemDtos[1].Amount, Is.EqualTo(1));
			Assert.That(inventoryDto.ItemDtos[2].Amount, Is.EqualTo(1));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryTest))]
		public void LoadInventory_test() {
			Inventory before = new Inventory(3);

			List<Item> initial = new List<Item> {
			new Item(GetItemDataFixture(1), 20),
			new Item(GetItemDataFixture(3), 1),
			new Item(GetItemDataFixture(3), 1)
		};

			before.AddItemsToInventory(initial, out _);

			InventoryDto inventoryDto = new InventoryDto(before);

			Inventory after = new Inventory(3);
			after.Initialize(inventoryDto);

			Assert.That(after.Items[0].Amount, Is.EqualTo(20));
			Assert.That(after.Items[1].Amount, Is.EqualTo(1));
			Assert.That(after.Items[2].Amount, Is.EqualTo(1));
		}

		private ItemData GetItemDataFixture() {
			ItemData itemData = ItemManager.Instance.GetItemData("branch");
			Debug.Log(itemData);
			return ItemManager.Instance.GetItemData("branch");
		}

		private ItemData GetNonStackableItemData() {
			ItemData itemData = GetItemDataFixture(3);
			itemData.Stackable = false;
			return itemData;
		}

		private ItemData GetItemDataFixture(int index) {
			switch (index) {
				case 0:
					return ItemManager.Instance.GetItemData("branch");
				case 1:
					return ItemManager.Instance.GetItemData("log");
				case 2:
					return ItemManager.Instance.GetItemData("stone");
				case 3:
					return ItemManager.Instance.GetItemData("leather_boots");
				default:
					return ItemManager.Instance.GetItemData("branch");
			}
		}
	}
}