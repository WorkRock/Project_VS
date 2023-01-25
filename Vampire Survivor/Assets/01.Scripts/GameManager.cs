using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Player player;
    public PoolManager pool;

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
        nowExp.text = "Exp: " + player.nowExp.ToString(); 
        needExp.text = "Need Exp: " + player.needExpPerLV.ToString();
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
