using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Space(10f)]
    [Header("Sword")]
    public Transform passivePos_Sword;    // 평타 생성 위치
    public GameObject passive_Main_Sword;   // 검-평타-주무기
    public GameObject passive_Sub_Sword;    // 검-평타-보조무기

    [Space(10f)]
    [Header("Bow")]
    public GameObject passive_Main_Bow;     // 활-평타-주무기
    public GameObject passive_Sub_Bow;      // 활-평타-보조무기

    [Space(10f)]
    [Header("Knife")]
    public Transform passivePos_Knife;    // 평타 생성 위치
    public GameObject passive_Main_Knife;   // 단검-평타-주무기
    public GameObject passive_Sub_Knife;    // 단검-평타-보조무기

    [Space(10f)]
    [Header("Spear")]
    public Transform passivePos_Spear;    // 평타 생성 위치
    public GameObject passive_Main_Spear;   // 창-평타-주무기
    public GameObject passive_Sub_Spear;    // 창-평타-보조무기

    [Space(10f)]
    [Header("Wand")]
    public Transform passivePos_Wand;    // 평타 생성 위치
    public GameObject passive_Main_Wand_Setup;  // 완드-평타-주무기-마법진
    public GameObject passive_Main_Wand;   // 완드-평타-주무기-폭발
    public GameObject passive_Sub_Wand;    // 완드-평타-보조무기

    [Space(10f)]
    [Header("Axe")]
    public Transform passivePos_Axe;    // 평타 생성 위치
    public GameObject passive_Main_Axe;   // 도끼-평타-주무기
    public GameObject passive_Sub_Axe;    // 도끼-평타-보조무기

    [Space(10f)]
    [Header("Shield")]
    public Transform passivePos_Shield;    // 평타 생성 위치
    public GameObject passive_Main_Shield;   // 방패-평타-주무기
    public GameObject passive_Sub_Shield;    // 방패-평타-보조무기

    // 무기 id
    public int id;
    // 프리팹 id
    public int prefabId;
    // 데미지
    public float damage;
    public float maxDamage = 5;
    // 관통 가능 적 개수
    public int count;
    public int maxCount = 7;
    // 방패: 쿨타임, 단검: 공격주기
    public float CT;
    public float maxCT = 230;
    // 방패: 스피드, 단검: 투사체 속도
    public float atkSpeed;
    public float maxatkSpeed = 230;
    // 레벨 업(레벨에 따라 방패 변경)
    public int level;
    public int maxLevel = 10;
   
    SpriteRenderer[] nowShieldSprites;
    public Sprite[] shields;

    private float timer;
    Player player;
    public Vector3 rotVec;
    // 플레이어의 마지막 입력방향 값 저장
    public Vector3 lastDir;

    private void Awake()
    {    
        //player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        player = GameManager.instance.player;
        //Init();
        // 시작 시에는 좌측으로 초기화
        lastDir = new Vector3(-1, 0);
    }
    void Update()
    {
        switch (id)
        {
            // 검
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
            // 화살
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
            // 단검
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
            // 창
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
            // 완드
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
            // 도끼
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
            // 방패 - 서브 (뱀서 성서)
            case 6:
                timer += Time.deltaTime;

                if (timer > CT)
                {
                    timer = 0f;
                    SetShieldPosition();
                }
                // 축 회전
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.35f, player.transform.position.z);
                transform.Rotate(Vector3.back * atkSpeed * Time.deltaTime);
                break;
            // 방패 - 메인
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

        // 레벨에 따라 방패 스프라이트 변경하기
        ChangeShieldSprite();

        // 방패 업그레이드
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 데미지 2, 개수 1, 스피드 10 증가
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

            // 활 - 주?
            case 1:
                //CT = 5f;   // 화살 투척 주기
                break;
            // 단검 - 보조 무기
            case 2:
                //CT = 3f;   // 단검 투척 주기
                break;
            

            // 방패
            case 6:
                //atkSpeed = 150;    // 방패 회전 스피드
                //level = 1;          
                SetShieldPosition();
                break;
        }
    }

    
    // 방패 배치 함수
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
                // Transform의 parent 속성을 통해 부모를 변경(Poolmanager -> WeaponManager)
                shield.parent = transform;
            }
          
            shield.localPosition = Vector3.zero;
            shield.localRotation = Quaternion.identity;

            // 방패 회전 로직
            rotVec = Vector3.forward * 360 * i / count;
            shield.Rotate(rotVec);
            shield.Translate(shield.up * 1.5f, Space.World);

            shield.GetComponent<Weapon>().Init(damage, -1, Vector3.zero, atkSpeed); // -1은 무한으로 관통함을 의미.
        }
    }

    #region 평타 - 검
    // 평타 - 검 - 주무기(뱀서 채찍)
    void Passive_Main_Sword()
    {
        if (!player.isSlash)
        {
            // 사운드 재생
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }
        passive_Main_Sword = GameManager.instance.pool.Get(9);  // 풀에서 평타-검(주무기) 꺼내오기
        passive_Main_Sword.transform.position = passivePos_Sword.position;    // 평타의 위치 지정
        passive_Main_Sword.GetComponent<SpriteRenderer>().flipX = player.playerDir == -1; // 플레이어의 좌우반전에 따라 평타도 반전시키기
        Invoke("Passive_Main_Sword_Off", 0.4f);
    }
    void Passive_Main_Sword_Off()
    {
        passive_Main_Sword.SetActive(false);
        player.isSlash = false;
    }

    // 평타 - 검 - 보조무기(검기 발사)
    void Passive_Sub_Sword()
    {
        if (!player.isSlash)
        {
            // 사운드 재생
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
        // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
        passive_Sub_Sword_two.rotation = Quaternion.FromToRotation(Vector3.right, dir_two);
        passive_Sub_Sword_two.GetComponent<Weapon>().Init(4, 1, dir_two, atkSpeed);

    }
    #endregion

    #region 평타 - 활
    // 평타 - 활 - 주무기
    void Passive_Main_Bow()
    {
        if (!player.isSlash)
        {
            // 사운드 재생
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
        // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
        passive_Main_Bow_two.rotation = Quaternion.FromToRotation(Vector3.up, dir_two);
        passive_Main_Bow_two.GetComponent<Weapon>().Init(4, 1, dir_two, atkSpeed);
    }

    // 평타 - 활 - 보조무기
    void Passive_Sub_Bow()
    {
        if (!player.isSlash)
        {
            // 사운드 재생
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
        // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
        passive_Sub_Bow_two.rotation = Quaternion.FromToRotation(Vector3.up, dir_two);
        passive_Sub_Bow_two.GetComponent<Weapon>().Init(4, 1, dir_two, atkSpeed);
    }

    #endregion

    #region 평타 - 단검
    // 평타 - 단검 - 주무기
    void Passive_Main_Knife()
    {
        if (!player.isSlash)
        {
            // 사운드 재생
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }
        passive_Main_Knife = GameManager.instance.pool.Get(11);  // 풀에서 평타-단검(주무기) 꺼내오기
        passive_Main_Knife.transform.position = passivePos_Knife.position;    // 평타의 위치 지정
        passive_Main_Knife.GetComponent<SpriteRenderer>().flipX = player.playerDir == -1; // 플레이어의 좌우반전에 따라 평타도 반전시키기
        Invoke("Passive_Main_Knife_Off", 0.3f);
    }
    void Passive_Main_Knife_Off()
    {
        passive_Main_Knife.SetActive(false);
        player.isSlash = false;
    }

    // 평타 - 단검 - 보조무기
    // 단검 발사(원거리 공격)
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

    #region 평타 - 창
    // 평타 - 창 - 주무기(길게 찌르기)
    void Passive_Main_Spear()
    {
        if (!player.isSlash)
        {
            // 사운드 재생
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }
        passive_Main_Spear = GameManager.instance.pool.Get(14);  // 풀에서 평타-창(주무기) 꺼내오기
        passive_Main_Spear.transform.position = passivePos_Spear.position;    // 평타의 위치 지정
        passive_Main_Spear.GetComponent<SpriteRenderer>().flipX = player.playerDir == -1; // 플레이어의 좌우반전에 따라 평타도 반전시키기
        Invoke("Passive_Main_Spear_Off", 0.4f);
    }

    void Passive_Main_Spear_Off()
    {
        passive_Main_Spear.SetActive(false);
        player.isSlash = false;
    }
    // 평타 - 창 - 보조무기(원형 휘두르기)
    void Passive_Sub_Spear()
    {
        if (!player.isSlash)
        {
            // 사운드 재생
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }
        passive_Sub_Spear = GameManager.instance.pool.Get(13);  // 풀에서 평타-창(보조무기) 꺼내오기
        passive_Sub_Spear.transform.position = transform.position;    // 평타의 위치 지정
        passive_Sub_Spear.GetComponent<SpriteRenderer>().flipX = player.playerDir == 1; // 플레이어의 좌우반전에 따라 평타도 반전시키기
        Invoke("Passive_Sub_Spear_Off", 0.4f);
    }
    void Passive_Sub_Spear_Off()
    {
        passive_Sub_Spear.SetActive(false);
        player.isSlash = false;
    }
    #endregion

    #region 평타 - 완드
    // 평타 - 완드 - 주무기(익스플로전)
    void Passive_Main_Wand()
    {
        // 1. 마법진 생성
        Vector3 dir = player.inputVec.normalized;
        if (dir == new Vector3(0, 0))
            dir = lastDir;
        
        lastDir = dir;

        passive_Main_Wand_Setup = GameManager.instance.pool.Get(16);
        passive_Main_Wand_Setup.transform.position = transform.position + dir * 5;  // 마법진의 위치 = 플레이어위치 + (입력방향 * 거리(5); 거리는 변수화해서 변경 가능)
        passive_Main_Wand_Setup.transform.rotation = Quaternion.identity;   // 마법진은 로테이션 바꿀필요 X

        Invoke("Passive_Main_Wand_Setup_Off", 1f);  // 마법진 비활성화 및 폭발 생성      

        return;
    }

    void Passive_Main_Wand_Setup_Off()
    {
        // 2. 마법진 위치에 폭발 생성
        passive_Main_Wand = GameManager.instance.pool.Get(17);
        passive_Main_Wand.transform.position = passive_Main_Wand_Setup.transform.position;  // 폭발의 위치 = 마법진의 위치
        passive_Main_Wand.transform.rotation = Quaternion.identity;
        passive_Main_Wand_Setup.SetActive(false);   // 마법진이 먼저 사라져버리면 폭발 위치를 받아오지 못하므로 폭발 생성 후 비활성화
        Invoke("ExplosionOff", 0.3f);     // 폭발 비활성화
    }

    void ExplosionOff()
    {
        passive_Main_Wand.SetActive(false);
    }

    // 평타 - 완드 - 보조무기(에너지 볼트)
    void Passive_Sub_Wand()
    {
        if (!player.isSlash)
        {
            // 사운드 재생
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
        // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
        passive_Sub_Wand_two.rotation = Quaternion.FromToRotation(Vector3.right, dir_two);
        passive_Sub_Wand_two.GetComponent<Weapon>().Init(4, 1, dir_two, atkSpeed);
    }
    #endregion

    #region 평타 - 도끼
    // 평타 - 도끼 - 주무기(내려찍기)
    void Passive_Main_Axe()
    {
        if (!player.isSlash)
        {
            // 사운드 재생
            SoundManager.instance.PlaySE("Passive Atk_Sword");
            player.isSlash = true;
        }
        passive_Main_Axe = GameManager.instance.pool.Get(18);  // 풀에서 평타-도끼(주무기) 꺼내오기
        passive_Main_Axe.transform.position = passivePos_Axe.position;    // 평타의 위치 지정
        passive_Main_Axe.GetComponent<SpriteRenderer>().flipX = player.playerDir == 1; // 플레이어의 좌우반전에 따라 평타도 반전시키기
        Invoke("Passive_Main_Axe_Off", 0.4f);
    }

    void Passive_Main_Axe_Off()
    {
        passive_Main_Axe.SetActive(false);
        player.isSlash = false;
    }

    // 평타 - 도끼 - 보조무기(튕기는 도끼)
    void Passive_Sub_Axe()
    {
        passive_Sub_Axe = GameManager.instance.pool.Get(19);   // 풀에서 평타-도끼(보조무기) 꺼내오기
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

    #region 평타 - 방패
    // 평타 - 방패 - 주무기(돌아오는 방패)
    void Passive_Main_Shield()
    {
        if (!player.isSlash)
        {
            // 사운드 재생
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
        // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
        passive_Main_Shield_two.rotation = Quaternion.FromToRotation(Vector3.up, dirTwo);
        passive_Main_Shield_two.GetComponent<Weapon>().Init(4, 1, dirTwo, atkSpeed);
    }

    // 평타 - 방패 - 보조무기(뱀서 성서)
    void Passive_Sub_Shield()
    {
        
    }

    void Passive_Sub_Shield_Off()
    {
       
    }
    #endregion

    // 방패 스프라이트 변경
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
