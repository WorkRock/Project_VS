using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 가까운 적을 스캔해서 원거리 공격을 발사하는 스크립트
public class Scanner : MonoBehaviour
{
    // 스캔할 범위
    public float scanRange;
    // 레이어
    public LayerMask targetLayer;
    // 스캔 결과를 담을 배열
    public RaycastHit2D[] targets;
    // 스캔 결과 중 가장 가까운 목표를 담을 변수
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        // CircleCastAll(1, 2, 3, 4, 5): 원형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        /* 1. 캐스팅 시작 위치
         * 2. 원의 반지름
         * 3. 캐스팅 방향
         * 4. 캐스팅 길이
         * 5. 대상 레이어
         */
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        // 지속적으로 가장 가까운 목표물을 업데이트
        nearestTarget = GetNearest();
    }

    // 가장 가까운 적을 반환하는 함수
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos); // Vector3.Distance: 벡터 A와 B사이의 거리를 계산해주는 함수
            
            // 반복문들 돌며 가져온 거리가 저장된 거리(100)보다 작으면 교체
            if(curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;       
            }
        }

        return result;
    }
}
