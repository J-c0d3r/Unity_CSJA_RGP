using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public static GameObject ItemBeginDragged;

    private Vector3 startPos;
    private Transform startParent;

    public Item item;

    void Start()
    {
        GetComponent<Image>().sprite = item.icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemBeginDragged = gameObject;
        startPos = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ItemBeginDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if(transform.parent == startParent)
        {
            transform.position = startPos;
        }
    }

    public void SetParent(Transform slotTransform, Slots slot)
    {
        if (item.SlotType.ToString() ==  slot.SlotType.ToString())
        {
            transform.SetParent(slotTransform);
            item.GetAction();
        }
        else if (slot.SlotType.ToString() == "inventory")
        {
            transform.SetParent(slotTransform);
            item.RemoveStats();
        }
    }


}
