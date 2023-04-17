using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // 플레이어 고유 스탯(모든 캐릭터 동일)
    [Header("Player Basic Stat")]
    public float AllDmg;            // 전체 피해량
    public float BasicAtkDmg;  // 평타 피해량
    public float SynergyDmg_Pyro;   // 속성 피해량_Pyro
    public float SynergyDmg_Electro;// 속성 피해량_Electro
    public float SynergyDmg_Ice;    // 속성 피해량_Ice
    public float AtkSpeed;          // 공격 속도
    public float AtkRange;          // 공격 범위
    public float ProjectileSpeed;   // 투사체 속도
    public int ProjectileCount;     // 투사체 수
    public float SkillCT;           // 스킬 쿨타임
    public float SwapCT;            // 스왑 쿨타임
    public float GainUlt;           // 궁극기 충전량
    public int Pent;                // 관통
    public float MovementSpeed;     // 이동 속도
    public float GainGold;          // 골드 획득량
    public float GainExp;           // 경험치 획득량
    public float Cri;               // 치명타 확률
    public float Magnet;            // 자석력
    public int Revive;              // 부활
    public int MaxHP;               // 최대 체력
    public int NowHP;               // 현재 체력
    public float HPRegen;           // 체력 재생
    public float Reflect;           // 피해량 반사
    public float GainDmg;           // 받는 피해량

    // 캐릭터 보너스 스탯(캐릭터마다 상이)
    [Header("Char Bonus Stat")]
    public float AllDmg_CB;            // 전체 피해량
    public float BasicAtkDmg_CB;  // 평타 피해량
    public float SynergyDmg_Pyro_CB;   // 속성 피해량_Pyro
    public float SynergyDmg_Electro_CB;// 속성 피해량_Electro
    public float SynergyDmg_Ice_CB;    // 속성 피해량_Ice
    public float AtkSpeed_CB;          // 공격 속도(쿨타임)
    public float AtkRange_CB;          // 공격 범위
    public float ProjectileSpeed_CB;   // 투사체 속도
    public int ProjectileCount_CB;     // 투사체 수
    public float SkillCT_CB;           // 스킬 쿨타임
    public float SwapCT_CB;            // 스왑 쿨타임
    public float GainUlt_CB;           // 궁극기 충전량
    public int Pent_CB;                // 관통
    public float MovementSpeed_CB;     // 이동 속도
    public float GainGold_CB;          // 골드 획득량
    public float GainExp_CB;           // 경험치 획득량
    public float Cri_CB;               // 치명타 확률
    public float Magnet_CB;            // 자석력
    public int Revive_CB;              // 부활
    public int MaxHP_CB;               // 최대 체력
    public int NowHP_CB;               // 현재 체력
    public float HPRegen_CB;           // 체력 재생
    public float Reflect_CB;           // 피해량 반사
    public float GainDmg_CB;           // 받는 피해량

    [Header("Etc")]
    public Vector2 inputVec;
    public Scanner scanner;
    private bool isLive;    // 생존 체크
    private SpriteRenderer[] playerBodies;  // 피격 효과(알파값 조정)을 위해 플레이어의 자식으로 있는 스프라이트 렌더러들을 연결
    private Color[] originColor;            // 원래 색깔 저장  
    public float playerDir; // 플레이어가 바라보는 방향(transform.localScale.x)값 저장

    [Space(10f)]
    [Header("Lv & Exp")]  
    public int playerLV;
    public float baseExp = 5;
    public float nowExp;  // 현재 경험치
    public float needExpPerLV; // 레벨 당 필요 경험치

    [Space(10f)]
    [Header("Swap")]
    // 현재 착용 중인 무기 스프라이트
    public SpriteRenderer mainWeaponSprite;
    public SpriteRenderer subWeaponSprite;
    // public WeaponChange weaponChange;
    public Item mainWeapon;
    public Item subWeapon;
    public int weaponNum;
    // 현재 무기가 주무기인지, 보조무기인지 저장(테스트용)
    public bool isMain;

    // 퍽-스왑 테스트
    public bool canSwap;    // 스왑 가능 여부 체크
    public float swap_Cnt;

    // 컴포넌트
    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    Animator animator; 
   
    void Awake()
    {
        // 컴포넌트 연결
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();    
        scanner = GetComponent<Scanner>();
        // 피격 효과
        playerBodies = GetComponentsInChildren<SpriteRenderer>();   // 알파값 조정을 위해 플레이어의 자식으로 있는 스프라이트 렌더러들을 연결
        originColor = new Color[playerBodies.Length];   // 플레이어의 원래 색깔을 저장

        // 시작 시 main무기와 sub무기 정보를 받아온다.
        mainWeapon = GameManager.instance.weaponChange.getMainWeapoon();
        subWeapon = GameManager.instance.weaponChange.getSubWeapoon();
    }

    void Start()
    {
        canSwap = true;

        // 무기 번호 초기화 (기본-0, 검)
        weaponNum = 0;
        // 플레이어 관련 옵션 초기화
        //MaxHP = 10;
        //NowHP = MaxHP;
        playerLV = 1;
        needExpPerLV = baseExp;
        isLive = true;
        
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
        {
            SceneManager.LoadScene("Lobby");
            return;
        }
            
        // 주무기(오른쪽) 보조무기(왼쪽) 스왑에 따른 평타 변경
        if (Time.timeScale > 0 && canSwap &&Input.GetKeyDown(KeyCode.O))
        {
            canSwap = false;
            GameManager.instance.weaponChange.swapWeapon();
            // 스왑 시 쿨타임 초기화
            GameManager.instance.weaponManager.invokeTime_Main = 0f;
            GameManager.instance.weaponManager.invokeTime_Sub = 0f;
        }

        if (subWeapon != null && !canSwap)
        {
            swap_Cnt += Time.deltaTime;
        }

        if (swap_Cnt > SwapCT)
        {
            swap_Cnt = 0;
            canSwap = true;
            GameManager.instance.perkValueCheck.swapCheck = false;
        }

        // update에서 플레이어가 바라보는 방향을 지속적으로 받아오기
        playerDir = transform.localScale.x;
        
        OnMove();   // 이동
        Flip();     // 좌우 반전
        Dead();     // 사망 체크
    }

    // rigid 관련 로직은 fixedUpdate에서 수행
    void FixedUpdate()
    {
        if (!isLive)
            return;

        // 이동거리(벡터) = 방향 * 속도 * 시간
        Vector2 nextVec = inputVec.normalized * MovementSpeed * Time.fixedDeltaTime;
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
        if (GameManager.instance.GM_NowHP <= 0)
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
            //NowHP--;
            GameManager.instance.GM_NowHP--;
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
        if (GameManager.instance.GM_NowHP <= 0)
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
}