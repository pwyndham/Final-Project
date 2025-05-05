using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public Transform PlayerTransform => transform;

    private void Awake()
    {
        Instance = this;
    }
}