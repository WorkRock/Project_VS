﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어 관련 속성
    [Header("Player Attributes")]
    public int speed;
    public Vector2 inputVec;
    public Scanner scanner;

    // 플레이어 체력
    [Space(10f)]
    [Header("HP")]
    public float player_Hp;
    public float player_MaxHp = 10;

    // 플레이어 공격 관련
    [Space(10f)]
    [Header("Atk")]
    public float player_Atk;    // 공격력
    public BoxCollider2D hitBox;    // 히트박스(스킬)

    // 0-검 1-활 2-단검 3-창 4-완드 5-도끼 6-방패
    public int weaponNum;
    // 현재 무기가 주무기인지, 보조무기인지 저장(테스트용)
    private bool isMain;
    public float playerDir; // 플레이어가 바라보는 방향(transform.localScale.x)값 저장

    [Space(10f)]
    [Header("Sword")]
    public float curSlashTime;      // 공격 주기
    public float maxSlashTime = 1.5f;  
    public Transform passivePos;    // 평타 생성 위치
    public GameObject passive_Main_Sword;   // 검-평타-주무기
    public GameObject passive_Sub_Sword;    // 검-평타-보조무기

    [Space(10f)]
    [Header("Bow")]
    public float curBowTime;
    public float maxBowTime = 1.5f;
    public GameObject passive_Main_Bow;     // 활-평타-주무기
    public GameObject passive_Sub_Bow;      // 활-평타-보조무기

    [Space(10f)]
    [Header("Knife")]
    public float curKnifeTime;
    public float maxKnifeTime = 1.5f;
    public Transform passivePos_Knife;    // 평타 생성 위치
    public GameObject passive_Main_Knife;   // 단검-평타-주무기
    public GameObject passive_Sub_Knife;    // 단검-평타-보조무기

    [Space(10f)]
    [Header("Spear")]
    public float curSpearTime;
    public float maxSpearTime = 1.5f;
    public Transform passivePos_Spear;    // 평타 생성 위치
    public GameObject passive_Main_Spear;   // 창-평타-주무기
    public GameObject passive_Sub_Spear;    // 창-평타-보조무기

    // 플레이어 레벨, 경험치
    [Space(10f)]
    [Header("Lv & Exp")]  
    public int playerLV;
    public int baseExp = 5;
    public int nowExp;  // 현재 경험치
    public int needExpPerLV; // 레벨 당 필요 경험치

    // 무기 스왑 관련
    [Space(10f)]
    [Header("Swap")]
    public Sprite[] weaponsSprites; // 무기 스왑용 스프라이트 배열 -> 나중에 최적화 고려하면 게임매니저에다 만드는게 나을듯
    // 현재 착용 중인 무기 스프라이트
    public SpriteRenderer rightWeapon;
    public SpriteRenderer leftWeapon;

    // 컴포넌트
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer sprite;
    Animator animator;
 
    private bool isLive;    // 생존 체크
    private bool isAtk;     // 공격 체크
    private float atkTime;  // 히트박스 활성화 시간 체크 
    private SpriteRenderer[] playerBodies;  // 피격 효과(알파값 조정)을 위해 플레이어의 자식으로 있는 스프라이트 렌더러들을 연결
    private Color[] originColor;    // 원래 색깔 저장  
    private bool isSlash;   // 사운드 한번만 사용되도록 체크  
    private Vector3 maxRange = new Vector3(12f, 12f, 12f);   // 최대 공격 범위
    private Vector3 plusRange = new Vector3(1.5f, 1.5f, 1.5f);  // 공격 범위 증가 폭(기본 3, 50%증가)

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); 
        playerBodies = GetComponentsInChildren<SpriteRenderer>();   // 피격 효과(알파값 조정)을 위해 플레이어의 자식으로 있는 스프라이트 렌더러들을 연결
        originColor = new Color[playerBodies.Length];   // 피격 효과를 위해 플레이어의 원래 색깔을 저장
        scanner = GetComponent<Scanner>();
    }

    void Start()
    {
        // 무기 번호 초기화 (기본-0, 검)
        weaponNum = 0;
        // 플레이어 관련 옵션 초기화
        player_Hp = player_MaxHp;
        playerLV = 1;
        needExpPerLV = baseExp;
        isLive = true;
        isAtk = false;
        atkTime = 0f;
        curSlashTime = 0f;
        curKnifeTime = 0f;
        curBowTime = 0f;
        curSpearTime = 0f;

        isMain = true;
        
        // 피격 효과를 위해 플레이어의 원래 색깔을 저장
        for (int i = 0; i < playerBodies.Length; i++)
        {
            originColor[i] = playerBodies[i].color;
        }
    }

    void Update()
    {
        // 모든 동작은 살아있을 때만
        if (!isLive)
            return;

        // 무기 변경
        ChangeWeapon();

        // @@@@@@@@주무기(오른쪽) 보조무기(왼쪽) 스왑에 따른 평타 변경 테스트
        if (Input.GetKeyDown(KeyCode.O))
        {
            isMain = !isMain;
            if(rightWeapon.sprite != null)
            {
                leftWeapon.sprite = rightWeapon.sprite;
                rightWeapon.sprite = null;
            }
            else if(rightWeapon.sprite == null)
            {
                rightWeapon.sprite = leftWeapon.sprite;
                leftWeapon.sprite = null;
            }         
        }

        // update에서 플레이어가 바라보는 방향을 지속적으로 받아오기
        playerDir = transform.localScale.x;

        // 업그레이드 테스트
        if (Input.GetKeyDown(KeyCode.U))
            UpgradeDefaultAtkSpeed();   // 공격 속도 강화
        if (Input.GetKeyDown(KeyCode.R))
            UpgradeDefaultAtkRange();   // 공격 범위 강화

        // 무기 종류에 따라 다른 평타가 나가도록 검사
        switch (weaponNum)
        {
            // 검
            case 0:
                SlashOn();              // 검-메인-평타
                Passive_Sub_Sword_On(); // 검-서브-평타
                break;
            // 활
            case 1:
                Passive_Main_Bow();     // 활-메인-평타
                Passive_Sub_Bow();      // 활-서브-평타
                break;
            // 단검
            case 2:
                Passive_Main_Knife();   // 단검-메인-평타          
                Passive_Sub_Knife();    // 단검-서브-평타
                break;
            // 창
            case 3:
                Passive_Main_Spear();   // 창-메인-평타
                Passive_Sub_Spear();    // 창-서브-평타
                break;
        }
        
        OnMove();   // 이동
        Flip();     // 좌우 반전
        Dead();     // 사망 체크
        ActiveAttack(); // 수동 공격

        // 공격 테스트
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isAtk = true;
            // 플레이어 수동공격 사운드 재생
            SoundManager.instance.PlaySE("Active Atk_Sword");
            animator.SetTrigger("isNormal");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            isAtk = true;
            animator.SetTrigger("isBow");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            isAtk = true;
            animator.SetTrigger("isMagic");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            isAtk = true;
            // 플레이어 특수공격 사운드 재생
            SoundManager.instance.PlaySE("Special Atk_Sword");
            animator.SetTrigger("SNormal");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            isAtk = true;
            animator.SetTrigger("SBow");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            isAtk = true;
            animator.SetTrigger("SMagic");
        }   
    }

    // rigid 관련 로직은 fixedUpdate에서 수행
    void FixedUpdate()
    {
        if (!isLive)
            return;

        // 이동거리(벡터) = 방향 * 속도 * 시간
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        
    }

    // 수동 공격
    void ActiveAttack()
    {
        if (isAtk)
        {
            // 공격 중일 땐 이동 x
            inputVec = new Vector2(0, 0);

            // 히트 박스 활성화
            hitBox.enabled = true;
            atkTime += Time.deltaTime;

            // 0.2초 후 히트 박스 끄고 시간 초기화, 공격 상태 false
            if (atkTime >= 0.2f)
            {
                hitBox.enabled = false;
                atkTime = 0f;
                isAtk = false;
            }
        }
    }

    #region 평타 - 검
    // 평타 - 검 - 주무기(뱀서 채찍)
    void SlashOn()
    {
        if (!isMain)
            return;

        curSlashTime += Time.deltaTime;
        if (curSlashTime >= maxSlashTime)
        {
            if(!isSlash)
            {
                // 사운드 재생
                SoundManager.instance.PlaySE("Passive Atk_Sword");
                isSlash = true;
            }
            passive_Main_Sword = GameManager.instance.pool.Get(9);  // 풀에서 평타-검(주무기) 꺼내오기
            passive_Main_Sword.transform.position = passivePos.position;    // 평타의 위치 지정
            passive_Main_Sword.GetComponent<SpriteRenderer>().flipX = playerDir == -1; // 플레이어의 좌우반전에 따라 평타도 반전시키기
            Invoke("SlashOff", 0.4f);
            curSlashTime = 0f;
        }
    }
    void SlashOff()
    {
        passive_Main_Sword.SetActive(false);
        isSlash = false;
    }

    // 평타 - 검 - 보조무기(검기 발사)
    void Passive_Sub_Sword_On()
    {
        if (isMain)
            return;

        curSlashTime += Time.deltaTime;
        if (curSlashTime >= maxSlashTime)
        {
            if (!isSlash)
            {
                // 사운드 재생
                SoundManager.instance.PlaySE("Passive Atk_Sword");
                isSlash = true;
            }
            passive_Sub_Sword = GameManager.instance.pool.Get(10);  // 풀에서 평타-검(보조무기) 꺼내오기
            passive_Sub_Sword.transform.position = passivePos.position; // 평타의 위치 지정
            passive_Sub_Sword.GetComponent<SpriteRenderer>().flipX = playerDir == -1; // 플레이어의 좌우반전에 따라 평타도 반전시키기                                                                                  
            passive_Sub_Sword.GetComponent<Rigidbody2D>().AddForce(Vector2.left * playerDir * 4.5f, ForceMode2D.Impulse);   // 플레이어가 바라보는 방향(playerDir)으로 검기 발사
            curSlashTime = 0f;
        }
    }

    #endregion

    #region 평타 - 활(석궁)
    // 평타 - 활 - 주무기
    void Passive_Main_Bow()
    {
        if (!isMain)
            return;

        curBowTime += Time.deltaTime;

        if (curBowTime > maxBowTime)
        {
            curBowTime = 0f;
            if (!this.scanner.nearestTarget)
            {
                Transform bowTwo = GameManager.instance.pool.Get(12).transform;
                bowTwo.position = transform.position;

                Vector3 dirTwo = this.inputVec;
                dirTwo.x = (-1) * this.transform.localScale.x;

                bowTwo.rotation = Quaternion.FromToRotation(Vector3.up, dirTwo);
                bowTwo.GetComponent<Weapon>().Init(4, 1, dirTwo);
                //rigid.AddForce(transform.position, ForceMode2D.Impulse);

                return;
            }

            Vector3 targetPos = this.scanner.nearestTarget.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;

            Transform bow = GameManager.instance.pool.Get(12).transform;
            bow.position = transform.position;
            // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
            bow.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bow.GetComponent<Weapon>().Init(4, 1, dir);
        }
    }

    // 평타 - 활 - 보조무기
    void Passive_Sub_Bow()
    {
        if (isMain)
            return;

        curBowTime += Time.deltaTime;

        if (curBowTime > maxBowTime)
        {
            curBowTime = 0f;
            if (!this.scanner.nearestTarget)
            {
                Transform bowTwo = GameManager.instance.pool.Get(8).transform;
                bowTwo.position = transform.position;

                Vector3 dirTwo = this.inputVec;
                dirTwo.x = (-1) * this.transform.localScale.x;

                bowTwo.rotation = Quaternion.FromToRotation(Vector3.up, dirTwo);
                bowTwo.GetComponent<Weapon>().Init(1, 1, dirTwo);
                //rigid.AddForce(transform.position, ForceMode2D.Impulse);

                return;
            }

            Vector3 targetPos = this.scanner.nearestTarget.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;

            Transform bow = GameManager.instance.pool.Get(8).transform;
            bow.position = transform.position;
            // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
            bow.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bow.GetComponent<Weapon>().Init(1, 1, dir);
        }
    }
    #endregion

    #region 평타 - 단검
    // 평타 - 단검 - 주무기
    void Passive_Main_Knife()
    {
        if (!isMain)
            return;

        curKnifeTime += Time.deltaTime;
        if (curKnifeTime >= maxKnifeTime)
        {
            if (!isSlash)
            {
                // 사운드 재생
                SoundManager.instance.PlaySE("Passive Atk_Sword");
                isSlash = true;
            }
            passive_Main_Knife = GameManager.instance.pool.Get(11);  // 풀에서 평타-단검(주무기) 꺼내오기
            passive_Main_Knife.transform.position = passivePos_Knife.position;    // 평타의 위치 지정
            passive_Main_Knife.GetComponent<SpriteRenderer>().flipX = playerDir == -1; // 플레이어의 좌우반전에 따라 평타도 반전시키기
            Invoke("Passive_Main_Knife_Off", 0.3f);
            curKnifeTime = 0f;
        }
    }
    void Passive_Main_Knife_Off()
    {
        passive_Main_Knife.SetActive(false);
        isSlash = false;
    }

    // 평타 - 단검 - 보조무기
    // 단검 발사(원거리 공격)
    void Passive_Sub_Knife()
    {
        if (isMain)
            return;

        curKnifeTime += Time.deltaTime;

        if (curKnifeTime > maxKnifeTime)
        {
            curKnifeTime = 0f;
            if (!this.scanner.nearestTarget)
            {
                Transform knifeTwo = GameManager.instance.pool.Get(7).transform;
                knifeTwo.position = transform.position;

                Vector3 dirTwo = this.inputVec;
                dirTwo.x = (-1) * this.transform.localScale.x;

                knifeTwo.rotation = Quaternion.FromToRotation(Vector3.up, dirTwo);
                knifeTwo.GetComponent<Weapon>().Init(1, 1, dirTwo);
                //rigid.AddForce(transform.position, ForceMode2D.Impulse);

                return;
            }

            Vector3 targetPos = this.scanner.nearestTarget.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;

            Transform knife = GameManager.instance.pool.Get(7).transform;
            knife.position = transform.position;
            // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
            knife.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            knife.GetComponent<Weapon>().Init(1, 1, dir);
        }    
    }
    #endregion

    #region 평타 - 창
    // 평타 - 창 - 주무기(길게 찌르기)
    void Passive_Main_Spear()
    {
        if (!isMain)
            return;

        curSpearTime += Time.deltaTime;
        if (curSpearTime >= maxSpearTime)
        {
            if (!isSlash)
            {
                // 사운드 재생
                SoundManager.instance.PlaySE("Passive Atk_Sword");
                isSlash = true;
            }
            passive_Main_Spear = GameManager.instance.pool.Get(14);  // 풀에서 평타-창(주무기) 꺼내오기
            passive_Main_Spear.transform.position = passivePos_Spear.position;    // 평타의 위치 지정
            passive_Main_Spear.GetComponent<SpriteRenderer>().flipX = playerDir == -1; // 플레이어의 좌우반전에 따라 평타도 반전시키기
            Invoke("Passive_Main_Spear_Off", 0.4f);
            curSpearTime = 0f;
        }
    }

    void Passive_Main_Spear_Off()
    {
        passive_Main_Spear.SetActive(false);
        isSlash = false;
    }
    // 평타 - 창 - 보조무기(원형 휘두르기)
    void Passive_Sub_Spear()
    {
        if (isMain)
            return;

        curSpearTime += Time.deltaTime;
        if (curSpearTime >= maxSpearTime)
        {
            if (!isSlash)
            {
                // 사운드 재생
                SoundManager.instance.PlaySE("Passive Atk_Sword");
                isSlash = true;
            }
            passive_Sub_Spear = GameManager.instance.pool.Get(13);  // 풀에서 평타-창(보조무기) 꺼내오기
            passive_Sub_Spear.transform.position = transform.position;    // 평타의 위치 지정
            passive_Sub_Spear.GetComponent<SpriteRenderer>().flipX = playerDir == 1; // 플레이어의 좌우반전에 따라 평타도 반전시키기
            Invoke("Passive_Sub_Spear_Off", 0.4f);
            curSpearTime = 0f;
        }
    }
    void Passive_Sub_Spear_Off()
    {
        passive_Sub_Spear.SetActive(false);
        isSlash = false;
    }
    #endregion
    void OnMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        inputVec = new Vector2(h, v);

        if (inputVec != new Vector2(0, 0))
            animator.SetBool("isRun", true);
        else
            animator.SetBool("isRun", false);
    }

    void Flip()
    {
        if (inputVec.x < 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (inputVec.x > 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    void Dead()
    {
        if (player_Hp <= 0)
        {
            for (int i = 0; i < playerBodies.Length; i++)
            {
                playerBodies[i].color = originColor[i];
            }

            SoundManager.instance.PlaySE("Player Die");
            capsuleCollider.enabled = false;
            animator.SetTrigger("isDead");
            isLive = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            SoundManager.instance.PlaySE("Player Hit");
            animator.SetTrigger("isHit");
            player_Hp--;
            HurtEffectOn();     
        }
    }

    // 플레이어 피격효과 함수(알파값 조정)
    public void HurtEffectOn()
    {
        // 피격 시 콜라이더 끄기
        capsuleCollider.enabled = false;

        for (int i = 0; i < playerBodies.Length; i++)
        {
            playerBodies[i].color = new Color(1, 1, 1, 0.4f);
        }
        Invoke("HurtEffectOff", 0.5f);
    }

    public void HurtEffectOff()
    {
        if (player_Hp <= 0)
            return;
        // 콜라이더 켜기
        capsuleCollider.enabled = true;

        for (int i = 0; i < playerBodies.Length; i++)
        {
            playerBodies[i].color = originColor[i];
        }
    }

    // 평타 공격 속도 증가 함수
    void UpgradeDefaultAtkSpeed()
    {
        maxSlashTime -= 0.3f;
        if (maxSlashTime <= 0.6f)
            maxSlashTime = 0.6f;
    }

    // 평타 공격 범위 증가 함수
    void UpgradeDefaultAtkRange()
    {
        passive_Main_Sword.transform.localScale += plusRange;
        if (passive_Main_Sword.transform.localScale.x >= maxRange.x)
            passive_Main_Sword.transform.localScale = maxRange;
    }

    // 무기 변경 함수
    void ChangeWeapon()
    {
        // F1 ~ 5
        // 1. 검
        if (Input.GetKeyDown(KeyCode.F1))
        {
            rightWeapon.sprite = weaponsSprites[0];
            weaponNum = 0;
        }
        // 2. 활
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            rightWeapon.sprite = weaponsSprites[1];
            weaponNum = 1;
        }
        // 3. 단검
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            rightWeapon.sprite = weaponsSprites[2];
            weaponNum = 2;
        }
        // 4. 창
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            rightWeapon.sprite = weaponsSprites[3];
            weaponNum = 3;
        }
        // 5. 완드
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            rightWeapon.sprite = weaponsSprites[4];
            weaponNum = 4;
        }
        // 6. 도끼
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            rightWeapon.sprite = weaponsSprites[5];
            weaponNum = 5;
        }
        // 7. 방패
        else if (Input.GetKeyDown(KeyCode.F7))
        {
            rightWeapon.sprite = weaponsSprites[6];
            weaponNum = 6;
        }
    }
}
