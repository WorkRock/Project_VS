using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // ������
    public float damage;
    // ����
    public int per;
    // ȸ���ϴ� �������� üũ
    public bool isRotate;
    // ȸ�� �ӵ�
    public float rotateSpeed;
    // ƨ�� üũ
    public bool isPing;
    // �θ޶� üũ(�÷��̾�� ���ƿ�)
    public bool isReturn;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    
    }

    
    private void Update()
    {
        // ���а� ���ư��� ȸ����Ű��
        if(isRotate)
            transform.Rotate(0, 0, Time.deltaTime * rotateSpeed, Space.Self);
        // ƨ���� �����ϴٸ� ������ �������� ������ �ٲٱ�
        if (isPing && isRotate &&!isReturn)
            rigid.velocity = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2)) * 8;
        // isReturn�̸�(����) ���� ������ �ݴ� �������� ���ƿ���
        if (isPing && isRotate && isReturn)
        {
            rigid.velocity *= -1;
            isReturn = false;
        }           
    }
    
    // ���� �ʱ�ȭ �Լ�
    public void Init(float damage, int per, Vector3 dir, float speed)
    {
        this.damage = damage;
        this.per = per;

        // ������ -1(����) ���� ū ���� �ӵ��� ����
        if (per > -1)
        {
            rigid.velocity = dir * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
            gameObject.SetActive(false);

        // �浹 ��ü�� ���� �ƴϰų� ���������� ���� X
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        // ������ �������
        isReturn = true;

        // ���� �Ұ����� ���°� �ƴ϶�� isPing = true
        if (per >= -1)
            isPing = true;
        
        // �浹 �� ���밡�� Ƚ�� ����
        per--;
        // �� �̻� ���� �Ұ� �� ��Ȱ��ȭ
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
            // ���� �浹�� ������ �� false�� �ٲ��� ������ ������� ��� ������ �ٲܰ��̹Ƿ� �� üũ
            isPing = false;
        }
    }
}
