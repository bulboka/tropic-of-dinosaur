using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> _activateObjects;
    [SerializeField] private List<GameObject> _deactivateObjects;
    [SerializeField] private bool _isSingleUse = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var targetObject in _activateObjects)
        {
            targetObject.SetActive(true);
        }

        foreach (var targetObject in _deactivateObjects)
        {
            targetObject.SetActive(false);
        }

        if (_isSingleUse)
        {
            gameObject.SetActive(false);
        }
    }
}