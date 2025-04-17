using UnityEngine;

public class CameraShiftArea : MonoBehaviour
{
    [SerializeField] private Vector3 _shift;

    public Vector3 Shift => _shift;
}