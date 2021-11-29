using NUnit.Framework;
using ItemSystem;
using UnityEngine.TestTools;
using System.Collections.Generic;
using UnityEngine;

namespace UnitTest {
	public class InventoryUtilsTest : IPrebuildSetup {

		public void Setup() {
			ItemManager itemManager = new GameObject().AddComponent<ItemManager>();
		}

		[Test]
		[PrebuildSetup(typeof(InventoryUtilsTest))]
		public void GroupItemsByID_test() {
			Item[] items = new Item[] {
				new Item(GetItemDataFixture(0), 4),
				new Item(GetItemDataFixture(1), 2)
			};

			Dictionary<string, int> groupedItems = InventoryUtils.GroupItemsByID(items);

			Assert.That(groupedItems["branch"], Is.EqualTo(4));
			Assert.That(groupedItems["log"], Is.EqualTo(2));
		}

		[Test]
		[PrebuildSetup(typeof(InventoryUtilsTest))]
		public void GroupItemsByID_combine_test() {
			Item[] items = new Item[] {
				new Item(GetItemDataFixture(0), 4),
				new Item(GetItemDataFixture(0), 2)
			};

			Dictionary<string, int> groupedItems = InventoryUtils.GroupItemsByID(items);

			Assert.That(groupedItems["branch"], Is.EqualTo(6));
		}

		private ItemData GetItemDataFixture(int index) {
			switch (index) {
				case 0:
					return ItemManager.Instance.GetItemData("branch");
				case 1:
					return ItemManager.Instance.GetItemData("log");
				case 2:
					return ItemManager.Instance.GetItemData("stone");
				default:
					return ItemManager.Instance.GetItemData("branch");
			}
		}
	}
}
