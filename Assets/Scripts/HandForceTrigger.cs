using UnityEngine;

public class HandForceTrigger : MonoBehaviour
{
    [SerializeField] private float _minForce;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _airbornForceMult;

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);
        GameSession.Hand.MinForce = _minForce;
        GameSession.Hand.MaxForce = _maxForce;
        GameSession.Hand.AirbornForceMult = _airbornForceMult;
    }
}