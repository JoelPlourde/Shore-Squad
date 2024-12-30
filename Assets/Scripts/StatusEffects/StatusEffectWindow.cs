using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace StatusEffectSystem {
	public class StatusEffectWindow : EditorWindow {

		[MenuItem("Window/Import/Status Effects")]
		static void CreateMyAsset() {

			Directory.CreateDirectory(Application.dataPath + "/Resources/Sprites");
			Directory.CreateDirectory(Application.dataPath + "/Resources/Scriptable Objects/Status Effects");

			string line;
			string path = EditorUtility.OpenFilePanel("Select File to import", "", "csv");
			if (path.Length != 0) {
				bool firstLine = true;
				StreamReader file = new StreamReader(path);
				while ((line = file.ReadLine()) != null) {

					if (firstLine) {
						firstLine = false;
						continue;
					}

					string[] entry = line.Split(',');

					// NAME, TOOLTIP, DURATION, RESET, TEMPORARY, CAN STACK
					string name = entry[0];
					string tooltip = entry[1];
					Debug.Log(entry[2]);
					int duration = int.Parse(entry[2]);
					bool reset = bool.Parse(entry[3]);
					bool temporary = bool.Parse(entry[4]);
					bool canStack = bool.Parse(entry[5]);
					bool hidden = bool.Parse(entry[6]);
					StatusEffectCategory statusEffectCategory = StatusEffectCategory.NEUTRAL;
					try {
						statusEffectCategory = (StatusEffectCategory)Enum.Parse(typeof(StatusEffectCategory), entry[7], true);
					} catch (ArgumentException) {
						Console.WriteLine("{0} is not a member of the StatusEffectCategory enumeration.", entry[7]);
					}

					string[] types = entry[8].Split('/');
					List<StatusEffectType> statusEffectTypes = new List<StatusEffectType>();
					foreach (string type in types) {
						try {
							statusEffectTypes.Add((StatusEffectType)Enum.Parse(typeof(StatusEffectType), type, true));
						} catch (ArgumentException) {
							Console.WriteLine("{0} is not a member of the StatusEffectType enumeration.", type);

						}
					}

					string slug = name.Replace(' ', '_').ToLower();

					StatusEffectData asset = CreateInstance<StatusEffectData>();
					asset.Name = name;
					asset.name = name;
					asset.Reset = reset;
					asset.Temporary = temporary;
					asset.CanStack = canStack;
					asset.Hidden = hidden;
					asset.StatusEffectCategory = statusEffectCategory;
					asset.statusEffectTypes = statusEffectTypes;

					string spritePath = "Sprites/" + slug;
					asset.Sprite = Resources.Load<Sprite>(spritePath);
					if (asset.Sprite == null) {
						Debug.LogError(spritePath + ".png does not exists! Have you set the Texture Type as Sprite ?");
					}

					AssetDatabase.CreateAsset(asset, "Assets/Resources/Scriptable Objects/Status Effects/" + name + ".asset");
					AssetDatabase.SaveAssets();
					Debug.Log(slug + " has been imported successfully !");
				}
				EditorUtility.FocusProjectWindow();
			}
		}
	}
}
