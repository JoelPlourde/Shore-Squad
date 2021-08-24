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
				itemData.Tooltip = string.Format("<size=24>{0}</size>\n<size=18>{1}</size>\n", itemData.name, FormatItemType(itemData.ItemType));

				int count = itemData.ItemEffects.Count;
				foreach (var itemEffect in itemData.ItemEffects) {
					itemData.Tooltip += string.Format("<size=18><color=#1DFF00>{0}</color></size>", FormatItemEffect(itemEffect));
					count--;
					if (count > 0) {
						itemData.Tooltip += "\n";
					}
				}
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
