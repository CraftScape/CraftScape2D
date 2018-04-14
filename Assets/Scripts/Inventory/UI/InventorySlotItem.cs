﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotItem : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject hoverTextPrefab;
    GameObject hoverText;
    GameItem item;
	InventorySlot parent;

    Vector3 offset = new Vector3(-125f, -42f, 0f);

    public void OnDrop(PointerEventData eventData)
    {
		if (eventData.button == PointerEventData.InputButton.Left) {
            GameObject parentContainer = transform.parent.gameObject;
            InventorySlot inventorySlot = parentContainer.GetComponent<InventorySlot>();
            inventorySlot.OnDropInventoryItem(eventData.pointerDrag.gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
		UpdateItem ();
        StartHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		UpdateItem ();
        StopHover();
    }

    void StartHover() {
		GameObject canvas = GameObject.FindWithTag("MainCanvas");
		hoverText = GameObject.Instantiate(hoverTextPrefab);
		hoverText.transform.SetParent(canvas.transform);
    }

    void StopHover() {
        if (hoverText != null)
            Destroy(hoverText);
    }

    void Update() {

		if (hoverText != null && Input.GetMouseButton(1)) {
			
			hoverText.transform.position = Input.mousePosition + offset;

			if (Input.GetMouseButtonDown(1)) {
				UpdateItem ();
				hoverText.GetComponentInChildren<Text> ().text = string.Format ("Name: {0}\n      ID: {1}\n UUID: {2}", item.Name, item.Id.ToString(), item.Uuid);
			}

		} else if (hoverText != null) {
			hoverText.transform.position = new Vector3 (1000f, 1000f, 0);
		}
    }

	void UpdateItem()
	{
		parent = transform.parent.gameObject.GetComponent<InventorySlot> ();
		GameObject player = GameObject.FindWithTag("Player");
		InventoryController controller = player.GetComponent<InventoryController>();
		item = controller.inventory.GameItems[parent.slotIndex];
	}
}