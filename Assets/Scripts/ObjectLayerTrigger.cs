using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectLayerTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    [SerializeField] private List<string> _includeTags;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_includeTags.Any() && _includeTags.All(includedTag => !other.gameObject.CompareTag(includedTag)))
        {
            return;
        }

        gameObject.SetActive(false);
        other.gameObject.layer = _layer.value;
    }
}