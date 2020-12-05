using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ItemSystem;
using System;

public class ItemManager : MonoBehaviour
{
	private static Dictionary<string, ItemData> _itemDatas;

	static ItemManager() {
		ItemData[] itemDatas = Resources.LoadAll<ItemData>("Scriptable Objects/Items");
		Array.ForEach(itemDatas, itemData => {
			itemData.ID = itemData.name.ToLower().Replace(" ", "_");
		});
		_itemDatas = itemDatas.ToDictionary(x => x.ID);
	}

	public static ItemData GetItemData(string name) {
		if (_itemDatas.TryGetValue(name.ToLower().Replace(" ", "_"), out ItemData itemData)) {
			return itemData;
		} else {
			throw new UnityException("The Item couldn't be found by its name. Please define this Item Data: " + name);
		}
	}
}
