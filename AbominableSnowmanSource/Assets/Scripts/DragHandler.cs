using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{
    public static GameObject itemBeingDragged;
    Vector3 startPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        transform.position = startPosition;
    }
}
