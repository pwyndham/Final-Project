using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    Camera m_MainCamera;

    void Awake()
    {
        m_MainCamera = Camera.main;
    }
    void Update()
    {
        Vector3 direction = transform.position - m_MainCamera.transform.position;
        direction.y = 0f; // Clamp vertical rotation to avoid glitchy look.
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
