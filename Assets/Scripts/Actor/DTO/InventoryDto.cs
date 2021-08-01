using ItemSystem;
using System;
using UnityEngine;

namespace SaveSystem {
	[Serializable]
	public class InventoryDto {

		[SerializeField]
		public ItemDto[] ItemDtos;

		public InventoryDto(Inventory inventory) {
			ItemDtos = new ItemDto[Inventory.MAX_STACK];

			for (int i = 0; i < inventory.Items.Length; i++) {
				if (!ReferenceEquals(inventory.Items[i], null)) {
					ItemDtos[i] = new ItemDto(inventory.Items[i]);
				}
			}
		}
	}
}