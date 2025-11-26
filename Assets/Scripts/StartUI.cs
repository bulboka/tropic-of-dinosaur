using System;
using UnityEngine;

public class StartUI : MonoBehaviour
{
    [SerializeField] private GameObject _focusContent;
    [SerializeField] private GameObject _startContent;

    public Action OnComplete;
    private bool _isFocused;

    private void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            Debug.Log($"StartUI.Input at {Time.time}");
            TryComplete();
        }
    }

    public void Show()
    {
#if UNITY_WEBGL
        _focusContent.SetActive(true);
        _startContent.SetActive(false);
#else
        _focusContent.SetActive(false);
        _startContent.SetActive(true);
#endif
        
        gameObject.SetActive(true);
    }

    private void TryComplete()
    {
        Debug.Log($"StartUI.TryComplete at {Time.time}");

#if UNITY_WEBGL
        if (!_isFocused)
        {
            _isFocused = true;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
            Screen.fullScreen = true;
            _focusContent.SetActive(false);
            _startContent.SetActive(true);

            return;
        }
#endif

        gameObject.SetActive(false);
        OnComplete?.Invoke();
        GameSession.OnStartUIComplete();
    }

#if UNITY_WEBGL
    private void OnApplicationFocus(bool hasFocus)
    {
        if (gameObject.activeSelf && hasFocus)
        {
            TryComplete();
        }
    }
#endif

}