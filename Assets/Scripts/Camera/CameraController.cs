using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private Transform cameraTransform;

    public CameraRecorder cameraRecorder;

    private Vector3 initalPosition;
    private Quaternion initalRotation;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private static Vector3 targetPosition;
    private static Quaternion targetRotation;

    public static event Action<string[]> OnTourIDsReceived;
    public static event Action OnCameraReached;
    private bool canContinue = false;

    [SerializeField] private Animator animator;
    // private float currentRotationX = 0f;

    [System.Serializable]
    public class TourResponse
    {
        public string Introduction;
        public string[] Tour;
        public string[] TourID;
    }

    public string[] tourIDs;
    public string[] tours;

    public NavMeshAgent navMeshAgent;
    private bool navigating;

    private float rotationStartDistance = 50f;
    private float rotationDistance;
    private float rotationSpeed = 30f;

    private WaitForSeconds wait = new WaitForSeconds(1f);

    private string Landmark = "";

    [SerializeField] private GameObject arrowPrefab;
    private GameObject arrow;
    private float arrowSpeed = 5f;

    public Minimap miniMap;

    private void Start()
    {
        // Find the main camera GameObject and Transform
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        cameraTransform = transform;

        // Perform initial camera setup
        initalPosition = cameraTransform.position;
        initalRotation = cameraTransform.localRotation;

        startPosition = cameraTransform.position;
        startRotation = cameraTransform.localRotation;

        targetPosition = cameraTransform.position;
        targetRotation = cameraTransform.localRotation;

        // currentRotationX = cameraTransform.localEulerAngles.x;

        // Ensure that the camera starts in a valid NavMesh area
        SetCameraToNearestNavMeshPosition();
    }

    private void SetCameraToNearestNavMeshPosition()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(cameraTransform.position, out hit, 20f, NavMesh.AllAreas))
        {
            cameraTransform.position = hit.position;
            navMeshAgent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning("Could not find a valid NavMesh position for the camera starting point.");
        }
    }

    // public float transitionSpeed = 1f;
    //[SerializeField] private bool init = false;
    private void Update()
    {
        // Test camera movement
        Test();
        if (navigating)
        {
            // var targetRotationX = animator.GetBool("Walk") ? 60f : 0f;
            // currentRotationX = Mathf.Lerp(currentRotationX, targetRotationX,  transitionSpeed * Time.deltaTime);
            // mainCamera.transform.localEulerAngles = new Vector3(currentRotationX, 0f, 0f);
            MoveArrow();
            RotateTowardsDestination();

        }
        if (playing)
        {
            miniMap.UpdateMinimap();
        }

        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    init = true;
        //}

        //if (init) Init();
    }

    public bool playing = false;
    [SerializeField] GameObject PlayButton;
    [SerializeField] GameObject PauseButton;
    [SerializeField] Text TourButtonText;

    private void Init()
    {
        cameraTransform.position = initalPosition;
        cameraTransform.localRotation = initalRotation;
        navigating = false;
        navMeshAgent.SetDestination(initalPosition);
    }


    public void PlayButtonClicked()
    {
        playing = true;
        isOrNotPlaying();
    }

    private void isOrNotPlaying()
    {
        if (playing)
        {
            PlayButton.SetActive(false);
            PauseButton.SetActive(true);
            TourButtonText.text = "Touring";
        }
        else
        {
            PlayButton.SetActive(true);
            PauseButton.SetActive(false);
            TourButtonText.text = "Continue Tour";
        }
    }

    private void Test()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    string chatGPTContent = "{\n    \"Reasoning\": \"The visitor has a special interest in Chinese art, show him more related paintings.\",\n    \"Tour\": [\n        \"Guernica\",\n        \"The Birth of Venus\",\n        \"The Scream\",\n        \"The Great Wave off Kanagawa\",\n        \"The Persistence of Memory\",\n        \"The Last Judgment\",\n        \"The Creation of Adam\",\n        \"The Starry Night\"\n    ],\n    \"TourID\": [\n        \"painting 015\",\n        \"painting 013\",\n        \"painting 014\",\n        \"painting 008\",\n        \"painting 010\",\n        \"painting 012\",\n        \"painting 011\",\n        \"painting 009\"\n    ]\n}";
        //    ProcessChatGPTResponse(chatGPTContent);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    string chatGPTContent = "{\n    \"Reasoning\": \"The visitor has a special interest in Chinese art, show him more related paintings.\",\n    \"Tour\": [\n        \"Guernica\",\n        \"The Birth of Venus\",\n        \"The Scream\",\n        \"The Great Wave off Kanagawa\",\n        \"The Persistence of Memory\",\n        \"The Last Judgment\",\n        \"The Creation of Adam\",\n        \"The Starry Night\"\n    ],\n    \"TourID\": [\n        \"painting 000\"\n ]\n}";
        //    ProcessChatGPTResponse(chatGPTContent);
        //}
    }

    private void OnEnable()
    {
        AvatarController.OnAvatarReached += HandleAvatarReached;
    }

    private void OnDisable()
    {
        AvatarController.OnAvatarReached -= HandleAvatarReached;
    }

    private void HandleAvatarReached()
    {
        canContinue = true;
    }

    public string GetCurrentLandmark()
    {
        return Landmark;
    }

    public void ProcessChatGPTResponse(string chatGPTContent)
    {
        Debug.Log("Process ChatGPT Response");

        if (!string.IsNullOrEmpty(chatGPTContent))
        {
            // Parse the response and extract camera control commands
            TourResponse tourResponse = JsonUtility.FromJson<TourResponse>(chatGPTContent);
            tourIDs = tourResponse.TourID;
            //string reasoning = tourResponse.Reasoning;
            tours = tourResponse.Tour;

            OnTourIDsReceived?.Invoke(tourIDs);
            Debug.Log("Start navigation");
            StartCoroutine(NavigationTour(tourIDs));
        }
    }

    public void ProcessChatGPTResponse(string chatGPTContent, bool Voice, Text2Speech textToSpeech)
    {
        Debug.Log("Process ChatGPT Response");

        if (!string.IsNullOrEmpty(chatGPTContent))
        {
            // Parse the response and extract camera control commands
            TourResponse tourResponse = JsonUtility.FromJson<TourResponse>(chatGPTContent);
            tourIDs = tourResponse.TourID;
            //string reasoning = tourResponse.Reasoning;
            tours = tourResponse.Tour;

            if (Voice) textToSpeech.MakeAudioRequest(tourResponse.Introduction);

            OnTourIDsReceived?.Invoke(tourIDs);
            Debug.Log("Start navigation");
            StartCoroutine(NavigationTour(tourIDs));
        }
    }

    // detect whether the response string is a json format
    public string ResponseJsonOrNot(string response)
    {
        try
        {
            // Try to deserialize the JSON string into a dummy C# object
            TourResponse tourResponse = JsonUtility.FromJson<TourResponse>(response);
            // If deserialization succeeds, it means the string is in valid JSON format
            return tourResponse.Introduction;
        }
        catch (System.Exception)
        {
            // If deserialization fails, it means the string is not in valid JSON format
            return response;
        }
    }

    private void PrintLandmarks(string[] tourIDs)
    {
        for (int i = 0; i < tourIDs.Length; i++)
        {
            Debug.Log(tourIDs[i]);
        }
    }

    private void ReorderLandmarks(string[] tourIDs)
    {
        // If there are no landmarks to visit, return
        if (tourIDs.Length <= 1)
            return;

        List<string> reorderedTourIDs = new List<string>();
        Vector3 currentPosition = cameraTransform.position;

        // Find the closest landmark and add it to the reordered list
        while (tourIDs.Length > 0)
        {
            string closestLandmarkID = FindClosestLandmark(currentPosition);
            reorderedTourIDs.Add(closestLandmarkID);

            tourIDs = RemoveElementFromArray(tourIDs, closestLandmarkID);

            currentPosition = GetPositionFromTourID(closestLandmarkID);
        }

        tourIDs = reorderedTourIDs.ToArray();

        Debug.Log("Reorder finished");
        PrintLandmarks(tourIDs);
    }

    // Helper function to find the closest landmark to a given position
    private string FindClosestLandmark(Vector3 position)
    {
        string closestID = "";
        float closestDistance = float.MaxValue;

        foreach (string tourID in tourIDs)
        {
            Vector3 landmarkPosition = GetPositionFromTourID(tourID);
            float distance = Vector3.Distance(position, landmarkPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestID = tourID;
            }
        }

        return closestID;
    }

    // Helper function to remove an element from an array
    private T[] RemoveElementFromArray<T>(T[] array, T element)
    {
        List<T> list = new List<T>(array);
        list.Remove(element);
        return list.ToArray();
    }

    //public void PlayButtonClicked()
    //{
    //    Debug.Log("Replay the camera");

    //    cameraTransform.position = initalPosition;
    //    cameraTransform.rotation = initalRotation;

    //    StartCoroutine(NavigationTour(tourIDs));
    //}

    private void UpdateTargetCamera(string tourID)
    {
        string cameraID = tourID.Replace("painting ", "Camera.");

        targetPosition = cameraRecorder.GetCameraPosition(cameraID);
        targetRotation = cameraRecorder.GetCameraOrientation(cameraID);
    }

    public Vector3 GetPositionFromTourID(string tourID)
    {
        string cameraID = tourID.Replace("painting ", "Camera.");
        return cameraRecorder.GetCameraPosition(cameraID);
    }


    private IEnumerator NavigationTour(string[] tourIDs)
    {
        navigating = true;

        playing = true;
        isOrNotPlaying();

        NavMeshPath path = new NavMeshPath();

        for (int i = 0; i < tourIDs.Length; i++)
        {

            while (!playing)
            {
                yield return null;
            }

            Landmark = tourIDs[i];
            Debug.Log(tourIDs[i]);

            // update camera target positions and orientations
            UpdateTargetCamera(tourIDs[i]);
            navMeshAgent.SetDestination(targetPosition);

            visitedTour.Add(tourIDs[i]);

            NavMesh.CalculatePath(cameraTransform.position, targetPosition, NavMesh.AllAreas, path);
            DrawArrow(path);

            animator.SetBool("Walk", true);

            // Wait until the camera reaches the painting
            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                yield return null;
            }

            animator.SetBool("Walk", false);

            yield return new WaitUntil(() => canContinue);
            canContinue = false;
            OnCameraReached?.Invoke();

            playing = false;
            isOrNotPlaying();

            yield return wait;
        }

        navigating = false;
    }

    private bool rotating = false;
    private void RotateTowardsDestination()
    {
        if (navMeshAgent.remainingDistance < rotationStartDistance)
        {
            if (!rotating)
            {
                var rotateAngle = Quaternion.Angle(cameraTransform.rotation, targetRotation);
                rotationSpeed = Mathf.Min(50f, rotateAngle / navMeshAgent.remainingDistance * navMeshAgent.speed);
            }
            rotating = true;
            if (Quaternion.Angle(cameraTransform.rotation, targetRotation) > 0.1f)
            {
                navMeshAgent.updateRotation = false;
                cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            rotating = false;
            rotationSpeed = 30f;
            if (Quaternion.Angle(cameraTransform.localRotation, Quaternion.identity) > 0.1f)
            {
                cameraTransform.localRotation = Quaternion.RotateTowards(cameraTransform.localRotation, Quaternion.identity, rotationSpeed * Time.deltaTime);
            }
            navMeshAgent.updateRotation = true;
        }
    }


    //TODO: arrow list, add more arrows for longer path
    private void DrawArrow(NavMeshPath path)
    {
        if (path.status == NavMeshPathStatus.PathComplete & path.corners.Length > 1)
        {
            //for (int i = 0; i < path.corners.Length; i++)
            //{
            //    Debug.Log(path.corners[i]);
            //}
            // Instantiate the arrow prefab at the calculated position
            arrow = Instantiate(arrowPrefab, Vector3.Lerp(path.corners[0], path.corners[1], 0.75f), Quaternion.identity);
            Vector3 arrowPosition = arrow.transform.position;
            arrowPosition.y = 6.0f;
            arrow.transform.position = arrowPosition;
            arrow.transform.localScale = new Vector3(3f, 3f, 3f);

            Destroy(arrow, GetPathLength(path) / navMeshAgent.speed);
        }
    }

    private float GetPathLength(NavMeshPath path)
    {
        float distance = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return distance;
    }

    private void MoveArrow()
    {
        if (arrow != null)
        {
            Vector3 direction = (targetPosition - cameraTransform.position).normalized;
            arrow.transform.Translate(direction * arrowSpeed * Time.deltaTime, Space.World);

            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                arrow.transform.rotation = rotation * Quaternion.Euler(0f, 90f, 0f);
            }
        }
    }

    private static List<string> visitedTour = new List<string> { };
    public List<string> GetTourHistory()
    {
        return visitedTour;
    }
}
