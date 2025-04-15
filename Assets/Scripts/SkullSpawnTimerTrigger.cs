using UnityEngine;

public class SkullSpawnTimerTrigger : MonoBehaviour
{
    [SerializeField] private float _timerDuration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Body"))
        {
            return;
        }

        gameObject.SetActive(false);

        if (GameSession.SkullManager.NextSkullPrefabIndex > 0)
        {
            return;
        }

        GameSession.SkullManager.SetTimer(_timerDuration);
    }
}