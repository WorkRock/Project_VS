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
        // UI ����
        level.text = "Level: " + player.playerLV.ToString();
        // Instantiate(prefabs[PlayerData.charNum],pos);
        
        playerNum = PlayerData.charNum;
        // weaponChange���ְ�
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
