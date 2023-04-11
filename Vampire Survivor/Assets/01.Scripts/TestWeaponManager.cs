using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponManager : MonoBehaviour
{
    public Item mainWeapon;
    public Item subWeapon;

    public GameObject[] passive_Main_Setup;  // 마법진 등 준비 동작
    public GameObject[] passive_Main;    // 메인 공격
    public GameObject[] passive_Sub;     // 서브 공격
    // 방패(보조)
    Transform passive;
    // 도끼(보조)
    public GameObject[] passiveAxe;
    // 부메랑 방향(도끼_sub 에 사용중)
    public Vector3[] boomerangDir;    
    bool Thrown;      // 도끼 발사 플래그 
    bool Back;      // 도끼 리턴 플래그
    public float thrownTime;        // 도끼 발사 시간    
    public float keepThrownTime;    // 도끼 체공 시간   
    public float ThrowSpeed;        // 도끼 발사 속도

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
    public int projectileCount;

    // Item 옵션 - Sub
    public int id_Sub;
    public string itemName_Sub;
    public int prefabId_Sub;
    public float damage_Sub;
    public int count_Sub;   // 관통
    public float CT_Sub;    // 쿨타임
    public float atkSpeed_Sub;  // 공격 속도
    public string atkType_Sub;
    public float animTime_Sub;
    public int projectileCount_Sub; // 투사체 개수

    // 타이머
    public float invokeTime_Main;  // 주무기 쿨타임
    public float invokeTime_Sub;  // 보조무기 쿨타임

    // 플레이어
    public Player player;
    // 플레이어가 마지막으로 입력한 방향
    public Vector3 lastDir;
    // 플레이어 이미지가 마지막으로 바라본 방향
    Vector3 lastSpriteDir;

    public Vector3 rotVec;
    private bool isWeaponChangeCheck;

    void Start()
    {
        lastDir = new Vector3(-1, 0);
        isWeaponChangeCheck = false;

        // 평타(투사체)의 1회당 최대 갯수만큼 초기화
        passive_Main_Setup = new GameObject[10];
        passive_Main = new GameObject[10];
        passive_Sub = new GameObject[10];
        passiveAxe = new GameObject[10];
        boomerangDir = new Vector3[10];
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어 이미지가 바라보는 방향을 받아옴
        lastSpriteDir.x = GameManager.instance.player.transform.localScale.x;
        
        transform.position = GameManager.instance.player.transform.position;

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
               
                StaticAtk();                 
                
                // 축 회전
                transform.position = new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y + 0.35f, GameManager.instance.player.transform.position.z);
                transform.Rotate(Vector3.back * atkSpeed_Sub * Time.deltaTime);
                break;
            // 도끼(보조)
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

        
        // 방패(보조) ON OFF
        ShieldOnOff();

        // 도끼 돌아오게하기
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

    #region atkHorDir
    // 횡방향 공격
    void HorAtk()
    {
        switch (projectileCount)
        {
            // # 투사체 개수(projectileCount) == 1
            case 1:
                passive_Main[0] = GameManager.instance.pool.Get(prefabId);
                passive_Main[0].transform.position = GameManager.instance.player.transform.position;
                if (GameManager.instance.player.playerDir == 1 && passive_Main[0].transform.localScale.x > 0)
                    passive_Main[0].transform.localScale = new Vector3(passive_Main[0].transform.localScale.x * -1, passive_Main[0].transform.localScale.y, 0);

                else if (GameManager.instance.player.playerDir == -1 && passive_Main[0].transform.localScale.x < 0)
                    passive_Main[0].transform.localScale = new Vector3(passive_Main[0].transform.localScale.x * -1, passive_Main[0].transform.localScale.y, 0);
      
                Invoke("HorAtk_Off", animTime);
                break;
            // # 투사체 개수(projectileCount) == 2
            case 2:
                passive_Main[0] = GameManager.instance.pool.Get(prefabId);
                passive_Main[1] = GameManager.instance.pool.Get(prefabId);

                passive_Main[0].transform.position = GameManager.instance.player.transform.position;
                passive_Main[1].transform.position = GameManager.instance.player.transform.position;

                passive_Main[1].transform.localScale = new Vector3(passive_Main[0].transform.localScale.x * -1, passive_Main[0].transform.localScale.y, 0);

                Invoke("HorAtk_Off", animTime);
                break;
            // # 투사체 개수(projectileCount) == 3
            case 3:
                passive_Main[0] = GameManager.instance.pool.Get(prefabId);
                passive_Main[1] = GameManager.instance.pool.Get(prefabId);
                passive_Main[2] = GameManager.instance.pool.Get(prefabId);

                passive_Main[0].transform.position = GameManager.instance.player.transform.position;
                passive_Main[1].transform.position = GameManager.instance.player.transform.position;
                passive_Main[2].transform.position = GameManager.instance.player.transform.position;


                passive_Main[0].transform.rotation = Quaternion.FromToRotation(Vector3.right, Vector3.left + Vector3.up);//Quaternion.Euler(0, 0, 45f);
                passive_Main[1].transform.rotation = Quaternion.FromToRotation(Vector3.right, Vector3.right + Vector3.up);//Quaternion.Euler(0, 0, 135f);
                passive_Main[2].transform.rotation = Quaternion.FromToRotation(Vector3.right, Vector3.down);//Quaternion.Euler(0, 0, 270f);

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

    // 횡방향 공격 - 보조(창_보조 휘두르기)
    void HorAtk_Sub()
    {
        
        switch (projectileCount_Sub)
        {
            
            // # 투사체 개수(projectileCount) == 1
            case 1:
                passive_Sub[0] = GameManager.instance.pool.Get(prefabId_Sub);
                passive_Sub[0].transform.position = GameManager.instance.player.transform.position;
                if (GameManager.instance.player.playerDir == 1 && passive_Sub[0].transform.localScale.x > 0)
                    passive_Sub[0].transform.localScale = new Vector3(passive_Sub[0].transform.localScale.x * -1, passive_Sub[0].transform.localScale.y, 0);

                else if (GameManager.instance.player.playerDir == -1 && passive_Sub[0].transform.localScale.x < 0)
                    passive_Sub[0].transform.localScale = new Vector3(passive_Sub[0].transform.localScale.x * -1, passive_Sub[0].transform.localScale.y, 0);

                Invoke("HorAtk_Off_Sub", animTime_Sub);
                break;
            // # 투사체 개수(projectileCount) == 2
            case 2:
                passive_Sub[0] = GameManager.instance.pool.Get(prefabId_Sub);
                passive_Sub[1] = GameManager.instance.pool.Get(prefabId_Sub);

                passive_Sub[0].transform.position = GameManager.instance.player.transform.position;
                passive_Sub[1].transform.position = GameManager.instance.player.transform.position;

                passive_Sub[0].transform.rotation = Quaternion.Euler(0, 0, 0);
                passive_Sub[1].transform.rotation = Quaternion.Euler(0, 0, 180f);

                Invoke("HorAtk_Off_Sub", animTime_Sub);
                break;
            // # 투사체 개수(projectileCount) == 3
            case 3:
                /*
                passive_Sub[0] = GameManager.instance.pool.Get(prefabId_Sub);
                passive_Sub[1] = GameManager.instance.pool.Get(prefabId_Sub);
                passive_Sub[2] = GameManager.instance.pool.Get(prefabId_Sub);

                passive_Sub[0].transform.position = GameManager.instance.player.transform.position;
                passive_Sub[1].transform.position = GameManager.instance.player.transform.position;
                passive_Sub[2].transform.position = GameManager.instance.player.transform.position;


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
    #endregion

    #region atkAllDir
    // 전방향 공격
    void AllAtk()
    {
        // # 투사체 개수(projectileCount) == 1

        // # 투사체 개수(projectileCount) == 2

        // # 투사체 개수(projectileCount) == 3
    }

    // 전방향 공격 - 보조
    void AllAtk_Sub()
    {
        // 현재 투사체 개수에 따라서
        switch (projectileCount_Sub)
        {
            case 1:
                // # 투사체 개수(projectileCount) == 1
                Transform passive_Sub = GameManager.instance.pool.Get(prefabId_Sub).transform;
                
                passive_Sub.position = GameManager.instance.player.transform.position;
                
                Vector3 dir = GameManager.instance.player.inputVec.normalized;
                if (dir == new Vector3(0, 0))
                    dir = lastDir;
                
                passive_Sub.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                
                lastDir = dir;
                
                passive_Sub.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir, atkSpeed_Sub);
                //return;
                break;
            case 2:
                // # 투사체 개수(projectileCount) == 2
                Transform passsive_Sub_1 = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passsive_Sub_2 = GameManager.instance.pool.Get(prefabId_Sub).transform;

                passsive_Sub_1.position = GameManager.instance.player.transform.position;
                passsive_Sub_2.position = GameManager.instance.player.transform.position + Vector3.right * 1.05f;

                Vector3 dir_2 = GameManager.instance.player.inputVec.normalized;
                if (dir_2 == new Vector3(0, 0))
                    dir_2 = lastDir;

                passsive_Sub_1.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);
                passsive_Sub_2.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);
                
                lastDir = dir_2;

                passsive_Sub_1.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_2, atkSpeed_Sub);
                passsive_Sub_2.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_2, atkSpeed_Sub);
                break;
            case 3:
                // # 투사체 개수(projectileCount) == 3
                Transform passsive_Sub_a = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passsive_Sub_b = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passsive_Sub_c = GameManager.instance.pool.Get(prefabId_Sub).transform;

                passsive_Sub_a.position = GameManager.instance.player.transform.position + Vector3.left * 1.05f;
                passsive_Sub_b.position = GameManager.instance.player.transform.position;
                passsive_Sub_c.position = GameManager.instance.player.transform.position + Vector3.right * 1.05f;

                Vector3 dir_3 = GameManager.instance.player.inputVec.normalized;
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
    #endregion

    #region atkAllDir_Fix
    // 전방향 공격(고정)
    void AllAtk_Fix()
    {
        // 현재 투사체 개수에 따라서
        switch (projectileCount)
        {    
            case 1:
                // # 투사체 개수(projectileCount) == 1
                // 1.마법진 생성
                Vector3 dir_C1 = GameManager.instance.player.inputVec.normalized;
                if (dir_C1 == new Vector3(0, 0))
                    dir_C1 = lastSpriteDir * -1;

                passive_Main_Setup[0] = GameManager.instance.pool.Get(prefabId - 1);
                passive_Main_Setup[0].transform.position = GameManager.instance.player.transform.position + dir_C1 * 5;  // 마법진의 위치 = 플레이어위치 + (입력방향 * 거리(5); 거리는 변수화해서 변경 가능)
                passive_Main_Setup[0].transform.rotation = Quaternion.identity;   // 마법진은 로테이션 바꿀필요 X

                // 2. 마법진 비활성화 및 폭발 생성
                Invoke("AllAtk_Fix_Off", 1f);
                break;       
            case 2:
                // # 투사체 개수(projectileCount) == 2
                // 1.마법진 생성
                Vector3 dir_C2 = GameManager.instance.player.inputVec.normalized;
                if (dir_C2 == new Vector3(0, 0))
                    dir_C2 = lastSpriteDir * -1;

                passive_Main_Setup[0] = GameManager.instance.pool.Get(prefabId - 1);
                passive_Main_Setup[1] = GameManager.instance.pool.Get(prefabId - 1);

                passive_Main_Setup[0].transform.position = GameManager.instance.player.transform.position + dir_C2 * 5;  // 마법진의 위치 = 플레이어위치 + (입력방향 * 거리(5); 거리는 변수화해서 변경 가능)
                passive_Main_Setup[1].transform.position = GameManager.instance.player.transform.position + dir_C2 * -5;

                passive_Main_Setup[0].transform.rotation = Quaternion.identity;   // 마법진은 로테이션 바꿀필요 X
                passive_Main_Setup[1].transform.rotation = Quaternion.identity;   // 마법진은 로테이션 바꿀필요 X

                // 2. 마법진 비활성화 및 폭발 생성
                Invoke("AllAtk_Fix_Off", 1f);
                break;
            case 3:
                // # 투사체 개수(projectileCount) == 3
                // 1.마법진 생성
                Vector3 dir_C3 = GameManager.instance.player.inputVec.normalized;
                if (dir_C3 == new Vector3(0, 0))
                    dir_C3 = lastSpriteDir * -1;

                passive_Main_Setup[0] = GameManager.instance.pool.Get(prefabId - 1);
                passive_Main_Setup[1] = GameManager.instance.pool.Get(prefabId - 1);
                passive_Main_Setup[2] = GameManager.instance.pool.Get(prefabId - 1);

                passive_Main_Setup[0].transform.position = GameManager.instance.player.transform.position + dir_C3 * 5;  // 마법진의 위치 = 플레이어위치 + (입력방향 * 거리(5); 거리는 변수화해서 변경 가능)
                passive_Main_Setup[1].transform.position = GameManager.instance.player.transform.position + dir_C3 * -5;
                passive_Main_Setup[2].transform.position = GameManager.instance.player.transform.position + dir_C3 * 5 + Vector3.up * 5;

                passive_Main_Setup[0].transform.rotation = Quaternion.identity;   // 마법진은 로테이션 바꿀필요 X
                passive_Main_Setup[1].transform.rotation = Quaternion.identity;   // 마법진은 로테이션 바꿀필요 X
                passive_Main_Setup[2].transform.rotation = Quaternion.identity;

                // 2. 마법진 비활성화 및 폭발 생성
                Invoke("AllAtk_Fix_Off", 1f);
                break;
        }
    }

    void AllAtk_Fix_Off()
    {
        switch (projectileCount)
        {       
            case 1:
                // # 투사체 개수(projectileCount) == 1
                // 2. 마법진 위치에 폭발 생성
                passive_Main[0] = GameManager.instance.pool.Get(prefabId);
                passive_Main[0].transform.position = passive_Main_Setup[0].transform.position;  // 폭발의 위치 = 마법진의 위치
                passive_Main[0].transform.rotation = Quaternion.identity;
                passive_Main_Setup[0].SetActive(false);   // 마법진이 먼저 사라져버리면 폭발 위치를 받아오지 못하므로 폭발 생성 후 비활성화
                Invoke("ExplosionOff", 0.6f);     // 폭발 비활성화
                break;
            case 2:
                // # 투사체 개수(projectileCount) == 2
                // 2. 마법진 위치에 폭발 생성
                passive_Main[0] = GameManager.instance.pool.Get(prefabId);
                passive_Main[1] = GameManager.instance.pool.Get(prefabId);

                passive_Main[0].transform.position = passive_Main_Setup[0].transform.position;  // 폭발의 위치 = 마법진의 위치
                passive_Main[1].transform.position = passive_Main_Setup[1].transform.position;

                passive_Main[0].transform.rotation = Quaternion.identity;
                passive_Main[1].transform.rotation = Quaternion.identity;
                passive_Main_Setup[0].SetActive(false);   // 마법진이 먼저 사라져버리면 폭발 위치를 받아오지 못하므로 폭발 생성 후 비활성화
                passive_Main_Setup[1].SetActive(false);
                Invoke("ExplosionOff", 0.6f);     // 폭발 비활성화
                break;
            case 3:
                // # 투사체 개수(projectileCount) == 3
                // 2. 마법진 위치에 폭발 생성
                passive_Main[0] = GameManager.instance.pool.Get(prefabId);
                passive_Main[1] = GameManager.instance.pool.Get(prefabId);
                passive_Main[2] = GameManager.instance.pool.Get(prefabId);

                passive_Main[0].transform.position = passive_Main_Setup[0].transform.position;  // 폭발의 위치 = 마법진의 위치
                passive_Main[1].transform.position = passive_Main_Setup[1].transform.position;
                passive_Main[2].transform.position = passive_Main_Setup[2].transform.position;

                passive_Main[0].transform.rotation = Quaternion.identity;
                passive_Main[1].transform.rotation = Quaternion.identity;
                passive_Main[2].transform.rotation = Quaternion.identity;
                passive_Main_Setup[0].SetActive(false);   // 마법진이 먼저 사라져버리면 폭발 위치를 받아오지 못하므로 폭발 생성 후 비활성화
                passive_Main_Setup[1].SetActive(false);
                passive_Main_Setup[2].SetActive(false);
                Invoke("ExplosionOff", 0.6f);     // 폭발 비활성화
                break;
        }
    }
    void ExplosionOff()
    {
        for (int i = 0; i < passive_Main.Length; i++)
        {
            if (passive_Main[i] != null)
                passive_Main[i].SetActive(false);
        }
    }
    #endregion

    #region atkFollow
    // 유도 공격 - 주 (활, 방패)
    void FollowAtk()
    {
        switch (projectileCount)
        {
            // # 투사체 개수(projectileCount) == 1
            case 1:
                if (!GameManager.instance.player.scanner.nearestTarget)
                {
                    Transform passive = GameManager.instance.pool.Get(prefabId).transform;
                    passive.position = GameManager.instance.player.transform.position;
                    Vector3 dir = GameManager.instance.player.inputVec;
                    if (dir == new Vector3(0, 0))
                        dir = lastDir;
                    passive.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    lastDir = dir;
                    passive.GetComponent<Weapon>().Init(damage, count, dir, atkSpeed);

                    return;
                }

                Vector3 targetPos = GameManager.instance.player.scanner.nearestTarget.position;
                Vector3 dir_two = targetPos - GameManager.instance.player.transform.position;
                dir_two = dir_two.normalized;

                Transform passive_two = GameManager.instance.pool.Get(prefabId).transform;
                passive_two.position = GameManager.instance.player.transform.position;
                // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
                passive_two.rotation = Quaternion.FromToRotation(Vector3.up, dir_two);
                passive_two.GetComponent<Weapon>().Init(damage, count, dir_two, atkSpeed);

                break;
            // # 투사체 개수(projectileCount) == 2
            case 2:
                if (!GameManager.instance.player.scanner.nearestTarget)
                {
                    Transform passive_1 = GameManager.instance.pool.Get(prefabId).transform;
                    Transform passive_2 = GameManager.instance.pool.Get(prefabId).transform;

                    passive_1.position = GameManager.instance.player.transform.position;
                    passive_2.position = GameManager.instance.player.transform.position + Vector3.right * 1.05f;

                    Vector3 dir = GameManager.instance.player.inputVec;
                    if (dir == new Vector3(0, 0))
                        dir = lastDir;
                    passive_1.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    passive_2.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    lastDir = dir;
                    passive_1.GetComponent<Weapon>().Init(damage, count, dir, atkSpeed);
                    passive_2.GetComponent<Weapon>().Init(damage, count, dir, atkSpeed);
                    return;
                }

                Vector3 targetPos_Case2 = GameManager.instance.player.scanner.nearestTarget.position;
                Vector3 dir_2 = targetPos_Case2 - GameManager.instance.player.transform.position;
                dir_2 = dir_2.normalized;

                Transform passive_S_1 = GameManager.instance.pool.Get(prefabId).transform;
                Transform passive_S_2 = GameManager.instance.pool.Get(prefabId).transform;

                passive_S_1.position = GameManager.instance.player.transform.position;
                passive_S_2.position = GameManager.instance.player.transform.position + Vector3.right * 1.05f;

                passive_S_1.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);
                passive_S_1.GetComponent<Weapon>().Init(damage, count, dir_2, atkSpeed);
                passive_S_2.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);
                passive_S_2.GetComponent<Weapon>().Init(damage, count, dir_2, atkSpeed);

                break;
            // # 투사체 개수(projectileCount) == 3
            case 3:
                if (!GameManager.instance.player.scanner.nearestTarget)
                {
                    Transform passive_1 = GameManager.instance.pool.Get(prefabId).transform;
                    Transform passive_2 = GameManager.instance.pool.Get(prefabId).transform;
                    Transform passive_3 = GameManager.instance.pool.Get(prefabId).transform;

                    passive_1.position = GameManager.instance.player.transform.position + Vector3.left * 1.05f;
                    passive_2.position = GameManager.instance.player.transform.position + Vector3.right * 1.05f;
                    passive_3.position = GameManager.instance.player.transform.position;

                    Vector3 dir = GameManager.instance.player.inputVec;
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

                Vector3 targetPos_Case3 = GameManager.instance.player.scanner.nearestTarget.position;
                Vector3 dir_3 = targetPos_Case3 - GameManager.instance.player.transform.position;
                dir_3 = dir_3.normalized;

                Transform passive_S_a = GameManager.instance.pool.Get(prefabId).transform;
                Transform passive_S_b = GameManager.instance.pool.Get(prefabId).transform;
                Transform passive_S_c = GameManager.instance.pool.Get(prefabId).transform;

                passive_S_a.position = GameManager.instance.player.transform.position + Vector3.left * 1.05f;
                passive_S_b.position = GameManager.instance.player.transform.position + Vector3.right * 1.05f;
                passive_S_c.position = GameManager.instance.player.transform.position;

                passive_S_a.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);
                passive_S_b.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);
                passive_S_c.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);

                passive_S_a.GetComponent<Weapon>().Init(damage, count, dir_3, atkSpeed);
                passive_S_b.GetComponent<Weapon>().Init(damage, count, dir_3, atkSpeed);
                passive_S_c.GetComponent<Weapon>().Init(damage, count, dir_3, atkSpeed);
                break;
        }        
    }

    // 유도 공격 - 보조 (검, 활, 완드)
    void FollowAtk_Sub()
    {
        switch (projectileCount_Sub)
        {
            // # 투사체 개수(projectileCount) == 1
            case 1:
                if (!GameManager.instance.player.scanner.nearestTarget)
                {
                    Transform passive = GameManager.instance.pool.Get(prefabId_Sub).transform;
                    passive.position = transform.position;
                    Vector3 dir = GameManager.instance.player.inputVec;
                    if (dir == new Vector3(0, 0))
                        dir = lastDir;
                    passive.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    lastDir = dir;
                    passive.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir, atkSpeed_Sub);

                    return;
                }

                Vector3 targetPos = GameManager.instance.player.scanner.nearestTarget.position;
                Vector3 dir_two = targetPos - transform.position;
                dir_two = dir_two.normalized;

                Transform passive_two = GameManager.instance.pool.Get(prefabId_Sub).transform;
                passive_two.position = transform.position;
                // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
                passive_two.rotation = Quaternion.FromToRotation(Vector3.up, dir_two);
                passive_two.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_two, atkSpeed_Sub);

                break;
            // # 투사체 개수(projectileCount) == 2
            case 2:
                if (!GameManager.instance.player.scanner.nearestTarget)
                {
                    Transform passive_1 = GameManager.instance.pool.Get(prefabId_Sub).transform;
                    Transform passive_2 = GameManager.instance.pool.Get(prefabId_Sub).transform;

                    passive_1.position = GameManager.instance.player.transform.position;
                    passive_2.position = GameManager.instance.player.transform.position + Vector3.right * 1.05f;

                    Vector3 dir = GameManager.instance.player.inputVec;
                    if (dir == new Vector3(0, 0))
                        dir = lastDir;
                    passive_1.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    passive_2.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                    lastDir = dir;
                    passive_1.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir, atkSpeed_Sub);
                    passive_2.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir, atkSpeed_Sub);
                    return;
                }

                Vector3 targetPos_Case2 = GameManager.instance.player.scanner.nearestTarget.position;
                Vector3 dir_2 = targetPos_Case2 - GameManager.instance.player.transform.position;
                dir_2 = dir_2.normalized;

                Transform passive_S_1 = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passive_S_2 = GameManager.instance.pool.Get(prefabId_Sub).transform;

                passive_S_1.position = GameManager.instance.player.transform.position;
                passive_S_2.position = GameManager.instance.player.transform.position + Vector3.right * 1.05f;

                passive_S_1.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);        
                passive_S_2.rotation = Quaternion.FromToRotation(Vector3.up, dir_2);

                passive_S_1.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_2, atkSpeed_Sub);
                passive_S_2.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_2, atkSpeed_Sub);

                break;
            // # 투사체 개수(projectileCount) == 3
            case 3:
                if (!GameManager.instance.player.scanner.nearestTarget)
                {
                    Transform passive_1 = GameManager.instance.pool.Get(prefabId_Sub).transform;
                    Transform passive_2 = GameManager.instance.pool.Get(prefabId_Sub).transform;
                    Transform passive_3 = GameManager.instance.pool.Get(prefabId_Sub).transform;

                    passive_1.position = GameManager.instance.player.transform.position + Vector3.left * 1.05f;
                    passive_2.position = GameManager.instance.player.transform.position + Vector3.right * 1.05f;
                    passive_3.position = GameManager.instance.player.transform.position;

                    Vector3 dir = GameManager.instance.player.inputVec;
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

                Vector3 targetPos_Case3 = GameManager.instance.player.scanner.nearestTarget.position;
                Vector3 dir_3 = targetPos_Case3 - GameManager.instance.player.transform.position;
                dir_3 = dir_3.normalized;

                Transform passive_S_a = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passive_S_b = GameManager.instance.pool.Get(prefabId_Sub).transform;
                Transform passive_S_c = GameManager.instance.pool.Get(prefabId_Sub).transform;

                passive_S_a.position = GameManager.instance.player.transform.position + Vector3.left * 1.05f;
                passive_S_b.position = GameManager.instance.player.transform.position + Vector3.right * 1.05f;
                passive_S_c.position = GameManager.instance.player.transform.position;

                passive_S_a.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);
                passive_S_b.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);
                passive_S_c.rotation = Quaternion.FromToRotation(Vector3.up, dir_3);

                passive_S_a.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_3, atkSpeed_Sub);
                passive_S_b.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_3, atkSpeed_Sub);
                passive_S_c.GetComponent<Weapon>().Init(damage_Sub, count_Sub, dir_3, atkSpeed_Sub);
                break;
        }
    }
    #endregion

    #region atkStatic
    // 고정 공격(방패_보조)
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
                // Transform의 parent 속성을 통해 부모를 변경(Poolmanager -> WeaponManager)
                passive.parent = transform;
            }

            passive.localPosition = Vector3.zero;
            passive.localRotation = Quaternion.identity;

            // 방패 회전 로직
            rotVec = Vector3.forward * 360 * i / projectileCount_Sub;
            passive.Rotate(rotVec);
            passive.Translate(passive.up * 1.5f, Space.World);

            passive.GetComponent<Weapon>().Init(damage_Sub, -1, Vector3.zero, atkSpeed_Sub); // -1은 무한으로 관통함을 의미.
        }
    }

    // 방패(보조) ON OFF
    void ShieldOnOff()
    {
        // 0. main이나 sub 중 하나라도 shield일 때만 실행
        if (subWeapon != null)
        {
            // 1. 현재 보조무기가 방패가 아니면 -> 돌고있는 방패를 비활성화
            if (subWeapon.atkType_Sub != Item.AtkType.atkStatic)
            {
                int getChildCount = gameObject.transform.childCount;
                for (int i = 0; i < getChildCount; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            
            // 2. 현재 보조무기가 방패라면 -> 방패를 다시 활성화
            else if (subWeapon.atkType_Sub == Item.AtkType.atkStatic)
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
    #endregion

    #region atkBoomerang
    // 부메랑 공격_main
    void BoomerangAtk()
    {

    }

    // 부메랑 공격_sub (도끼-보조) -> 랜덤한 방향으로 날아가서 적과 충돌하면 포물선을 그리며 플레이어에게 돌아옴
    void BoomerangAtk_Sub()
    {
        switch (projectileCount_Sub)
        {
            case 1:
                // # 투사체 개수(projectileCount) == 1
                passiveAxe[0] = GameManager.instance.pool.Get(prefabId_Sub);
                passiveAxe[0].transform.position = GameManager.instance.player.transform.position;

                // 8방향 랜덤
                boomerangDir[0] = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
                if (boomerangDir[0] == Vector3.zero)
                    boomerangDir[0] = Vector3.right.normalized;

                passiveAxe[0].transform.rotation = Quaternion.FromToRotation(Vector3.up, boomerangDir[0]);  // 프리팹의 방향을 조정
               
                break;
            case 2:
                // # 투사체 개수(projectileCount) == 2
                passiveAxe[0] = GameManager.instance.pool.Get(prefabId_Sub);
                passiveAxe[1] = GameManager.instance.pool.Get(prefabId_Sub);

                passiveAxe[0].transform.position = GameManager.instance.player.transform.position;
                passiveAxe[1].transform.position = GameManager.instance.player.transform.position + new Vector3(0.2f, 0.2f, 0f);

                // 8방향 랜덤
                boomerangDir[0] = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
                boomerangDir[1] = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
                if (boomerangDir[0] == Vector3.zero || boomerangDir[1] == Vector3.zero)
                {
                    boomerangDir[0] = Vector3.right.normalized;
                    boomerangDir[1] = Vector3.left.normalized;
                }
                passiveAxe[0].transform.rotation = Quaternion.FromToRotation(Vector3.up, boomerangDir[0]);  // 프리팹의 방향을 조정
                passiveAxe[1].transform.rotation = Quaternion.FromToRotation(Vector3.up, boomerangDir[1]);  // 프리팹의 방향을 조정

                break;
            case 3:
                // # 투사체 개수(projectileCount) == 3
                passiveAxe[0] = GameManager.instance.pool.Get(prefabId_Sub);
                passiveAxe[1] = GameManager.instance.pool.Get(prefabId_Sub);
                passiveAxe[2] = GameManager.instance.pool.Get(prefabId_Sub);

                passiveAxe[0].transform.position = GameManager.instance.player.transform.position;
                passiveAxe[1].transform.position = GameManager.instance.player.transform.position + new Vector3(0.2f, 0.2f, 0f);
                passiveAxe[2].transform.position = GameManager.instance.player.transform.position - new Vector3(0.2f, 0.2f, 0f);

                // 8방향 랜덤
                boomerangDir[0] = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
                boomerangDir[1] = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
                boomerangDir[2] = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
                if (boomerangDir[0] == Vector3.zero || boomerangDir[1] == Vector3.zero || boomerangDir[2] == Vector3.zero)
                {
                    boomerangDir[0] = Vector3.right.normalized;
                    boomerangDir[1] = Vector3.left.normalized;
                    boomerangDir[2] = Vector3.up.normalized;
                }
                passiveAxe[0].transform.rotation = Quaternion.FromToRotation(Vector3.up, boomerangDir[0]);  // 프리팹의 방향을 조정
                passiveAxe[1].transform.rotation = Quaternion.FromToRotation(Vector3.up, boomerangDir[1]);  // 프리팹의 방향을 조정
                passiveAxe[2].transform.rotation = Quaternion.FromToRotation(Vector3.up, boomerangDir[1]);  // 프리팹의 방향을 조정

                break;
        }       
    }

    void ThrowAndReturn()
    {
        if (Time.timeScale == 0f)
            return;

        if (Thrown)
        {
            for (int i = 0; i < passiveAxe.Length; i++)
            {
                if (passiveAxe[i] != null && passiveAxe[i].activeSelf)
                {
                    // 도끼를 던진다.
                    passiveAxe[i].transform.position = Vector3.Slerp(passiveAxe[i].transform.position, passiveAxe[i].transform.position + (boomerangDir[i] * 1.5f), 0.04f);
                }
            }
            thrownTime += Time.deltaTime;
            if (thrownTime >= keepThrownTime)
            {
                Thrown = false;
                Back = true;
                
                thrownTime = 0f;
            }
        }

        if (!Thrown) //&& Back[0])
        {
            for (int i = 0; i < passiveAxe.Length; i++)
            {
                if (passiveAxe[i] != null && passiveAxe[i].activeSelf)
                {
                    // 도끼를 플레이어에게 되돌아오게 한다.
                    passiveAxe[i].transform.position = Vector3.MoveTowards(passiveAxe[i].transform.position, GameManager.instance.player.transform.position, ThrowSpeed * Time.deltaTime);
                    if (passiveAxe[i].transform.position == GameManager.instance.player.transform.position)
                    {
                        Back = false;
                        passiveAxe[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    void ForceBack()
    {
        if (Time.timeScale == 0f)
            return;

        if (Thrown && subWeapon.itemName != "Axe")
        {
            for (int i = 0; i < passiveAxe.Length; i++)
            {
                if (passiveAxe[i] != null && passiveAxe[i].activeSelf)
                {
                    passiveAxe[i].transform.position = Vector3.MoveTowards(passiveAxe[i].transform.position, GameManager.instance.player.transform.position, ThrowSpeed * Time.deltaTime);
                    if (passiveAxe[i].transform.position == GameManager.instance.player.transform.position)
                    {
                        Back = false;
                        passiveAxe[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    #endregion
}
