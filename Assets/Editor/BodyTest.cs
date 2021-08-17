using BodySystem;
using ItemSystem.EquipmentSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnitTest {
	public class BodyTest {

		[Test]
		public void GetMaterial_ForNormalBodyPart_AsMale_test() {
			BodyPartType[] normalBodyPartTypes = new BodyPartType[] { BodyPartType.ARMS, BodyPartType.FEET, BodyPartType.HANDS, BodyPartType.SKIN, BodyPartType.PANTS, BodyPartType.CHEST };
			GetMaterial_test(new Body(), normalBodyPartTypes, Characteristic.SKIN);
		}

		[Test]
		public void GetMaterial_ForNormalBodyPart_AsFemale_test() {
			BodyPartType[] normalBodyPartTypes = new BodyPartType[] { BodyPartType.ARMS, BodyPartType.FEET, BodyPartType.HANDS, BodyPartType.SKIN, BodyPartType.PANTS, BodyPartType.CHEST };
			GetMaterial_test(new Body { SexType = SexType.FEMALE }, normalBodyPartTypes, Characteristic.SKIN);
		}

		[Test]
		public void GetMaterials_ForUnderwear_AsMale_test() {
			GetMaterials_ForX_base(new Body(), BodyPartType.UNDERWEAR, Characteristic.UNDERWEAR);
		}

		[Test]
		public void GetMaterials_ForUnderwear_AsFemale_test() {
			GetMaterials_ForX_base(new Body { SexType = SexType.FEMALE }, BodyPartType.UNDERWEAR, Characteristic.UNDERWEAR);
		}

		[Test]
		public void GetMaterials_ForUndergarment_AsMale_test() {
			GetMaterials_ForX_base(new Body(), BodyPartType.UNDERGARMENT, Characteristic.SKIN);
		}

		[Test]
		public void GetMaterial_ForUndergarment_AsFemale_test() {
			GetMaterials_ForX_base(new Body { SexType = SexType.FEMALE }, BodyPartType.UNDERGARMENT, Characteristic.UNDERWEAR);
		}

		[Test]
		public void GetMaterials_ForBody_AsMale_test() {
			GetMaterials_ForBody_base(new Body(), Characteristic.SKIN);
		}

		[Test]
		public void GetMaterials_ForBody_AsFemale_test() {
			GetMaterials_ForBody_base(new Body { SexType = SexType.FEMALE }, Characteristic.UNDERWEAR);
		}

		[Test]
		public void GetMaterials_ForLegs_AsMale_test() {
			GetMaterials_ForLegs_base(new Body(), Characteristic.UNDERWEAR);
		}

		[Test]
		public void GetMaterials_ForLegs_AsFemale_test() {
			GetMaterials_ForLegs_base(new Body { SexType = SexType.FEMALE }, Characteristic.UNDERWEAR);
		}

		private void GetMaterials_ForX_base(Body body, BodyPartType bodyPartType, Characteristic characteristic) {
			Material material = body.GetMaterial(bodyPartType, false);
			Assert.AreEqual(material.color, MaterialUtils.INVISIBLE.color);

			material = body.GetMaterial(bodyPartType, true);
			Assert.AreEqual(material.color, body.CharacteristicsColor[characteristic]);
		}

		private void GetMaterials_ForBody_base(Body body, Characteristic characteristic) {
			Material[] materials = GetDefaultMaterials();
			materials = body.GetMaterials(materials, SlotType.BODY, false);
			Assert.AreEqual(materials[(int)BodyPartType.CHEST].color, MaterialUtils.INVISIBLE.color);
			Assert.AreEqual(materials[(int)BodyPartType.UNDERGARMENT].color, MaterialUtils.INVISIBLE.color);

			materials = body.GetMaterials(materials, SlotType.BODY, true);
			Assert.AreEqual(materials[(int)BodyPartType.CHEST].color, body.CharacteristicsColor[Characteristic.SKIN]);
			Assert.AreEqual(materials[(int)BodyPartType.UNDERGARMENT].color, body.CharacteristicsColor[characteristic]);
		}

		private void GetMaterials_ForLegs_base(Body body, Characteristic characteristic) {
			Material[] materials = GetDefaultMaterials();
			materials = body.GetMaterials(materials, SlotType.PANTS, false);
			Assert.AreEqual(materials[(int)BodyPartType.PANTS].color, MaterialUtils.INVISIBLE.color);
			Assert.AreEqual(materials[(int)BodyPartType.UNDERWEAR].color, MaterialUtils.INVISIBLE.color);

			materials = body.GetMaterials(materials, SlotType.PANTS, true);
			Assert.AreEqual(materials[(int)BodyPartType.PANTS].color, body.CharacteristicsColor[Characteristic.SKIN]);
			Assert.AreEqual(materials[(int)BodyPartType.UNDERWEAR].color, body.CharacteristicsColor[characteristic]);
		}

		private void GetMaterial_test(Body body, BodyPartType[] bodyPartTypes, Characteristic characteristic) {
			Material material;
			foreach (BodyPartType bodyPartType in bodyPartTypes) {
				material = body.GetMaterial(bodyPartType, false);
				Assert.AreEqual(material.color, MaterialUtils.INVISIBLE.color);

				material = body.GetMaterial(bodyPartType, true);
				Assert.AreEqual(material.color, body.CharacteristicsColor[characteristic]);
			}
		}

		private Material[] GetDefaultMaterials() {
			Material[] materials = new Material[8];
			for (int i = 0; i < materials.Length; i++) {
				materials[i] = MaterialUtils.TOON;
			}
			return materials;
		}
	}
}
