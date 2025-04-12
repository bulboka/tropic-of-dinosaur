using UnityEngine;

public class SkullSpawnTimerTrigger : MonoBehaviour
{
    [SerializeField] private float _timerDuration;

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);

        if (GameSession.SkullManager.NextSkullPrefabIndex > 0)
        {
            return;
        }

        GameSession.SkullManager.SetTimer(_timerDuration);
    }
}