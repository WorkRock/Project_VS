using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // 방패 데미지
    public float damage;
    // 관통 각도
    public int per;

    // 변수 초기화 함수
    public void Init(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }
}
