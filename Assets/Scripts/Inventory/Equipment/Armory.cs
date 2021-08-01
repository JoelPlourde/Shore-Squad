using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem {
	namespace EquipmentSystem {
		public class Armory : MonoBehaviour {

			private readonly Dictionary<SlotType, Attachment> _equipments = new Dictionary<SlotType, Attachment>() {
				{ SlotType.HEAD,    new Attachment() },
				{ SlotType.BODY,    new Attachment() },
				{ SlotType.PANTS,   new Attachment() },
				{ SlotType.GLOVES,  new Attachment() },
				{ SlotType.BOOTS,   new Attachment() },
				{ SlotType.WEAPON,  new Attachment() },
				{ SlotType.SHIELD,  new Attachment() },
				{ SlotType.RING,    new Attachment() },
				{ SlotType.NECK,    new Attachment() }
			};

			public void Initialize(Actor actor) {
				Actor = actor;
			}

			/// <summary>
			/// Equip the equipment in the slot.
			/// </summary>
			/// <param name="slotType">The slot of the equipment</param>
			/// <param name="equipment">The equipment to be equipped.</param>
			/// <param name="previousEquipment">The previous equipment that was equipped.</param>
			/// <returns>Return true if the equipment could be equipped properly. Exception if any error occured.</returns>
			public bool Equip(Equipment equipment, out Equipment previousEquipment) {
				if (_equipments.TryGetValue(equipment.EquipmentData.SlotType, out Attachment attachment)) {
					previousEquipment = null;

					if (attachment.IsAttached && !Unequip(attachment, out previousEquipment)) {
						throw new UnityException("The Unequip method returned false, there is a problem, please verify.");
					}

					attachment.Attach(transform, equipment);

					if (equipment.EquipmentData.HideBodyPart) {
						Actor.Body.DisplayBodyParts(equipment.EquipmentData.SlotType, false);
					}

					Actor.Statistics.UpdateStatistics(equipment.EquipmentData.EquipmentStats.Statistics, true);

					return true;
				} else {
					throw new UnityException("The slot is not defined in this Equipment, please verify.");
				}
			}

			/// <summary>
			/// Unequip the Equipment from the slot.
			/// </summary>
			/// <param name="slotType">The slot type</param>
			/// <param name="equipment">The equipment that will be unequipped.</param>
			/// <returns>Return true if there is an equipment attached. Else false.</returns>
			public bool Unequip(SlotType slotType, out Equipment equipment) {
				if (_equipments.TryGetValue(slotType, out Attachment attachment)) {
					return Unequip(attachment, out equipment);
				} else {
					throw new UnityException("The slot is not defined in this Equipment, please verify.");
				}
			}

			/// <summary>
			/// Unequip method that detach the equipment from the body.
			/// </summary>
			/// <param name="attachment">The attachment</param>
			/// <param name="equipment">The equipment that is attached, if successful.</param>
			/// <returns>Return true, if there is an equipment attached.</returns>
			private bool Unequip(Attachment attachment, out Equipment equipment) {
				if (!attachment.IsAttached) {
					equipment = null;
					return false;
				}

				equipment = attachment.Detach();

				if (!ReferenceEquals(equipment, null) && equipment.EquipmentData.HideBodyPart) {
					Actor.Body.DisplayBodyParts(equipment.EquipmentData.SlotType, true);
				}

				Actor.Statistics.UpdateStatistics(equipment.EquipmentData.EquipmentStats.Statistics, false);

				return true;
			}

			public Actor Actor { get; private set; }
		}
	}
}
