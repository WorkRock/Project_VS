using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Text text;
    //[SerializeField] GameObject Equip;
    private Item _item;
    public Item item
    {
        get { return _item; }
        set
        {
            _item = value;
            if (_item != null)
            {
                image.sprite = item.itemImage;
                text.text = item.itemName;
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                text.text = "";
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void WeaponUIOn()
    {
        if(_item != null)
        {
            //WeaponChange.Instance.ToSaveItemData(_item);
        }

        /*
        if(_item.id == WeaponChange.Instance.checkMainCode() || _item.id == WeaponChange.Instance.checkSubCode())
        {
            Equip.SetActive(true);
        }
        else
            Equip.SetActive(false);
        /*
        switch (item.atkType)
        {
            case Item.at.Weapon:
                WeaponChange.Instance.weaponUI(item);
                //transform.Find("ItemIcon").GetComponent<Image>();
                break;
        }*/
    }
}
