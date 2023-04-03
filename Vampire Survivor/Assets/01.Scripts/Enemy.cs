using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // 플레이어 추적
    Rigidbody2D target;
    public GameObject fireDotEffect;    // 불 도트뎀 이펙트
    bool isFire;    // 불타고 있는지 체크
    float fireTime;

    // 무엇에 맞았는지 체크
    public bool byPyroAtk;     // 불 공격에 맞음
    public bool byBasicAtk;    // 평타에 맞음

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
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
        if (!isLive)
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
            // 평타에 맞았다고 체크
            byBasicAtk = true;
            // 적 히트 사운드 재생
            SoundManager.instance.PlaySE("Enemy Hit");
            animator.SetTrigger("isHit");
            // 체력 감소
            enemy_Hp -= GameManager.instance.player.AllDmg;

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
            // 평타에 맞았다고 체크
            byBasicAtk = true;
            // 적 히트 사운드 재생
            SoundManager.instance.PlaySE("Enemy Hit");
            animator.SetTrigger("isHit");
            enemy_Hp -= GameManager.instance.player.AllDmg;

            if (enemy_Hp <= 0)
            {
                EnemyDie();
            }
        }

        // 3. 방패와 충돌(방패는 튕김 체크 때문에 별도의 태그 사용)
        if (collision.gameObject.tag == "Shield")
        {
            //// 불 도트 효과 테스트 /////
            // fireDotEffectOn();
            // Invoke("fireDotEffectOff", 3f);

            // 평타에 맞았다고 체크
            byBasicAtk = true;
            // 적 히트 사운드 재생
            SoundManager.instance.PlaySE("Enemy Hit");
            animator.SetTrigger("isHit");
            enemy_Hp -= GameManager.instance.player.AllDmg;

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
}

