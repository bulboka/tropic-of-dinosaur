using UnityEngine;

public class FanfurikEnterTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;
    [SerializeField] private float _heartScale;
    [SerializeField] private float _skullScale;

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameObject.SetActive(false);

        foreach (var heart in FindObjectsOfType<ChickenHeart>())
        {
            heart.gameObject.layer = LayerMask.NameToLayer("ChickenHeartsGround");
            heart.transform.localScale = Vector3.one * _heartScale;
            heart.FreeFollowLiftingForce = 30000f;
        }

        foreach (var skull in FindObjectsOfType<Skull>())
        {
            skull.transform.localScale = Vector3.one * _skullScale;
        }

        GameSession.Hand.MaxForce = 4000f;
        //GameSession.Hand.AirbornForceMult = 0.1f;
    }
}