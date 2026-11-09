using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public static class CreateFlightDemoScene
{
    [MenuItem("Tools/Flight Simulator/Create Demo Scene")]
    public static void CreateScene()
    {
        const string folder = "Assets/Scenes";
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject bootstrap = new GameObject("Flight Game Bootstrap");
        bootstrap.AddComponent<FlightGameBootstrap>();

        const string scenePath = folder + "/FlightDemo.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(scenePath, true) };
        AssetDatabase.Refresh();
        EditorSceneManager.OpenScene(scenePath);
        Debug.Log("Демо саҳна яратилди: " + scenePath);
    }
}
