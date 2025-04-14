using System;
using UnityEngine;

public class CameraShiftTrigger : MonoBehaviour
{
    [SerializeField] private Vector3 _shift;
    [SerializeField] private bool _oneTime;

    public Vector3 Shift => _shift;

    public bool OneTime => _oneTime;
}