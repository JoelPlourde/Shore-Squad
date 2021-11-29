using ItemSystem.EffectSystem;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace ItemSystem {
	[CustomEditor(typeof(ItemData))]
	public class ItemDataEditor : Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			ItemData itemData = (ItemData) target;

			if (itemData.Burnable) {
				itemData.Power = EditorGUILayout.IntField("Power", itemData.Power);
			}

			if (itemData.ItemType == ItemType.CONSUMABLE) {
				if (itemData.ItemEffects.Count == 0) {
					itemData.ItemEffects.Add(new ItemEffect {
						ItemEffectType = ItemEffectType.EAT,
						Magnitude = 1
					});
				}
			}

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
			if (GUILayout.Button("Generate Tooltip")) {
				itemData.Tooltip = "";
				if (itemData.ItemEffects.Count > 0) {
					itemData.Tooltip = string.Format("<color=#BFBFBF><size=20>{0}</size></color> ", EnumExtensions.FormatItemEffectType(itemData.ItemEffects[0].ItemEffectType));
				}

				itemData.Tooltip += string.Format("<size=22>{0}</size>\n<size=16>{1}</size>", itemData.name, FormatItemType(itemData.ItemType));

				int count = itemData.ItemEffects.Count;
				if (count > 0) {
					itemData.Tooltip += "\n";
				}

				foreach (var itemEffect in itemData.ItemEffects) {
					itemData.Tooltip += string.Format("<size=16><color=#1DFF00>{0}</color></size>", FormatItemEffect(itemEffect));
					count--;
					if (count > 0) {
						itemData.Tooltip += "\n";
					}
				}
			}

			if (GUILayout.Button("Save")) {
				EditorUtility.SetDirty(itemData);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		private string FormatItemType(ItemType itemType) {
			switch (itemType) {
				case ItemType.EQUIPMENT:
					return "Equipment";
				case ItemType.CONSUMABLE:
					return "Consumable";
				default:
					return "Resource";
			}
		}

		private string FormatItemEffect(ItemEffect itemEffect) {
			switch (itemEffect.ItemEffectType) {
				case ItemEffectType.EQUIP:
					return "Equipment";
				case ItemEffectType.EAT:
					return "+" + Mathf.RoundToInt(itemEffect.Magnitude * 4) + " Nutrition";
				default:
					return "???";
			}
		}
	}
}
