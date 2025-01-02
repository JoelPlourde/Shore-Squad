using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem {
	public class SaveManager : MonoBehaviour {

		public static SaveManager Instance;

		private void Awake() {
			Instance = this;
		}

		private void Start() {
			if (!LoadRecentSaveFile()) {
				NewGame();
			}
		}

		public bool LoadRecentSaveFile() {
			string path = Application.persistentDataPath;
			DirectoryInfo directory = new DirectoryInfo(path);
			FileInfo[] files = directory.GetFiles("*.json");

			if (files.Length == 0) {
				return false;
			}

			string sceneName = SceneManager.GetActiveScene().name;

			FileInfo mostRecentFile = files[0];
			foreach (FileInfo file in files) {
				if (!file.Name.Contains(sceneName)) {
					continue;
				}

				if (file.LastWriteTime > mostRecentFile.LastWriteTime) {
					mostRecentFile = file;
				}
			}

			return LoadPlayerSaveFile(mostRecentFile.Name);
		}

		public void NewGame() {
			Save = new Save();
			JsonUtility.FromJsonOverwrite(Resources.Load<TextAsset>("Saves/default-save-game").text, Save);
			foreach (ISaveable saveable in GetComponents<ISaveable>()) {
				saveable.Load(Save);
			}
		}

		public bool LoadPlayerSaveFile(string filename = "default") {
			Save = new Save();

			string path = Application.persistentDataPath + "/" + filename;
			if (!File.Exists(path)) {
				return false;
			}

			StreamReader reader = new StreamReader(path);
			JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), Save);
			reader.Close();

			foreach (ISaveable saveable in GetComponents<ISaveable>()) {
				saveable.Load(Save);
			}
			return true;
		}

		public void SaveGame() {
			Save = new Save();

			foreach (ISaveable saveable in GetComponents<ISaveable>()) {
				saveable.Save(Save);
			}

			string path = Application.persistentDataPath + "/";
			Directory.CreateDirectory(path);

			string sceneName = SceneManager.GetActiveScene().name;
			string filename = sceneName + "-" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + ".json";
			string absolutePath = Path.Combine(path, filename);
			string fileContents = JsonUtility.ToJson(Save, true);

			File.WriteAllText(absolutePath, fileContents);

			Debug.Log("Game saved to: " + absolutePath);
		}

		public Save Save { get; private set; }
	}
}
