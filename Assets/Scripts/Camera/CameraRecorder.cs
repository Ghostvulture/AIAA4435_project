using System.Collections.Generic;
using UnityEngine;

public class CameraRecorder : MonoBehaviour
{
    [System.Serializable]
    public struct CameraData
    {
        public string cameraID;
        public Vector3 position;
        public Quaternion orientation;
    }

    public List<CameraData> recordedCameraData = new List<CameraData>();

    public void AddCameraData(string cameraID, Vector3 position, Quaternion orientation)
    {
        CameraData data = new CameraData();
        data.cameraID = cameraID;
        data.position = position;
        data.orientation = orientation;

        recordedCameraData.Add(data);
    }

    public Vector3 GetCameraPosition(string cameraID)
    {
        foreach (CameraData data in recordedCameraData)
        {
            if (data.cameraID == cameraID)
            {
                return data.position;
            }
        }

        // If no matching camera ID found, return a default position
        return Vector3.zero;
    }

    public Quaternion GetCameraOrientation(string cameraID)
    {
        foreach (CameraData data in recordedCameraData)
        {
            if (data.cameraID == cameraID)
            {
                return data.orientation;
            }
        }

        // If no matching camera ID found, return a default orientation
        return Quaternion.identity;
    }
}
