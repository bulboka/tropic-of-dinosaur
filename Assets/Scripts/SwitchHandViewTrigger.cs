using UnityEngine;

public class SwitchHandViewTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _handViewPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var newView = Instantiate(_handViewPrefab);
        GameSession.Hand.SetView(newView);
        gameObject.SetActive(false);
    }
}