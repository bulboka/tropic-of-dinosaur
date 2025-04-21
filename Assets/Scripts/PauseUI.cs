using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private AudioSource _music;

    public void Show()
    {
        gameObject.SetActive(true);

#if UNITY_EDITOR

#else
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
#endif

        _music.pitch = 0.4f;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        gameObject.SetActive(false);

#if UNITY_EDITOR

#else
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif

        _music.pitch = 1f;
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}