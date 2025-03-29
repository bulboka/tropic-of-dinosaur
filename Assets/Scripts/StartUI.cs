using System;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public Action OnComplete;

    private void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            Complete();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Complete()
    {
        gameObject.SetActive(false);
        OnComplete?.Invoke();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (gameObject.activeSelf && hasFocus)
        {
            Complete();
        }
    }
}