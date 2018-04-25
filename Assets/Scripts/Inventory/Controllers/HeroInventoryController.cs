using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class HeroInventoryController : InventoryController
{

    public void SetupInventory()
    {
        // Get reference to main canvas object
        GameObject mainCanvas = GameObject.FindWithTag("MainCanvas");

        // Get reference to equipment inventory container
        GameObject equipmentInventoryContainer = mainCanvas.transform.Find("EquipmentInventoryContainer").gameObject;

        // Instantiate inventory panel
        base.inventoryPanel = Instantiate(inventoryPanelPrefab, Vector3.zero, Quaternion.identity, equipmentInventoryContainer.transform);

        // Tell inventory what file to write objects to
        // base.inventory.SetInventoryFileName(string.Format("inventory-{0}.json", GetComponent<SetupLocalHero>().netId.ToString()));

        // Load inventory items from file
        // base.inventory.LoadInventory();

        // Layout/render inventory on canvas
        base.LayoutInventory();
    }

	void ToggleInventory()
	{
		if (GameObject.FindWithTag ("Player").GetComponent<SetupLocalHero> ().isLocalPlayer) {

			if (Input.GetKeyDown (KeyCode.B)) 
			{
				base.ToggleInventory();
			}

		}
	}


}
