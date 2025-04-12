using UnityEngine;

public class SuckerFinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);
        GameSession.SkullManager.StopSucking();
    }
}