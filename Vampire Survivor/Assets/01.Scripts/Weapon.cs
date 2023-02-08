using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 데미지
    public float damage;
    // 관통
    public int per;
  
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    
    }

    // 변수 초기화 함수
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // 관통이 -1(무한) 보다 큰 것은 속도를 적용
        if (per > -1)
        {
            rigid.velocity = dir * 6f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌 객체가 적이 아니거나 근접무기라면 실행 X
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        // 충돌 시 관통가능 횟수 차감
        per--;
        // 더 이상 관통 불가 시 비활성화
        if (per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
