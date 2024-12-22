using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IconCreator))]
public class IconCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Repaint();

        var iconCreator = (IconCreator) target;

        DrawDefaultInspector();

        GUILayout.Space(10);

        EditorGUILayout.LabelField("Model preview index", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<", GUILayout.Width(100)))
        {
            iconCreator.CurrentIndex--;
            iconCreator.ChangePreviewModel();
        }
        GUILayout.FlexibleSpace();
        GUILayout.Label((iconCreator.CurrentIndex + 1).ToString());
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(">", GUILayout.Width(100)))
        {
            iconCreator.CurrentIndex++;
            iconCreator.ChangePreviewModel();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUI.color = Color.green;

        if (!GUILayout.Button("Create icons")) return;

        var assembly = typeof(EditorWindow).Assembly;
        var type = assembly.GetType("UnityEditor.GameView");
        var gameView = EditorWindow.GetWindow(type);
        gameView.Focus();

        iconCreator.CreateIcons();
    }
}