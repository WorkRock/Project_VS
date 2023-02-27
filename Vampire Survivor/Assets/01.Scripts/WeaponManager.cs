using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Space(10f)]
    [Header("Sword")]
    public Transform passivePos_Sword;    // ��Ÿ ���� ��ġ
    public GameObject passive_Main_Sword;   // ��-��Ÿ-�ֹ���
    public GameObject passive_Sub_Sword;    // ��-��Ÿ-��������

    [Space(10f)]
    [Header("Bow")]
    public GameObject passive_Main_Bow;     // Ȱ-��Ÿ-�ֹ���
    public GameObject passive_Sub_Bow;      // Ȱ-��Ÿ-��������

    [Space(10f)]
    [Header("Knife")]
    public Transform passivePos_Knife;    // ��Ÿ ���� ��ġ
    public GameObject passive_Main_Knife;   // �ܰ�-��Ÿ-�ֹ���
    public GameObject passive_Sub_Knife;    // �ܰ�-��Ÿ-��������

    [Space(10f)]
    [Header("Spear")]
    public Transform passivePos_Spear;    // ��Ÿ ���� ��ġ
    public GameObject passive_Main_Spear;   // â-��Ÿ-�ֹ���
    public GameObject passive_Sub_Spear;    // â-��Ÿ-��������

    [Space(10f)]
    [Header("Wand")]
    public Transform passivePos_Wand;    // ��Ÿ ���� ��ġ
    public GameObject passive_Main_Wand;   // �ϵ�-��Ÿ-�ֹ���
    public GameObject passive_Sub_Wand;    // �ϵ�-��Ÿ-��������

    [Space(10f)]
    [Header("Axe")]
    public Transform passivePos_Axe;    // ��Ÿ ���� ��ġ
    public GameObject passive_Main_Axe;   // ����-��Ÿ-�ֹ���
    public GameObject passive_Sub_Axe;    // ����-��Ÿ-��������

    [Space(10f)]
    [Header("Shield")]
    public Transform passivePos_Shield;    // ��Ÿ ���� ��ġ
    public GameObject passive_Main_Shield;   // ����-��Ÿ-�ֹ���
    public GameObject passive_Sub_Shield;    // ����-��Ÿ-��������

    // ���� id
    public int id;
    // ������ id
    public int prefabId;
    // ������
    public float damage;
    public float maxDamage = 5;
    // ���� ���� �� ����
    public int count;
    public int maxCount = 7;
    // ����: ���ǵ�, �ܰ�: �����ֱ�
    public float CT;
    public float maxCT = 230;
    // ����: ���ǵ�, �ܰ�: ����ü �ӵ�
    public float atkSpeed;
    public float maxatkSpeed = 230;
    // ���� ��(������ ���� ���� ����)
    public int level;
    public int maxLevel = 10;
   
    SpriteRenderer[] nowShieldSprites;
    public Sprite[] shields;

    private float timer;
    Player player;
    public Vector3 rotVec;
    // �÷��̾��� ������ �Է¹��� �� ����
    public Vector3 lastDir;

    private void Awake()
    {    
        //player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        player = GameManager.instance.player;
        Init();
        // ���� �ÿ��� �������� �ʱ�ȭ
        lastDir = new Vector3(-1, 0);
    }
    void Update()
    {
        switch (id)
        {
            // ��
            case 0:
                timer += Time.deltaTime;

                if (timer > CT)
                {
                    timer = 0f;
                    if (player.isMain)
                        Passive_Main_Sword();
                    else
                        Passive_Sub_Sword();
                }
                break;
            // ȭ��
            case 1:
                timer += Time.deltaTime;

                if (timer > CT)
                {
                    timer = 0f;
                    if (player.isMain)
                        Passive_Main_Bow();
                    else
                        Passive_Sub_Bow();
                }
                break;
            // �ܰ�
            case 2:
                timer += Time.deltaTime;

                if(timer > CT)
                {
                    timer = 0f;
                    if (player.isMain)
                        Passive_Main_Knife();
                    else
                        Passive_Sub_Knife();
                }
                break;
            // â
            case 3:
                timer += Time.deltaTime;

                if (timer > CT)
                {
                    timer = 0f;
                    if (player.isMain)
                        Passive_Main_Spear();
                    else
                        Passive_Sub_Spear();
                }
                break;
            // �ϵ�
            case 4:
                timer += Time.deltaTime;

                if (timer > CT)
                {
                    timer = 0f;
                    if (player.isMain)
                        Passive_Main_Wand();
                    else
                        Passive_Sub_Wand();
                }
                break;
               
            // ����
            case 6:
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.35f, player.transform.position.z);
                transform.Rotate(Vector3.back * CT * Time.deltaTime);
                break;

        }

        // ������ ���� ���� ��������Ʈ �����ϱ�
        if (level >= 3 && level < 5)
        {
            nowShieldSprites = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < nowShieldSprites.Length; i++)
            {
                nowShieldSprites[i].sprite = shields[0];
            }
        }
        else if (level >= 5 && level < 7)
        {
            nowShieldSprites = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < nowShieldSprites.Length; i++)
            {
                nowShieldSprites[i].sprite = shields[1];
            }
        }
        else if (level >= 7)
        {
            nowShieldSprites = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < nowShieldSprites.Length; i++)
            {
                nowShieldSprites[i].sprite = shields[2];
            }
        }

        // ���� ���׷��̵�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ������ 2, ���� 1, ���ǵ� 10 ����
            LevelUp(2, 1, 10);
            if (damage >= maxDamage)
                damage = maxDamage;
            if (count >= maxCount)
                count = maxCount;
            if (CT >= maxCT)
                CT = maxCT;
            if (level >= maxLevel)
                level = maxLevel;
        }
    }

    public void LevelUp(float damage, int count, float CT)
    {
        level++;
        this.damage += damage;
        this.count += count;
        this.CT += CT;

        if (id == 0)
        {
            SetShieldPosition();
        }      
    }

    public void Init()
    {
        switch (id)
        {

            // Ȱ - ��?
            case 1:
                //CT = 5f;   // ȭ�� ��ô �ֱ�
                break;
            // �ܰ� - ���� ����
            case 2:
                //CT = 3f;   // �ܰ� ��ô �ֱ�
                break;
            

            // ����
            case 6:
                CT = 150;    // ���� ȸ�� ���ǵ�
                level = 1;
                SetShieldPosition();
                break;
        }
    }

    
    // ���� ��ġ �Լ�
    void SetShieldPosition()
    {
        Transform shield;

        for (int i = 0; i < count; i++)
        {
            if(i < transform.childCount)
            {
                shield = transform.GetChild(i);
            }
            else
            {
                shield = GameManager.instance.pool.Get(prefabId).transform;
                // Transform�� parent �Ӽ��� ���� �θ� ����(Poolmanager -> WeaponManager)
                shield.parent = transform;
            }
          
            shield.localPosition = Vector3.zero;
            shield.localRotation = Quaternion.identity;

            // ���� ȸ�� ����
            rotVec = Vector3.forward * 360 * i / count;
            shield.Rotate(rotVec);
            shield.Translate(shield.up * 1.5f, Space.World);

            shield.GetComponent<Weapon>().Init(damage, -1, Vector3.zero,CT); // -1�� �������� �������� �ǹ�.
        }
    }

    #region ��Ÿ - ��
    // ��Ÿ - �� - �ֹ���(�켭 ä��)
    void Passive_Main_Sword()
    {
        if (!player.isSlash)
        {
            // ���� ���
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }
        passive_Main_Sword = GameManager.instance.pool.Get(9);  // Ǯ���� ��Ÿ-��(�ֹ���) ��������
        passive_Main_Sword.transform.position = passivePos_Sword.position;    // ��Ÿ�� ��ġ ����
        passive_Main_Sword.GetComponent<SpriteRenderer>().flipX = player.playerDir == -1; // �÷��̾��� �¿������ ���� ��Ÿ�� ������Ű��
        Invoke("Passive_Main_Sword_Off", 0.4f);
    }
    void Passive_Main_Sword_Off()
    {
        passive_Main_Sword.SetActive(false);
        player.isSlash = false;
    }

    // ��Ÿ - �� - ��������(�˱� �߻�)
    void Passive_Sub_Sword()
    {
        if (!player.isSlash)
        {
            // ���� ���
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }
        
        if (!player.scanner.nearestTarget)
        {
            Transform passive_Sub_Sword = GameManager.instance.pool.Get(10).transform;
            passive_Sub_Sword.position = transform.position;
            Vector3 dir = player.inputVec;
            if (dir == new Vector3(0, 0))
                dir = lastDir;
            passive_Sub_Sword.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            lastDir = dir;
            passive_Sub_Sword.GetComponent<Weapon>().Init(4, 1, dir, atkSpeed);

            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir_two = targetPos - transform.position;
        dir_two = dir_two.normalized;

        Transform passive_Sub_Sword_two = GameManager.instance.pool.Get(10).transform;
        passive_Sub_Sword_two.position = passivePos_Sword.position;
        // Quaternion.FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        passive_Sub_Sword_two.rotation = Quaternion.FromToRotation(Vector3.right, dir_two);
        passive_Sub_Sword_two.GetComponent<Weapon>().Init(4, 1, dir_two, atkSpeed);

    }
    #endregion

    #region ��Ÿ - Ȱ
    // ��Ÿ - Ȱ - �ֹ���
    void Passive_Main_Bow()
    {
        if (!player.isSlash)
        {
            // ���� ���
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }

        if (!player.scanner.nearestTarget)
        {
            Transform passive_Main_Bow = GameManager.instance.pool.Get(12).transform;
            passive_Main_Bow.position = transform.position;
            Vector3 dir = player.inputVec;
            if (dir == new Vector3(0, 0))
                dir = lastDir;
            passive_Main_Bow.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            lastDir = dir;
            passive_Main_Bow.GetComponent<Weapon>().Init(4, 1, dir, atkSpeed);

            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir_two = targetPos - transform.position;
        dir_two = dir_two.normalized;

        Transform passive_Main_Bow_two = GameManager.instance.pool.Get(12).transform;
        passive_Main_Bow_two.position = transform.position;
        // Quaternion.FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        passive_Main_Bow_two.rotation = Quaternion.FromToRotation(Vector3.up, dir_two);
        passive_Main_Bow_two.GetComponent<Weapon>().Init(4, 1, dir_two, atkSpeed);
    }

    // ��Ÿ - Ȱ - ��������
    void Passive_Sub_Bow()
    {
        if (!player.isSlash)
        {
            // ���� ���
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }

        if (!player.scanner.nearestTarget)
        {
            Transform passive_Sub_Bow = GameManager.instance.pool.Get(8).transform;
            passive_Sub_Bow.position = transform.position;
            Vector3 dir = player.inputVec;
            if (dir == new Vector3(0, 0))
                dir = lastDir;
            passive_Sub_Bow.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            lastDir = dir;
            passive_Sub_Bow.GetComponent<Weapon>().Init(4, 1, dir, atkSpeed);

            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir_two = targetPos - transform.position;
        dir_two = dir_two.normalized;

        Transform passive_Sub_Bow_two = GameManager.instance.pool.Get(8).transform;
        passive_Sub_Bow_two.position = transform.position;
        // Quaternion.FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        passive_Sub_Bow_two.rotation = Quaternion.FromToRotation(Vector3.up, dir_two);
        passive_Sub_Bow_two.GetComponent<Weapon>().Init(4, 1, dir_two, atkSpeed);
    }

    #endregion

    #region ��Ÿ - �ܰ�
    // ��Ÿ - �ܰ� - �ֹ���
    void Passive_Main_Knife()
    {
        if (!player.isMain)
            return;

        if (!player.isSlash)
        {
            // ���� ���
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }
        passive_Main_Knife = GameManager.instance.pool.Get(11);  // Ǯ���� ��Ÿ-�ܰ�(�ֹ���) ��������
        passive_Main_Knife.transform.position = passivePos_Knife.position;    // ��Ÿ�� ��ġ ����
        passive_Main_Knife.GetComponent<SpriteRenderer>().flipX = player.playerDir == -1; // �÷��̾��� �¿������ ���� ��Ÿ�� ������Ű��
        Invoke("Passive_Main_Knife_Off", 0.3f);
    }
    void Passive_Main_Knife_Off()
    {
        passive_Main_Knife.SetActive(false);
        player.isSlash = false;
    }

    // ��Ÿ - �ܰ� - ��������
    // �ܰ� �߻�(���Ÿ� ����)
    void Passive_Sub_Knife()
    {
        if (player.isMain)
            return;

        Transform knife = GameManager.instance.pool.Get(7).transform;
        knife.position = transform.position;
        Vector3 dir = player.inputVec.normalized;
        if (dir == new Vector3(0, 0))
            dir = lastDir; 
        knife.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        lastDir = dir;
        knife.GetComponent<Weapon>().Init(1, 1, dir,atkSpeed);

        return;
    }
    #endregion

    #region ��Ÿ - â
    // ��Ÿ - â - �ֹ���(��� ���)
    void Passive_Main_Spear()
    {
        if (!player.isSlash)
        {
            // ���� ���
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }
        passive_Main_Spear = GameManager.instance.pool.Get(14);  // Ǯ���� ��Ÿ-â(�ֹ���) ��������
        passive_Main_Spear.transform.position = passivePos_Spear.position;    // ��Ÿ�� ��ġ ����
        passive_Main_Spear.GetComponent<SpriteRenderer>().flipX = player.playerDir == -1; // �÷��̾��� �¿������ ���� ��Ÿ�� ������Ű��
        Invoke("Passive_Main_Spear_Off", 0.4f);
    }

    void Passive_Main_Spear_Off()
    {
        passive_Main_Spear.SetActive(false);
        player.isSlash = false;
    }
    // ��Ÿ - â - ��������(���� �ֵθ���)
    void Passive_Sub_Spear()
    {
        if (!player.isSlash)
        {
            // ���� ���
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }
        passive_Sub_Spear = GameManager.instance.pool.Get(13);  // Ǯ���� ��Ÿ-â(��������) ��������
        passive_Sub_Spear.transform.position = transform.position;    // ��Ÿ�� ��ġ ����
        passive_Sub_Spear.GetComponent<SpriteRenderer>().flipX = player.playerDir == 1; // �÷��̾��� �¿������ ���� ��Ÿ�� ������Ű��
        Invoke("Passive_Sub_Spear_Off", 0.4f);
    }
    void Passive_Sub_Spear_Off()
    {
        passive_Sub_Spear.SetActive(false);
        player.isSlash = false;
    }
    #endregion

    #region ��Ÿ - �ϵ�
    // ��Ÿ - �ϵ� - �ֹ���(�ͽ��÷���)
    void Passive_Main_Wand()
    {

    }

    // ��Ÿ - �ϵ� - ��������(������ ��Ʈ)
    void Passive_Sub_Wand()
    {
        if (!player.isSlash)
        {
            // ���� ���
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }

        if (!player.scanner.nearestTarget)
        {
            Transform passive_Sub_Wand = GameManager.instance.pool.Get(15).transform;
            passive_Sub_Wand.position = transform.position;
            Vector3 dir = player.inputVec;
            if (dir == new Vector3(0, 0))
                dir = lastDir;
            passive_Sub_Wand.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            lastDir = dir;
            passive_Sub_Wand.GetComponent<Weapon>().Init(4, 1, dir, atkSpeed);

            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir_two = targetPos - transform.position;
        dir_two = dir_two.normalized;

        Transform passive_Sub_Wand_two = GameManager.instance.pool.Get(15).transform;
        passive_Sub_Wand_two.position = transform.position;
        // Quaternion.FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        passive_Sub_Wand_two.rotation = Quaternion.FromToRotation(Vector3.right, dir_two);
        passive_Sub_Wand_two.GetComponent<Weapon>().Init(4, 1, dir_two, atkSpeed);
    }
    #endregion

    #region ��Ÿ - ����

    #endregion

    #region ��Ÿ - ����

    #endregion


}
