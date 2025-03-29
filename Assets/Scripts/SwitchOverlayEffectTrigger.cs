using UnityEngine;

public class SwitchOverlayEffectTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _overlayEffectPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameSession.Camera.SwitchOverlayEffect(_overlayEffectPrefab);
        gameObject.SetActive(false);
    }
}