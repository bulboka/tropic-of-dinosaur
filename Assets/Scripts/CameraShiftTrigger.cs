using UnityEngine;

public class CameraShiftTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 _shift;

    public Vector3 Shift => _shift;
}