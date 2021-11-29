using DropSystem;
using ItemSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnitTest {
	public class DropTest : IPrebuildSetup {

		public void Setup() {
			GameObject gameobject = new GameObject();
			gameobject.AddComponent<ItemManager>();
		}

		[Test]
		[PrebuildSetup(typeof(DropTest))]
		public void DropTable_test() {
			DropTable dropTable = GetDropTableFixture();
			Assert.That(dropTable.Drops.Length, Is.EqualTo(2));
		}

		[Test]
		[PrebuildSetup(typeof(DropTest))]
		public void GetRandomDrop_test() {
			DropTable dropTable = GetDropTableFixture();
			Drop drop = dropTable.GetRandomDrop();
			Assert.That(drop.ItemData, Is.EqualTo(GetItemDataFixture(0)).Or.EqualTo(GetItemDataFixture(1)));
			Assert.That(drop.Amount, Is.GreaterThanOrEqualTo(1));
		}

		private DropTable GetDropTableFixture() {
			Drop[] drops = new Drop[2];
			drops[0] = new Drop {
				ItemData = GetItemDataFixture(0),
				Weight = 10,
				Quantity = new Quantity {
					Min = 1,
					Max = 2
				}
			};
			drops[1] = new Drop {
				ItemData = GetItemDataFixture(1),
				Weight = 90,
				Quantity = new Quantity {
					Min = 1,
					Max = 2
				}
			};
			return new DropTable {
				Drops = drops
			};
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
