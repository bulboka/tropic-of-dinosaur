using UnityEngine;

public class ChickenHeartsFreeFollowTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);
        GameSession.ChickenHeartsManager.FreeHearts();
    }
}