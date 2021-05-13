using DropSystem;
using ItemSystem;
using NUnit.Framework;

public class DropTest
{
	[Test]
	public void DropTable_test() {
		DropTable dropTable = GetDropTableFixture();
		Assert.That(dropTable.Drops.Length, Is.EqualTo(2));
	}

	[Test]
	public void GetRandomDrop_test() {
		DropTable dropTable = GetDropTableFixture();
		Drop drop = dropTable.GetRandomDrop();
		Assert.That(drop.ItemData, Is.EqualTo(GetItemDataFixture(0)).Or.EqualTo(GetItemDataFixture(1)));
		Assert.That(drop.Amount, Is.GreaterThanOrEqualTo(1));
	}

	private DropTable GetDropTableFixture() {
		return DropTableManager.GetDropTable("Normal Tree");
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
