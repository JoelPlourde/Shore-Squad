using ItemSystem;
using ItemSystem.EquipmentSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AttachmentTest : IPrebuildSetup {

	public void Setup() {
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		ItemManager itemManager = gameObject.AddComponent<ItemManager>();
	}

	[Test]
	public void Constructor_test() {
		Attachment attachment = new Attachment();
		Assert.That(!attachment.IsAttached);
	}

	[Test]
	[PrebuildSetup(typeof(AttachmentTest))]
	public void Attach_test() {
		Attachment attachment = new Attachment();
		attachment.Attach(GameObject.CreatePrimitive(PrimitiveType.Cube).transform, GetEquipmentFixture());
		Assert.That(attachment.IsAttached);
	}

	[Test]
	[PrebuildSetup(typeof(AttachmentTest))]
	public void Detach_test() {
		Attachment attachment = new Attachment();
		attachment.Attach(GameObject.CreatePrimitive(PrimitiveType.Cube).transform, GetEquipmentFixture());
		Equipment equipment = attachment.Detach();
		Assert.That(equipment != null);
		Assert.That(equipment.Amount == 1);
		Assert.That(equipment.EquipmentData.ID == "leather_boots");
		Assert.That(!attachment.IsAttached);
	}

	private Equipment GetEquipmentFixture() {
		return new Equipment(ItemManager.Instance.GetEquipmentData("leather_boots"), 1);
	}
}
