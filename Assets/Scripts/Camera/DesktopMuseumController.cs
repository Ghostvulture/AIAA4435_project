using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>Offline keyboard/mouse visitor for the supplied museum NavMesh.</summary>
[DefaultExecutionOrder(-1000)]
public sealed class DesktopMuseumController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float mouseSensitivity = 2f;
    private NavMeshAgent agent;
    private Transform view;
    private float pitch;
    private bool ready;

    private void Awake()
    {
        view = Camera.main.transform;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.6f, 0.6f, 0.6f);
        foreach (var light in FindObjectsOfType<Light>())
            if (light.type == LightType.Directional) light.shadowStrength = 0.45f;
        foreach (var camera in FindObjectsOfType<Camera>())
            if (camera.transform != view) camera.enabled = false;
        foreach (var listener in FindObjectsOfType<AudioListener>())
            if (listener.transform != view) listener.enabled = false;
        foreach (var behaviour in FindObjectsOfType<MonoBehaviour>())
        {
            if (behaviour is CameraController || behaviour is AvatarController ||
                behaviour is ChatGPTTester || behaviour is OpenAI.Speech2Text ||
                behaviour is Text2Speech || behaviour is Minimap || behaviour is ObjectFinder)
                behaviour.enabled = false;
        }
        foreach (var canvas in FindObjectsOfType<Canvas>()) canvas.gameObject.SetActive(false);
        foreach (var childAgent in GetComponentsInChildren<NavMeshAgent>())
            if (childAgent.gameObject != gameObject) childAgent.enabled = false;
        foreach (var body in GetComponentsInChildren<Rigidbody>()) body.isKinematic = true;
        foreach (var collider in GetComponentsInChildren<Collider>()) collider.enabled = false;
        foreach (Transform child in transform)
            if (child != view && !view.IsChildOf(child)) child.gameObject.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        Camera.main.stereoTargetEye = StereoTargetEyeMask.None;
    }

    private void Start()
    {
        // NavMeshSurface registers its baked data during OnEnable, before Start.
        if (NavMesh.SamplePosition(transform.position, out var hit, 30f, agent.areaMask))
        {
            transform.position = hit.position;
            agent.enabled = true;
            ready = agent.Warp(hit.position);
        }
        if (!ready) Debug.LogError("Desktop museum: no walkable NavMesh at the starting point.");
        SetCapture(true);
        if (System.Array.IndexOf(System.Environment.GetCommandLineArgs(), "-museum-smoke-test") >= 0)
            StartCoroutine(SmokeTest());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SetCapture(false);
        if (Input.GetMouseButtonDown(0)) SetCapture(true);
        if (Cursor.lockState != CursorLockMode.Locked || !ready) return;

        transform.Rotate(0f, Input.GetAxisRaw("Mouse X") * mouseSensitivity, 0f);
        pitch = Mathf.Clamp(pitch - Input.GetAxisRaw("Mouse Y") * mouseSensitivity, -80f, 80f);
        view.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        var input = Vector3.ClampMagnitude(new Vector3(
            (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f), 0f,
            (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f)), 1f);
        var displacement = transform.TransformDirection(input) * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift)) displacement *= 2f;
        // Stop at baked walkable boundaries instead of crossing walls or leaving the floor.
        var target = agent.nextPosition + displacement;
        if (NavMesh.Raycast(agent.nextPosition, target, out var boundary, agent.areaMask))
            target = boundary.position;
        agent.Move(target - agent.nextPosition);
    }

    private static void SetCapture(bool capture)
    {
        Cursor.lockState = capture ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !capture;
    }

    private void OnApplicationFocus(bool focused) { if (!focused) SetCapture(false); }
    private void OnDisable() { SetCapture(false); }

    private IEnumerator SmokeTest()
    {
        yield return new WaitForSeconds(3f);
        if (!ready) { Application.Quit(1); yield break; }
        var start = transform.position;
        var destination = start + transform.forward * 2f;
        if (NavMesh.Raycast(agent.nextPosition, destination, out var hit, agent.areaMask))
            destination = hit.position;
        agent.Move(destination - agent.nextPosition);
        yield return null;
        if (Vector3.Distance(start, transform.position) < 0.1f)
        {
            Debug.LogError("DESKTOP_SMOKE_FAILED: visitor did not move");
            Application.Quit(1);
            yield break;
        }
        Debug.Log("DESKTOP_SMOKE_OK: NavMesh loaded and visitor moved. Position=" + transform.position);
        // Explicit rendering also works when the smoke-test window is hidden.
        var camera = view.GetComponent<Camera>();
        var target = RenderTexture.GetTemporary(1280, 720, 24);
        var previousTarget = camera.targetTexture;
        var previousActive = RenderTexture.active;
        camera.targetTexture = target;
        camera.Render();
        RenderTexture.active = target;
        var image = new Texture2D(1280, 720, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, 1280, 720), 0, 0);
        image.Apply();
        System.IO.File.WriteAllBytes("Logs/desktop-museum.png", image.EncodeToPNG());
        camera.targetTexture = previousTarget;
        RenderTexture.active = previousActive;
        RenderTexture.ReleaseTemporary(target);
        Destroy(image);
        yield return new WaitForSeconds(2f);
        Application.Quit(0);
    }
    private void OnGUI()
    {
        GUI.Box(new Rect(12, 12, 490, 52), "WASD: move   Mouse: look   Shift: faster\nEsc: release cursor   Click: capture cursor");
        if (!ready) GUI.Box(new Rect(12, 70, 490, 30), "No walkable NavMesh found. Check the Console.");
    }
}
