using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject minimap;
    public GameObject placeholder;

    public GameObject museum;
    public GameObject mainCamera;
    public GameObject userAvatar;

    // Start is called before the first frame update
    void Start()
    {
        ResetMinimapPosition();

        UpdatePlaceholder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Calculate the user's relative position on the minimap
    public void UpdatePlaceholder()
    {
        Vector3 userRelativePosition = museum.transform.InverseTransformPoint(userAvatar.transform.position);
        Vector3 minimapPosition = new Vector3(
            userRelativePosition.x, 
            placeholder.transform.localPosition.y, 
            userRelativePosition.z
            );
        placeholder.transform.localPosition = minimapPosition;
    }

    // reset the minimap position in the left front
    public void ResetMinimapPosition()
    {
        Vector3 userForward = userAvatar.transform.forward;
        Vector3 userRight = userAvatar.transform.right;
        Vector3 offset = (userForward - userRight).normalized;
        Vector3 newPosition = mainCamera.transform.position + (offset * 2f);
        newPosition += userAvatar.transform.right * 0.4f;
        newPosition.y = 17f;
        minimap.transform.position = newPosition;

        Vector3 originalRotation = minimap.transform.rotation.eulerAngles;
        Quaternion newRotation = Quaternion.LookRotation(minimap.transform.position - mainCamera.transform.position);
        newRotation.eulerAngles = new Vector3(originalRotation.x, newRotation.eulerAngles.y, originalRotation.z);
        minimap.transform.rotation = newRotation;
    }

    public void ShowMinimap()
    {
        minimap.SetActive(true);
    }

    public void UpdateMinimap()
    {
        ShowMinimap();
        UpdatePlaceholder();
        ResetMinimapPosition();
    }

    public void HideMinimap()
    {
        minimap.SetActive(false);
    }
}
