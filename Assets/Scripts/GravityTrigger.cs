using UnityEngine;

public class GravityTrigger : MonoBehaviour
{
    [SerializeField] private Vector2 _gravityValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Body"))
        {
            return;
        }

        Physics2D.gravity = _gravityValue;
        gameObject.SetActive(false);
    }
}