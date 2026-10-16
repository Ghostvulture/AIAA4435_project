using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;

public class ObjectFinder : MonoBehaviour
{
    [System.Serializable]
    public class ObjectInfo
    {
        public string name;
        public float[] position;
        public float[] orientation;
    }

    private void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        List<ObjectInfo> objectInfoList = new List<ObjectInfo>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("placeholder.0"))
            {
                ObjectInfo info = new ObjectInfo();
                info.name = obj.name;
                info.position = new float[] { obj.transform.localPosition.x, obj.transform.localPosition.y, obj.transform.localPosition.z };
                info.orientation = new float[] { obj.transform.localEulerAngles.x, obj.transform.localEulerAngles.z, obj.transform.localEulerAngles.y };
                objectInfoList.Add(info);
            }
        }

        string json = JsonUtility.ToJson(new Wrapper { items = objectInfoList }, true);

        string path = Path.Combine(Application.dataPath, "output.json");
        File.WriteAllText(path, json);
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<ObjectInfo> items;
    }
}