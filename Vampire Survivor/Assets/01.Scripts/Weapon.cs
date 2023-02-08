using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // ������
    public float damage;
    // ����
    public int per;
  
    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();    
    }

    // ���� �ʱ�ȭ �Լ�
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        // ������ -1(����) ���� ū ���� �ӵ��� ����
        if (per > -1)
        {
            rigid.velocity = dir * 6f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹 ��ü�� ���� �ƴϰų� ���������� ���� X
        if (!collision.CompareTag("Enemy") || per == -1)
            return;

        // �浹 �� ���밡�� Ƚ�� ����
        per--;
        // �� �̻� ���� �Ұ� �� ��Ȱ��ȭ
        if (per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
