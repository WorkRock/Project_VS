using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour, IObjectItem
{
    [Header("������")]
    public Item mainItem;   // ���� ���� ����
    public Item subItem;    // ���� ���� ����
    [Header("������ �̹���")]
    public SpriteRenderer itemImage;

    public GameObject weaponChangeCheck;
    public GameObject Alert;

    void Start()
    {
        itemImage.sprite = mainItem.itemImage;
    }

    public Item ClickItem()
    {
        return this.mainItem;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ �������� ���� ��
        if (collision.gameObject.tag == "Player")
        {
            /*
            if (mainItem != null)
            {
                WeaponChange.Instance.ToSaveItemData(mainItem);
            }
            */
            //////////// �׽�Ʈ /////////////////
            if(mainItem != null && subItem != null)
            {
                WeaponChange.Instance.ToSaveItemDatas(mainItem, subItem);
            }

            weaponChangeCheck.SetActive(true);
            Time.timeScale = 0;
            // �������� ��������Ʈ ����
            //if (GameManager.instance.player.leftWeapon.sprite == null)
            //    GameManager.instance.player.leftWeapon.sprite = itemImage.sprite;
            gameObject.SetActive(false);
        }
    }

}
