using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using System.IO;
public class CreateScriptAction : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        string className = Path.GetFileNameWithoutExtension(pathName);

        string template = File.ReadAllText(resourceFile);
        string scriptContent = template.Replace("#SCRIPTNAME#", className);

        File.WriteAllText(pathName, scriptContent);
        AssetDatabase.Refresh();

        Object newScript = AssetDatabase.LoadAssetAtPath<Object>(pathName);
        Selection.activeObject = newScript;
    }
}
