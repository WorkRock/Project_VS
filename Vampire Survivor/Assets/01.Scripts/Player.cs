using System.Collections;
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
    public BoxCollider2D hitBox;    // 히트박스
    public GameObject slashEffect;  // 슬래시 이펙트
    public float curSlashTime;      // 공격 주기
    public float maxSlashTime = 1.5f;

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
        // 플레이어 관련 옵션 초기화
        player_Hp = player_MaxHp;
        playerLV = 1;
        needExpPerLV = baseExp;
        isLive = true;
        isAtk = false;
        atkTime = 0f;
        curSlashTime = 0f;

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

        // 업그레이드 테스트
        if (Input.GetKeyDown(KeyCode.U))
            UpgradeDefaultAtkSpeed();   // 공격 속도 강화
        if (Input.GetKeyDown(KeyCode.R))
            UpgradeDefaultAtkRange();   // 공격 범위 강화

        SlashOn();
        OnMove();
        Flip();
        Dead();
        ActiveAttack();

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



        // 무기 스왑 테스트 
        // 왼손 무기 : L키 + 숫자 1 2 3 4 5
        if (Input.GetKey(KeyCode.L))
        {
            // 1. 도끼
            if (Input.GetKeyDown(KeyCode.F1))
                leftWeapon.sprite = weaponsSprites[0];
            // 2. 활
            else if (Input.GetKeyDown(KeyCode.F2))       
                leftWeapon.sprite = weaponsSprites[1];
            // 3. 창 
            else if (Input.GetKeyDown(KeyCode.F3))
                leftWeapon.sprite = weaponsSprites[2];
            // 4. 완드
            else if (Input.GetKeyDown(KeyCode.F4))
                leftWeapon.sprite = weaponsSprites[3];
            // 5. 방패
            else if (Input.GetKeyDown(KeyCode.F5))
                leftWeapon.sprite = weaponsSprites[4];
        }
        // 오른손 무기 : R키 + 숫자 1 2 3 4 5
        else if(Input.GetKey(KeyCode.R))
        {
            // 1. 도끼
            if (Input.GetKeyDown(KeyCode.F1))
                rightWeapon.sprite = weaponsSprites[0];
            // 2. 활
            else if (Input.GetKeyDown(KeyCode.F2))
                rightWeapon.sprite = weaponsSprites[1];
            // 3. 창 
            else if (Input.GetKeyDown(KeyCode.F3))
                rightWeapon.sprite = weaponsSprites[2];
            // 4. 완드
            else if (Input.GetKeyDown(KeyCode.F4))
                rightWeapon.sprite = weaponsSprites[3];
            // 5. 방패
            else if (Input.GetKeyDown(KeyCode.F5))
                rightWeapon.sprite = weaponsSprites[4];
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

    // 자동 공격(평타)
    void SlashOn()
    {
        curSlashTime += Time.deltaTime;
        if (curSlashTime >= maxSlashTime)
        {
            if(!isSlash)
            {
                // 자동공격 사운드 재생
                SoundManager.instance.PlaySE("Passive Atk_Sword");
                isSlash = true;
            }
            
            slashEffect.SetActive(true);
            Invoke("SlashOff", 0.4f);
            curSlashTime = 0f;
        }
    }

    void SlashOff()
    {
        slashEffect.SetActive(false);
        isSlash = false;
    }

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
        for (int i = 0; i < playerBodies.Length; i++)
        {
            playerBodies[i].color = new Color(1, 1, 1, 0.4f);
        }
        Invoke("HurtEffectOff", 0.5f);
    }

    public void HurtEffectOff()
    {
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
        slashEffect.transform.localScale += plusRange;
        if (slashEffect.transform.localScale.x >= maxRange.x)
            slashEffect.transform.localScale = maxRange;
    }
}
