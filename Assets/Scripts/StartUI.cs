using System;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    public Action OnComplete;

    private void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            Debug.Log($"StartUI.Input at {Time.time}");
            Complete();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Complete()
    {
        Debug.Log($"StartUI.Complete at {Time.time}");
        gameObject.SetActive(false);
        OnComplete?.Invoke();
    }

#if UNITY_WEBGL
    private void OnApplicationFocus(bool hasFocus)
    {
        if (gameObject.activeSelf && hasFocus)
        {
            Complete();
        }
    }
#endif

}