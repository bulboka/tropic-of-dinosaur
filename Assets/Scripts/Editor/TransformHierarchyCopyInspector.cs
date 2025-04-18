using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransformHierarchyCopy))]
public class TransformHierarchyCopyInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Copy child transforms"))
        {
            var transformHierarchy = (TransformHierarchyCopy)target;

            for (var i = 0; i < transformHierarchy.transform.childCount; i++)
            {
                var targetChild = transformHierarchy.transform.GetChild(i);
                var sourceChild = transformHierarchy.TargetTransformRoot.Find(targetChild.name);

                if (sourceChild == null)
                {
                    continue;
                }

                targetChild.localPosition = sourceChild.localPosition;
                targetChild.localRotation = sourceChild.localRotation;
                targetChild.localScale = sourceChild.localScale;
            }
        }
    }
}