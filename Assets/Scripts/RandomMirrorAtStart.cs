using UnityEngine;
using Random = UnityEngine.Random;

public class RandomMirrorAtStart : MonoBehaviour
{
    private void Start()
    {
        if (Random.value <= 0.5f)
        {
            var scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }
}