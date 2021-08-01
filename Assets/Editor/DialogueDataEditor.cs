using System.IO;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem {
	[CustomEditor(typeof(DialogueData))]
	public class DialogueDataEditor : Editor {

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			DialogueData dialogueData = (DialogueData)target;

			if (GUI.changed) {

				ValidateNode(dialogueData.Entry);

			}

			if (GUILayout.Button("Export")) {
				if (dialogueData.Identifier.Length == 0 || dialogueData.Identifier == null) {
					throw new UnityException("Please write an identifier for this dialogue.");
				}


				string filename = dialogueData.Identifier.Replace(" ", "_").Replace("!", "").ToLower();

				string assetPath = AssetDatabase.GetAssetPath(dialogueData.GetInstanceID());
				AssetDatabase.RenameAsset(assetPath, filename);
				AssetDatabase.SaveAssets();

				Directory.CreateDirectory(Application.dataPath + "/Dialogues");
				File.WriteAllText(Application.dataPath + "/Dialogues/" + dialogueData.Identifier.Replace(" ", "_").ToLower() + ".json", JsonUtility.ToJson(dialogueData, true));
			}

			if (GUILayout.Button("Import")) {
				StreamReader reader = new StreamReader(EditorUtility.OpenFilePanel("Select File to import", "", "json"));
				JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), dialogueData);
				reader.Close();
			}
		}

		private void ValidateNode(Node node) {

			if (node.Nodes.Length > 1) {
				node.NodeType = NodeType.CHOICE;
			}

			if (node.Nodes.Length == 0 && node.NodeType != NodeType.ACTION) {
				node.NodeType = NodeType.QUIT;
			}

			foreach (Node childrenNode in node.Nodes) {
				ValidateNode(childrenNode);
			}
		}
	}
}