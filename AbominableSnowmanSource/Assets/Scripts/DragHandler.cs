using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged;
    private static GameObject trap;
    Vector3 startPosition;
    //checker to ensure that each item is only dragged once
    bool alreadyDragged = false;

    [SerializeField]    private GameObject trapType;

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        Debug.Log("on begin drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if (!alreadyDragged)
        {
            transform.position = Input.mousePosition;
        }
        Debug.Log("on drag, pos: (" + transform.position.x + ", " + transform.position.y + ")");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(OutOfBounds())
        {
            itemBeingDragged = null;
            transform.position = startPosition;
        }
        else
        {
            trap = (GameObject) Instantiate(trapType, Input.mousePosition, Quaternion.identity);
            trap.gameObject.transform.SetParent(null);
            transform.position = startPosition;
            alreadyDragged = true;
        }
        Debug.Log("on end drag");
    }

    /// <summary>
    /// check if item being dragged is out of bounds
    /// </summary>
    /// <returns>true if item is being dragged to an illegal position</returns>
    private bool OutOfBounds()
    {
        //top of cliff
        if(transform.position.y > 241)
        {
            return true;
        } 
        else if(transform.position.x < 62 || transform.position.x > 987) //cliff sides
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
