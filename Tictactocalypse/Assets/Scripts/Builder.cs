using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Builder
{
    public static void Build()
    {
        Debug.Log("BUILDING");
        BuildReport report = BuildPipeline.BuildPlayer(new[]
            {
                "Assets/Scenes/MainMenu.unity",
                "Assets/Scenes/Main.unity",
            }, "Build/WIN/TICTACTOCALYPSE/TICTACTOCALYPSE.exe",
            BuildTarget.StandaloneWindows64, BuildOptions.None);
        Debug.Log("DONE");
        Debug.Log(report);
    }
}
