using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    // 적 재배치를 위한 변수, Collider2D는 기본 도형의 모든 콜라이더 2D를 포함
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    // 타일맵, 적 재배치 로직
    void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어의 자식인 Area의 콜라이더가 타일맵의 콜라이더를 벗어날 경우
        if (!collision.CompareTag("Area"))
            return;

        // 거리 차이를 구하기 위해 위치값 저장
        Vector3 playerPos = GameManager.instance.player.transform.position; //플레이어
        Vector3 myPos = transform.position; //타일맵

        // 거리 차이 저장
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        // 플레이어의 방향
        Vector3 playerDir = GameManager.instance.player.inputVec;

        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        // Reposition 스크립트가 붙은 오브젝트의 태그를 검사한다.
        switch (transform.tag)
        {
            // 태그가 Ground(타일맵)인 경우
            case "Ground":
                // 두 오브젝트의 거리차이
                // 1. X좌표의 차이가 Y좌표의 차이보다 크면 맵을 수평 이동시킨다.
                if(diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                // 2. Y좌표의 차이가 X좌표의 차이보다 크면 맵을 수직 이동시킨다.
                else if(diffY > diffX)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            // 태그가 Enemy인 경우
            case "Enemy":
                // 적의 콜라이더가 활성화 되어있다면(적이 살아있다면)
                if(coll.enabled)
                {
                    // 플레이어의 이동 방향에 따라 맞은 편 20만큼의 거리에서 등장하도록 이동
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0));
                }
                break;
        }
    }
}
