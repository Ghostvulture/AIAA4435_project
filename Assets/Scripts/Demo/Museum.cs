using System;
using UnityEngine;

public class Museum : MonoBehaviour
{
    public CameraRecorder cameraRecorder;
    //private Camera mainCamera;

    private void Start()
    {
        //mainCamera = Camera.main;
        //cameraRecorder = GetComponent<CameraRecorder>();

        cameraRecorder.AddCameraData("Camera.000", new Vector3(-40f, 18f, 0f), Quaternion.Euler(0f, -90f, 0f));
        cameraRecorder.AddCameraData("Avatar.000", new Vector3(-55f, 17.1f, -8.5f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.001", new Vector3(-140f, 18f, 0f), Quaternion.Euler(0f, 90f, 0f));
        cameraRecorder.AddCameraData("Avatar.001", new Vector3(-125f, 17.1f, -16f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.002", new Vector3(-271f, 15.5f, 0f), Quaternion.Euler(0f, -90f, 0f));
        cameraRecorder.AddCameraData("Avatar.002", new Vector3(-294f, 17.1f, -8.5f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.003", new Vector3(-150f, 19f, -122f), Quaternion.Euler(0f, 180f, 0f));
        cameraRecorder.AddCameraData("Avatar.003", new Vector3(-142f, 17.1f, -134f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.004", new Vector3(-130f, 18.5f, -107f), Quaternion.Euler(0f, 120f, 0f));
        cameraRecorder.AddCameraData("Avatar.004", new Vector3(-109f, 17.1f, -100f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.005", new Vector3(-171f, 19f, -107f), Quaternion.Euler(0f, -120f, 0f));
        cameraRecorder.AddCameraData("Avatar.005", new Vector3(-190f, 17.1f, -103f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.006", new Vector3(-261f, 18f, -56f), Quaternion.Euler(0f, -120f, 0f));
        cameraRecorder.AddCameraData("Avatar.006", new Vector3(-266f, 17.1f, -74f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.007", new Vector3(-248f, 15f, 67.5f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Avatar.007", new Vector3(-235f, 17.1f, 82f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.008", new Vector3(-39f, 14.5f, 60f), Quaternion.Euler(0f, 60f, 0f));
        cameraRecorder.AddCameraData("Avatar.008", new Vector3(-30f, 17.1f, 66f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.009", new Vector3(-238f, 16.5f, -68f), Quaternion.Euler(0f, 180f, 0f));
        cameraRecorder.AddCameraData("Avatar.009", new Vector3(-247f, 17.1f, -82f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.010", new Vector3(-62f, 16.5f, 67f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Avatar.010", new Vector3(-51f, 17.1f, 80f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.011", new Vector3(-264f, 17f, 59f), Quaternion.Euler(0f, -60f, 0f));
        cameraRecorder.AddCameraData("Avatar.011", new Vector3(-270f, 17.1f, 70f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.012", new Vector3(-223f, 16.5f, 66f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Avatar.012", new Vector3(-233f, 17.1f, 82f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.013", new Vector3(-49f, 16.5f, -64f), Quaternion.Euler(0f, 180f, 0f));
        cameraRecorder.AddCameraData("Avatar.013", new Vector3(-60f, 17.1f, -78f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.014", new Vector3(-40f, 15f, -55f), Quaternion.Euler(0f, 120f, 0f));
        cameraRecorder.AddCameraData("Avatar.014", new Vector3(-32f, 17.1f, -70f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.015", new Vector3(-74f, 15.5f, -66f), Quaternion.Euler(0f, 180f, 0f));
        cameraRecorder.AddCameraData("Avatar.015", new Vector3(-64f, 17.1f, -78f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.016", new Vector3(-272f, 15f, 25f), Quaternion.Euler(0f, -90f, 0f));
        cameraRecorder.AddCameraData("Avatar.016", new Vector3(-284f, 17.1f, 19f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.017", new Vector3(-269f, 15f, -26f), Quaternion.Euler(0f, -90f, 0f));
        cameraRecorder.AddCameraData("Avatar.017", new Vector3(-284f, 17.1f, -14f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.018", new Vector3(-27f, 15f, -26f), Quaternion.Euler(0f, 90f, 0f));
        cameraRecorder.AddCameraData("Avatar.018", new Vector3(-16f, 17.1f, -33f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.019", new Vector3(-29f, 15f, 26f), Quaternion.Euler(0f, 90f, 0f));
        cameraRecorder.AddCameraData("Avatar.019", new Vector3(-15.5f, 17.1f, 19f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.020", new Vector3(-163f, 18f, 0f), Quaternion.Euler(0f, -90f, 0f));
        cameraRecorder.AddCameraData("Avatar.020", new Vector3(-175f, 17.1f, 7f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.021", new Vector3(-105f, 16.5f, 48f), Quaternion.Euler(0f, 180f, 0f));
        cameraRecorder.AddCameraData("Avatar.021", new Vector3(-114f, 17.1f, 34f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.022", new Vector3(-78f, 16.5f, 47f), Quaternion.Euler(0f, 180f, 0f));
        cameraRecorder.AddCameraData("Avatar.022", new Vector3(-67f, 17.1f, 34f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.023", new Vector3(-196f, 18f, -45f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Avatar.023", new Vector3(-187f, 17.1f, -33f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.024", new Vector3(-222f, 16.5f, -47f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Avatar.024", new Vector3(-215f, 17.1f, -34f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.025", new Vector3(-197f, 16.5f, 45f), Quaternion.Euler(0f, 180f, 0f));
        cameraRecorder.AddCameraData("Avatar.025", new Vector3(-207f, 17.1f, 34f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.026", new Vector3(-223f, 18.5f, 46f), Quaternion.Euler(0f, 180f, 0f));
        cameraRecorder.AddCameraData("Avatar.026", new Vector3(-230f, 17.1f, 34f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.027", new Vector3(-102.5f, 18.5f, -46.5f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Avatar.027", new Vector3(-108f, 17.1f, -34f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.028", new Vector3(-77.5f, 17f, -48f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Avatar.028", new Vector3(-68f, 17.1f, -34f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.029", new Vector3(-268f, 24f, 0f), Quaternion.Euler(0f, 90f, 0f));
        cameraRecorder.AddCameraData("Avatar.029", new Vector3(-245f, 17.1f, -9f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.030", new Vector3(-142f, 20f, 173f), Quaternion.Euler(0f, 90f, 0f));
        cameraRecorder.AddCameraData("Avatar.030", new Vector3(-129f, 17.1f, 179f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.031", new Vector3(-157f, 19f, 172f), Quaternion.Euler(0f, -90f, 0f));
        cameraRecorder.AddCameraData("Avatar.031", new Vector3(-170f, 17.1f, 178f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.032", new Vector3(-130f, 20f, 241f), Quaternion.Euler(0f, -90f, 0f));
        cameraRecorder.AddCameraData("Avatar.032", new Vector3(-145f, 17.1f, 249f), Quaternion.Euler(0f, 0f, 0f));
        cameraRecorder.AddCameraData("Camera.033", new Vector3(-168f, 21f, 241f), Quaternion.Euler(0f, 90f, 0f));
        cameraRecorder.AddCameraData("Avatar.033", new Vector3(-153f, 17.1f, 233f), Quaternion.Euler(0f, 0f, 0f));
  }

    private void Update()
    {
        //Debug.Log(mainCamera.transform.position);

        //Vector3 position = cameraRecorder.GetCameraPosition("Camera.000");
        //Quaternion orientation = cameraRecorder.GetCameraOrientation("Camera.000");
        //mainCamera.transform.position = position;
        //mainCamera.transform.rotation = orientation;

        //Debug.Log(position);
        //Debug.Log(orientation);

    }
}
