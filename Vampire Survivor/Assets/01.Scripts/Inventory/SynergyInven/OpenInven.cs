using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInven : MonoBehaviour
{
    public GameObject inventoryUI;
    public InventoryManager inventory;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            openinventoryUI();
        }
    }

    public void openinventoryUI()
    {
        if (inventoryUI.activeSelf)
        {
            inventoryUI.SetActive(false);
            Time.timeScale = 1f;
            return;
        }
        else
        {
            inventory.FreshSlot();
            inventoryUI.SetActive(true);
            Time.timeScale = 0f;
            return;
        }
    }
}
