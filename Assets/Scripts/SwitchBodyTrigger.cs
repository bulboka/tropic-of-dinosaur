using UnityEngine;

public class SwitchBodyTrigger : MonoBehaviour
{
    [SerializeField] private Body _bodyPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Body"))
        {
            return;
        }

        GameSession.SwitchBody(_bodyPrefab);
        gameObject.SetActive(false);
    }
}