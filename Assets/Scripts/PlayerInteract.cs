using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
   
   [SerializeField] private LayerMask mask;
   private PlayerUIHandler playerUI;
   float maxDistance = 100f;
    void Interact() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Debug.DrawRay(ray.origin,ray.direction * 100f);
            //Vector3 direction = (hit.point).normalized;
            
        }
    }
 
    void Update()
    {

        playerUI.UpdateText(string.Empty);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, mask))
        {
            // Debug.DrawRay(ray.origin,ray.direction * maxDistance, Color.red);
            // Debug.Log($"Hit: {hit.collider.gameObject.name} at {hit.point}");

            if  (hit.collider.GetComponent<Interactable>() != null) 
            {

                //Debug.Log(hit.collider.GetComponent<Interactable>().promptMessage);
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                playerUI.UpdateText(hit.collider.GetComponent<Interactable>().promptMessage);

                if (Input.GetKeyDown(KeyCode.E)){
                    interactable.baseInteract();
                    Debug.Log("Interact Button Pressed");
                }
        
            }
            
            //Vector3 direction = (hit.point).normalized;
            
        }  
    }
    void Start()
    {
        playerUI = GetComponent<PlayerUIHandler>();
    }
}
