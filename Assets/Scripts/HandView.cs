using System.Collections.Generic;
using UnityEngine;

public class HandView : MonoBehaviour
{
    [SerializeField] private List<Age> _availableAtAges;

    public List<Age> AvailableAtAges => _availableAtAges;
}