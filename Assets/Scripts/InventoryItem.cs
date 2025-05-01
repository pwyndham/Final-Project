
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //public Item item;
    public Image image;
    public Transform originalParentReference;
    void Start()
    {
        //InitializeItem(item);
    }
    // public void InitializeItem(Item newItem) {
    //     //item = newItem;
    //     image.sprite = newItem.image;
    // }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParentReference = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParentReference);
        image.raycastTarget = true;
    }

    
}
