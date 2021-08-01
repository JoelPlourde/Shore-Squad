using System.IO;
using UnityEditor;
using UnityEngine;

namespace QuestSystem {
	[CustomEditor(typeof(QuestData))]
	public class QuestDataEditor : Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			QuestData questData = (QuestData)target;

			if (GUILayout.Button("Export")) {
				if (questData.Title.Length == 0 || questData.Title == null) {
					throw new UnityException("Please write a title for this quest.");
				}

				string filename = questData.Title.Replace(" ", "_").Replace("!", "").ToLower();

				string assetPath = AssetDatabase.GetAssetPath(questData.GetInstanceID());
				AssetDatabase.RenameAsset(assetPath, filename);
				AssetDatabase.SaveAssets();

				Directory.CreateDirectory(Application.dataPath + "/Quests");
				File.WriteAllText(Application.dataPath + "/Quests/" + filename + ".json", JsonUtility.ToJson(questData, true));
			}

			if (GUILayout.Button("Import")) {
				StreamReader reader = new StreamReader(EditorUtility.OpenFilePanel("Select File to import", "", "json"));
				JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), questData);
				reader.Close();
			}
		}
	}
}
