using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    // 적 관련 속성
    [Header("Enemy Attributes")]
    public float speed;
    bool isLive = true;
    // 적 체력
    public float enemy_Hp;
    public float enemy_MaxHp = 6;
    // 적 공격력
    public float enemy_Atk;

    // 컴포넌트
    SpriteRenderer sprite;
    Rigidbody2D rigid;
    Animator animator;
    WaitForFixedUpdate wait;

    // 플레이어 추적
    Rigidbody2D target;
    public GameObject fireDotEffect;    // 불 도트뎀 이펙트
    bool isFire;    // 불타고 있는지 체크
    float fireTime;

    // 무엇에 맞았는지 체크
    public bool byPyroAtk;     // 불 공격에 맞음
    public bool byBasicAtk;    // 평타에 맞음

    // 피격 효과
    private SpriteRenderer[] enemyBodies;  // 피격 효과(알파값 조정)을 위해 플레이어의 자식으로 있는 스프라이트 렌더러들을 연결
    private Color[] originColor;            // 원래 색깔 저장  

    public float knockBackPower = 3f;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        wait = new WaitForFixedUpdate();

        // 피격 효과
        enemyBodies = GetComponentsInChildren<SpriteRenderer>();   // 알파값 조정을 위해 플레이어의 자식으로 있는 스프라이트 렌더러들을 연결
        originColor = new Color[enemyBodies.Length];   // 적의 원래 색깔을 저장
    }

    void Start()
    {
        // 피격 효과를 위해 플레이어의 원래 색깔을 저장
        for (int i = 0; i < enemyBodies.Length; i++)
        {
            originColor[i] = enemyBodies[i].color;
        }

        enemy_Hp = enemy_MaxHp;
    }

    void Update()
    {
        // 불 도트뎀 받는 상태이면 1초마다 hp감소
        if (isFire)
        {
            fireTime += Time.deltaTime;
            if (fireTime >= 1f)
            {
                fireTime = 0;
                enemy_Hp -= 2f;
                if(enemy_Hp <= 0)
                    EnemyDie();
                //Debug.Log(enemy_Hp);          
            }
        }
    }

    // Invoke로 참조중이므로 삭제x
    void Delete()
    {
        gameObject.SetActive(false);
    }
    void EnemyDie()
    {
        for (int i = 0; i < enemyBodies.Length; i++)
        {
            enemyBodies[i].color = originColor[i];
        }
        // 콜라이더 비활성화
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        // 적 사망 사운드 재생
        SoundManager.instance.PlaySE("Enemy Die");
        animator.SetTrigger("isDead");
        Invoke("Delete", 0.5f);
        // 경험치 드랍(풀의 인덱스 5번)
        GameObject exp = GameManager.instance.pool.Get(5);
        exp.transform.position = this.transform.position;
    }

    // 물리적인 이동(rigid)은 fixedUpdate 사용
    void FixedUpdate()
    {
        // 적 이동 시 넉백이 씹히므로 현재 스턴 애니메이션이 실행중일 경우 적은 이동하지 않게 해준다.
        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("3_Debuff_Stun"))
            return;

        // 이동할 방향 벡터
        Vector2 dirVec = target.position - rigid.position;
        // 이동할 거리 벡터
        Vector2 moveVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveVec);
        // 물리 속도가 이동에 영향을 주지 않도록 속도 제거
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        // spum으로 만든 프리팹은 하나의 스프라이트로 동작하지 않아서 spriteRenderer못쓸듯
        //sprite.flipX = target.position.x < rigid.position.x;
        if (target.position.x < rigid.position.x)
            this.GetComponentInChildren<Transform>().transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        else if (target.position.x > rigid.position.x)
            this.GetComponentInChildren<Transform>().transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
    }

    void OnEnable()
    {
        // 콜라이더 활성화
        this.GetComponent<CapsuleCollider2D>().enabled = true;
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 평타와 충돌
        if (collision.gameObject.tag == "BasicAtk")
        {
            // 스프라이트 색 변경
            HurtEffectOn();
            // 넉백
            StartCoroutine(KnockBack());
            // 평타에 맞았다고 체크
            byBasicAtk = true;
            // 적 히트 사운드 재생
            SoundManager.instance.PlaySE("Enemy Hit");
            animator.SetTrigger("isHit");
            // 체력 감소
            enemy_Hp -= GameManager.instance.player.AllDmg + collision.gameObject.GetComponent<CheckItem>().dmg;
            // 데미지 표시
            GameObject dmgText = GameManager.instance.pool.Get(22);
            dmgText.transform.position = transform.position + Vector3.up * 1.2f;
            dmgText.GetComponent<TextMeshPro>().text = (GameManager.instance.player.AllDmg + collision.gameObject.GetComponent<CheckItem>().dmg).ToString();

            if (enemy_Hp <= 0)
            {
                EnemyDie();
            }

            // 발화 테스트
            int igniPer = Random.Range(0, 100);
            //if(igniPer < GameManager.instance.perkInven.perks[i].basicX)
        }

        // 2. 평타-투사체와 충돌
        if (collision.gameObject.tag == "Projectile")
        {
            // 스프라이트 색 변경
            HurtEffectOn();
            // 넉백
            StartCoroutine(KnockBack());
            // 평타에 맞았다고 체크
            byBasicAtk = true;
            // 적 히트 사운드 재생
            SoundManager.instance.PlaySE("Enemy Hit");
            animator.SetTrigger("isHit");
            enemy_Hp -= GameManager.instance.player.AllDmg + collision.gameObject.GetComponent<CheckItem>().dmg;
            // 데미지 표시
            GameObject dmgText = GameManager.instance.pool.Get(22);
            dmgText.transform.position = transform.position + Vector3.up * 1.2f;
            dmgText.GetComponent<TextMeshPro>().text = (GameManager.instance.player.AllDmg + collision.gameObject.GetComponent<CheckItem>().dmg).ToString();

            if (enemy_Hp <= 0)
            {
                EnemyDie();
            }
        }

        // 3. 방패와 충돌(방패는 튕김 체크 때문에 별도의 태그 사용)
        if (collision.gameObject.tag == "Shield")
        {
            // 스프라이트 색 변경
            HurtEffectOn();
            // 넉백
            StartCoroutine(KnockBack());
            //// 불 도트 효과 테스트 /////
            // fireDotEffectOn();
            // Invoke("fireDotEffectOff", 3f);

            // 평타에 맞았다고 체크
            byBasicAtk = true;
            // 적 히트 사운드 재생
            SoundManager.instance.PlaySE("Enemy Hit");
            animator.SetTrigger("isHit");
            enemy_Hp -= GameManager.instance.player.AllDmg + collision.gameObject.GetComponent<CheckItem>().dmg;
            // 데미지 표시
            GameObject dmgText = GameManager.instance.pool.Get(22);
            dmgText.transform.position = transform.position + Vector3.up * 1.2f;
            dmgText.GetComponent<TextMeshPro>().text = (GameManager.instance.player.AllDmg + collision.gameObject.GetComponent<CheckItem>().dmg).ToString();

            if (enemy_Hp <= 0)
            {
                EnemyDie();
            }
        }

        // 4. 연소, 낙뢰, 빙결 등 액티브 퍽과 충돌 (넉백 X)
        if(collision.gameObject.tag == "ActivePerk")
        {
            // 스프라이트 색 변경
            HurtEffectOn();
            // 평타에 맞았다고 체크
            byBasicAtk = true;
            // 적 히트 사운드 재생
            SoundManager.instance.PlaySE("Enemy Hit");
            animator.SetTrigger("isHit");
            // 체력 감소
            enemy_Hp -= collision.gameObject.GetComponent<CheckPerk>().perk.basicX + collision.gameObject.GetComponent<CheckPerk>().perk.basicY;
            // 데미지 표시
            GameObject dmgText = GameManager.instance.pool.Get(22);
            dmgText.transform.position = transform.position + Vector3.up * 1.2f;
            dmgText.GetComponent<TextMeshPro>().text = (collision.gameObject.GetComponent<CheckPerk>().perk.basicX + collision.gameObject.GetComponent<CheckPerk>().perk.basicY).ToString();

            if (enemy_Hp <= 0)
            {
                EnemyDie();
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // 플레이어와 닿아있는 동안 공격 애니메이션을 재생한다.
        if (collision.gameObject.tag == "Player")
        {
            // 일단은 활, 마법, 일반 안 나누고 일반 공격 재생
            animator.SetTrigger("isNormal");
        }
    }

    void fireDotEffectOn()
    {
        fireDotEffect.SetActive(true);
        isFire = true;
    }

    void fireDotEffectOff()
    {
        fireDotEffect.SetActive(false);
        isFire = false;
    }

    // 넉백 코루틴 함수
    IEnumerator KnockBack()
    {
        yield return wait;  // 다음 하나의 물리 프레임을 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * knockBackPower, ForceMode2D.Impulse);
    }

    // 피격효과 함수(알파값 조정)
    public void HurtEffectOn()
    {
        for (int i = 0; i < enemyBodies.Length; i++)
        {
            enemyBodies[i].color = new Color(1, 1, 1, 0.7f);
        }
        Invoke("HurtEffectOff", 0.5f);
    }

    public void HurtEffectOff()
    {
        if (enemy_Hp <= 0)
            return;

        for (int i = 0; i < enemyBodies.Length; i++)
        {
            enemyBodies[i].color = originColor[i];
        }
    }
}
