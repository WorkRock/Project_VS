using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
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
        // UI 갱신
        level.text = "Level: " + player.playerLV.ToString();
        // Instantiate(prefabs[PlayerData.charNum],pos);
        
        playerNum = PlayerData.charNum;
        // weaponChange켜주고
        weaponChange.saveItemDatas[0] = allWeaponData[playerNum];
        weaponChange.saveItemDatas[1] = allWeaponData[playerNum];
        //var PrefPlayer = players[playerNum];
        var PrefPlayer = Instantiate(players[playerNum]);
        PrefPlayer.transform.SetParent(playerParent.transform);
        player = FindObjectOfType<Player>();

        cinemachine.Follow = player.GetComponentInChildren<Transform>();
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
}
