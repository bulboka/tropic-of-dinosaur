using UnityEngine;

public class BodyFire : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        if (Target == null)
        {
            return;
        }

        transform.position = Target.position;
    }
}