using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager instance = null;

    public Transform prefabParent;
    public GameObject[] prefabs;

    #region 비활성화를 요구하는 버튼 오브젝트 모음
    [Header("캐릭터 구입 버튼")]
    public Button buyArcherBtn;
    /*
     * public Button buyArcherBtn;
     * public Button buyThiefBtn;
     * public Button buyMagicianBtn;
     * public Button buySpearWarriorBtn;
     * public Button buyShieldGuyBtn
     * public Button buyBabarianBtn;
    */

    [Header("업그레이드 구입 버튼")]
    public Button AllDmgBtn;
    public Button BasicAtkDmgBtn;
    public Button SynergyDmgBtn;
    public Button AtkSpeedBtn;
    public Button AtkRangeBtn;
    public Button ProjectileSpeedBtn;
    public Button ProjectileCountBtn;
    public Button SkillCTBtn;
    public Button SwapCTBtn;
    public Button GainUltBtn;
    public Button PentBtn;
    public Button MovementSpeedBtn;
    public Button GainGoldBtn;
    public Button GainExpBtn;
    public Button CriBtn;
    public Button MagnetBtn;
    public Button ReviveBtn;
    public Button MaxHPBtn;
    public Button HPRegenBtn;
    public Button ReflectBtn;
    public Button GainDMGBtn;
    #endregion

    public GameObject CharacterSel;
    public GameObject Info;
    public GameObject UGPanel;
    public GameObject AlertUI;

    public Text AlertText;

    public Text className;
    public Text classInfo;

    public Text nowGold;

    public int startChar;

    public Transform content;

    #region 업그레이드 관련 변수들
    [Header("UG_Coin_Balance")]
    [SerializeField] private int AllDmgCoin;
    [SerializeField] private int BasicAtkDmgCoin;
    [SerializeField] private int SynergyDmgCoin;
    [SerializeField] private int AtkSpeedCoin;
    [SerializeField] private int AtkRangeCoin;
    [SerializeField] private int ProjectileSpeedCoin;
    [SerializeField] private int ProjectileCountCoin;
    [SerializeField] private int SkillCTCoin;
    [SerializeField] private int SwapCTCoin;
    [SerializeField] private int GainUltCoin;
    [SerializeField] private int PentCoin;
    [SerializeField] private int MovementSpeedCoin;
    [SerializeField] private int GainGoldCoin;
    [SerializeField] private int GainExpCoin;
    [SerializeField] private int CriCoin;
    [SerializeField] private int MagnetCoin;
    [SerializeField] private int ReviveCoin;
    [SerializeField] private int MaxHPCoin;
    [SerializeField] private int HPRegenCoin;
    [SerializeField] private int ReflectCoin;
    [SerializeField] private int GainDMGCoin;

    [Header("UG_MaxLevel_Balance")]
    [SerializeField] private int AllDmgMaxLv;
    [SerializeField] private int BasicAtkDmgMaxLv;
    [SerializeField] private int SynergyDmgMaxLv;
    [SerializeField] private int AtkSpeedMaxLv;
    [SerializeField] private int AtkRangeMaxLv;
    [SerializeField] private int ProjectileSpeedMaxLv;
    [SerializeField] private int ProjectileCountMaxLv;
    [SerializeField] private int SkillCTMaxLv;
    [SerializeField] private int SwapCTMaxLv;
    [SerializeField] private int GainUltMaxLv;
    [SerializeField] private int PentMaxLv;
    [SerializeField] private int MovementSpeedMaxLv;
    [SerializeField] private int GainGoldMaxLv;
    [SerializeField] private int GainExpMaxLv;
    [SerializeField] private int CriMaxLv;
    [SerializeField] private int MagnetMaxLv;
    [SerializeField] private int ReviveMaxLv;
    [SerializeField] private int MaxHPMaxLv;
    [SerializeField] private int HPRegenMaxLv;
    [SerializeField] private int ReflectMaxLv;
    [SerializeField] private int GainDMGMaxLv;
    #endregion
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CharacterSel.SetActive(false);
        UGPanel.SetActive(false);
        Info.SetActive(false);
        delChild();

        DataManager.instance.JsonLoad();
        charSlotOn();
        nowGold.text = PlayerData.Gold.ToString();
        btnRefresh();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            goldChange(8000);
        }

    }
    #region 데이터 관리 메소드
    public void saveData()
    { // 데이터 저장
        DataManager.instance.JsonSave();
    }

    public void resetData()
    { // 데이터 리셋
        DataManager.instance.JsonReset();
        goldChange(0);
        charSlotOn();
        btnRefresh();
    }
    public void loadData()
    { // 데이터 불러오기
        DataManager.instance.JsonLoad();
    }
    #endregion

    #region 캐릭터 슬롯 관련 메소드    
    public void ResetSlot()
    { // 슬롯 자식 초기화
        if (content.childCount != 0)
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void charSlotOn()
    { // 게임 시작 시, 캐릭터 구매 시 캐릭터 슬롯 추가
        ResetSlot();

        for (int i = 0; i < prefabs.Length; i++)
        {
            switch (i)
            {
                case 0:
                    if (PlayerData.isKnightGet) Instantiate(prefabs[i], content);
                    Debug.Log("Knight On");
                    break;
                case 1:
                    if (PlayerData.isArcherGet) Instantiate(prefabs[i], content);
                    break;
                case 2:
                    if (PlayerData.isThiefGet) Instantiate(prefabs[i], content);
                    break;
                case 3:
                    if (PlayerData.isMagicianGet) Instantiate(prefabs[i], content);
                    break;
                case 4:
                    if (PlayerData.isSpearWarriorGet) Instantiate(prefabs[i], content);
                    break;
                case 5:
                    if (PlayerData.isShieldGuyGet) Instantiate(prefabs[i], content);
                    break;
                case 6:
                    if (PlayerData.isBabarianGet) Instantiate(prefabs[i], content);
                    break;

            }
        }
    }
    #endregion

    #region 업그레이드 구매(캐릭터)

    public void buyArcher()
    {
        if (!goldRequire(8000)) return;
        goldChange((-1) * 8000);
        nowGold.text = PlayerData.Gold.ToString();
        AlertUI.SetActive(true);
        AlertText.text = "구매 완료!";
        PlayerData.isArcherGet = true;
        btnRefresh();
        charSlotOn();
        saveData();
    }

    /*
    public void buyThief()
    {
       if (!goldRequire(8000)) return;
        goldChange((-1)*8000);
        nowGold.text = PlayerData.Gold.ToString();
        AlertUI.SetActive(true);
        AlertText.text = "구매 완료!";
        PlayerData.isThiefGet = true;
        btnRefresh();
        charSlotOn();
        saveData();
    }

    public void buyMagician()
    {
        if (!goldRequire(8000)) return;
        goldChange((-1)*8000);
        nowGold.text = PlayerData.Gold.ToString();
        AlertUI.SetActive(true);
        AlertText.text = "구매 완료!";
        PlayerData.isMagicianGet = true;
        btnRefresh();
        charSlotOn();
        saveData();
    }

    public void buySpearWarrior()
    {
        if (!goldRequire(8000)) return;
        goldChange((-1)*8000);
        nowGold.text = PlayerData.Gold.ToString();
        AlertUI.SetActive(true);
        AlertText.text = "구매 완료!";
        PlayerData.isSpearWarriorGet = true;
        btnRefresh();
        charSlotOn();
        saveData();
    }

    public void buyShieldGuy()
    {
        if (!goldRequire(8000)) return;
        goldChange((-1)*8000);
        nowGold.text = PlayerData.Gold.ToString();
        AlertUI.SetActive(true);
        AlertText.text = "구매 완료!";
        PlayerData.isShieldGuyGet = true;
        btnRefresh();
        charSlotOn();
        saveData();
    }

    public void buyBabarian()
    {
        if (!goldRequire(8000)) return;
        goldChange((-1)*8000);
        nowGold.text = PlayerData.Gold.ToString();
        AlertUI.SetActive(true);
        AlertText.text = "구매 완료!";
        PlayerData.isBabarianGet = true;
        btnRefresh();
        charSlotOn();
        saveData();
    }
    */
    #endregion

    #region 업그레이드 구매(스탯)
    public void BuyAllDmg()
    {
        if (!goldRequire((PlayerData.UGLv_AllDmg + 1) * AllDmgCoin) || PlayerData.UGLv_AllDmg >= AllDmgMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_AllDmg + 1) * AllDmgCoin);
        PlayerData.UGLv_AllDmg = PlayerData.UGLv_AllDmg + 1;
    }

    public void BuyBasicAtkDmg()
    {
        if (!goldRequire((PlayerData.UGLv_BasicAtkDmg + 1) * BasicAtkDmgCoin) || PlayerData.UGLv_BasicAtkDmg >= BasicAtkDmgMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_BasicAtkDmg + 1) * BasicAtkDmgCoin);
        PlayerData.UGLv_BasicAtkDmg = PlayerData.UGLv_BasicAtkDmg + 1;
    }

    public void BuySynergyDmg()
    {
        if (!goldRequire((PlayerData.UGLv_SynergyDmg + 1) * SynergyDmgCoin) || PlayerData.UGLv_SynergyDmg >= SynergyDmgMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_SynergyDmg + 1) * SynergyDmgCoin);
        PlayerData.UGLv_SynergyDmg = PlayerData.UGLv_SynergyDmg + 1;
    }

    public void BuyAtkSpeed()
    {
        if (!goldRequire((PlayerData.UGLv_AtkSpeed + 1) * AtkSpeedCoin) || PlayerData.UGLv_AtkSpeed >= AtkSpeedMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_AtkSpeed + 1) * AtkSpeedCoin);
        PlayerData.UGLv_AtkSpeed = PlayerData.UGLv_AtkSpeed + 1;
    }

    public void BuyAtkRange()
    {
        if (!goldRequire((PlayerData.UGLv_AtkRange + 1) * AtkRangeCoin) || PlayerData.UGLv_AtkRange >= AtkRangeMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_AtkRange + 1) * AtkRangeCoin);
        PlayerData.UGLv_AtkRange = PlayerData.UGLv_AtkRange + 1;
    }

    public void BuyProjectileSpeed()
    {
        if (!goldRequire((PlayerData.UGLv_ProjectileSpeed + 1) * ProjectileSpeedCoin) || PlayerData.UGLv_ProjectileSpeed >= ProjectileSpeedMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_ProjectileSpeed + 1) * ProjectileSpeedCoin);
        PlayerData.UGLv_ProjectileSpeed = PlayerData.UGLv_ProjectileSpeed + 1;
    }

    public void BuyProjectileCount()
    {
        if (!goldRequire((PlayerData.UGLv_ProjectileCount + 1) * ProjectileCountCoin) || PlayerData.UGLv_ProjectileCount >= ProjectileCountMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_ProjectileCount + 1) * ProjectileCountCoin);
        PlayerData.UGLv_ProjectileCount = PlayerData.UGLv_ProjectileCount + 1;
    }

    public void BuySkillCT()
    {
        if (!goldRequire((PlayerData.UGLv_SkillCT + 1) * SkillCTCoin) || PlayerData.UGLv_SkillCT >= SkillCTMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_SkillCT + 1) * SkillCTCoin);
        PlayerData.UGLv_SkillCT = PlayerData.UGLv_SkillCT + 1;
    }

    public void BuySwapCT()
    {
        if (!goldRequire((PlayerData.UGLv_SwapCT + 1) * SwapCTCoin) || PlayerData.UGLv_SwapCT >= SwapCTMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_SwapCT + 1) * SwapCTCoin);
        PlayerData.UGLv_SwapCT = PlayerData.UGLv_SwapCT + 1;
    }

    public void BuyGainUlt()
    {
        if (!goldRequire((PlayerData.UGLv_GainUlt + 1) * GainUltCoin) || PlayerData.UGLv_GainUlt >= GainUltMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_GainUlt + 1) * GainUltCoin);
        PlayerData.UGLv_GainUlt = PlayerData.UGLv_GainUlt + 1;
    }
    public void BuyPent()
    {
        if (!goldRequire((PlayerData.UGLv_Pent + 1) * PentCoin) || PlayerData.UGLv_Pent >= PentMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_Pent + 1) * PentCoin);
        PlayerData.UGLv_Pent = PlayerData.UGLv_Pent + 1;
    }
    public void BuyMovementSpeed()
    {
        if (!goldRequire((PlayerData.UGLv_MovementSpeed + 1) * MovementSpeedCoin) || PlayerData.UGLv_MovementSpeed >= MovementSpeedMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_MovementSpeed + 1) * MovementSpeedCoin);
        PlayerData.UGLv_MovementSpeed = PlayerData.UGLv_MovementSpeed + 1;
    }
    public void BuyGainGold()
    {
        if (!goldRequire((PlayerData.UGLv_GainGold + 1) * GainGoldCoin) || PlayerData.UGLv_GainGold >= GainGoldMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_GainGold + 1) * GainGoldCoin);
        PlayerData.UGLv_GainGold = PlayerData.UGLv_GainGold + 1;
    }
    public void BuyGainExp()
    {
        if (!goldRequire((PlayerData.UGLv_GainExp + 1) * GainExpCoin) || PlayerData.UGLv_GainExp >= GainExpMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_GainExp + 1) * GainExpCoin);
        PlayerData.UGLv_GainExp = PlayerData.UGLv_GainExp + 1;
    }
    public void BuyCri()
    {
        if (!goldRequire((PlayerData.UGLv_Cri + 1) * CriCoin) || PlayerData.UGLv_Cri >= CriMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_Cri + 1) * CriCoin);
        PlayerData.UGLv_Cri = PlayerData.UGLv_Cri + 1;
    }
    public void BuyMagnet()
    {
        if (!goldRequire((PlayerData.UGLv_Magnet + 1) * MagnetCoin) || PlayerData.UGLv_Magnet >= MagnetMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_Magnet + 1) * MagnetCoin);
        PlayerData.UGLv_Magnet = PlayerData.UGLv_Magnet + 1;
    }
    public void BuyRevive()
    {
        if (!goldRequire((PlayerData.UGLv_Revive + 1) * ReviveCoin) || PlayerData.UGLv_Revive >= ReviveMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_Revive + 1) * ReviveCoin);
        PlayerData.UGLv_Revive = PlayerData.UGLv_Revive + 1;
    }
    public void BuyMaxHP()
    {
        if (!goldRequire((PlayerData.UGLv_MaxHP + 1) * MaxHPCoin) || PlayerData.UGLv_MaxHP >= MaxHPMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_MaxHP + 1) * MaxHPCoin);
        PlayerData.UGLv_MaxHP = PlayerData.UGLv_MaxHP + 1;
    }
    public void BuyHPRegen()
    {
        if (!goldRequire((PlayerData.UGLv_HPRegen + 1) * HPRegenCoin) || PlayerData.UGLv_HPRegen >= HPRegenMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_HPRegen + 1) * HPRegenCoin);
        PlayerData.UGLv_HPRegen = PlayerData.UGLv_HPRegen + 1;
    }
    public void BuyReflect()
    {
        if (!goldRequire((PlayerData.UGLv_Reflect + 1) * ReflectCoin) || PlayerData.UGLv_Reflect >= ReflectMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_Reflect + 1) * ReflectCoin);
        PlayerData.UGLv_Reflect = PlayerData.UGLv_Reflect + 1;
    }
    public void BuyGainDMG()
    {
        if (!goldRequire((PlayerData.UGLv_GainDMG + 1) * GainDMGCoin) || PlayerData.UGLv_GainDMG >= GainDMGMaxLv)
            return;
        btnRefresh();
        goldChange((-1) * (PlayerData.UGLv_GainDMG + 1) * GainDMGCoin);
        PlayerData.UGLv_GainDMG = PlayerData.UGLv_GainDMG + 1;
    }
    #endregion

    #region 버튼 초기화, 필요 골드량 체크
    public void btnRefresh()
    {
        buyArcherBtn.interactable = !PlayerData.isArcherGet;
        /*
        buyThiefBtn = !PlayerData.isThiefGet;
        buyMagicianBtn = !PlayerData.isMagicianGet;
        buySpearWarriorBtn = !PlayerData.isSpearWarriorGet;
        buyShieldGuyBtn = !PlayerData.isShieldGuyGet;
        buyBabarianBtn = !PlayerData.isBabarianGet;
        */
        AllDmgBtn.interactable = PlayerData.UGLv_AllDmg != AllDmgMaxLv ? true : false;
        /*
        BasicAtkDmgBtn.interactable = PlayerData.UGLv_BasicAtkDmg != BasicAtkDmgMaxLv ? true : false;
        SynergyDmgBtn.interactable = PlayerData.UGLv_SynergyDmg != SynergyDmgMaxLv ? true : false;
        AtkSpeedBtn.interactable = PlayerData.UGLv_AtkSpeed != AtkSpeedMaxLv ? true : false;
        AtkRangeBtn.interactable = PlayerData.UGLv_AtkRange != AtkRangeMaxLv ? true : false;
        ProjectileSpeedBtn.interactable = PlayerData.UGLv_ProjectileSpeed != ProjectileSpeedMaxLv ? true : false;
        ProjectileCountBtn.interactable = PlayerData.UGLv_ProjectileCount != ProjectileCountMaxLv ? true : false;
        SkillCTBtn.interactable = PlayerData.UGLv_SkillCT != SkillCTMaxLv ? true : false;
        SwapCTBtn.interactable = PlayerData.UGLv_SwapCT != SwapCTMaxLv ? true : false;
        GainUltBtn.interactable = PlayerData.UGLv_GainUlt != GainUltMaxLv ? true : false;
        PentBtn.interactable = PlayerData.UGLv_Pent != PentMaxLv ? true : false;
        MovementSpeedBtn.interactable = PlayerData.UGLv_MovementSpeed != MovementSpeedMaxLv ? true : false;
        GainGoldBtn.interactable = PlayerData.UGLv_GainGold != GainGoldMaxLv ? true : false;
        GainExpBtn.interactable = PlayerData.UGLv_GainExp != GainExpMaxLv ? true : false;
        CriBtn.interactable = PlayerData.UGLv_Cri != CriMaxLv ? true : false;
        MagnetBtn.interactable = PlayerData.UGLv_Magnet != MagnetMaxLv ? true : false;
        ReviveBtn.interactable = PlayerData.UGLv_Revive != ReviveMaxLv ? true : false;
        MaxHPBtn.interactable = PlayerData.UGLv_MaxHP != MaxHPMaxLv ? true : false;
        HPRegenBtn.interactable = PlayerData.UGLv_HPRegen != HPRegenMaxLv ? true : false;
        ReflectBtn.interactable = PlayerData.UGLv_Reflect != ReflectMaxLv ? true : false;
        GainDMGBtn.interactable = PlayerData.UGLv_GainDMG != GainDMGMaxLv ? true : false;
        */
    }
    public bool goldRequire(int value)
    {
        if (PlayerData.Gold < value)
        {
            AlertUI.SetActive(true);
            AlertText.text = "골드가 부족합니다";
            return false;
        }

        else return true;
    }
    #endregion

    #region UI Setting
    public void goldChange(int value)
    {
        PlayerData.Gold += value;
        nowGold.text = PlayerData.Gold.ToString();
    }
    public void clickGameStart()
    { // 로비 메인 게임 스타트 버튼 클릭 시
        CharacterSel.SetActive(true);
    }

    public void clickUGPanel()
    { // 로비 메인 업그레이드 버튼 클릭 시
        UGPanel.SetActive(true);
    }

    public void InitPrefab(int i)
    { // 캐릭터 선택 Info 정보 표출 및 캐릭터 표시
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

    public void click(int value)
    { // 캐릭터 프리펩 클릭 시 발동
        InitPrefab(value);
    }

    public void InfoActiveAndDelChild()
    { // 다른 캐릭터 클릭 시 기존에 있던 정보 삭제를 위한 메소드
        if (!Info.activeSelf)
            Info.SetActive(true);

        delChild();
    }

    public void delChild()
    { // 기존에 존재하던 정보 삭제
        if (prefabParent.childCount != 0)
        {
            foreach (Transform child in prefabParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
    #endregion
    public void startIngame()
    { // 인게임으로 Scene 전환(캐릭터 선택화면 게임 스타트 클릭)
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
