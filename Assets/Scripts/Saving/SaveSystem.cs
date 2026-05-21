using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveSystem : MonoBehaviour
{
    private static readonly HashSet<string> destroyedObjectIds = new HashSet<string>();
    private static string pendingLoadJson;
    private static string pendingSceneName;
    private static bool sceneLoadedHooked;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        pendingLoadJson = null;
        pendingSceneName = null;
        sceneLoadedHooked = false;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void RegisterSceneLoadedHandler()
    {
        if (sceneLoadedHooked)
        {
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        sceneLoadedHooked = true;
    }

    public static void SaveGame()
    {
        SaveableObject[] objectsToSave = FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);
        PlayerController player = FindFirstObjectByType<PlayerController>();
        List<SaveableObjectData> savedObjects = new List<SaveableObjectData>();

        SaveData saveData = new SaveData();
        saveData.destroyedObjectIds = GetDestroyedObjectIds();
        saveData.playerPosition = player != null ? player.transform.position : Vector3.zero;
        saveData.sceneName = SceneManager.GetActiveScene().name;
        // capture score and timer if present
        ScoreManager scoreMgr = FindFirstObjectByType<ScoreManager>();
        if (scoreMgr != null)
        {
            saveData.score = scoreMgr.GetScore();
        }

        global::Timer timer = FindFirstObjectByType<global::Timer>();
        if (timer != null)
        {
            saveData.remainingTime = timer.GetRemainingTime();
        }

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

            if (saveData == null)
            {
                Debug.LogWarning("Save data could not be loaded.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(saveData.sceneName) && SceneManager.GetActiveScene().name != saveData.sceneName)
            {
                pendingLoadJson = json;
                pendingSceneName = saveData.sceneName;
                SceneManager.LoadScene(saveData.sceneName);
                return;
            }

            ApplySaveData(saveData);
        }
    }

    public static bool HasSave()
    {
        return PlayerPrefs.HasKey("saveData");
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(pendingLoadJson))
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(pendingSceneName) && scene.name != pendingSceneName)
        {
            return;
        }

        string json = pendingLoadJson;
        pendingLoadJson = null;
        pendingSceneName = null;

        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        if (saveData == null)
        {
            Debug.LogWarning("Save data could not be loaded after scene change.");
            return;
        }

        ApplySaveData(saveData);
    }

    private static void ApplySaveData(SaveData saveData)
    {
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

        // restore score and timer
        ScoreManager scoreMgr2 = FindFirstObjectByType<ScoreManager>();
        if (scoreMgr2 != null)
        {
            scoreMgr2.SetScore(saveData.score);
        }

        global::Timer timer2 = FindFirstObjectByType<global::Timer>();
        if (timer2 != null)
        {
            timer2.SetRemainingTime(saveData.remainingTime);
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
    public string sceneName;
    public int score;
    public float remainingTime;
}