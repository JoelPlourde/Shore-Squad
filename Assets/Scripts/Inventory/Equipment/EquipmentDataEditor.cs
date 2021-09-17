using System;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace ItemSystem {
	namespace EquipmentSystem {
		[CustomEditor(typeof(EquipmentData))]
		public class EquipmentDataEditor : Editor {

			public override void OnInspectorGUI() {
				base.OnInspectorGUI();

				EquipmentData equipmentData = (EquipmentData)target;

				equipmentData.ItemType = ItemType.EQUIPMENT;

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Conditional Properties", EditorStyles.boldLabel);

				using (new EditorGUI.DisabledScope(equipmentData.SlotType != SlotType.BODY)) {
					equipmentData.HideArms = EditorGUILayout.Toggle("Hide Arms", equipmentData.HideArms);
				}

				using (new EditorGUI.DisabledScope(equipmentData.SlotType != SlotType.HEAD)) {
					equipmentData.HideHair = EditorGUILayout.Toggle("Hide Hair", equipmentData.HideHair);
				}

				if (equipmentData.SlotType == SlotType.WEAPON || equipmentData.SlotType == SlotType.SHIELD) {
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("Weapon-Specific Properties", EditorStyles.boldLabel);
					equipmentData.WeaponType = (WeaponType)EditorGUILayout.EnumPopup("Weapon Type", equipmentData.WeaponType);
				} else {
					equipmentData.WeaponType = WeaponType.NONE;
				}

				if (equipmentData.SlotType != SlotType.BODY) {
					equipmentData.HideArms = false;
				}

				if (equipmentData.SlotType != SlotType.HEAD) {
					equipmentData.HideHair = false;
				}

				if (equipmentData.Stackable) {
					EditorGUILayout.HelpBox("This equipment is set as Stackable, is this ok ?", MessageType.Warning);
				}

				if (equipmentData.EquipmentStats.Statistics.Count == 0) {
					equipmentData.EquipmentStats.Statistics.Add(new Statistic {
						StatisticType = StatisticType.ARMOR,
						Value = 1
					});
				}

				if (equipmentData.ItemEffects.Count == 0) {
					equipmentData.ItemEffects.Add(new EffectSystem.ItemEffect {
						ItemEffectType = EffectSystem.ItemEffectType.EQUIP,
						Magnitude = 0
					});
				}

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
				if (GUILayout.Button("Generate Tooltip")) {
					equipmentData.Tooltip = "";

					if (equipmentData.ItemEffects.Count > 0) {
						equipmentData.Tooltip = string.Format("<color=#BFBFBF><size=20>{0}</size></color> ", EnumExtensions.FormatItemEffectType(equipmentData.ItemEffects[0].ItemEffectType));
					}

					equipmentData.Tooltip += string.Format("<size=22>{0}</size>\n<size=16>{1}</size>\n", equipmentData.name, FormatEquipmentType(equipmentData));

					int count = equipmentData.EquipmentStats.Statistics.Count;
					foreach (Statistic statistic in equipmentData.EquipmentStats.Statistics) {
						equipmentData.Tooltip += string.Format("<size=16><color=#1DFF00>+{0} {1}</color></size>", statistic.Value, FormatEnum(statistic.StatisticType.ToString()));
						count--;
						if (count > 0) {
							equipmentData.Tooltip += "\n";
						}
					}
				}

				if (GUILayout.Button("Save")) {
					EditorUtility.SetDirty(equipmentData);
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}

			private string FormatEnum(string input) {
				return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.Replace("_", " ").ToLower());
			}

			private string FormatEquipmentType(EquipmentData equipmentData) {
				switch (equipmentData.SlotType) {
					case SlotType.HEAD:
						return "Headpiece";
					case SlotType.BODY:
						return "Chestpiece";
					case SlotType.PANTS:
						return "Leggings";
					case SlotType.GLOVES:
						return "Gloves";
					case SlotType.BOOTS:
						return "Boots";
					case SlotType.WEAPON:
						switch (equipmentData.WeaponType) {
							case WeaponType.SINGLE_HANDED:
								return "Single-handed";
							case WeaponType.TWO_HANDED:
								return "Two-handed";
							default:
								return "Tool";
						}
					case SlotType.SHIELD:
						if (equipmentData.WeaponType != WeaponType.NONE) {
							return "Off-hand";
						}
						return "Shield";
					case SlotType.RING:
						return "Ring";
					case SlotType.NECK:
						return "Necklace";
					default:
						return "Equipment";
				}
			}
		}
	}
}
