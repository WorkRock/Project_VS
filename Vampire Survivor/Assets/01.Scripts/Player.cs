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
    public BoxCollider2D hitBox;    // 히트박스(스킬)

    public GameObject[] selectedWeapon;

    public WeaponChange weaponChange;
    public Item mainWeapon;
    public Item subWeapon;
    // 0-검 1-활 2-단검 3-창 4-완드 5-도끼 6-방패
    public int weaponNum;
    // 현재 무기가 주무기인지, 보조무기인지 저장(테스트용)
    public bool isMain;
    public float playerDir; // 플레이어가 바라보는 방향(transform.localScale.x)값 저장

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
    public SpriteRenderer mainWeaponSprite;
    public SpriteRenderer subWeaponSprite;

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
    public bool isSlash;   // 사운드 한번만 사용되도록 체크  
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
        // 시작 시 main무기와 sub무기 정보를 받아온다.
        mainWeapon = weaponChange.getMainWeapoon();
        subWeapon = weaponChange.getSubWeapoon();


        // 무기 번호 초기화 (기본-0, 검)
        weaponNum = 0;
        selectedWeapon[0].SetActive(true);
        // 플레이어 관련 옵션 초기화
        player_Hp = player_MaxHp;
        playerLV = 1;
        needExpPerLV = baseExp;
        isLive = true;
        isAtk = false;
        atkTime = 0f;

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


        // @@@@@@@@주무기(오른쪽) 보조무기(왼쪽) 스왑에 따른 평타 변경 테스트
        if (Input.GetKeyDown(KeyCode.O))
        {
            weaponChange.swapWeapon();
          
            /*
            isMain = !isMain;
            if(mainWeaponSprite.sprite != null)
            {
                subWeaponSprite.sprite = mainWeaponSprite.sprite;
                mainWeaponSprite.sprite = null;
            }
            else if(mainWeaponSprite.sprite == null)
            {
                mainWeaponSprite.sprite = subWeaponSprite.sprite;
                subWeaponSprite.sprite = null;
            }
            */
        }

        // update에서 플레이어가 바라보는 방향을 지속적으로 받아오기
        playerDir = transform.localScale.x;
        
        OnMove();   // 이동
        Flip();     // 좌우 반전
        Dead();     // 사망 체크
        ActiveAttack(); // 수동 공격
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

    public void setMainWeapon(Item mainWeapon)
    {
        this.mainWeapon = mainWeapon;
    }

    public void setSubWeapon(Item subWeapon)
    {
        this.subWeapon = subWeapon;
    }
    public Item getMainWeapoon()
    {
        return mainWeapon;
    }

    public Item getSubWeapoon()
    {
        return subWeapon;
    }

    public void setIsMain(bool isMain)
    {
        this.isMain = isMain;
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

    void OnMove()
    {
        if(Time.timeScale != 0)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            inputVec = new Vector2(h, v);

            if (inputVec != new Vector2(0, 0))
                animator.SetBool("isRun", true);
            else
                animator.SetBool("isRun", false);
        }
        
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

    // 무기 변경 함수
    public void ChangeWeapon()
    {
        if(isMain) 
            mainWeaponSprite.sprite = mainWeapon.itemImage;
        else
            subWeaponSprite.sprite = subWeapon.itemImage;
    }

    void ActiveAtkTest()
    {
        /*
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
        */
    }
}
