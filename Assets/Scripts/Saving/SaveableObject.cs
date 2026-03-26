using UnityEngine;

public class SaveableObject : MonoBehaviour
{
    public string objectName;

    public static void LoadObject(SaveableObjectData data)
    {
        GameObject obj = new GameObject(data.objectName);
        obj.transform.position = data.position;
        obj.transform.rotation = data.rotation;
    }

    public SaveableObjectData GetData()
    {
        SaveableObjectData data = new SaveableObjectData();
        data.objectName = objectName;
        data.position = transform.position;
        data.rotation = transform.rotation;
        return data;
    }
}

[System.Serializable]
public class SaveableObjectData
{
    public string objectName;
    public Vector3 position;
    public Quaternion rotation;
}
