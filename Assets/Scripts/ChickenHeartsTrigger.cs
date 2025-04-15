using UnityEngine;

public class ChickenHeartsTrigger : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private float _width;
    [SerializeField] private float _height;

    private int _nextLocatorIndex;
    public Vector3 GetNextChickenHeartPosition()
    {
        var nextLocator = transform.GetChild(_nextLocatorIndex);

        _nextLocatorIndex++;

        if (_nextLocatorIndex == transform.childCount)
        {
            _nextLocatorIndex = 0;
        }

        return nextLocator.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Body"))
        {
            return;
        }

        GameSession.ChickenHeartsManager.SummonHeartsTo(this);

        var hearts = Physics2D.OverlapBoxAll(transform.position, new Vector2(_width, _height), 0,
            LayerMask.GetMask("ChickenHearts"));

        foreach (var heart in hearts)
        {
            var chickenHeart = heart.GetComponent<ChickenHeart>();

            if (chickenHeart != null)
            {
                chickenHeart.State = ChickenHeartState.Awake;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_width, _height, 0));
    }
}