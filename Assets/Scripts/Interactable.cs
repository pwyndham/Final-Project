using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    
    public string promptMessage;

    public void baseInteract()
    {
        Interact();
    }
    protected virtual void Interact()
    {

    }

}
