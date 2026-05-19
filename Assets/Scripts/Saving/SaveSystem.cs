using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveSystem : MonoBehaviour
{
    private static readonly HashSet<string> destroyedObjectIds = new HashSet<string>();

    public static void SaveGame()
    {
        SaveableObject[] objectsToSave = FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);
        PlayerController player = FindFirstObjectByType<PlayerController>();
        List<SaveableObjectData> savedObjects = new List<SaveableObjectData>();

        SaveData saveData = new SaveData();
        saveData.destroyedObjectIds = GetDestroyedObjectIds();
        saveData.playerPosition = player != null ? player.transform.position : Vector3.zero;

        for (int i = 0; i < objectsToSave.Length; i++)
        {
            SaveableObject saveableObject = objectsToSave[i];
            if (saveableObject == null || destroyedObjectIds.Contains(saveableObject.ObjectId))
            {
                continue;
            }

            savedObjects.Add(saveableObject.GetData());
        }

        saveData.objects = savedObjects.ToArray();

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("saveData", json);
        PlayerPrefs.Save();
    }

    public static void LoadGame()
    {
        if (PlayerPrefs.HasKey("saveData"))
        {
            string json = PlayerPrefs.GetString("saveData");
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            SetDestroyedObjectIds(saveData.destroyedObjectIds);

            SaveableObject[] liveObjects = FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);
            foreach (SaveableObject liveObject in liveObjects)
            {
                if (liveObject != null && destroyedObjectIds.Contains(liveObject.ObjectId))
                {
                    Destroy(liveObject.gameObject);
                }
            }

            if (saveData.objects != null)
            {
                foreach (SaveableObjectData objData in saveData.objects)
                {
                    SaveableObject.LoadObject(objData);
                }
            }

            PlayerController player = FindFirstObjectByType<PlayerController>();
            if (player != null)
            {
                player.transform.position = saveData.playerPosition;
            }
        }
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey("saveData");
        destroyedObjectIds.Clear();
    }

    public static void MarkDestroyed(string objectId)
    {
        if (string.IsNullOrEmpty(objectId))
        {
            return;
        }

        if (destroyedObjectIds.Add(objectId))
        {
            PersistDestroyedObjectIds();
        }
    }

    public static bool IsDestroyed(string objectId)
    {
        return !string.IsNullOrEmpty(objectId) && destroyedObjectIds.Contains(objectId);
    }

    private static void SetDestroyedObjectIds(string[] objectIds)
    {
        destroyedObjectIds.Clear();

        if (objectIds == null)
        {
            return;
        }

        for (int i = 0; i < objectIds.Length; i++)
        {
            if (!string.IsNullOrEmpty(objectIds[i]))
            {
                destroyedObjectIds.Add(objectIds[i]);
            }
        }
    }

    private static string[] GetDestroyedObjectIds()
    {
        string[] objectIds = new string[destroyedObjectIds.Count];
        destroyedObjectIds.CopyTo(objectIds);
        return objectIds;
    }

    private static void PersistDestroyedObjectIds()
    {
        if (!PlayerPrefs.HasKey("saveData"))
        {
            return;
        }

        SaveData saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString("saveData"));
        if (saveData == null)
        {
            saveData = new SaveData();
        }

        saveData.destroyedObjectIds = GetDestroyedObjectIds();

        PlayerPrefs.SetString("saveData", JsonUtility.ToJson(saveData));
        PlayerPrefs.Save();
    }
}

// Editor Buttons for Debugging
#if UNITY_EDITOR
[CustomEditor(typeof(SaveSystem))]
public class SaveSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        SaveSystem saveSystem = (SaveSystem)target;
        if (GUILayout.Button("Save Game"))
        {
            SaveSystem.SaveGame();
        }
        if (GUILayout.Button("Load Game"))
        {
            SaveSystem.LoadGame();
        }
        if (GUILayout.Button("Delete Save"))
        {
            SaveSystem.DeleteSave();
        }
    }
}
#endif

[System.Serializable]
public class SaveData
{
    public SaveableObjectData[] objects;
    public string[] destroyedObjectIds;
    public Vector3 playerPosition;
}