using UnityEngine;

public class BodyFire : MonoBehaviour
{
    private void Update()
    {
        transform.position = GameSession.Body.FireLocator.position;
    }
}