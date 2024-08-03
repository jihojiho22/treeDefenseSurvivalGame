using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLoad : MonoBehaviour
{
    private static Dictionary<string, GameObject> persistentObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        string objectId = gameObject.name + gameObject.GetInstanceID();

        if (!persistentObjects.ContainsKey(objectId))
        {
            persistentObjects[objectId] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (persistentObjects[objectId] != gameObject)
        {
            Destroy(gameObject);
        }
    }
     public static void ClearPersistentObjects()
    {
        foreach (var obj in persistentObjects.Values)
        {
            Destroy(obj);
        }
        persistentObjects.Clear();
    }
}