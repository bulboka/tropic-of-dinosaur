using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectLayerTrigger : MonoBehaviour
{
    [SerializeField] private string _layer;
    [SerializeField] private List<string> _includeTags;
    [SerializeField] private bool _oneTimeUse;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_includeTags.Any() && _includeTags.All(includedTag => !other.gameObject.CompareTag(includedTag)))
        {
            return;
        }

        if (_oneTimeUse)
        {
            gameObject.SetActive(false);
        }

        other.gameObject.layer = LayerMask.NameToLayer(_layer);
    }
}