using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{

    void Start() {
        Cursor.visible = false;
    }
    void Update()
    {
        
        Vector2 mousePosition = Input.mousePosition;
        transform.position = mousePosition;

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
    }
}
