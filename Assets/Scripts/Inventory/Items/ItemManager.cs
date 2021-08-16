using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using ItemSystem.EquipmentSystem;

namespace ItemSystem {
	[ExecuteInEditMode]
	public class ItemManager : MonoBehaviour {

		public static ItemManager Instance;

		private Dictionary<string, ItemData> _itemDatas;

		private void Awake() {
			Instance = this;

			ItemData[] itemDatas = Resources.LoadAll<ItemData>("Scriptable Objects/Items");
			Array.ForEach(itemDatas, itemData => {
				itemData.ID = itemData.name.ToLower().Replace(" ", "_");
			});
			_itemDatas = itemDatas.ToDictionary(x => x.ID);
		}

		public ItemData GetItemData(string name) {
			if (_itemDatas.TryGetValue(name.ToLower().Replace(" ", "_"), out ItemData itemData)) {
				return itemData;
			} else {
				throw new UnityException("The Item couldn't be found by its name. Please define this Item Data: " + name);
			}
		}

		public EquipmentData GetEquipmentData(string name) {
			return (GetItemData(name) as EquipmentData);
		}
	}
}
