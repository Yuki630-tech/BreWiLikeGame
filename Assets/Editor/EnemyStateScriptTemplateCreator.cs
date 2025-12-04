using UnityEngine;
using UnityEditor;
using System.IO;
using System.IO.Enumeration;

public class EnemyStateScriptTemplateCreator
{
    private const string templatePath = "Assets/Editor/ScriptTemplate/EnemyStateTemplate.txt";

    [MenuItem("Assets/Create/Template/EnemyStateTemplate")]
    public static void CreateScriptFromTemplate()
    {
        string folderPath = GetClickedFolderPath();

        string createPath = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/NewEnemyBaseState.cs");

        //名称変更モードでファイル作成を開始
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0,
            ScriptableObject.CreateInstance<CreateScriptAction>(),
            createPath,
            null,
            templatePath);
    }

    private static string GetClickedFolderPath()
    {
        //右クリック選択されたアセット
        string[] guids = Selection.assetGUIDs;

        if (guids.Length == 0)
        {
            return "Assets";
        }

        string path = AssetDatabase.GUIDToAssetPath(guids[0]);

        //フォルダーならそのまま
        if(AssetDatabase.IsValidFolder(path))
        {
            return path;
        }

        //ファイルが選択されていた場合はその親フォルダー
        return Path.GetDirectoryName(path).Replace("\\", "/");
    }
}
