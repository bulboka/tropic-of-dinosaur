using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private AudioSource _music;
    [SerializeField] private GameObject _thanksText;
    [SerializeField] private GameObject _quitBtn;

    public void Show()
    {
        gameObject.SetActive(true);

#if UNITY_EDITOR

#else
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
#endif

        _thanksText.SetActive(GameSession.IsGameOver);

#if UNITY_WEBGL
        _quitBtn.SetActive(false);
#endif

        if (!GameSession.IsGameOver)
        {
            _music.pitch = 0.4f;
        }

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