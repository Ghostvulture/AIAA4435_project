using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public static class DesktopMuseumBuild
{
    [MenuItem("Museum/Build Windows Desktop")]
    public static void Build()
    {
        var scene = EditorSceneManager.OpenScene("Assets/Scenes/DesktopMuseum.unity");
        if (Camera.main == null || UnityEngine.Object.FindObjectOfType<DesktopMuseumController>() == null)
            throw new Exception("Desktop camera/controller is missing.");
        int missing = 0;
        foreach (var root in scene.GetRootGameObjects())
            foreach (var transform in root.GetComponentsInChildren<Transform>(true))
                missing += GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(transform.gameObject);
        if (missing != 0) throw new Exception("Missing scripts: " + missing);
        var triangulation = NavMesh.CalculateTriangulation();
        Debug.Log("Museum validation: roots=" + scene.rootCount + ", navmesh vertices=" + triangulation.vertices.Length);
        if (triangulation.vertices.Length == 0) throw new Exception("Museum NavMesh is missing.");
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/DesktopMuseum.unity" },
            locationPathName = "Builds/Windows/AIAA4435_project.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.Development
        });
        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Desktop build failed: " + report.summary.result);
        Debug.Log("DESKTOP_MUSEUM_BUILD_OK");
    }
}
