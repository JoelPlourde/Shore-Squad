using SaveSystem;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This class is because Unity can't get their shit together and generate a unique identifier between Editor and Runtime.
/// 
/// Therefore, whenever we copy & paste a prefab, the UUID on an InteractableItem is the same. Therefore, this class is responsible
/// to regenerate the UUIDs.
/// </summary>
[InitializeOnLoad]
public class GameObjectChangeEvents
{
    static GameObjectChangeEvents()
    {
        ObjectChangeEvents.changesPublished += ChangesPublished;
    }

    static void ChangesPublished(ref ObjectChangeEventStream stream)
    {
        for (int i = 0; i < stream.length; ++i)
        {
            var type = stream.GetEventType(i);
            switch (type)
            {
                case ObjectChangeKind.CreateGameObjectHierarchy:
                    stream.GetCreateGameObjectHierarchyEvent(i, out var createGameObjectHierarchyEvent);
                    var newGameObject = EditorUtility.InstanceIDToObject(createGameObjectHierarchyEvent.instanceId) as GameObject;

                    IWorldSaveable worldSaveable = newGameObject.GetComponent<IWorldSaveable>();
                    if (!ReferenceEquals(worldSaveable, null)) {
                    Debug.Log($"{type}: {newGameObject} in scene {createGameObjectHierarchyEvent.scene}, must regenerate the UUID.");
                        worldSaveable.RegenerateUUID();
                        return;
                    }
                    
                    break;
            }
        }
    }
}