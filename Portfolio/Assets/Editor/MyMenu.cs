using UnityEditor;
using UnityEngine;

public class MyMenu
{
    private static string packageFile = "Portfolio.unitypackage";

    [MenuItem("My Menu/Export Backup", false, 0)]
    static void ExportBackup()
    {
        string[] exportPaths = new string[]
        {
            "Assets/Animations",
            "Assets/AudioClips",
            "Assets/AudioMixers",
            "Assets/Audios",
            "Assets/Editor",
            "Assets/Fonts",
            "Assets/Materials",
            "Assets/Models",
            "Assets/Resources",
            "Assets/Scenes",
            "Assets/Scripts",
            "Assets/Sprites",
            "ProjectSettings/TagManager.asset",
            "ProjectSettings/EditorBuildSettings.asset",
            "ProjectSettings/InputManager.asset"
            
        };
        AssetDatabase.ExportPackage(exportPaths, packageFile,
            ExportPackageOptions.Interactive |
            ExportPackageOptions.Recurse |
            ExportPackageOptions.IncludeDependencies);
        Debug.Log("Backup Completed...");
    }

    [MenuItem("My Menu/Import Backup", false, 1)]
    static void ImportBackup()
    {
        AssetDatabase.ImportPackage(packageFile, true);
    }

}
