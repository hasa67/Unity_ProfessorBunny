using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    [HideInInspector] public Transform parentAfterDrag;

    public void OnBeginDrag(PointerEventData eventData) {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.parent.SetAsLastSibling();
        GetComponent<Image>().raycastTarget = false;
        GetComponent<CanvasGroup>().alpha = 0.7f;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parentAfterDrag);
        GetComponent<Image>().raycastTarget = true;
        GetComponent<CanvasGroup>().alpha = 1f;
        transform.localScale = Vector3.one;
    }

}
