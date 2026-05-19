using System;
using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    [SerializeField] private string objectId;

    private void Awake()
    {
        EnsureObjectId();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        EnsureObjectId();
        EnsureUniqueObjectId();
    }

    private void EnsureUniqueObjectId()
    {
        if (string.IsNullOrEmpty(objectId))
        {
            return;
        }

        SaveableObject[] saveableObjects = FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);
        int matchingCount = 0;

        for (int i = 0; i < saveableObjects.Length; i++)
        {
            SaveableObject saveableObject = saveableObjects[i];
            if (saveableObject != null && saveableObject.objectId == objectId)
            {
                matchingCount++;
                if (matchingCount > 1)
                {
                    objectId = Guid.NewGuid().ToString("N");
                    break;
                }
            }
        }
    }
#endif

    public string ObjectId => objectId;

    public void MarkDestroyed()
    {
        EnsureObjectId();
        SaveSystem.MarkDestroyed(objectId);
    }

    private void EnsureObjectId()
    {
        if (string.IsNullOrEmpty(objectId))
        {
            objectId = Guid.NewGuid().ToString("N");
        }
    }

    public static void LoadObject(SaveableObjectData data)
    {
        if (data == null || SaveSystem.IsDestroyed(data.objectId))
        {
            return;
        }

        SaveableObject[] saveableObjects = FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);
        SaveableObject targetObject = null;

        if (!string.IsNullOrEmpty(data.objectId))
        {
            foreach (SaveableObject saveableObject in saveableObjects)
            {
                if (saveableObject != null && saveableObject.ObjectId == data.objectId)
                {
                    targetObject = saveableObject;
                    break;
                }
            }
        }

        if (targetObject == null && !string.IsNullOrEmpty(data.objectName))
        {
            foreach (SaveableObject saveableObject in saveableObjects)
            {
                if (saveableObject != null && saveableObject.gameObject.name == data.objectName)
                {
                    targetObject = saveableObject;
                    break;
                }
            }
        }

        if (targetObject == null)
        {
            Debug.LogWarning($"Could not find saveable object for '{data.objectName}'.");
            return;
        }

        targetObject.transform.SetPositionAndRotation(data.position, data.rotation);
    }

    public SaveableObjectData GetData()
    {
        EnsureObjectId();

        SaveableObjectData data = new SaveableObjectData();
        data.objectId = objectId;
        data.objectName = gameObject.name;
        data.position = transform.position;
        data.rotation = transform.rotation;
        return data;
    }
}

[System.Serializable]
public class SaveableObjectData
{
    public string objectId;
    public string objectName;
    public Vector3 position;
    public Quaternion rotation;
}
