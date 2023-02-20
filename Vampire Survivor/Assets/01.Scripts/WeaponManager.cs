using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // 무기 id
    public int id;
    // 프리팹 id
    public int prefabId;
    // 데미지
    public float damage;
    public float maxDamage = 5;
    // 관통 가능 적 개수
    public int count;
    public int maxCount = 7;
    // 방패: 스피드, 단검: 공격주기
    public float speed;
    public float maxSpeed = 230;
    // 레벨 업(레벨에 따라 방패 변경)
    public int level;
    public int maxLevel = 10;
   
    SpriteRenderer[] nowShieldSprites;
    public Sprite[] shields;

    private float timer;
    Player player;
    public Vector3 rotVec;

    private void Awake()
    {    
        //player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        player = GameManager.instance.player;
        Init();
    }
    void Update()
    {
        switch (id)
        {
            // 방패
            case 0:
                //transform.position = player.transform.position;
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.35f, player.transform.position.z);
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                //transform.Rotate(Vector3.back * (-1) * player.transform.localScale.x * speed * Time.deltaTime);
                break;

            // 단검
            case 1:
                timer += Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0f;
                    FireKnife();
                }
                break;
            // 화살
            case 2:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    FireArrow();
                }
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
        {
            SetShieldPosition();
        }      
    }

    public void Init()
    {
        switch (id)
        {
            // 방패
            case 0:
                speed = 150;    // 방패 회전 스피드
                level = 1;
                SetShieldPosition();
                break;
            case 1:
                speed = 3f;   // 단검 투척 주기
                break;
            case 2:
                speed = 5f;   // 화살 투척 주기
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
                // Transform의 parent 속성을 통해 부모를 변경(Poolmanager -> WeaponManager)
                shield.parent = transform;
            }
          
            shield.localPosition = Vector3.zero;
            shield.localRotation = Quaternion.identity;

            // 방패 회전 로직
            rotVec = Vector3.forward * 360 * i / count;
            shield.Rotate(rotVec);
            shield.Translate(shield.up * 1.5f, Space.World);

            shield.GetComponent<Weapon>().Init(damage, -1, Vector3.zero); // -1은 무한으로 관통함을 의미.
        }
    }

    // 단검 발사(원거리 공격)
    void FireKnife()
    {
        if (!player.scanner.nearestTarget)
        {
            Transform knifeTwo = GameManager.instance.pool.Get(prefabId).transform;
            knifeTwo.position = transform.position;
            
            Vector3 dirTwo = player.inputVec;
            dirTwo.x = (-1) * player.transform.localScale.x;
            
            knifeTwo.rotation = Quaternion.FromToRotation(Vector3.up, dirTwo);
            knifeTwo.GetComponent<Weapon>().Init(damage, count, dirTwo);
            //rigid.AddForce(transform.position, ForceMode2D.Impulse);

            return;
        }
            

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform knife = GameManager.instance.pool.Get(prefabId).transform;
        knife.position = transform.position;
        // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
        knife.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        knife.GetComponent<Weapon>().Init(damage, count, dir);
    }

    // 화살 발사(원거리 공격)
    void FireArrow()
    {
        if (!player.scanner.nearestTarget)
        {
            Transform knifeTwo = GameManager.instance.pool.Get(prefabId).transform;
            knifeTwo.position = transform.position;

            Vector3 dirTwo = player.inputVec;
            dirTwo.x = (-1) * player.transform.localScale.x;

            knifeTwo.rotation = Quaternion.FromToRotation(Vector3.up, dirTwo);
            knifeTwo.GetComponent<Weapon>().Init(damage, count, dirTwo);
            //rigid.AddForce(transform.position, ForceMode2D.Impulse);

            return;
        }


        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform knife = GameManager.instance.pool.Get(prefabId).transform;
        knife.position = transform.position;
        // Quaternion.FromToRotation: 지정된 축을 중심으로 목표를 향해 회전하는 함수
        knife.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        knife.GetComponent<Weapon>().Init(damage, count, dir);
    }
}
