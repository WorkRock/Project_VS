using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    Item item;

    public Button RemoveButton;

    public void RemoveItem()
    {
        //InventoryManager.Instance.Remove(item);
        Destroy(gameObject);
    }


    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public void useItem()
    {
        /*
       switch(item.itemType)
        {
            case Item.ItemType.Weapon:
                WeaponChange.Instance.weaponUI(item);
                    //transform.Find("ItemIcon").GetComponent<Image>();
                break;
        }
        */
    }

}
