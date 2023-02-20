using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // ���� id
    public int id;
    // ������ id
    public int prefabId;
    // ������
    public float damage;
    public float maxDamage = 5;
    // ���� ���� �� ����
    public int count;
    public int maxCount = 7;
    // ����: ���ǵ�, �ܰ�: �����ֱ�
    public float speed;
    public float maxSpeed = 230;
    // ���� ��(������ ���� ���� ����)
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
            // ����
            case 0:
                //transform.position = player.transform.position;
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.35f, player.transform.position.z);
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                //transform.Rotate(Vector3.back * (-1) * player.transform.localScale.x * speed * Time.deltaTime);
                break;

            // �ܰ�
            case 1:
                timer += Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0f;
                    FireKnife();
                }
                break;
            // ȭ��
            case 2:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    FireArrow();
                }
                break;

        }

        // ������ ���� ���� ��������Ʈ �����ϱ�
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

        // ���� ���׷��̵�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ������ 2, ���� 1, ���ǵ� 10 ����
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
            // ����
            case 0:
                speed = 150;    // ���� ȸ�� ���ǵ�
                level = 1;
                SetShieldPosition();
                break;
            case 1:
                speed = 3f;   // �ܰ� ��ô �ֱ�
                break;
            case 2:
                speed = 5f;   // ȭ�� ��ô �ֱ�
                break;
        }
    }

    
    // ���� ��ġ �Լ�
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
                // Transform�� parent �Ӽ��� ���� �θ� ����(Poolmanager -> WeaponManager)
                shield.parent = transform;
            }
          
            shield.localPosition = Vector3.zero;
            shield.localRotation = Quaternion.identity;

            // ���� ȸ�� ����
            rotVec = Vector3.forward * 360 * i / count;
            shield.Rotate(rotVec);
            shield.Translate(shield.up * 1.5f, Space.World);

            shield.GetComponent<Weapon>().Init(damage, -1, Vector3.zero); // -1�� �������� �������� �ǹ�.
        }
    }

    // �ܰ� �߻�(���Ÿ� ����)
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
        // Quaternion.FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        knife.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        knife.GetComponent<Weapon>().Init(damage, count, dir);
    }

    // ȭ�� �߻�(���Ÿ� ����)
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
        // Quaternion.FromToRotation: ������ ���� �߽����� ��ǥ�� ���� ȸ���ϴ� �Լ�
        knife.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        knife.GetComponent<Weapon>().Init(damage, count, dir);
    }
}
