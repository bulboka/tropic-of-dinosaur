using System;
using System.Collections.Generic;
using UnityEngine;

public class MainStarLocator : MonoBehaviour
{
    [SerializeField] private List<MainStarLocator> _connectedLocators;
    [HideInInspector] public Stub AttachedStub;

    public Action OnActivated;

    public List<MainStarLocator> ConnectedLocators => _connectedLocators;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        if (_connectedLocators == null)
        {
            return;
        }

        foreach (var connectedLocator in _connectedLocators)
        {
            Gizmos.DrawLine(transform.position, connectedLocator.transform.position);
        }
    }
}