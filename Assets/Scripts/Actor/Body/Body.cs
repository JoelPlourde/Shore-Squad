using ItemSystem.EquipmentSystem;
using SaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BodySystem {

	public enum SexType {
		MALE, FEMALE
	}

	public class Body {

		private SkinnedMeshRenderer _skinnedMeshRenderer;

		/// <summary>
		/// Color of the Characteristics of this body. This dictionary contains the current color for each.
		/// </summary>
		public readonly Dictionary<Characteristic, Color> CharacteristicsColor = new Dictionary<Characteristic, Color>() {
			{ Characteristic.SKIN, Color.HSVToRGB(35, 57, 73) },
			{ Characteristic.UNDERWEAR, Color.white },
			{ Characteristic.HAIR, Color.yellow }
		};

		/// <summary>
		/// Initialize the Body
		/// </summary>
		/// <param name="skinnedMeshRenderer"></param>
		public void Initialize(SkinnedMeshRenderer skinnedMeshRenderer, FeaturesDto featuresDto) {
			_skinnedMeshRenderer = skinnedMeshRenderer;

			if (ColorUtility.TryParseHtmlString(featuresDto.HairColor, out Color hairColor)) {
				ChangeCharacteristicColor(Characteristic.HAIR, hairColor);
			}

			if (ColorUtility.TryParseHtmlString(featuresDto.SkinColor, out Color skinColor)) {
				ChangeCharacteristicColor(Characteristic.SKIN, skinColor);
			}

			if (ColorUtility.TryParseHtmlString(featuresDto.UnderwearColor, out Color underwearColor)) {
				ChangeCharacteristicColor(Characteristic.UNDERWEAR, underwearColor);
			}
		}

		// TODO TEST THIS.
		/// <summary>
		/// Change the sex of this Actor.
		/// </summary>
		/// <param name="sexType">The sex type.</param>
		public void ChangeSex(SexType sexType) {
			SexType = sexType;

			// TODO check if the Actor is wearing anything on its chest first. If not, show = true.
			DisplayBodyParts(SlotType.BODY, true);
		}

		/// <summary>
		/// Change the color of the characteristic.
		/// </summary>
		/// <param name="characteristic">The characteristic to change.</param>
		/// <param name="color">The color.</param>
		public void ChangeCharacteristicColor(Characteristic characteristic, Color color) {
			CharacteristicsColor[characteristic] = color;

			foreach (SlotType slotType in Enum.GetValues(typeof(SlotType))) {
				// TODO check if the actor is wearing anything in this slot.
				DisplayBodyParts(slotType, true);
			}
		}

		/// <summary>
		/// Display or Hide the respective Body Parts based on the Slot Type.
		/// </summary>
		/// <param name="slotType">The Slot Type of the equipment</param>
		/// <param name="show">If true, show. Else hide.</param>
		public void DisplayBodyParts(SlotType slotType, bool show) {
			Material[] materials = _skinnedMeshRenderer.materials;
			_skinnedMeshRenderer.materials = GetMaterials(materials, slotType, show);
		}

		public void DisplayBodyParts(BodyPartType bodyPartType, bool show) {
			Material[] materials = _skinnedMeshRenderer.materials;
			materials[(int)bodyPartType] = GetMaterial(bodyPartType, show);
			_skinnedMeshRenderer.materials = materials;
		}

		/// <summary>
		/// Display or Hide the respective Body Parts based on the Slot Type.
		/// </summary>
		/// <param name="slotType">The Slot Type of the equipment</param>
		/// <param name="show">If true, show. Else hide.</param>
		public Material[] GetMaterials(Material[] materials, SlotType slotType, bool show) {
			if (_slotToBodyPart.TryGetValue(slotType, out BodyPartType bodyPartType)) {
				if (_compositeBodyParts.TryGetValue(bodyPartType, out BodyPartType[] compositeBodyParts)) {
					foreach (BodyPartType compositeBodyPart in compositeBodyParts) {
						materials[(int)compositeBodyPart] = GetMaterial(compositeBodyPart, show);
					}
				} else {
					materials[(int)bodyPartType] = GetMaterial(bodyPartType, show);
				}
			}
			return materials;
		}

		/// <summary>
		/// Get the material specific to the body part depending if its shown or not.
		/// </summary>
		/// <param name="bodyPartType">The body part to get the material for</param>
		/// <param name="show">Whether or not to display it.</param>
		/// <returns>The material to be assigned.</returns>
		public Material GetMaterial(BodyPartType bodyPartType, bool show) {
			if (show) {
				Material material = new Material(MaterialUtils.TOON);
				if (_bodyPartToCharacteristic.TryGetValue(bodyPartType, out Characteristic characteristic)) {

					// TODO check if the wore equipment needs to hide arms or not.

					if (bodyPartType == BodyPartType.UNDERGARMENT && SexType == SexType.MALE) {
						material.color = CharacteristicsColor[Characteristic.SKIN];
					} else {
						material.color = CharacteristicsColor[characteristic];
					}
				} else {
					material.color = CharacteristicsColor[Characteristic.SKIN];
				}
				return material;
			} else {
				return new Material(MaterialUtils.INVISIBLE);
			}
		}

		#region Map
		private static readonly Dictionary<SlotType, BodyPartType> _slotToBodyPart = new Dictionary<SlotType, BodyPartType>() {
			{ SlotType.BODY, BodyPartType.CHEST },
			{ SlotType.PANTS, BodyPartType.PANTS },
			{ SlotType.GLOVES, BodyPartType.HANDS },
			{ SlotType.BOOTS, BodyPartType.FEET }
		};

		private static readonly Dictionary<BodyPartType, BodyPartType[]> _compositeBodyParts = new Dictionary<BodyPartType, BodyPartType[]> {
			{ BodyPartType.PANTS, new BodyPartType[] { BodyPartType.UNDERWEAR, BodyPartType.PANTS } },
			{ BodyPartType.CHEST, new BodyPartType[] { BodyPartType.CHEST, BodyPartType.UNDERGARMENT, BodyPartType.ARMS } }
		};

		private static readonly Dictionary<BodyPartType, Characteristic> _bodyPartToCharacteristic = new Dictionary<BodyPartType, Characteristic>() {
			{ BodyPartType.UNDERWEAR, Characteristic.UNDERWEAR },
			{ BodyPartType.UNDERGARMENT, Characteristic.UNDERWEAR }
		};

		public SexType SexType { get; set; } = SexType.MALE;
		#endregion
	}
}
