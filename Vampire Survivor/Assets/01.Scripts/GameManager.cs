using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    // 플레이어 최종 스탯
    [Header("Player Total Stat")]
    public float GM_AllDmg;                 // 전체 피해량
    public float GM_BasicAtkDmg_Main;       // 평타(MAIN) 피해량
    public float GM_BasicAtkDmg_Sub;        // 평타(SUB) 피해량
    public float GM_SynergyDmg_Pyro;        // 속성 피해량_Pyro
    public float GM_SynergyDmg_Electro;     // 속성 피해량_Electro
    public float GM_SynergyDmg_Ice;         // 속성 피해량_Ice
    public float GM_AtkSpeed_Main;          // 공격 속도(Main) (평타 쿨타임)
    public float GM_AtkSpeed_Sub;           // 공격 속도(Sub)  (평타 쿨타임)
    public float GM_AtkRange;               // 공격 범위
    public float GM_ProjectileSpeed_Main;   // 투사체 속도(Main)
    public float GM_ProjectileSpeed_Sub;    // 투사체 속도(Sub)
    public int GM_ProjectileCount_Main;     // 투사체 수(Main)
    public int GM_ProjectileCount_Sub;      // 투사체 수(Sub)
    public float GM_SkillCT;                // 스킬 쿨타임
    public float GM_SwapCT;                 // 스왑 쿨타임
    public float GM_GainUlt;                // 궁극기 충전량
    public int GM_Pent_Main;                // 관통(Main)
    public int GM_Pent_Sub;                 // 관통(Sub)
    public float GM_MovementSpeed;          // 이동 속도
    public float GM_GainGold;               // 골드 획득량
    public float GM_GainExp;                // 경험치 획득량
    public float GM_Cri;                    // 치명타 확률
    public float GM_Magnet;                 // 자석력
    public int GM_Revive;                   // 부활
    public int GM_MaxHP;                    // 최대 체력
    public int GM_NowHP;                    // 현재 체력
    public float GM_HPRegen;                // 체력 재생
    public float GM_Reflect;                // 피해량 반사
    public float GM_GainDmg;                // 받는 피해량

    [Header("Level Up")]
    public GameObject levelUpUI;
    public Slider expBar;

    // 싱글톤
    public static GameManager instance = null;
    // 플레이어 배열
    public GameObject[] players;
    public int playerNum;
    // 플레이어 부모
    public Transform playerParent;

    public CinemachineVirtualCamera cinemachine;

    // 인스턴스
    public Player player;
    public PoolManager pool;
    public TestWeaponManager weaponManager;
    public PerkInvenManager perkInven;
    public PerkValueCheck perkValueCheck;
    public WeaponChange weaponChange;
    public InventoryManager synergyInven;

    public Item[] allWeaponData;

    public Text level;
    public Text nowExp;
    public Text needExp;

    // 플레이어 무기 스왑용 스프라이트 배열
    public Sprite[] sprites;

    // 5분마다 플레이어 주위로 생성할 울타리
    public GameObject fence;
    private float fenceTime;
    public bool nowFence;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {        
        playerNum = PlayerData.charNum;
        // weaponChange켜주고
        weaponChange.saveItemDatas[0] = allWeaponData[playerNum];
        weaponChange.saveItemDatas[1] = allWeaponData[playerNum];
        // 로비에서 선택한 플레이어를 playerParent의 자식으로 생성한다.
        var PrefPlayer = Instantiate(players[playerNum]);
        PrefPlayer.transform.SetParent(playerParent.transform);
        player = FindObjectOfType<Player>();
        // 카메라가 플레이어를 따라가게 한다.
        cinemachine.Follow = player.GetComponentInChildren<Transform>();
        // UI 갱신
        level.text = "Level: " + player.playerLV.ToString();

        // # 00. 플레이어 스탯 초기화(DataManager 읽어오기)
        GM_AllDmg = player.AllDmg;
        GM_BasicAtkDmg_Main = (player.BasicAtkDmg + player.mainWeapon.dmg_Main) * (1 + player.BasicAtkDmg_CB + (PlayerData.UGLv_BasicAtkDmg * 0.1f) + (PlayerData.UGLv_AllDmg * 0.1f));
        //GM_BasicAtkDmg_Sub = (player.BasicAtkDmg + player.subWeapon.dmg_Sub) * (1 + player.BasicAtkDmg_CB + (PlayerData.UGLv_BasicAtkDmg * 0.1f) + (PlayerData.UGLv_AllDmg * 0.1f));
        GM_SynergyDmg_Pyro = player.SynergyDmg_Pyro * (1 + player.SynergyDmg_Pyro_CB + (PlayerData.UGLv_SynergyDmg * 0.1f) + (PlayerData.UGLv_AllDmg * 0.1f));
        GM_SynergyDmg_Electro = player.SynergyDmg_Electro * (1 + player.SynergyDmg_Electro_CB + (PlayerData.UGLv_SynergyDmg * 0.1f) + (PlayerData.UGLv_AllDmg * 0.1f));
        GM_SynergyDmg_Ice = player.SynergyDmg_Ice * (1 + player.SynergyDmg_Ice_CB + (PlayerData.UGLv_SynergyDmg * 0.1f) + (PlayerData.UGLv_AllDmg * 0.1f));
        GM_AtkSpeed_Main = (player.AtkSpeed + player.mainWeapon.AtkSpeed_Main) * (1 + player.AtkSpeed_CB + (PlayerData.UGLv_AtkSpeed * 0.1f));
        //GM_AtkSpeed_Sub = (player.AtkSpeed + player.subWeapon.CT_Sub) * (1 + player.AtkSpeed_CB + (PlayerData.UGLv_AtkSpeed * 0.1f));
        GM_AtkRange = player.AtkRange * (1 + player.AtkRange_CB + (PlayerData.UGLv_AtkRange * 0.1f));
        GM_ProjectileSpeed_Main = (player.ProjectileSpeed + player.mainWeapon.projectileSpeed_Main) * (1 + player.ProjectileSpeed_CB + (PlayerData.UGLv_ProjectileSpeed * 0.1f));
        //GM_ProjectileSpeed_Sub = (player.ProjectileSpeed + player.subWeapon.atkSpeed_Sub) * (1 + player.ProjectileSpeed_CB + (PlayerData.UGLv_ProjectileSpeed * 0.1f));
        GM_ProjectileCount_Main = player.ProjectileCount + player.mainWeapon.projectileCount_Main + player.ProjectileCount_CB + PlayerData.UGLv_ProjectileCount;
        //GM_ProjectileCount_Sub = player.ProjectileCount + player.subWeapon.projectileCount_Sub + player.ProjectileCount_CB + PlayerData.UGLv_ProjectileCount;
        GM_SkillCT = player.SkillCT * (1 + player.SkillCT + (PlayerData.UGLv_SkillCT * 0.1f));
        GM_SwapCT = player.SwapCT * (1 + player.SwapCT_CB + (PlayerData.UGLv_SwapCT * 0.1f));
        GM_GainUlt = player.GainUlt * (1 + player.GainUlt_CB + (PlayerData.UGLv_GainUlt * 0.1f));
        GM_Pent_Main = player.Pent + player.mainWeapon.Pent_Main + player.Pent_CB + PlayerData.UGLv_Pent;
        //GM_Pent_Sub = player.Pent + player.subWeapon.count_Sub + player.Pent_CB + PlayerData.UGLv_Pent;
        GM_MovementSpeed = player.MovementSpeed * (1 + player.MovementSpeed_CB + (PlayerData.UGLv_MovementSpeed * 0.1f));
        GM_GainGold = player.GainGold * (1 + player.GainGold_CB + (PlayerData.UGLv_GainGold * 0.1f));
        GM_GainExp = player.GainExp * (1 + player.GainExp_CB + (PlayerData.UGLv_GainExp * 0.1f));
        GM_Cri = player.Cri * (1 + player.Cri_CB + (PlayerData.UGLv_Cri * 0.1f));
        GM_Magnet = player.Magnet * (1 + player.Magnet_CB + (PlayerData.UGLv_Magnet * 0.1f));
        GM_Revive = player.Revive + player.Revive_CB + PlayerData.UGLv_Revive;
        GM_MaxHP = Mathf.RoundToInt(player.MaxHP * (1 + player.MaxHP_CB + (PlayerData.UGLv_MaxHP * 0.1f)));
        GM_NowHP = GM_MaxHP;
        GM_HPRegen = player.HPRegen * (1 + player.HPRegen_CB + (PlayerData.UGLv_HPRegen * 0.1f));
        GM_Reflect = player.Reflect * (1 + player.Reflect_CB + (PlayerData.UGLv_Reflect * 0.1f));
        GM_GainDmg = Mathf.RoundToInt(player.GainDmg * (1 + player.GainDmg_CB + (PlayerData.UGLv_GainDMG * 0.1f)));
}

    void Update()
    {
        FenceOn();
    }

    void FenceOn()
    {
        fenceTime += Time.deltaTime;
        if (fenceTime >= 10f)
        {
            fence.SetActive(true);
            nowFence = true;
            Invoke("FenceOff", 10f);
        }
    }

    void FenceOff()
    {
        fenceTime = 0f;
        fence.SetActive(false);
        nowFence = false;
    }

    // 1. 스왑을 했을 때 2. 보조무기를 획득 했을 때
    // 스탯(무기 관련)을 초기화 해주는 함수
    public void RefreshStats()
    {
        // 1. subWeapon이 비어있으면 main 스탯만 갱신해준다.
        if(player.subWeapon == null)
        {
            GM_AllDmg = player.AllDmg;
            // 평타 (main)
            GM_BasicAtkDmg_Main = (player.BasicAtkDmg + player.mainWeapon.dmg_Main) * (1 + player.BasicAtkDmg_CB + (PlayerData.UGLv_BasicAtkDmg * 0.1f) + (PlayerData.UGLv_AllDmg * 0.1f));         
            // 공격 주기(main)
            GM_AtkSpeed_Main = (player.AtkSpeed + player.mainWeapon.AtkSpeed_Main) * (1 + player.AtkSpeed_CB + (PlayerData.UGLv_AtkSpeed * 0.1f));
            // 투사체 속도(main)
            GM_ProjectileSpeed_Main = (player.ProjectileSpeed + player.mainWeapon.projectileSpeed_Main) * (1 + player.ProjectileSpeed_CB + (PlayerData.UGLv_ProjectileSpeed * 0.1f));        
            // 투사체 개수(main)
            GM_ProjectileCount_Main = player.ProjectileCount + player.mainWeapon.projectileCount_Main + player.ProjectileCount_CB + PlayerData.UGLv_ProjectileCount;
            // 관통 가능 횟수(main, sub)
            GM_Pent_Main = player.Pent + player.mainWeapon.Pent_Main + player.Pent_CB + PlayerData.UGLv_Pent;
        }
        // 2. subWeapon이 비어있지 않으면 main, sub 둘다 갱신해준다.
        else
        {
            GM_AllDmg = player.AllDmg;
            // 평타 (main, sub)
            GM_BasicAtkDmg_Main = (player.BasicAtkDmg + player.mainWeapon.dmg_Main) * (1 + player.BasicAtkDmg_CB + (PlayerData.UGLv_BasicAtkDmg * 0.1f) + (PlayerData.UGLv_AllDmg * 0.1f));
            GM_BasicAtkDmg_Sub = (player.BasicAtkDmg + player.subWeapon.dmg_Sub) * (1 + player.BasicAtkDmg_CB + (PlayerData.UGLv_BasicAtkDmg * 0.1f) + (PlayerData.UGLv_AllDmg * 0.1f));

            // 공격 주기(main, sub)
            GM_AtkSpeed_Main = (player.AtkSpeed + player.mainWeapon.AtkSpeed_Main) * (1 + player.AtkSpeed_CB + (PlayerData.UGLv_AtkSpeed * 0.1f));
            GM_AtkSpeed_Sub = (player.AtkSpeed + player.subWeapon.AtkSpeed_Sub) * (1 + player.AtkSpeed_CB + (PlayerData.UGLv_AtkSpeed * 0.1f));

            // 투사체 속도(main, sub)
            GM_ProjectileSpeed_Main = (player.ProjectileSpeed + player.mainWeapon.projectileSpeed_Main) * (1 + player.ProjectileSpeed_CB + (PlayerData.UGLv_ProjectileSpeed * 0.1f));
            GM_ProjectileSpeed_Sub = (player.ProjectileSpeed + player.subWeapon.projectileSpeed_Sub) * (1 + player.ProjectileSpeed_CB + (PlayerData.UGLv_ProjectileSpeed * 0.1f));

            // 투사체 개수(main, sub)
            GM_ProjectileCount_Main = player.ProjectileCount + player.mainWeapon.projectileCount_Main + player.ProjectileCount_CB + PlayerData.UGLv_ProjectileCount;
            GM_ProjectileCount_Sub = player.ProjectileCount + player.subWeapon.projectileCount_Sub + player.ProjectileCount_CB + PlayerData.UGLv_ProjectileCount;

            // 관통 가능 횟수(main, sub)
            GM_Pent_Main = player.Pent + player.mainWeapon.Pent_Main + player.Pent_CB + PlayerData.UGLv_Pent;
            GM_Pent_Sub = player.Pent + player.subWeapon.Pent_Sub + player.Pent_CB + PlayerData.UGLv_Pent;
        }              
    }
}
