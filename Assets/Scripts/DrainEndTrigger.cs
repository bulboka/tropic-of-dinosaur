using UnityEngine;

public class DrainEndTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameObject _antiDrainEffector;

    private ContactFilter2D _contactFilter;
    private Collider2D[] _overlapResults = {};

    private void Start()
    {
        _contactFilter = new ContactFilter2D
        {
            layerMask = LayerMask.NameToLayer("Stub"),
            useTriggers = true,
            useLayerMask = true
        };
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_collider.OverlapCollider(_contactFilter, _overlapResults) == 0)
        {
            _antiDrainEffector.SetActive(false);
        }
    }
}