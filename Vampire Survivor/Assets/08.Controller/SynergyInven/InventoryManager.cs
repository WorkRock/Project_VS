using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> items;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

    [SerializeField]
    private Slot slot;
    [SerializeField]
    private Transform Content;

    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }

    void Awake()
    {
        FreshSlot();
    }

    public void FreshSlot()
    {
        int i = 0;
        for (; i < items.Count ; i++)
        {
            slots[i].item = items[i];
        }
        for (; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }

    public void AddItem(Item _item)
    {
        items.Add(_item);
        Instantiate(slot, slotParent);
        slots = slotParent.GetComponentsInChildren<Slot>();
        FreshSlot();

        /*
        if (items.Count < slots.Length)
        {
            items.Add(_item);
            FreshSlot();
        }
        else
        {
            print("슬롯이 가득 차 있습니다.");
        }
        */
    }
    
    public void DeleteItem(Item _item)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i] == _item)
            {
                items.RemoveAt(i);
                Slot[] slotChild = slots[items.Count].GetComponentsInChildren<Slot>();
                
                for(int x = 0; x < slotChild.Length; x++)
                    Destroy(slotChild[x].gameObject);
                FreshSlot();
                break;
            }
                
        }
    }



    /*
    public void Swap(int count)
    {
        Item _item;

        for(int i = count; i < items.Count; i++)
        {
            _item = items[i+1];
            items[i] = _item;
        }
        
    }

    /*
    public GameObject Inventory;

    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    public Toggle EnableRemove;

    public InventoryItemController[] InventoryItems;

    public GameObject inventoryButton;

    private int count;
    private void Awake()
    {
        Instance = this;
        
    }

    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
        count--;
    }

    public void ListItems()
    {
        EnableRemove.isOn = false;
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }


        foreach(var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var removeButton = obj.transform.Find("RemoveButton").GetComponent<Button>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.itemImage;

            if (EnableRemove.isOn)
                removeButton.gameObject.SetActive(true);
        }

        if(Items.Count > InventoryItems.Length)
            SetInventoryItems();
    }

    public void EnableItemsRemove()
    {
        if(EnableRemove.isOn)
        {
            foreach(Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(true);
            }
        }
        
        else
        {
            foreach (Transform item in ItemContent)
            {
                item.Find("RemoveButton").gameObject.SetActive(false);
            }
        }
    }

    public void enableInventory()
    {
        if (inventoryButton.activeSelf)
        {
            InventoryItems = new InventoryItemController[count];
            Inventory.SetActive(false);
        }
        else
            Inventory.SetActive(true);
    }



    public void SetInventoryItems()
    {       
        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();

        for(int i = 0; i < Items.Count; i++)
        {
            InventoryItems[i].AddItem(Items[i]);
            count++;
            Debug.Log("아이템 추가");
        }
    }
    */
}
