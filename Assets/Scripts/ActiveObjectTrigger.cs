using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveObjectTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> _activateObjects;
    [SerializeField] private List<GameObject> _deactivateObjects;
    [SerializeField] private bool _isSingleUse = true;
    [SerializeField] private List<string> _onlyTriggeredByTags;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_onlyTriggeredByTags.Count > 0 && _onlyTriggeredByTags.All(item => !other.gameObject.CompareTag(item)))
        {
            return;
        }

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