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
    public GameObject passive_Main_Wand_Setup;  // �ϵ�-��Ÿ-�ֹ���-������
    public GameObject passive_Main_Wand;   // �ϵ�-��Ÿ-�ֹ���-����
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
    // ����: ��Ÿ��, �ܰ�: �����ֱ�
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
        //Init();
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
            case 5:
                timer += Time.deltaTime;

                if (timer > CT)
                {
                    timer = 0f;
                    if (player.isMain)
                        Passive_Main_Axe();
                    else
                        Passive_Sub_Axe();
                }
                break;
            // ���� - ���� (�켭 ����)
            case 6:
                timer += Time.deltaTime;

                if (timer > CT)
                {
                    timer = 0f;
                    SetShieldPosition();
                }
                // �� ȸ��
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.35f, player.transform.position.z);
                transform.Rotate(Vector3.back * atkSpeed * Time.deltaTime);
                break;
            // ���� - ����
            case 7:
                timer += Time.deltaTime;

                if (timer > CT)
                {
                    timer = 0f;
                    if (player.isMain)
                        Passive_Main_Shield();
                }
                break;

        }

        // ������ ���� ���� ��������Ʈ �����ϱ�
        ChangeShieldSprite();

        // ���� ���׷��̵�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ������ 2, ���� 1, ���ǵ� 10 ����
            LevelUp(2, 1, 10);
            if (damage >= maxDamage)
                damage = maxDamage;
            if (count >= maxCount)
                count = maxCount;
            if (atkSpeed >= maxatkSpeed)
                atkSpeed = maxatkSpeed;
            if (level >= maxLevel)
                level = maxLevel;
        }
    }

    public void LevelUp(float damage, int count, float atkSpeed)
    {
        level++;
        this.damage += damage;
        this.count += count;
        this.atkSpeed += atkSpeed;

        if (id == 6)
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
                //atkSpeed = 150;    // ���� ȸ�� ���ǵ�
                //level = 1;          
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

            shield.GetComponent<Weapon>().Init(damage, -1, Vector3.zero, atkSpeed); // -1�� �������� �������� �ǹ�.
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
        // 1. ������ ����
        Vector3 dir = player.inputVec.normalized;
        if (dir == new Vector3(0, 0))
            dir = lastDir;
        
        lastDir = dir;

        passive_Main_Wand_Setup = GameManager.instance.pool.Get(16);
        passive_Main_Wand_Setup.transform.position = transform.position + dir * 5;  // �������� ��ġ = �÷��̾���ġ + (�Է¹��� * �Ÿ�(5); �Ÿ��� ����ȭ�ؼ� ���� ����)
        passive_Main_Wand_Setup.transform.rotation = Quaternion.identity;   // �������� �����̼� �ٲ��ʿ� X

        Invoke("Passive_Main_Wand_Setup_Off", 1f);  // ������ ��Ȱ��ȭ �� ���� ����      

        return;
    }

    void Passive_Main_Wand_Setup_Off()
    {
        // 2. ������ ��ġ�� ���� ����
        passive_Main_Wand = GameManager.instance.pool.Get(17);
        passive_Main_Wand.transform.position = passive_Main_Wand_Setup.transform.position;  // ������ ��ġ = �������� ��ġ
        passive_Main_Wand.transform.rotation = Quaternion.identity;
        passive_Main_Wand_Setup.SetActive(false);   // �������� ���� ����������� ���� ��ġ�� �޾ƿ��� ���ϹǷ� ���� ���� �� ��Ȱ��ȭ
        Invoke("ExplosionOff", 0.3f);     // ���� ��Ȱ��ȭ
    }

    void ExplosionOff()
    {
        passive_Main_Wand.SetActive(false);
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
    // ��Ÿ - ���� - �ֹ���(�������)
    void Passive_Main_Axe()
    {
        if (!player.isSlash)
        {
            // ���� ���
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }
        passive_Main_Axe = GameManager.instance.pool.Get(18);  // Ǯ���� ��Ÿ-����(�ֹ���) ��������
        passive_Main_Axe.transform.position = passivePos_Axe.position;    // ��Ÿ�� ��ġ ����
        passive_Main_Axe.GetComponent<SpriteRenderer>().flipX = player.playerDir == 1; // �÷��̾��� �¿������ ���� ��Ÿ�� ������Ű��
        Invoke("Passive_Main_Axe_Off", 0.4f);
    }

    void Passive_Main_Axe_Off()
    {
        passive_Main_Axe.SetActive(false);
        player.isSlash = false;
    }

    // ��Ÿ - ���� - ��������(ƨ��� ����)
    void Passive_Sub_Axe()
    {
        passive_Sub_Axe = GameManager.instance.pool.Get(19);   // Ǯ���� ��Ÿ-����(��������) ��������
        passive_Sub_Axe.transform.position = transform.position;
        Vector3 dir = player.inputVec.normalized;
        if (dir == new Vector3(0, 0))
            dir = lastDir;
        passive_Sub_Axe.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        lastDir = dir;
        passive_Sub_Axe.GetComponent<Weapon>().Init(1, 1, dir, atkSpeed);

        return;
    }
    #endregion

    #region ��Ÿ - ����
    // ��Ÿ - ���� - �ֹ���(���ƿ��� ����)
    void Passive_Main_Shield()
    {
        if (!player.isSlash)
        {
            // ���� ���
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }

        if (!player.scanner.nearestTarget)
        {
            Transform passive_Main_Shield = GameManager.instance.pool.Get(20).transform;
            passive_Main_Shield.position = transform.position;
            Vector3 dir = player.inputVec;
            if (dir == new Vector3(0, 0))
                dir = lastDir;
            passive_Main_Shield.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            lastDir = dir;
            passive_Main_Shield.GetComponent<Weapon>().Init(4, 1, dir, atkSpeed);

            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dirTwo = targetPos - transform.position;
        dirTwo = dirTwo.normalized;

        Transform passive_Main_Shield_two = GameManager.instance.pool.Get(20).transform;
        passive_Main_Shield_two.position = passivePos_Sword.position;
        // Quaternion.FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        passive_Main_Shield_two.rotation = Quaternion.FromToRotation(Vector3.up, dirTwo);
        passive_Main_Shield_two.GetComponent<Weapon>().Init(4, 1, dirTwo, atkSpeed);
    }

    // ��Ÿ - ���� - ��������(�켭 ����)
    void Passive_Sub_Shield()
    {
        
    }

    void Passive_Sub_Shield_Off()
    {
       
    }
    #endregion

    // ���� ��������Ʈ ����
    void ChangeShieldSprite()
    {
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
    }
}
