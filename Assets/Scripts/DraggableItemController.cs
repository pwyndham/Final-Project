
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;

// public class DraggableItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
// {

//     p
//     public Image image;
//     public Transform originalParentReference;
//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         originalParentReference = transform.parent;
//         transform.SetParent(transform.root);
//         transform.SetAsLastSibling();
//         image.raycastTarget = false;
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         transform.position = Input.mousePosition;
//     }

//     public void OnEndDrag(PointerEventData eventData)
//     {
//         transform.SetParent(originalParentReference);
//         image.raycastTarget = true;
//     }

    
// }
