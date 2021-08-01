using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ItemSystem {
	public class InventoryWindow : EditorWindow {

		[MenuItem("Window/Import/Inventory")]
		static void CreateMyAsset() {

			Directory.CreateDirectory(Application.dataPath + "/Resources/Sprites");
			Directory.CreateDirectory(Application.dataPath + "/Resources/GameObjects");
			Directory.CreateDirectory(Application.dataPath + "/Resources/Scriptable Objects/Items");

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

					bool valid = true;
					string[] entry = line.Split(',');

					string name = entry[0];
					string tooltip = entry[1];
					string slug = name.Replace(' ', '_').ToLower();

					ItemData asset = CreateInstance<ItemData>();
					asset.name = name;
					asset.Tooltip = tooltip;

					string spritePath = "Sprites/" + slug;
					asset.Sprite = Resources.Load<Sprite>(spritePath);
					if (asset.Sprite == null) {
						valid = false;
						Debug.LogError(spritePath + ".png does not exists! Have you set the Texture Type as Sprite ?");
					}

					string prefabPath = "GameObjects/" + slug;
					asset.Prefab = Resources.Load<GameObject>(prefabPath);
					if (asset.Prefab == null) {
						valid = false;
						Debug.LogError(prefabPath + ".prefab does not exists!");
					}

					if (valid == true) {
						AssetDatabase.CreateAsset(asset, "Assets/Resources/Scriptable Objects/Items/" + name + ".asset");
						AssetDatabase.SaveAssets();
						Debug.Log(slug + " has been imported successfully !");
					}
				}
				EditorUtility.FocusProjectWindow();
			}
		}
	}
}

