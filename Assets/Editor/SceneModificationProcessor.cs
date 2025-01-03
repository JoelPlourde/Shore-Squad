using System;
using SaveSystem;
using UnityEditor;
using UnityEngine;

public class SceneLifecycle : AssetModificationProcessor {

    public static string[] OnWillSaveAssets(string[] paths) {
        foreach (string path in paths) {
            if (path.Contains(".unity")) {
                Debug.Log("Scene saved: " + path);

                // Keep the last part of the string after the last / character
                string sceneName = path.Substring(path.LastIndexOf("/") + 1).Replace(".unity", "");

                OnSavedScene(sceneName);
            }
        }
        return paths;
    }

    public static void OnSavedScene(string sceneName) {
        // Load all gameobjects within the scene.
        GameObject[] gameObjects = UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        Debug.Log("Found: " + gameObjects.Length + " gameobjects in " + sceneName);

        Save save = new Save();

        // In each gameobject, find all components that implement ISaveable.
        foreach (GameObject gameObject in gameObjects) {
            ISaveable[] saveables = gameObject.GetComponents<ISaveable>();

            if (saveables.Length == 0) {
                continue;
            }

            foreach (ISaveable saveable in saveables) {
                saveable.Save(save);
            }
        }

        // The filename must be unique for each scene, there will only be one.
		string filename = "world-" + sceneName + ".json";

        SaveManager.SaveGameFile(save, filename);
    }
}