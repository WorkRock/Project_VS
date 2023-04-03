using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponManager : MonoBehaviour
{
    public Item mainWeapon;
    public Item subWeapon;

    GameObject passive_Main_Setup;  // ������ �� �غ� ����
    public GameObject passive_Main;    // ���� ����
    public GameObject passive_Sub;     // ���� ����

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

    // Item �ɼ� - Sub
    public int id_Sub;
    public string itemName_Sub;
    public int prefabId_Sub;
    public float damage_Sub;
    public int count_Sub;
    public float CT_Sub;
    public float atkSpeed_Sub;
    public string atkType_Sub;
    public float animTime_Sub;

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
                invokeTime_Sub += Time.deltaTime;
                if (invokeTime_Sub > CT_Sub)
                {
                    invokeTime_Sub = 0f;
                    StaticAtk();                 
                }
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
    }

    // Ⱦ���� ����
    void HorAtk()
    {
        passive_Main = GameManager.instance.pool.Get(prefabId);
        passive_Main.transform.position = player.transform.position;
        if (player.playerDir == 1 && passive_Main.transform.localScale.x > 0)
            passive_Main.transform.localScale = new Vector3(passive_Main.transform.localScale.x * -1, passive_Main.transform.localScale.y, 0);

        else if (player.playerDir == -1 && passive_Main.transform.localScale.x < 0)
            passive_Main.transform.localScale = new Vector3(passive_Main.transform.localScale.x * -1, passive_Main.transform.localScale.y, 0);
        //passive_Main.GetComponent<SpriteRenderer>().flipX = player.playerDir == -1;
        Invoke("HorAtk_Off", animTime);
    }
    void HorAtk_Off()
    {
        passive_Main.SetActive(false);
    }

    // Ⱦ���� ���� - ����
    void HorAtk_Sub()
    {
        passive_Sub = GameManager.instance.pool.Get(prefabId_Sub);
        passive_Sub.transform.position = transform.position;
        if (player.playerDir == -1)
            passive_Sub.transform.localScale = new Vector3(passive_Sub.transform.localScale.x * -1, passive_Sub.transform.localScale.y, 0);
        //passive_Sub.GetComponent<SpriteRenderer>().flipX = player.playerDir == -1;
        Invoke("HorAtk_Off_Sub", animTime_Sub);
    }
    void HorAtk_Off_Sub()
    {
        passive_Sub.SetActive(false);
    }

    // ������ ����
    void AllAtk()
    {
        passive_Sub = GameManager.instance.pool.Get(prefabId);
        passive_Sub.transform.position = player.transform.position;
        Vector3 dir = player.inputVec.normalized;
        if (dir == new Vector3(0, 0))
            dir = lastDir;
        passive_Sub.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        lastDir = dir;
        passive_Sub.GetComponent<Weapon>().Init(1, 1, dir, atkSpeed);

        return;
    }

    // ������ ���� - ����
    void AllAtk_Sub()
    {
        Transform passive_Sub = GameManager.instance.pool.Get(prefabId_Sub).transform;
        passive_Sub.position = player.transform.position;
        Vector3 dir = player.inputVec.normalized;
        if (dir == new Vector3(0, 0))
            dir = lastDir;
        passive_Sub.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        lastDir = dir;
        passive_Sub.GetComponent<Weapon>().Init(1, 1, dir, atkSpeed_Sub);

        return;
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
        passive_Main = GameManager.instance.pool.Get(prefabId);
        passive_Main.transform.position = passive_Main_Setup.transform.position;  // ������ ��ġ = �������� ��ġ
        passive_Main.transform.rotation = Quaternion.identity;
        passive_Main_Setup.SetActive(false);   // �������� ���� ����������� ���� ��ġ�� �޾ƿ��� ���ϹǷ� ���� ���� �� ��Ȱ��ȭ
        Invoke("ExplosionOff", 0.6f);     // ���� ��Ȱ��ȭ
    }
    void ExplosionOff()
    {
        passive_Main.SetActive(false);
    }

    // ���� ����
    void FollowAtk()
    {
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
    }

    // ���� ���� - ����
    void FollowAtk_Sub()
    {
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
    }

    // ���� ����
    void StaticAtk()
    {
        for (int i = 0; i < count_Sub; i++)
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
            rotVec = Vector3.forward * 360 * i / count_Sub;
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
        passiveAxe = GameManager.instance.pool.Get(prefabId_Sub);
        passiveAxe.transform.position = player.transform.position;

        // 8���� ����
        subDir = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
        if (subDir == new Vector3(0, 0))
            subDir = new Vector3(1, 0);

        passiveAxe.transform.rotation = Quaternion.FromToRotation(Vector3.up, subDir);  // �������� ������ ����
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
