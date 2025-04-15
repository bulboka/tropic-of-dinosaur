using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BodyPart))]
public class BodyPartInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Auto config"))
        {
            var bodyPart = (BodyPart)target;

            bodyPart.AutoConfig();
        }
    }
}