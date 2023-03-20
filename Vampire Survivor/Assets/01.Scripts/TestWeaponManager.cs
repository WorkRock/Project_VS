using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponManager : MonoBehaviour
{
    public Item mainWeapon;
    public Item subWeapon;

    GameObject passive_Main_Setup;  // 마법진 등 준비 동작
    GameObject passive_Main;    // 메인 공격
    GameObject passive_Sub;     // 서브 공격

    Transform passive;

    // Item 옵션 - Main
    public int id;
    public string itemName;
    public int prefabId;
    public float damage;
    public int count;
    public float CT;
    public float atkSpeed;
    public string atkType;
    public float animTime;

    // Item 옵션 - Sub
    public int id_Sub;
    public string itemName_Sub;
    public int prefabId_Sub;
    public float damage_Sub;
    public int count_Sub;
    public float CT_Sub;
    public float atkSpeed_Sub;
    public string atkType_Sub;
    public float animTime_Sub;

    // 타이머
    public float invokeTime_Main;  // 주무기 쿨타임
    public float invokeTime_Sub;  // 보조무기 쿨타임

    // 플레이어
    Player player;
    // 플레이어의 마지막 입력방향 값
    public Vector3 lastDir;

    public Vector3 rotVec;
    private bool isWeaponChangeCheck;

    bool isStatic;

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
        transform.position = player.transform.position;

        isWeaponChangeCheck = WeaponChange.Instance.getWeaponUION();

        if (isWeaponChangeCheck)
            WeaponChangeCheck();


        switch (atkType)
        {
            // 없음
            case "atkAllDir":
                break;
            // 완드(주)
            case "atkAllDir_Fix":
                invokeTime_Main += Time.deltaTime;
                if (invokeTime_Main > CT)
                {
                    invokeTime_Main = 0f;
                    AllAtk_Fix();
                }
                break;
            // 검(주), 단검(주), 창(주), 도끼(주)
            case "atkHorDir":
                invokeTime_Main += Time.deltaTime;
                if (invokeTime_Main > CT)
                {
                    invokeTime_Main = 0f;
                    HorAtk();
                }
                break;
            // 활(주), 방패(주)
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
            // 단검(보조), 도끼(보조)
            case "atkAllDir":
                invokeTime_Sub += Time.deltaTime;
                if (invokeTime_Sub > CT_Sub)
                {
                    invokeTime_Sub = 0f;
                    AllAtk_Sub();
                }
                break;
            // 없음
            case "atkAllDir_Fix":

                break;
            // 창(보조)
            case "atkHorDir":
                invokeTime_Sub += Time.deltaTime;
                if (invokeTime_Sub > CT_Sub)
                {
                    invokeTime_Sub = 0f;
                    HorAtk_Sub();
                }
                break;
            // 검(보조), 활(보조), 완드(보조)
            case "atkFollow":
                invokeTime_Sub += Time.deltaTime;
                if (invokeTime_Sub > CT_Sub)
                {
                    invokeTime_Sub = 0f;
                    FollowAtk_Sub();
                }
                break;
            // 방패(보조)
            case "atkStatic":
                invokeTime_Sub += Time.deltaTime;
                if (invokeTime_Sub > CT_Sub)
                {
                    invokeTime_Sub = 0f;
                    StaticAtk();
                }
                // 축 회전
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.35f, player.transform.position.z);
                transform.Rotate(Vector3.back * atkSpeed_Sub * Time.deltaTime);
                break;
            // 도끼(보조)
            case "atkBoomerang":
                invokeTime_Sub += Time.deltaTime;
                if (invokeTime_Sub > CT_Sub)
                {
                    invokeTime_Sub = 0f;
                    BoomerangAtk_Sub();
                }
                break;
        }

        // 방패(보조) ON OFF
        ShieldOnOff();       
    }

    public void WeaponChangeCheck()
    {
        Debug.Log("WeaponChangeCheck");
        mainWeapon = WeaponChange.Instance.getMainWeapoon();
        subWeapon = WeaponChange.Instance.getSubWeapoon();

        id = mainWeapon.id;
        itemName = mainWeapon.itemName;
        damage = mainWeapon.dmg;
        count = mainWeapon.count;
        CT = mainWeapon.CT;
        atkSpeed = mainWeapon.atkSpeed;
        atkType = mainWeapon.atkType.ToString();
        prefabId = mainWeapon.prefId;
        animTime = mainWeapon.animTime;

        if (subWeapon == null)
            return;
        id_Sub = subWeapon.id;
        itemName_Sub = subWeapon.itemName;
        damage_Sub = subWeapon.dmg;
        count_Sub = subWeapon.count;
        CT_Sub = subWeapon.CT;
        atkSpeed_Sub = subWeapon.atkSpeed;
        atkType_Sub = subWeapon.atkType.ToString();
        prefabId_Sub = subWeapon.prefId;
        animTime_Sub = subWeapon.animTime;
    }

    // 횡방향 공격
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

    // 횡방향 공격 - 보조
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

    // 전방향 공격
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

    // 전방향 공격 - 보조
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

    // 전방향 공격(고정)
    void AllAtk_Fix()
    {
        // 1. 마법진 생성
        Vector3 dir = player.inputVec.normalized;
        if (dir == new Vector3(0, 0))
            dir = lastDir;

        lastDir = dir;

        passive_Main_Setup = GameManager.instance.pool.Get(prefabId - 1);
        passive_Main_Setup.transform.position = player.transform.position + dir * 5;  // 마법진의 위치 = 플레이어위치 + (입력방향 * 거리(5); 거리는 변수화해서 변경 가능)
        passive_Main_Setup.transform.rotation = Quaternion.identity;   // 마법진은 로테이션 바꿀필요 X

        Invoke("AllAtk_Fix_Off", 1f);  // 마법진 비활성화 및 폭발 생성      

        return;
    }

    void AllAtk_Fix_Off()
    {
        // 2. 마법진 위치에 폭발 생성
        passive_Main = GameManager.instance.pool.Get(prefabId);
        passive_Main.transform.position = passive_Main_Setup.transform.position;  // 폭발의 위치 = 마법진의 위치
        passive_Main.transform.rotation = Quaternion.identity;
        passive_Main_Setup.SetActive(false);   // 마법진이 먼저 사라져버리면 폭발 위치를 받아오지 못하므로 폭발 생성 후 비활성화
        Invoke("ExplosionOff", 0.3f);     // 폭발 비활성화
    }
    void ExplosionOff()
    {
        passive_Main.SetActive(false);
    }

    // 유도 공격
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
            passive.GetComponent<Weapon>().Init(4, 1, dir, atkSpeed);

            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir_two = targetPos - player.transform.position;
        dir_two = dir_two.normalized;

        Transform passive_two = GameManager.instance.pool.Get(prefabId).transform;
        passive_two.position = player.transform.position;
        // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
        passive_two.rotation = Quaternion.FromToRotation(Vector3.up, dir_two);
        passive_two.GetComponent<Weapon>().Init(4, 1, dir_two, atkSpeed);
    }

    // 유도 공격 - 보조
    void FollowAtk_Sub()
    {
        if (!player.scanner.nearestTarget)
        {
            Transform passive = GameManager.instance.pool.Get(prefabId_Sub).transform;
            passive.position = transform.position;
            Vector3 dir = player.inputVec;
            if (dir == new Vector3(0, 0))
                dir = lastDir;
            passive.rotation = subWeapon.itemName == "Wand" ? Quaternion.FromToRotation(Vector3.right, dir) : Quaternion.FromToRotation(Vector3.up, dir);
            lastDir = dir;
            passive.GetComponent<Weapon>().Init(4, 1, dir, atkSpeed_Sub);

            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir_two = targetPos - transform.position;
        dir_two = dir_two.normalized;

        Transform passive_two = GameManager.instance.pool.Get(prefabId_Sub).transform;
        passive_two.position = transform.position;
        // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
        passive_two.rotation = Quaternion.FromToRotation(Vector3.right, dir_two);
        passive_two.GetComponent<Weapon>().Init(4, 1, dir_two, atkSpeed_Sub);
    }

    // 고정 공격
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
                // Transform의 parent 속성을 통해 부모를 변경(Poolmanager -> WeaponManager)
                passive.parent = transform;
            }

            passive.localPosition = Vector3.zero;
            passive.localRotation = Quaternion.identity;

            // 방패 회전 로직
            rotVec = Vector3.forward * 360 * i / count_Sub;
            passive.Rotate(rotVec);
            passive.Translate(passive.up * 1.5f, Space.World);

            passive.GetComponent<Weapon>().Init(damage_Sub, -1, Vector3.zero, atkSpeed_Sub); // -1은 무한으로 관통함을 의미.
        }
    }

    // 방패(보조) ON OFF
    void ShieldOnOff()
    {
        // 0. main이나 sub 중 하나라도 shield일 때만 실행
        if (subWeapon != null && (mainWeapon.itemName == "Shield" || subWeapon.itemName == "Shield"))
        {
            // 1. 현재 보조무기가 방패가 아니면 -> 돌고있는 방패를 비활성화
            if (subWeapon != null && subWeapon.atkType != Item.AtkType.atkStatic)
                passive.gameObject.SetActive(false);
            // 2. 현재 보조무기가 방패라면 -> 방패를 다시 활성화
            else if (subWeapon != null && subWeapon.atkType == Item.AtkType.atkStatic)
            {
                // 비활성화된 오브젝트를 활성화 시키려면 활성화된 부모를 찾아서 자식을 찾는 형식으로 접근해야함
                // GetChild함수는 주어진 매개변수의 숫자에 해당하는 자식을 반환한다.
                for (int i = 0; i < transform.childCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }

    // 부메랑 공격_main
    void BoomerangAtk()
    {

    }

    // 부메랑 공격_sub (도끼-보조) -> 랜덤한 방향으로 날아가서 적과 충돌하면 포물선을 그리며 플레이어에게 돌아옴
    void BoomerangAtk_Sub()
    {
        Transform passive_Sub = GameManager.instance.pool.Get(prefabId_Sub).transform;
        passive_Sub.position = player.transform.position;
        Vector3 dir = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
        passive_Sub.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        passive_Sub.GetComponent<Weapon>().Init(1, 1, dir, atkSpeed_Sub);

        

        return;
    }
}
