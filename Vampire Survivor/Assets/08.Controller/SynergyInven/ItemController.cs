using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour, IObjectItem
{
    [Header("아이템")]
    public Item mainItem;   // 메인 무기 정보
    public Item subItem;    // 서브 무기 정보
    [Header("아이템 이미지")]
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
        // 플레이어가 아이템을 먹을 때
        if (collision.gameObject.tag == "Player")
        {
            /*
            if (mainItem != null)
            {
                WeaponChange.Instance.ToSaveItemData(mainItem);
            }
            */
            //////////// 테스트 /////////////////
            if(mainItem != null && subItem != null)
            {
                WeaponChange.Instance.ToSaveItemDatas(mainItem, subItem);
            }

            weaponChangeCheck.SetActive(true);
            Time.timeScale = 0;
            // 보조무기 스프라이트 변경
            //if (GameManager.instance.player.leftWeapon.sprite == null)
            //    GameManager.instance.player.leftWeapon.sprite = itemImage.sprite;
            gameObject.SetActive(false);
        }
    }

}
