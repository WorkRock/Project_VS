using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChange : MonoBehaviour
{
    public static WeaponChange Instance;
    public TestWeaponManager weaponManager;

    public GameObject Inventory;
    public GameObject weaponChangeCheck;
    public GameObject Alert;

    public Text alertText;

    public Image nowMainWeaponImg;
    //public Text weaponMainText;
    public Image nowSubWeaponImg;
    //public Text weaponSubText;

    /*
    public Item startItem;      // 시작 무기
    public Item saveItemData;   // 현재 착용중인 무기
    */
    //////////////// 테스트 //////////////////
    public Item[] startItems;   // 시작 무기 -> 인스펙터에서 연결
    // 착용중인 무기, 0번엔 main, 1번엔 sub 정보를 담을것
    public Item[] saveItemDatas;

    // 습득한 무기, 0번엔 main, 1번엔 sub 정보를 담을것
    public Item[] getItemDatas;

    // 착용중인 무기
    public Item mainWeapon;
    public Item subWeapon;
    
    public bool isMain;
    public bool isSwapWeapon;
    public bool weaponUION;
    //public InventoryManager inventory;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ////////////// 테스트 ////////////
        // 시작 시 정보를 받아온다.
        saveItemDatas[0] = GameManager.instance.allWeaponData[PlayerData.charNum];
        saveItemDatas[1] = GameManager.instance.allWeaponData[PlayerData.charNum];
        //saveItemDatas[0] = startItems[0];
        //saveItemDatas[1] = startItems[1];
        getItemDatas[0] = saveItemDatas[0];
        getItemDatas[1] = saveItemDatas[1];

        isSwapWeapon = false;
        //mainWeapon = startItems[0];
        //saveItemData = startItem;
        isMain = true;
        weaponUION = true;
        weaponUI();
        /*
        // 시작 시 주무기는 플레이어가 착용중인 무기
        nowMainWeaponImg.sprite = GameManager.instance.player.rightWeapon.sprite;
        /*nowMainWeaponImg.color = new Color(nowMainWeaponImg.color.r, 
            nowMainWeaponImg.color.g, nowMainWeaponImg.color.b, 0f);*/

        nowSubWeaponImg.color = new Color(nowSubWeaponImg.color.r,
            nowSubWeaponImg.color.g, nowSubWeaponImg.color.b, 0f);
    }

    public bool getWeaponUION()
    {
        return weaponUION;
    }

    // O키를 눌렀을 때
    public void swapWeapon()
    {
        if(subWeapon != null)
        {
            /*
            isMain = true;
            saveItemData = mainWeapon;
            mainWeapon = subWeapon;
            subWeapon = saveItemData;
            */

            isMain = true;
 
            mainWeapon = saveItemDatas[2];
            subWeapon = saveItemDatas[1];

            for(int i = 0; i < 2; i++)
            {
                Item tempData = saveItemDatas[i];

                saveItemDatas[i] = saveItemDatas[i+2];
                saveItemDatas[i+2] = tempData;
            }
           
            
            
            for (int i = 0; i < 2; i++)
            {
                if (isMain) isMain = false;
                else isMain = true;
                weaponUION = true;
                isSwapWeapon = true;
                weaponUI();
            }
        }

        else
        {
            alertText.text = "주 무기는 비워 놓을 수 없습니다!".ToString();
            GameManager.instance.player.canSwap = true;
            Time.timeScale = 0;
            Alert.SetActive(true);
        }
    }

    public Item getMainWeapoon()
    {
        return mainWeapon;
    }

    public Item getSubWeapoon()
    {
        return subWeapon;
    }

    // 아이템 습득시 주무기 버튼을 눌렀을 때 호출
    public void checkMain()
    {
        isMain = true;
        weaponUION = true;
        weaponUI();
        weaponChangeCheck.SetActive(false);
    }

    // 아이템 습득시 보조무기 버튼을 눌렀을 때 호출
    public void checkSub()
    {
        isMain = false;
        weaponUION = true;
        weaponUI();
        weaponChangeCheck.SetActive(false);
    }

    /*
    // ItemController에서 아이템을 먹었을 때 호출되는 메서드
    public void ToSaveItemData(Item _item)
    {
        weaponChangeCheck.SetActive(true);
        saveItemData = _item; 
    }
    */

    /////////// 테스트 //////////////
    public void ToSaveItemDatas(Item _mainItem, Item _subItem)
    {
        weaponChangeCheck.SetActive(true);
        // 아이템을 먹었을 때 먹은 아이템의 main과 sub의 정보가 저장된다.
        //saveItemDatas[0] = _mainItem;
        //saveItemDatas[1] = _subItem;
        getItemDatas[0] = _mainItem;
        getItemDatas[1] = _subItem;
    }

    public void AlertOff()
    {
        Alert.SetActive(false);
        if(!Inventory.activeSelf) Time.timeScale = 1;
    }

    public void weaponUI()
    {
        switch (isMain)
        {
            // 아이템을 main무기로 먹었을 때
            case true:
                //////////// 테스트
                if(getItemDatas[0] == mainWeapon && !isSwapWeapon && subWeapon != null)
                {
                    Alert.SetActive(true);
                    alertText.text = "해당 슬롯에 같은 무기가 장착되어 있습니다!".ToString();
                    Time.timeScale = 0;
                    break;
                }

                ///////////// 테스트
                if (!isSwapWeapon)
                {
                    Time.timeScale = 1;
                    mainWeapon = getItemDatas[0];
                    for (int i = 0; i < 2; i++)
                    {
                        saveItemDatas[i] = getItemDatas[i];
                    }
                    
                }

                nowMainWeaponImg.color = new Color(nowMainWeaponImg.color.r,
            nowMainWeaponImg.color.g, nowMainWeaponImg.color.b, 1f); ;
                nowMainWeaponImg.sprite = mainWeapon.itemImage;
                GameManager.instance.player.setMainWeapon(mainWeapon);
                GameManager.instance.player.setIsMain(isMain);
                GameManager.instance.player.ChangeWeapon();
                
                //inventory.DeleteItem(saveItemData);
                weaponManager.WeaponChangeCheck();
                break;

            // 아이템을 sub무기로 먹었을 때
            case false:
                ////////// 테스트
                if(getItemDatas[1] == subWeapon && !isSwapWeapon)
                {
                    Alert.SetActive(true);
                    alertText.text = "해당 슬롯에 같은 무기가 장착되어 있습니다!".ToString();
                    Time.timeScale = 0;
                    break;
                }

                ////////// 테스트
                if (!isSwapWeapon)
                {
                    Time.timeScale = 1;
                    subWeapon = getItemDatas[1];
                    for (int i = 0; i < 2; i++)
                    {
                        saveItemDatas[i+2] = getItemDatas[i];
                    }
                }

                nowSubWeaponImg.color = new Color(nowSubWeaponImg.color.r,
            nowSubWeaponImg.color.g, nowSubWeaponImg.color.b, 1f);
                nowSubWeaponImg.sprite = subWeapon.itemImage;
                GameManager.instance.player.setSubWeapon(subWeapon);
                GameManager.instance.player.setIsMain(isMain);
                GameManager.instance.player.ChangeWeapon();             
                weaponManager.WeaponChangeCheck();
                break;
        }
        
        if (isSwapWeapon) isSwapWeapon = false;
        /*
        if (checkMainOn)
        {
            checkMainOn = false;
        }
        */
        weaponUION = false;
    }
}
