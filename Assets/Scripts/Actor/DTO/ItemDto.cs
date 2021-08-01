using ItemSystem;
using System;
using UnityEngine;

namespace SaveSystem {
	[Serializable]
	public class ItemDto {

		[SerializeField]
		public string ID;

		[SerializeField]
		public int Amount;

		public ItemDto(Item item) {
			ID = item.ItemData.ID;
			Amount = item.Amount;
		}
	}
}
