using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    // 방패 id
    public int id;
    // 프리팹 id
    public int prefabId;
    // 방패 데미지
    public float damage;
    public float maxDamage = 5;
    // 방패의 개수
    public int count;
    public int maxCount = 7;
    // 방패 스피드
    public float speed;
    public float maxSpeed = 230;

    // 레벨 업(레벨에 따라 방패 변경)
    public int level;
    public int maxLevel = 10;

    SpriteRenderer[] nowShieldSprites;
    public Sprite[] shields;

    private void Start()
    {
        Init();
    }
    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
        }

        // 레벨에 따라 방패 스프라이트 변경하기
        if (level >= 3 && level < 5)
        {
            nowShieldSprites = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < nowShieldSprites.Length; i++)
            {
                nowShieldSprites[i].sprite = shields[0];
            }
        }
        else if (level >= 5 && level < 7)
        {
            nowShieldSprites = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < nowShieldSprites.Length; i++)
            {
                nowShieldSprites[i].sprite = shields[1];
            }
        }
        else if (level >= 7)
        {
            nowShieldSprites = GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < nowShieldSprites.Length; i++)
            {
                nowShieldSprites[i].sprite = shields[2];
            }
        }

        // 방패 업그레이드
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 데미지 2, 개수 1, 스피드 10 증가
            LevelUp(2, 1, 10);
            if (damage >= maxDamage)
                damage = maxDamage;
            if (count >= maxCount)
                count = maxCount;
            if (speed >= maxSpeed)
                speed = maxSpeed;
            if (level >= maxLevel)
                level = maxLevel;
        }
    }

    public void LevelUp(float damage, int count, float speed)
    {
        level++;
        this.damage += damage;
        this.count += count;
        this.speed += speed;

        if (id == 0)
            SetShieldPosition();
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
                level = 1;
                SetShieldPosition();
                break;
        }
    }

    
    // 방패 배치 함수
    void SetShieldPosition()
    {
        Transform shield;

        for (int i = 0; i < count; i++)
        {
            if(i < transform.childCount)
            {
                shield = transform.GetChild(i);
            }
            else
            {
                shield = GameManager.instance.pool.Get(prefabId).transform;
                // Transform의 parent 속성을 통해 부모를 변경(Poolmanager -> ShieldManager)
                shield.parent = transform;
            }
          
            shield.localPosition = Vector3.zero;
            shield.localRotation = Quaternion.identity;

            // 방패 회전 로직
            Vector3 rotVec = Vector3.forward * 360 * i / count;
            shield.Rotate(rotVec);
            shield.Translate(shield.up * 1.5f, Space.World);

            shield.GetComponent<Shield>().Init(damage, -1); // -1은 무한으로 관통함을 의미.
        }
    }
}
