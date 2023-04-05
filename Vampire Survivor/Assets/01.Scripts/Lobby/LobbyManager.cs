using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public Transform prefabParent;
    public GameObject[] prefabs;

    public GameObject CharacterSel;
    public GameObject Info;
    public GameObject UGPanel;

    public ClassBtn BtnPrefab;

    public Text className;
    public Text classInfo;

    public int startChar;

    public Transform content;


    public Text UGLv;


    void Awake()
    {
        CharacterSel.SetActive(false);
        Info.SetActive(false);
        delChild();

        DataManager.instance.JsonLoad();
        UGLv.text = "Now Level : " + PlayerData.UGLv_AllDmg.ToString();
    }

    public void saveData()
    {
        DataManager.instance.JsonSave();
    }

    public void resetData()
    {
        DataManager.instance.JsonReset();
        UGLv.text = "Now Level : " + PlayerData.UGLv_AllDmg.ToString();
    }
    public void loadData()
    {
        DataManager.instance.JsonLoad();
        UGLv.text = "Now Level : " + PlayerData.UGLv_AllDmg.ToString();
    }

    public void AllDmg()
    {
        PlayerData.Gold = PlayerData.Gold + 200;
        PlayerData.UGLv_AllDmg = PlayerData.UGLv_AllDmg + 1;
        UGLv.text = "Now Level : " + PlayerData.UGLv_AllDmg.ToString();
    }

    public void clickGameStart()
    {
        CharacterSel.SetActive(true);
    }

    public void exitGameStart()
    {
        CharacterSel.SetActive(false);
    }

    public void clickUGPanel()
    {
        UGPanel.SetActive(true);
    }
    public void exitUGPanel()
    {
        UGPanel.SetActive(false);
    }

    public void InitPrefab(int i)
    {
        InfoActiveAndDelChild();
        startChar = i;
        Instantiate(prefabs[i], prefabParent);
        PlayerData.charNum = startChar;

        switch (i)
        {
            case 0:
                className.text = "기사";
                classInfo.text = "자신의 강함을 증명하기 위해 던전을 오게 되었습니다." +
                    "\n<b>치명타 증가 +10%</b>";
                break;
            case 1:
                className.text = "궁수";
                classInfo.text = "사라진 동료를 찾기 위해 던전에 발을 들입니다." +
                    "\n<b>공격속도 증가 +10%</b>";
                break;
            case 2:
                break;
        }
    }

    public void click01()
    {
        InitPrefab(0);
    }

    public void click02()
    {
        InitPrefab(1);
    }
    
    public void InfoActiveAndDelChild()
    {
        if (!Info.activeSelf)
            Info.SetActive(true);

        delChild();        
    }

    public void delChild()
    {
        if (prefabParent.childCount != 0)
        {
            foreach (Transform child in prefabParent)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void startIngame()
    {
        Debug.Log("Load Scene : Ingame");
        // SceneManager.LoadScene("Lobby");
        /*
        CharacterSel.SetActive(false);
        Info.SetActive(false);
        delChild();
         */
        // 싱글톤 전달해줄 값 스크립트 작성 필요
        SceneManager.LoadScene("Ingame");
    }
}
