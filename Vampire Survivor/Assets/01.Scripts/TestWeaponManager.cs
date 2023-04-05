using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponManager : MonoBehaviour
{
    public Item mainWeapon;
    public Item subWeapon;

    GameObject passive_Main_Setup;  // ������ �� �غ� ����
    public GameObject[] passive_Main;    // ���� ����
    public GameObject[] passive_Sub;     // ���� ����

    // ��Ÿ ����(����_sub �� �����)
    public Vector3 mainDir;
    public Vector3 subDir;

    // ����(����)
    Transform passive;
    // ����(����)
    GameObject passiveAxe;
    
    // Item �ɼ� - Main
    public int id;
    public string itemName;
    public int prefabId;
    public float damage;
    public int count;
    public float CT;
    public float atkSpeed;
    public string atkType;
    public float animTime;
    public int projectileCount;

    // Item �ɼ� - Sub
    public int id_Sub;
    public string itemName_Sub;
    public int prefabId_Sub;
    public float damage_Sub;
    public int count_Sub;   // ����
    public float CT_Sub;    // ��Ÿ��
    public float atkSpeed_Sub;  // ���� �ӵ�
    public string atkType_Sub;
    public float animTime_Sub;
    public int projectileCount_Sub; // ����ü ����

    // Ÿ�̸�
    public float invokeTime_Main;  // �ֹ��� ��Ÿ��
    public float invokeTime_Sub;  // �������� ��Ÿ��

    // �÷��̾�
    Player player;
    // �÷��̾ ���������� �Է��� ����
    public Vector3 lastDir;
    // �÷��̾� �̹����� ���������� �ٶ� ����
    Vector3 lastSpriteDir;

    public Vector3 rotVec;
    private bool isWeaponChangeCheck;

    bool isStatic;

    // ���� �߻� �÷���
    bool Thrown;
    // ���� ���� �÷���
    bool Back;
    // ���� �߻� �ð�
    public float thrownTime;
    // ���� ü�� �ð�
    public float keepThrownTime;
    // ���� �߻� �ӵ�
    public float ThrowSpeed;

    void Start()
    {
        //mainWeapon = WeaponChange.Instance.getMainWeapoon();
        //subWeapon = WeaponChange.Instance.getSubWeapoon();
      
        player = GameManager.instance.player;
        lastDir = new Vector3(-1, 0);
        isWeaponChangeCheck = false;

        passive_Main = new GameObject[10];
        passive_Sub = new GameObject[10];
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾� �̹����� �ٶ󺸴� ������ �޾ƿ�
        lastSpriteDir.x = player.transform.localScale.x;
        
        transform.position = player.transform.position;

        isWeaponChangeCheck = WeaponChange.Instance.getWeaponUION();

        if (isWeaponChangeCheck)
            WeaponChangeCheck();


        switch (atkType)
        {
            // ����
            case "atkAllDir":
                break;
            // �ϵ�(��)
            case "atkAllDir_Fix":
                invokeTime_Main += Time.deltaTime;
                if (invokeTime_Main > CT)
                {
                    invokeTime_Main = 0f;
                    AllAtk_Fix();
                }
                break;
            // ��(��), �ܰ�(��), â(��), ����(��)
            case "atkHorDir":
                invokeTime_Main += Time.deltaTime;
                if (invokeTime_Main > CT)
                {
                    invokeTime_Main = 0f;
                    HorAtk();
                }
                break;
            // Ȱ(��), ����(��)
            case "atkFollow":
                invokeTime_Main += Time.deltaTime;
                if (invokeTime_Main > CT)
                {
                    invokeTime_Main = 0f;
                    FollowAtk();
                }
                break;
            case "atkStatic":
                break;
            case "atkBoomerang":

                break;
        }

        switch (atkType_Sub)
        {
            // �ܰ�(����), ����(����)
            case "atkAllDir":
                invokeTime_Sub += Time.deltaTime;
                if (invokeTime_Sub > CT_Sub)
                {
                    invokeTime_Sub = 0f;
                    AllAtk_Sub();
                }
                break;
            // ����
            case "atkAllDir_Fix":

                break;
            // â(����)
            case "atkHorDir":
                invokeTime_Sub += Time.deltaTime;
                if (invokeTime_Sub > CT_Sub)
                {
                    invokeTime_Sub = 0f;
                    HorAtk_Sub();
                }
                break;
            // ��(����), Ȱ(����), �ϵ�(����)
            case "atkFollow":
                invokeTime_Sub += Time.deltaTime;
                if (invokeTime_Sub > CT_Sub)
                {
                    invokeTime_Sub = 0f;
                    FollowAtk_Sub();
                }
                break;
            // ����(����)
            case "atkStatic":
               
                StaticAtk();                 
                
                // �� ȸ��
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.35f, player.transform.position.z);
                transform.Rotate(Vector3.back * atkSpeed_Sub * Time.deltaTime);
                break;
            // ����(����)
            case "atkBoomerang":
                invokeTime_Sub += Time.deltaTime;
                if (invokeTime_Sub > CT_Sub)
                {      
                    BoomerangAtk_Sub();
                    Thrown = true;
                    invokeTime_Sub = 0f;
                }

                ThrowAndReturn();
                break;
        }

        
        // ����(����) ON OFF
        ShieldOnOff();

        // ���� ���ƿ����ϱ�
        ForceBack();
    }

    public void WeaponChangeCheck()
    {
        //Debug.Log("WeaponChangeCheck");
        mainWeapon = WeaponChange.Instance.getMainWeapoon();
        subWeapon = WeaponChange.Instance.getSubWeapoon();

        id = mainWeapon.id_Main;
        itemName = mainWeapon.itemName;
        damage = mainWeapon.dmg_Main;
        count = mainWeapon.count_Main;
        CT = mainWeapon.CT_Main;
        atkSpeed = mainWeapon.atkSpeed_Main;
        atkType = mainWeapon.atkType_Main.ToString();
        prefabId = mainWeapon.prefId_Main;
        animTime = mainWeapon.animTime_Main;
        projectileCount = mainWeapon.projectileCount_Main;

        if (subWeapon == null)
            return;
        id_Sub = subWeapon.id_Sub;
        itemName_Sub = subWeapon.itemName;
        damage_Sub = subWeapon.dmg_Sub;
        count_Sub = subWeapon.count_Sub;
        CT_Sub = subWeapon.CT_Sub;
        atkSpeed_Sub = subWeapon.atkSpeed_Sub;
        atkType_Sub = subWeapon.atkType_Sub.ToString();
        prefabId_Sub = subWeapon.prefId_Sub;
        animTime_Sub = subWeapon.animTime_Sub;
        projectileCount_Sub = subWeapon.projectileCount_Sub;
    }

    // Ⱦ���� ����
    void HorAtk()
    {
        switch (projectileCount)
        {
            // # ����ü ����(projectileCount) == 1
            case 1:
                passive_Main[0] = GameManager.instance.pool.Get(prefabId);
                passive_Main[0].transform.position = player.transform.position;
                if (player.playerDir == 1 && passive_Main[0].transform.localScale.x > 0)
                    passive_Main[0].transform.localScale = new Vector3(passive_Main[0].transform.localScale.x * -1, passive_Main[0].transform.localScale.y, 0);

                else if (player.playerDir == -1 && passive_Main[0].transform.localScale.x < 0)
                    passive_Main[0].transform.localScale = new Vector3(passive_Main[0].transform.localScale.x * -1, passive_Main[0].transform.localScale.y, 0);
      
                Invoke("HorAtk_Off", animTime);
                break;
            // # ����ü ����(projectileCount) == 2
            case 2:
                passive_Main[0] = GameManager.instance.pool.Get(prefabId);
                passive_Main[1] = GameManager.instance.pool.Get(prefabId);

                passive_Main[0].transform.position = player.transform.position;
                passive_Main[1].transform.position = player.transform.position;

                passive_Main[1].transform.localScale = new Vector3(passive_Main[0].transform.localScale.x * -1, passive_Main[0].transform.localScale.y, 0);

                Invoke("HorAtk_Off", animTime);
                break;
            // # ����ü ����(projectileCount) == 3
            case 3:
                passive_Main[0] = GameManager.instance.pool.Get(prefabId);
                passive_Main[1] = GameManager.instance.pool.Get(prefabId);
                passive_Main[2] = GameManager.instance.pool.Get(prefabId);

                passive_Main[0].transform.position = player.transform.position;
                passive_Main[1].transform.position = player.transform.position;
                passive_Main[2].transform.position = player.transform.position;


                passive_Main[0].transform.rotation = Quaternion.Euler(0, 0, 45f);
                passive_Main[1].transform.rotation = Quaternion.Euler(0, 0, 135f);
                passive_Main[2].transform.rotation = Quaternion.Euler(0, 0, 270f);

                Invoke("HorAtk_Off", animTime);
                break;
        }
    }
    void HorAtk_Off()
    {
        for(int i = 0; i < passive_Main.Length; i++)
        {
            if(passive_Main[i] != null)
                passive_Main[i].SetActive(false);
        }      
    }

    // Ⱦ���� ���� - ����
    void HorAtk_Sub()
    {
        
        switch (projectileCount_Sub)
        {
            
            // # ����ü ����(projectileCount) == 1
            case 1:
                passive_Sub[0] = GameManager.instance.pool.Get(prefabId_Sub);
                passive_Sub[0].transform.position = player.transform.position;
                if (player.playerDir == 1 && passive_Sub[0].transform.localScale.x > 0)
                    passive_Sub[0].transform.localScale = new Vector3(passive_Sub[0].transform.localScale.x * -1, passive_Sub[0].transform.localScale.y, 0);

                else if (player.playerDir == -1 && passive_Sub[0].transform.localScale.x < 0)
                    passive_Sub[0].transform.localScale = new Vector3(passive_Sub[0].transform.localScale.x * -1, passive_Sub[0].transform.localScale.y, 0);

                Invoke("HorAtk_Off_Sub", animTime_Sub);
                break;
            // # ����ü ����(projectileCount) == 2
            case 2:
                passive_Sub[0] = GameManager.instance.pool.Get(prefabId_Sub);
                passive_Sub[1] = GameManager.instance.pool.Get(prefabId_Sub);

                passive_Sub[0].transform.position = player.transform.position;
                passive_Sub[1].transform.position = player.transform.position;

                passive_Sub[0].transform.rotation = Quaternion.Euler(0, 0, 0);
                passive_Sub[1].transform.rotation = Quaternion.Euler(0, 0, 180f);

                Invoke("HorAtk_Off_Sub", animTime_Sub);
                break;
            // # ����ü ����(projectileCount) == 3
            case 3:
                /*
                passive_Sub[0] = GameManager.instance.pool.Get(prefabId_Sub);
                passive_Sub[1] = GameManager.instance.pool.Get(prefabId_Sub);
                passive_Sub[2] = GameManager.instance.pool.Get(prefabId_Sub);

                passive_Sub[0].transform.position = player.transform.position;
                passive_Sub[1].transform.position = player.transform.position;
                passive_Sub[2].transform.position = player.transform.position;


                passive_Sub[0].transform.rotation = Quaternion.Euler(0, 0, 45f);
                passive_Sub[1].transform.rotation = Quaternion.Euler(0, 0, 135f);
                passive_Sub[2].transform.rotation = Quaternion.Euler(0, 0, 270f);

                Invoke("HorAtk_Off_Sub", animTime_Sub);
                */
                break;
        }
    }
    void HorAtk_Off_Sub()
    {
        for(int i = 0; i < passive_Sub.Length; i++)
        {
            if (passive_Sub[i] != null)
                passive_Sub[i].SetActive(false);
        }        
    }

    // ������ ����
    void AllAtk()
    {
        // # ����ü ����(projectileCount) == 1

        // # ����ü ����(projectileCount) == 2

        // # ����ü ����(projectileCount) == 3
    }

    // ������ ���� - ����
    void AllAtk_Sub()
    {
        // ���� ����ü ������ ����
        switch (projectileCount_Sub)
        {
            case 1:
                // # ����ü ����(projectileCount) == 1
                Transform passive_Sub = GameManager.instance.pool.Get(prefabId_Sub).transform;
                
                passive_Sub.position = player.transform.position;
                
                Vector3 dir = player.inputVec.normalized;
                if (dir == new Vector3(0, 0))
                    dir = lastDir;
                
                passive_Sub.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                
                lastDir = dir;
                
                passive_Sub.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir, atkSpeed_Sub);
                //return;
                break;
            case 2:
                // # ����ü ����(projectileCount) == 2
                Transform passsive_Sub_1 = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passsive_Sub_2 = GameManager.instance.pool.Get(prefabId_Sub).transform;

                passsive_Sub_1.position = player.transform.position;
                passsive_Sub_2.position = player.transform.position + Vector3.right * 1.05f;

                Vector3 dir_2 = player.inputVec.normalized;
                if (dir_2 == new Vector3(0, 0))
                    dir_2 = lastDir;

                passsive_Sub_1.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);
                passsive_Sub_2.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);
                
                lastDir = dir_2;

                passsive_Sub_1.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_2, atkSpeed_Sub);
                passsive_Sub_2.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_2, atkSpeed_Sub);
                break;
            case 3:
                // # ����ü ����(projectileCount) == 3
                Transform passsive_Sub_a = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passsive_Sub_b = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passsive_Sub_c = GameManager.instance.pool.Get(prefabId_Sub).transform;

                passsive_Sub_a.position = player.transform.position + Vector3.left * 1.05f;
                passsive_Sub_b.position = player.transform.position;
                passsive_Sub_c.position = player.transform.position + Vector3.right * 1.05f;

                Vector3 dir_3 = player.inputVec.normalized;
                if (dir_3 == new Vector3(0, 0))
                    dir_3 = lastDir;

                passsive_Sub_a.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);
                passsive_Sub_b.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);
                passsive_Sub_c.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);

                lastDir = dir_3;

                passsive_Sub_a.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_3, atkSpeed_Sub);
                passsive_Sub_b.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_3, atkSpeed_Sub);
                passsive_Sub_c.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_3, atkSpeed_Sub);
                break;
        }
    }

    // ������ ����(����)
    void AllAtk_Fix()
    {
        // 1. ������ ����
        Vector3 dir = player.inputVec.normalized;
        if (dir == new Vector3(0, 0))
            //dir = lastDir;
            dir = lastSpriteDir * -1;
        //lastDir = dir;
       
        passive_Main_Setup = GameManager.instance.pool.Get(prefabId - 1);
        passive_Main_Setup.transform.position = player.transform.position + dir * 5;  // �������� ��ġ = �÷��̾���ġ + (�Է¹��� * �Ÿ�(5); �Ÿ��� ����ȭ�ؼ� ���� ����)
        passive_Main_Setup.transform.rotation = Quaternion.identity;   // �������� �����̼� �ٲ��ʿ� X

        Invoke("AllAtk_Fix_Off", 1f);  // ������ ��Ȱ��ȭ �� ���� ����      

        return;
    }

    void AllAtk_Fix_Off()
    {
        // 2. ������ ��ġ�� ���� ����
        passive_Main[0] = GameManager.instance.pool.Get(prefabId);
        passive_Main[0].transform.position = passive_Main_Setup.transform.position;  // ������ ��ġ = �������� ��ġ
        passive_Main[0].transform.rotation = Quaternion.identity;
        passive_Main_Setup.SetActive(false);   // �������� ���� ����������� ���� ��ġ�� �޾ƿ��� ���ϹǷ� ���� ���� �� ��Ȱ��ȭ
        Invoke("ExplosionOff", 0.6f);     // ���� ��Ȱ��ȭ
    }
    void ExplosionOff()
    {
        passive_Main[0].SetActive(false);
    }

    // ���� ���� - �� (Ȱ, ����)
    void FollowAtk()
    {
        switch (projectileCount)
        {
            // # ����ü ����(projectileCount) == 1
            case 1:
                if (!player.scanner.nearestTarget)
                {
                    Transform passive = GameManager.instance.pool.Get(prefabId).transform;
                    passive.position = player.transform.position;
                    Vector3 dir = player.inputVec;
                    if (dir == new Vector3(0, 0))
                        dir = lastDir;
                    passive.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    lastDir = dir;
                    passive.GetComponent<Weapon>().Init(damage, count, dir, atkSpeed);

                    return;
                }

                Vector3 targetPos = player.scanner.nearestTarget.position;
                Vector3 dir_two = targetPos - player.transform.position;
                dir_two = dir_two.normalized;

                Transform passive_two = GameManager.instance.pool.Get(prefabId).transform;
                passive_two.position = player.transform.position;
                // Quaternion.FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
                passive_two.rotation = Quaternion.FromToRotation(Vector3.up, dir_two);
                passive_two.GetComponent<Weapon>().Init(damage, count, dir_two, atkSpeed);

                break;
            // # ����ü ����(projectileCount) == 2
            case 2:
                if (!player.scanner.nearestTarget)
                {
                    Transform passive_1 = GameManager.instance.pool.Get(prefabId).transform;
                    Transform passive_2 = GameManager.instance.pool.Get(prefabId).transform;

                    passive_1.position = player.transform.position;
                    passive_2.position = player.transform.position + Vector3.right * 1.05f;

                    Vector3 dir = player.inputVec;
                    if (dir == new Vector3(0, 0))
                        dir = lastDir;
                    passive_1.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    passive_2.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    lastDir = dir;
                    passive_1.GetComponent<Weapon>().Init(damage, count, dir, atkSpeed);
                    passive_2.GetComponent<Weapon>().Init(damage, count, dir, atkSpeed);
                    return;
                }

                Vector3 targetPos_Case2 = player.scanner.nearestTarget.position;
                Vector3 dir_2 = targetPos_Case2 - player.transform.position;
                dir_2 = dir_2.normalized;

                Transform passive_S_1 = GameManager.instance.pool.Get(prefabId).transform;
                Transform passive_S_2 = GameManager.instance.pool.Get(prefabId).transform;

                passive_S_1.position = player.transform.position;
                passive_S_2.position = player.transform.position + Vector3.right * 1.05f;

                passive_S_1.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);
                passive_S_1.GetComponent<Weapon>().Init(damage, count, dir_2, atkSpeed);
                passive_S_2.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);
                passive_S_2.GetComponent<Weapon>().Init(damage, count, dir_2, atkSpeed);

                break;
            // # ����ü ����(projectileCount) == 3
            case 3:
                if (!player.scanner.nearestTarget)
                {
                    Transform passive_1 = GameManager.instance.pool.Get(prefabId).transform;
                    Transform passive_2 = GameManager.instance.pool.Get(prefabId).transform;
                    Transform passive_3 = GameManager.instance.pool.Get(prefabId).transform;

                    passive_1.position = player.transform.position + Vector3.left * 1.05f;
                    passive_2.position = player.transform.position + Vector3.right * 1.05f;
                    passive_3.position = player.transform.position;

                    Vector3 dir = player.inputVec;
                    if (dir == new Vector3(0, 0))
                        dir = lastDir;
                    passive_1.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    passive_2.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    passive_3.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    lastDir = dir;
                    passive_1.GetComponent<Weapon>().Init(damage, count, dir, atkSpeed);
                    passive_2.GetComponent<Weapon>().Init(damage, count, dir, atkSpeed);
                    passive_3.GetComponent<Weapon>().Init(damage, count, dir, atkSpeed);
                    return;
                }

                Vector3 targetPos_Case3 = player.scanner.nearestTarget.position;
                Vector3 dir_3 = targetPos_Case3 - player.transform.position;
                dir_3 = dir_3.normalized;

                Transform passive_S_a = GameManager.instance.pool.Get(prefabId).transform;
                Transform passive_S_b = GameManager.instance.pool.Get(prefabId).transform;
                Transform passive_S_c = GameManager.instance.pool.Get(prefabId).transform;

                passive_S_a.position = player.transform.position + Vector3.left * 1.05f;
                passive_S_b.position = player.transform.position + Vector3.right * 1.05f;
                passive_S_c.position = player.transform.position;

                passive_S_a.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);
                passive_S_b.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);
                passive_S_c.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);

                passive_S_a.GetComponent<Weapon>().Init(damage, count, dir_3, atkSpeed);
                passive_S_b.GetComponent<Weapon>().Init(damage, count, dir_3, atkSpeed);
                passive_S_c.GetComponent<Weapon>().Init(damage, count, dir_3, atkSpeed);
                break;
        }        
    }

    // ���� ���� - ���� (��, Ȱ, �ϵ�)
    void FollowAtk_Sub()
    {
        switch (projectileCount_Sub)
        {
            // # ����ü ����(projectileCount) == 1
            case 1:
                if (!player.scanner.nearestTarget)
                {
                    Transform passive = GameManager.instance.pool.Get(prefabId_Sub).transform;
                    passive.position = transform.position;
                    Vector3 dir = player.inputVec;
                    if (dir == new Vector3(0, 0))
                        dir = lastDir;
                    passive.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    lastDir = dir;
                    passive.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir, atkSpeed_Sub);

                    return;
                }

                Vector3 targetPos = player.scanner.nearestTarget.position;
                Vector3 dir_two = targetPos - transform.position;
                dir_two = dir_two.normalized;

                Transform passive_two = GameManager.instance.pool.Get(prefabId_Sub).transform;
                passive_two.position = transform.position;
                // Quaternion.FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
                passive_two.rotation = Quaternion.FromToRotation(Vector3.up, dir_two);
                passive_two.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_two, atkSpeed_Sub);

                break;
            // # ����ü ����(projectileCount) == 2
            case 2:
                if (!player.scanner.nearestTarget)
                {
                    Transform passive_1 = GameManager.instance.pool.Get(prefabId_Sub).transform;
                    Transform passive_2 = GameManager.instance.pool.Get(prefabId_Sub).transform;

                    passive_1.position = player.transform.position;
                    passive_2.position = player.transform.position + Vector3.right * 1.05f;

                    Vector3 dir = player.inputVec;
                    if (dir == new Vector3(0, 0))
                        dir = lastDir;
                    passive_1.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    passive_2.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    lastDir = dir;
                    passive_1.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir, atkSpeed_Sub);
                    passive_2.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir, atkSpeed_Sub);
                    return;
                }

                Vector3 targetPos_Case2 = player.scanner.nearestTarget.position;
                Vector3 dir_2 = targetPos_Case2 - player.transform.position;
                dir_2 = dir_2.normalized;

                Transform passive_S_1 = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passive_S_2 = GameManager.instance.pool.Get(prefabId_Sub).transform;

                passive_S_1.position = player.transform.position;
                passive_S_2.position = player.transform.position + Vector3.right * 1.05f;

                passive_S_1.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);        
                passive_S_2.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);

                passive_S_1.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_2, atkSpeed_Sub);
                passive_S_2.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_2, atkSpeed_Sub);

                break;
            // # ����ü ����(projectileCount) == 3
            case 3:
                if (!player.scanner.nearestTarget)
                {
                    Transform passive_1 = GameManager.instance.pool.Get(prefabId_Sub).transform;
                    Transform passive_2 = GameManager.instance.pool.Get(prefabId_Sub).transform;
                    Transform passive_3 = GameManager.instance.pool.Get(prefabId_Sub).transform;

                    passive_1.position = player.transform.position + Vector3.left * 1.05f;
                    passive_2.position = player.transform.position + Vector3.right * 1.05f;
                    passive_3.position = player.transform.position;

                    Vector3 dir = player.inputVec;
                    if (dir == new Vector3(0, 0))
                        dir = lastDir;
                    passive_1.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    passive_2.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    passive_3.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    lastDir = dir;
                    passive_1.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir, atkSpeed_Sub);
                    passive_2.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir, atkSpeed_Sub);
                    passive_3.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir, atkSpeed_Sub);
                    return;
                }

                Vector3 targetPos_Case3 = player.scanner.nearestTarget.position;
                Vector3 dir_3 = targetPos_Case3 - player.transform.position;
                dir_3 = dir_3.normalized;

                Transform passive_S_a = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passive_S_b = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passive_S_c = GameManager.instance.pool.Get(prefabId_Sub).transform;

                passive_S_a.position = player.transform.position + Vector3.left * 1.05f;
                passive_S_b.position = player.transform.position + Vector3.right * 1.05f;
                passive_S_c.position = player.transform.position;

                passive_S_a.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);
                passive_S_b.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);
                passive_S_c.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);

                passive_S_a.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_3, atkSpeed_Sub);
                passive_S_b.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_3, atkSpeed_Sub);
                passive_S_c.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_3, atkSpeed_Sub);
                break;
        }
    }

    // ���� ����(����_����)
    void StaticAtk()
    {
        for (int i = 0; i < projectileCount_Sub; i++)
        {
            if (i < transform.childCount)
            {
                passive = transform.GetChild(i);
            }
            else
            {
                passive = GameManager.instance.pool.Get(prefabId_Sub).transform;
                // Transform�� parent �Ӽ��� ���� �θ� ����(Poolmanager -> WeaponManager)
                passive.parent = transform;
            }

            passive.localPosition = Vector3.zero;
            passive.localRotation = Quaternion.identity;

            // ���� ȸ�� ����
            rotVec = Vector3.forward * 360 * i / projectileCount_Sub;
            passive.Rotate(rotVec);
            passive.Translate(passive.up * 1.5f, Space.World);

            passive.GetComponent<Weapon>().Init(damage_Sub, -1, Vector3.zero, atkSpeed_Sub); // -1�� �������� �������� �ǹ�.
        }
    }

    // ����(����) ON OFF
    void ShieldOnOff()
    {
        // 0. main�̳� sub �� �ϳ��� shield�� ���� ����
        if (subWeapon != null)
        {
            // 1. ���� �������Ⱑ ���а� �ƴϸ� -> �����ִ� ���и� ��Ȱ��ȭ
            if (subWeapon.atkType_Sub != Item.AtkType.atkStatic)
            {
                int getChildCount = gameObject.transform.childCount;
                for (int i = 0; i < getChildCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            
            // 2. ���� �������Ⱑ ���ж�� -> ���и� �ٽ� Ȱ��ȭ
            else if (subWeapon.atkType_Sub == Item.AtkType.atkStatic)
            {
                // ��Ȱ��ȭ�� ������Ʈ�� Ȱ��ȭ ��Ű���� Ȱ��ȭ�� �θ� ã�Ƽ� �ڽ��� ã�� �������� �����ؾ���
                // GetChild�Լ��� �־��� �Ű������� ���ڿ� �ش��ϴ� �ڽ��� ��ȯ�Ѵ�.
                for (int i = 0; i < transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }

    // �θ޶� ����_main
    void BoomerangAtk()
    {

    }

    // �θ޶� ����_sub (����-����) -> ������ �������� ���ư��� ���� �浹�ϸ� �������� �׸��� �÷��̾�� ���ƿ�
    void BoomerangAtk_Sub()
    {
        // # ����ü ����(projectileCount) == 1
        passiveAxe = GameManager.instance.pool.Get(prefabId_Sub);
        passiveAxe.transform.position = player.transform.position;

        // 8���� ����
        subDir = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
        if (subDir == new Vector3(0, 0))
            subDir = new Vector3(1, 0);

        passiveAxe.transform.rotation = Quaternion.FromToRotation(Vector3.up, subDir);  // �������� ������ ����

        // # ����ü ����(projectileCount) == 2

        // # ����ü ����(projectileCount) == 3
    }

    void ThrowAndReturn()
    {
        if (Thrown)
        {
            // ������ ������.
            //passiveAxe.GetComponent<Weapon>().Init(1, -1, subDir, atkSpeed_Sub);
            passiveAxe.transform.position = Vector3.Slerp(passiveAxe.transform.position, passiveAxe.transform.position + (subDir * 1.5f), 0.04f);

            thrownTime += Time.deltaTime;
            if (thrownTime >= keepThrownTime)
            {
                Thrown = false;
                Back = true;
                thrownTime = 0f;
            }
        }

        if (!Thrown && Back)
        {
            // ������ �÷��̾�� �ǵ��ƿ��� �Ѵ�.
            //passiveAxe.GetComponent<Weapon>().rigid.velocity = Vector3.zero;
            passiveAxe.transform.position = Vector3.MoveTowards(passiveAxe.transform.position, GameManager.instance.player.transform.position, ThrowSpeed * Time.deltaTime);
            if (passiveAxe.transform.position == GameManager.instance.player.transform.position)
            {
                Back = false;
                passiveAxe.gameObject.SetActive(false);
            }
        }
    }

    void ForceBack()
    {
        if(Thrown && subWeapon.itemName != "Axe")
        {
            passiveAxe.transform.position = Vector3.MoveTowards(passiveAxe.transform.position, GameManager.instance.player.transform.position, ThrowSpeed * Time.deltaTime);
            if (passiveAxe.transform.position == GameManager.instance.player.transform.position)
            {
                Back = false;
                passiveAxe.gameObject.SetActive(false);
            }
        }
    }
}
