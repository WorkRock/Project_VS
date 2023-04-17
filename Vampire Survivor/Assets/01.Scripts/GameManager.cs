using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    // �÷��̾� ���� ����
    [Header("Player Total Stat")]
    public float GM_AllDmg;                 // ��ü ���ط�
    public float GM_BasicAtkDmg_Main;       // ��Ÿ(MAIN) ���ط�
    public float GM_BasicAtkDmg_Sub;        // ��Ÿ(SUB) ���ط�
    public float GM_SynergyDmg_Pyro;        // �Ӽ� ���ط�_Pyro
    public float GM_SynergyDmg_Electro;     // �Ӽ� ���ط�_Electro
    public float GM_SynergyDmg_Ice;         // �Ӽ� ���ط�_Ice
    public float GM_AtkSpeed_Main;          // ���� �ӵ�(Main) (��Ÿ ��Ÿ��)
    public float GM_AtkSpeed_Sub;           // ���� �ӵ�(Sub)  (��Ÿ ��Ÿ��)
    public float GM_AtkRange;               // ���� ����
    public float GM_ProjectileSpeed_Main;   // ����ü �ӵ�(Main)
    public float GM_ProjectileSpeed_Sub;    // ����ü �ӵ�(Sub)
    public int GM_ProjectileCount_Main;     // ����ü ��(Main)
    public int GM_ProjectileCount_Sub;      // ����ü ��(Sub)
    public float GM_SkillCT;                // ��ų ��Ÿ��
    public float GM_SwapCT;                 // ���� ��Ÿ��
    public float GM_GainUlt;                // �ñر� ������
    public int GM_Pent_Main;                // ����(Main)
    public int GM_Pent_Sub;                 // ����(Sub)
    public float GM_MovementSpeed;          // �̵� �ӵ�
    public float GM_GainGold;               // ��� ȹ�淮
    public float GM_GainExp;                // ����ġ ȹ�淮
    public float GM_Cri;                    // ġ��Ÿ Ȯ��
    public float GM_Magnet;                 // �ڼ���
    public int GM_Revive;                   // ��Ȱ
    public int GM_MaxHP;                    // �ִ� ü��
    public int GM_NowHP;                    // ���� ü��
    public float GM_HPRegen;                // ü�� ���
    public float GM_Reflect;                // ���ط� �ݻ�
    public float GM_GainDmg;                // �޴� ���ط�

    [Header("Level Up")]
    public GameObject levelUpUI;
    public Slider expBar;

    // �̱���
    public static GameManager instance = null;
    // �÷��̾� �迭
    public GameObject[] players;
    public int playerNum;
    // �÷��̾� �θ�
    public Transform playerParent;

    public CinemachineVirtualCamera cinemachine;

    // �ν��Ͻ�
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

    // �÷��̾� ���� ���ҿ� ��������Ʈ �迭
    public Sprite[] sprites;

    // 5�и��� �÷��̾� ������ ������ ��Ÿ��
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
        // weaponChange���ְ�
        weaponChange.saveItemDatas[0] = allWeaponData[playerNum];
        weaponChange.saveItemDatas[1] = allWeaponData[playerNum];
        // �κ񿡼� ������ �÷��̾ playerParent�� �ڽ����� �����Ѵ�.
        var PrefPlayer = Instantiate(players[playerNum]);
        PrefPlayer.transform.SetParent(playerParent.transform);
        player = FindObjectOfType<Player>();
        // ī�޶� �÷��̾ ���󰡰� �Ѵ�.
        cinemachine.Follow = player.GetComponentInChildren<Transform>();
        // UI ����
        level.text = "Level: " + player.playerLV.ToString();

        // # 00. �÷��̾� ���� �ʱ�ȭ(DataManager �о����)
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

    // 1. ������ ���� �� 2. �������⸦ ȹ�� ���� ��
    // ����(���� ����)�� �ʱ�ȭ ���ִ� �Լ�
    public void RefreshStats()
    {
        // 1. subWeapon�� ��������� main ���ȸ� �������ش�.
        if(player.subWeapon == null)
        {
            GM_AllDmg = player.AllDmg;
            // ��Ÿ (main)
            GM_BasicAtkDmg_Main = (player.BasicAtkDmg + player.mainWeapon.dmg_Main) * (1 + player.BasicAtkDmg_CB + (PlayerData.UGLv_BasicAtkDmg * 0.1f) + (PlayerData.UGLv_AllDmg * 0.1f));         
            // ���� �ֱ�(main)
            GM_AtkSpeed_Main = (player.AtkSpeed + player.mainWeapon.AtkSpeed_Main) * (1 + player.AtkSpeed_CB + (PlayerData.UGLv_AtkSpeed * 0.1f));
            // ����ü �ӵ�(main)
            GM_ProjectileSpeed_Main = (player.ProjectileSpeed + player.mainWeapon.projectileSpeed_Main) * (1 + player.ProjectileSpeed_CB + (PlayerData.UGLv_ProjectileSpeed * 0.1f));        
            // ����ü ����(main)
            GM_ProjectileCount_Main = player.ProjectileCount + player.mainWeapon.projectileCount_Main + player.ProjectileCount_CB + PlayerData.UGLv_ProjectileCount;
            // ���� ���� Ƚ��(main, sub)
            GM_Pent_Main = player.Pent + player.mainWeapon.Pent_Main + player.Pent_CB + PlayerData.UGLv_Pent;
        }
        // 2. subWeapon�� ������� ������ main, sub �Ѵ� �������ش�.
        else
        {
            GM_AllDmg = player.AllDmg;
            // ��Ÿ (main, sub)
            GM_BasicAtkDmg_Main = (player.BasicAtkDmg + player.mainWeapon.dmg_Main) * (1 + player.BasicAtkDmg_CB + (PlayerData.UGLv_BasicAtkDmg * 0.1f) + (PlayerData.UGLv_AllDmg * 0.1f));
            GM_BasicAtkDmg_Sub = (player.BasicAtkDmg + player.subWeapon.dmg_Sub) * (1 + player.BasicAtkDmg_CB + (PlayerData.UGLv_BasicAtkDmg * 0.1f) + (PlayerData.UGLv_AllDmg * 0.1f));

            // ���� �ֱ�(main, sub)
            GM_AtkSpeed_Main = (player.AtkSpeed + player.mainWeapon.AtkSpeed_Main) * (1 + player.AtkSpeed_CB + (PlayerData.UGLv_AtkSpeed * 0.1f));
            GM_AtkSpeed_Sub = (player.AtkSpeed + player.subWeapon.AtkSpeed_Sub) * (1 + player.AtkSpeed_CB + (PlayerData.UGLv_AtkSpeed * 0.1f));

            // ����ü �ӵ�(main, sub)
            GM_ProjectileSpeed_Main = (player.ProjectileSpeed + player.mainWeapon.projectileSpeed_Main) * (1 + player.ProjectileSpeed_CB + (PlayerData.UGLv_ProjectileSpeed * 0.1f));
            GM_ProjectileSpeed_Sub = (player.ProjectileSpeed + player.subWeapon.projectileSpeed_Sub) * (1 + player.ProjectileSpeed_CB + (PlayerData.UGLv_ProjectileSpeed * 0.1f));

            // ����ü ����(main, sub)
            GM_ProjectileCount_Main = player.ProjectileCount + player.mainWeapon.projectileCount_Main + player.ProjectileCount_CB + PlayerData.UGLv_ProjectileCount;
            GM_ProjectileCount_Sub = player.ProjectileCount + player.subWeapon.projectileCount_Sub + player.ProjectileCount_CB + PlayerData.UGLv_ProjectileCount;

            // ���� ���� Ƚ��(main, sub)
            GM_Pent_Main = player.Pent + player.mainWeapon.Pent_Main + player.Pent_CB + PlayerData.UGLv_Pent;
            GM_Pent_Sub = player.Pent + player.subWeapon.Pent_Sub + player.Pent_CB + PlayerData.UGLv_Pent;
        }              
    }
}
