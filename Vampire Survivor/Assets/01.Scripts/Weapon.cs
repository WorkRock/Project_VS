using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 데미지
    public float damage;
    // 관통
    public int per;
    // 회전하는 무기인지 체크
    public bool isRotate;
    // 회전 속도
    public float rotateSpeed;
    // 튕김 체크
    public bool isPing;
    // 부메랑 체크(플레이어에게 돌아옴)
    public bool isReturn;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    
    }

    
    private void Update()
    {
        // 방패가 날아갈때 회전시키기
        if(isRotate)
            transform.Rotate(0, 0, Time.deltaTime * rotateSpeed, Space.Self);
        // 튕김이 가능하다면 랜덤한 방향으로 방향을 바꾸기
        if (isPing && isRotate &&!isReturn)
            rigid.velocity = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) * 8;
        // isReturn이면(방패) 던진 방향의 반대 방향으로 돌아오게
        if (isPing && isRotate && isReturn)
        {
            rigid.velocity *= -1;
            isReturn = false;
        }           
    }
    
    // 변수 초기화 함수
    public void Init(float damage, int per, Vector3 dir, float speed)
    {
        this.damage = damage;
        this.per = per;

        // 관통이 -1(무한) 보다 큰 것은 속도를 적용
        if (per > -1)
        {
            rigid.velocity = dir * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
            gameObject.SetActive(false);

        // 충돌 객체가 적이 아니거나 근접무기라면 실행 X
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        // 적에게 닿았으면
        isReturn = true;

        // 관통 불가능한 상태가 아니라면 isPing = true
        if (per >= -1)
            isPing = true;
        
        // 충돌 시 관통가능 횟수 차감
        per--;
        // 더 이상 관통 불가 시 비활성화
        if (per == -1)
        {
            isPing = false;
            isReturn = false;
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            // 적과 충돌이 해제될 때 false로 바꾸지 않으면 허공에서 계속 방향을 바꿀것이므로 꼭 체크
            isPing = false;
        }
    }
}
