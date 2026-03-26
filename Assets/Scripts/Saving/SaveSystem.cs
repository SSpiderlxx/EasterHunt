using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveSystem : MonoBehaviour
{
    public static void SaveGame()
    {
        SaveableObject[] objectsToSave = FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);

        SaveData saveData = new SaveData();
        saveData.objects = new SaveableObjectData[objectsToSave.Length];

        for (int i = 0; i < objectsToSave.Length; i++)
        {
            saveData.objects[i] = objectsToSave[i].GetData();
        }

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

            foreach (SaveableObjectData objData in saveData.objects)
            {
                SaveableObject.LoadObject(objData);
            }
        }
    }

    public static void DeleteSave()
    {
        PlayerPrefs.DeleteKey("saveData");
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
}