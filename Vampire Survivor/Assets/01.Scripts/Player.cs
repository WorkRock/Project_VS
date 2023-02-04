using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어 관련 속성
    public int speed;
    public Vector2 inputVec;
    public Scanner scanner;

    // 플레이어 체력
    public float player_Hp;
    public float player_MaxHp = 10;

    // 플레이어 공격력
    public float player_Atk;

    // 플레이어 레벨, 경험치
    public int playerLV;
    public int baseExp = 5;
    public int nowExp;  // 현재 경험치
    public int needExpPerLV; // 레벨 당 필요 경험치

    // 히트박스
    public BoxCollider2D hitBox;

    // 컴포넌트
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer sprite;
    Animator animator;

    // 생존 체크
    private bool isLive;
    // 공격 체크
    private bool isAtk;
    // 히트박스 활성화 시간 체크
    private float atkTime;

    // 무기 스왑용 스프라이트 배열 -> 나중에 최적화 고려하면 게임매니저에다 만드는게 나을듯
    public Sprite[] weaponsSprites;
    // 현재 착용 중인 무기 스프라이트
    public SpriteRenderer rightWeapon;
    public SpriteRenderer leftWeapon;

    // 피격 효과(알파값 조정)을 위해 플레이어의 자식으로 있는 스프라이트 렌더러들을 연결
    private SpriteRenderer[] playerBodies;
    
    // 원래 색깔 저장
    private Color[] originColor;

    // 슬래시 이펙트
    public GameObject slashEffect;
    public float curSlashTime;
    public float maxSlashTime = 1.5f;

    // 자동 공격 사운드 한번만 사용되도록
    private bool isSlash;

    // 최대 공격 범위
    public Vector3 maxRange = new Vector3(12f, 12f, 12f);
    // 공격 범위 증가 폭(기본 3, 50%증가)
    private Vector3 plusRange = new Vector3(1.5f, 1.5f, 1.5f);

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        // 피격 효과(알파값 조정)을 위해 플레이어의 자식으로 있는 스프라이트 렌더러들을 연결
        playerBodies = GetComponentsInChildren<SpriteRenderer>();
        originColor = new Color[playerBodies.Length];
        scanner = GetComponent<Scanner>();
    }

    void Start()
    {
        player_Hp = player_MaxHp;
        // 레벨 및 필요 경험치 초기화
        playerLV = 1;
        needExpPerLV = baseExp;
        isLive = true;
        isAtk = false;
        atkTime = 0f;
        curSlashTime = 0f;

        for(int i = 0; i < playerBodies.Length; i++)
        {
            originColor[i] = playerBodies[i].color;
        }
    }

    void Update()
    {
        // 모든 동작은 살아있을 때만
        if (!isLive)
            return;

        if (Input.GetKeyDown(KeyCode.U))
            UpgradeDefaultAtkSpeed();
        if (Input.GetKeyDown(KeyCode.R))
            UpgradeDefaultAtkRange();

        SlashOn();
        OnMove();
        Flip();
        Dead();


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
        
        

        if(isAtk)
        {
            // 공격 중일 땐 이동 x
            inputVec = new Vector2(0, 0);

            // 히트 박스 활성화
            hitBox.enabled = true; 
            atkTime += Time.deltaTime;

            // 0.2초 후 히트 박스 끄고 시간 초기화, 공격 상태 false
            if(atkTime >= 0.2f)
            {
                hitBox.enabled = false;
                atkTime = 0f;
                isAtk = false;
            }
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
        // 체력이 0이 되었을 때
        if (player_Hp <= 0)
        {
            // 플레이어 사망 사운드 재생
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
            // 플레이어 히트 사운드 재생
            SoundManager.instance.PlaySE("Player Hit");
            animator.SetTrigger("isHit");
            player_Hp--;

            HurtEffectOn();     
        }
    }

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
