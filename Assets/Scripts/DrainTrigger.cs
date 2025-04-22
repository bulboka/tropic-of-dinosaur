using UnityEngine;

public class DrainTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _antiDrainEffector;
    [SerializeField] private float _removeAntiDrainDelay;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private BodyFire _bodyFirePrefab;

    private float _removeAntiDrainTime = -1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Body"))
        {
            return;
        }

        _collider.enabled = false;

        var stubs = GameObject.FindGameObjectsWithTag("Stub");

        foreach (var stub in stubs)
        {
            stub.gameObject.layer = LayerMask.NameToLayer("Stub");
        }

        _removeAntiDrainTime = Time.time + _removeAntiDrainDelay;

        var bodyFire = Instantiate(_bodyFirePrefab);
        bodyFire.Target = GameSession.Body.FireLocator;
    }

    private void Update()
    {
        if (_removeAntiDrainTime < 0 || Time.time < _removeAntiDrainTime)
        {
            return;
        }

        _antiDrainEffector.SetActive(false);
        gameObject.SetActive(false);
    }
}