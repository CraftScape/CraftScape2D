﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public int slotIndex;

	// public InventoryItem gameItem { get; set; }

    InventoryController inventoryController;

	public GameObject draggedItem;

    GameObject owner;

	public void OnBeginDrag(PointerEventData eventData)
    {
		owner = GameObject.FindWithTag("Player");
        inventoryController = owner.GetComponent<InventoryController>();
		draggedItem = inventoryController.OnBeginDragInventoryItem(slotIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
		if (draggedItem != null)
		{
			draggedItem.transform.position = eventData.position;

			GameObject player = GameObject.FindWithTag("Player");
            inventoryController = player.GetComponent<InventoryController>();
			inventoryController.OnDragInventoryItem(eventData);
		}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject player = GameObject.FindWithTag("Player");
        inventoryController = player.GetComponent<InventoryController>();
		inventoryController.OnEndDragInventoryItem(this.slotIndex);
		draggedItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
		draggedItem = null;
    }
	
	public void OnDropInventoryItem(GameObject dropped)
	{	
		GameObject player = GameObject.FindWithTag("Player");
        inventoryController = player.GetComponent<InventoryController>();

		if (dropped.tag == "EquipmentSlot")
		{
			inventoryController.OnDropEquipmentItem(gameObject, dropped);
		}
		else if (dropped.tag == "InventorySlot")
		{
			inventoryController.SwapInventorySlots(gameObject, dropped);
		}
	}

}
