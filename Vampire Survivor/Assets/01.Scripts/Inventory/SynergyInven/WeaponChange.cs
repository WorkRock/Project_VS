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
    public Item startItem;      // ���� ����
    public Item saveItemData;   // ���� �������� ����
    */
    //////////////// �׽�Ʈ //////////////////
    public Item[] startItems;   // ���� ���� -> �ν����Ϳ��� ����
    // �������� ����, 0���� main, 1���� sub ������ ������
    public Item[] saveItemDatas;

    // ������ ����, 0���� main, 1���� sub ������ ������
    public Item[] getItemDatas;

    // �������� ����
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
        ////////////// �׽�Ʈ ////////////
        // ���� �� ������ �޾ƿ´�.
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
        // ���� �� �ֹ���� �÷��̾ �������� ����
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

    // OŰ�� ������ ��
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
            alertText.text = "�� ����� ��� ���� �� �����ϴ�!".ToString();
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

    // ������ ����� �ֹ��� ��ư�� ������ �� ȣ��
    public void checkMain()
    {
        isMain = true;
        weaponUION = true;
        weaponUI();
        weaponChangeCheck.SetActive(false);
    }

    // ������ ����� �������� ��ư�� ������ �� ȣ��
    public void checkSub()
    {
        isMain = false;
        weaponUION = true;
        weaponUI();
        weaponChangeCheck.SetActive(false);
    }

    /*
    // ItemController���� �������� �Ծ��� �� ȣ��Ǵ� �޼���
    public void ToSaveItemData(Item _item)
    {
        weaponChangeCheck.SetActive(true);
        saveItemData = _item; 
    }
    */

    /////////// �׽�Ʈ //////////////
    public void ToSaveItemDatas(Item _mainItem, Item _subItem)
    {
        weaponChangeCheck.SetActive(true);
        // �������� �Ծ��� �� ���� �������� main�� sub�� ������ ����ȴ�.
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
            // �������� main����� �Ծ��� ��
            case true:
                //////////// �׽�Ʈ
                if(getItemDatas[0] == mainWeapon && !isSwapWeapon && subWeapon != null)
                {
                    Alert.SetActive(true);
                    alertText.text = "�ش� ���Կ� ���� ���Ⱑ �����Ǿ� �ֽ��ϴ�!".ToString();
                    Time.timeScale = 0;
                    break;
                }

                ///////////// �׽�Ʈ
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

            // �������� sub����� �Ծ��� ��
            case false:
                ////////// �׽�Ʈ
                if(getItemDatas[1] == subWeapon && !isSwapWeapon)
                {
                    Alert.SetActive(true);
                    alertText.text = "�ش� ���Կ� ���� ���Ⱑ �����Ǿ� �ֽ��ϴ�!".ToString();
                    Time.timeScale = 0;
                    break;
                }

                ////////// �׽�Ʈ
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
