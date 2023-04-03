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
    // ���ƿ� üũ
    public bool isReturn;

    public Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        // ����, ������ ���ư��� ȸ����Ű��
        if (isRotate)
            transform.Rotate(0, 0, Time.deltaTime * rotateSpeed, Space.World);
        
        // isReturn�̸�(����) ���� ������ �ݴ� �������� ���ƿ���
        if (isPing && isRotate && isReturn)
        {
            rigid.velocity *= -1;
            isReturn = false;
        }
    }

    private void OnEnable()
    {
        isPing = false;
        isReturn = false;
    }

    // ���� �ʱ�ȭ �Լ�
    public void Init(float damage, int per, Vector3 dir, float speed)
    {
        this.damage = damage;
        this.per = per;

        // ������ -1(����) ���� ū ���� �ӵ��� ����
        if (per > -1 || gameObject.tag == "Projectile")
        {
            rigid.velocity = dir * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
            gameObject.SetActive(false);

        // �浹 ��ü = ��, ���Ѱ��� x, ���� x
        if (collision.CompareTag("Enemy") && per != -1 && gameObject.tag != "Shield")
        {
            // �浹 �� ���밡�� Ƚ�� ����
            per--;
            // �� �̻� ���� �Ұ� �� ��Ȱ��ȭ
            if (per == -1)
            {
                gameObject.SetActive(false);
            }
        }
        // �浹 ��ü = ��, ���Ѱ��� x, ���� o
        else if(collision.CompareTag("Enemy") && per != -1 && gameObject.tag == "Shield")
        {
            isPing = true;
            // ������ �������
            isReturn = true;
            // �浹 �� ���밡�� Ƚ�� ����
            per--;
            // �� �̻� ���� �Ұ� �� ��Ȱ��ȭ
            if (per == -1)
            {
                gameObject.SetActive(false);
            }       
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
