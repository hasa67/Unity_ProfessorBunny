using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData) {
        if (transform.childCount == 0) {
            GameObject dropped = eventData.pointerDrag;
            DraggableObject doj = dropped.GetComponent<DraggableObject>();
            doj.parentAfterDrag = transform;
            doj.transform.rotation = transform.rotation;
            // Debug.Log(doj.GetComponent<RectTransform>().localRotation.eulerAngles);
        }
    }
}
