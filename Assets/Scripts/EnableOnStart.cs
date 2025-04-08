using System.Collections.Generic;
using UnityEngine;

public class EnableOnStart : MonoBehaviour
{
    [SerializeField] private List<GameObject> _enableObjects;
    [SerializeField] private List<GameObject> _disableObjects;

    private void Start()
    {
        foreach (var item in _enableObjects)
        {
            item.SetActive(true);
        }

        foreach (var item in _disableObjects)
        {
            item.SetActive(false);
        }
    }
}