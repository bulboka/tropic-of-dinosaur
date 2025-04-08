using UnityEngine;

public class HideOnStart : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
}