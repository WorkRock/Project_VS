using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    // �ڼ� ������ �ɷȴ��� üũ
    public bool onMagnet;

    void Update()
    {
        // �ڼ� �����ȿ� �ɸ� ��� ����ġ�� �÷��̾� ��ġ�� �̵�
        if(onMagnet)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, 5f * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // ����ġ �������
        if(collision.gameObject.tag == "Magnet")
        {
            onMagnet = true;
        }

        // ����ġ ��Ȱ��ȭ -> �÷��̾�� ���ϴ� ����: �÷��̾� ��ũ��Ʈ�� �ڼ�������Ʈ�� ������ �־ �ڼ��� ���� ��� ����������
        if (collision.gameObject.tag == "Player")
        {        
            gameObject.SetActive(false);
            onMagnet = false;

            // ����ġ ����(�ϴ� 1���� 3�� �����ϰ�)
            GameManager.instance.player.nowExp += 3;
            // UI ����
            GameManager.instance.nowExp.text = "Exp: " + GameManager.instance.player.nowExp.ToString();
            // ���� ��
            if(GameManager.instance.player.nowExp >= GameManager.instance.player.needExpPerLV)
            {
                GameManager.instance.player.playerLV++;
                // ���� ����ġ 0���� �ʱ�ȭ
                GameManager.instance.player.nowExp = 0;
                GameManager.instance.nowExp.text = "Exp: " + GameManager.instance.player.nowExp.ToString();
             
                // UI ����
                GameManager.instance.level.text = "Level: " + GameManager.instance.player.playerLV.ToString();
                // ����ġ �ʿ䷮ ����
                GameManager.instance.player.needExpPerLV += 10;
                // UI ����
                GameManager.instance.needExp.text = "Need Exp: " + GameManager.instance.player.needExpPerLV.ToString();
            }
        }
    }
}
