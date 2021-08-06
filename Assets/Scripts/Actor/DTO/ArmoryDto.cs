using ItemSystem.EquipmentSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem {
	[Serializable]
	public class ArmoryDto {

		[SerializeField]
		public ItemDto[] EquipmentDtos;

		public ArmoryDto(Armory armory) {
			EquipmentDtos = new ItemDto[Enum.GetValues(typeof(SlotType)).Length];
			foreach (KeyValuePair<SlotType, Attachment> pair in armory.Equipments) {
				EquipmentDtos[(int)pair.Key] = (pair.Value.IsAttached) ? new ItemDto(pair.Value.Equipment.EquipmentData.ID, pair.Value.Equipment.Amount) : new ItemDto();
			}
		}
	}
}
