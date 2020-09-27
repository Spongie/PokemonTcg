using UnityEditor;
using UnityEngine;

public class Builder
{
    static void PerformBuild()
    {
        Debug.Log("### BUILDING ###");

        var report = BuildPipeline.BuildPlayer(
            new[] 
            { 
                "Assets/Scenes/Login Scene.unity",
                "Assets/Scenes/UI_2D.unity",
                "Assets/Scenes/MainMenu.unity",
                "Assets/Scenes/DeckBuilder.unity"
            },
            @"E:\PokemonBuild\Client\PokemonTCGClient.exe",
            BuildTarget.StandaloneWindows64,
            BuildOptions.None);

        Debug.Log("###   DONE   ###");

        Debug.Log(report);
    }
}
