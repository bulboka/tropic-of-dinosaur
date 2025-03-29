using UnityEngine;

public class SwitchBodyTrigger : MonoBehaviour
{
    [SerializeField] private Body _bodyPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameSession.SwitchBody(_bodyPrefab);
        gameObject.SetActive(false);
    }
}