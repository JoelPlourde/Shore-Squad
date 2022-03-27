using System;
using System.IO;
using UnityEngine;

namespace SaveSystem {
	public class SaveManager : MonoBehaviour {

		public static SaveManager Instance;

		private void Awake() {
			Instance = this;
		}

		private void Start() {
			NewGame();

			// LoadSaveFile("savefile-1618670670.json");

			// SaveGame();
		}

		public void NewGame() {
			Save = new Save();
			JsonUtility.FromJsonOverwrite(Resources.Load<TextAsset>("Saves/default-save-game").text, Save);
			foreach (ISaveable saveable in GetComponents<ISaveable>()) {
				saveable.Load(Save);
			}
		}

		public void LoadPlayerSaveFile(string filename = "default") {
			Save = new Save();

			string path = Application.persistentDataPath + "/" + filename;
			if (!File.Exists(path)) {
				throw new UnityException("File does not exist :(");
			}

			StreamReader reader = new StreamReader(path);
			JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), Save);
			reader.Close();

			foreach (ISaveable saveable in GetComponents<ISaveable>()) {
				saveable.Load(Save);
			}
		}

		public void SaveGame() {
			Save = new Save();

			foreach (ISaveable saveable in GetComponents<ISaveable>()) {
				saveable.Save(Save);
			}

			string path = Application.persistentDataPath + "/";
			Directory.CreateDirectory(path);

			Debug.Log(path);
			File.WriteAllText(Path.Combine(path, "savefile-" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + ".json"), JsonUtility.ToJson(Save, true));
		}

		public Save Save { get; private set; }
	}
}
