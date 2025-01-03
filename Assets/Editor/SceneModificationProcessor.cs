using System;
using System.Collections.Generic;
using SaveSystem;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This class is responsible to save the scene in a World file to determine what objects have been interacted with
/// and what to save as a result.
/// </summary>
public class SceneModificationProcessor : AssetModificationProcessor {

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

        HashSet<string> uniqueIds = new HashSet<string>();

        // In each gameobject, find all components that implement ISaveable.
        foreach (GameObject gameObject in gameObjects) {
            ISaveable[] saveables = gameObject.GetComponents<ISaveable>();

            if (saveables.Length == 0) {
                continue;
            }

            foreach (ISaveable saveable in saveables) {
                // re-think this solution.
                if (saveable is IWorldSaveable worldSaveable) {
                    if (uniqueIds.Contains(worldSaveable.GetUUID())) {
                        Debug.LogError("Duplicate UUID found: " + worldSaveable.GetUUID());
                    }
                    uniqueIds.Add(worldSaveable.GetUUID());
                }
                saveable.Save(save);
            }
        }

        // The filename must be unique for each scene, there will only be one.
		string filename = "world-" + sceneName + ".json";

        SaveManager.SaveGameFile(save, filename);
    }
}