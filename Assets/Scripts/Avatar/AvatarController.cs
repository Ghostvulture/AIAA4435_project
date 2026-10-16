using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AvatarController : MonoBehaviour
{
    public static event Action OnAvatarReached;
    private bool canContinue = false;

    [SerializeField] private CameraRecorder cameraRecorder;

    private Transform avatarTransform;
    private static Vector3 targetPosition;
    private static Quaternion targetRotation;
    private static Vector3 targetCameraPosition;

    [SerializeField] private NavMeshAgent navMeshAgent;

    [SerializeField] private Animator animator;
    [SerializeField] private CameraController cameraController;

    private Vector3 initalPosition;
    private Quaternion initalRotation;

    private void Start()
    {
        avatarTransform = transform;

        targetPosition = avatarTransform.position;

        SetCameraToNearestNavMeshPosition();

        initalPosition = transform.position;
        initalRotation = transform.rotation;
    }

    private void Update()
    {

    }

    private void SetCameraToNearestNavMeshPosition()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(avatarTransform.position, out hit, 20f, NavMesh.AllAreas))
        {
            avatarTransform.position = hit.position;
            navMeshAgent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogWarning("Could not find a valid NavMesh position for the camera starting point.");
        }
    }


    private void OnEnable()
    {
        CameraController.OnTourIDsReceived += HandleTourIDs;
        CameraController.OnCameraReached += HandleCameraReached;
    }

    private void OnDisable()
    {
        CameraController.OnTourIDsReceived -= HandleTourIDs;
        CameraController.OnCameraReached -= HandleCameraReached;
    }

    private void HandleCameraReached()
    {
        canContinue = true;
    }

    private void HandleTourIDs(string[] tourIDs)
    {
        Debug.Log("Avatar Start Navigation");
        StartCoroutine(NavigationTour(tourIDs));
    }

    // private float waitTime = 1f;
    private float rotationSpeed = 200f;

    private IEnumerator NavigationTour(string[] tourIDs)
    {
        for (int i = 0; i < tourIDs.Length; i++)
        {
            while (!cameraController.playing)
            {
                yield return null;
            }

            // update avatar target positions and orientations
            UpdateTargetAvatar(tourIDs[i]);
            animator.SetBool("Walk", true);
            navMeshAgent.SetDestination(targetPosition);

            // Wait until the avatar reaches the painting
            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                yield return null;
            }
            animator.SetBool("Walk", false);

            // After the camera reaches the target painting, rotate to the camera smoothly
            do
            {
                targetRotation = Quaternion.LookRotation(
                    new Vector3(targetCameraPosition.x - targetPosition.x, 0, targetCameraPosition.z - targetPosition.z)
                );
                avatarTransform.rotation = Quaternion.RotateTowards(avatarTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                yield return null;
            } while (Quaternion.Angle(avatarTransform.rotation, targetRotation) > 0.1f);

            OnAvatarReached?.Invoke();
            yield return new WaitUntil(() => canContinue);
            canContinue = false;
        }

    }

    private void UpdateTargetAvatar(string tourID)
    {
        string avatarID = tourID.Replace("painting ", "Avatar.");
        string cameraID = tourID.Replace("painting ", "Camera.");


        targetPosition = cameraRecorder.GetCameraPosition(avatarID);
        targetCameraPosition = cameraRecorder.GetCameraPosition(cameraID);
    }

}
