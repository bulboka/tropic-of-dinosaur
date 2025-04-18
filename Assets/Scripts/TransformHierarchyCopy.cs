using UnityEngine;

public class TransformHierarchyCopy : MonoBehaviour
{
    [SerializeField] private Transform _targetTransformRoot;

    public Transform TargetTransformRoot => _targetTransformRoot;
}