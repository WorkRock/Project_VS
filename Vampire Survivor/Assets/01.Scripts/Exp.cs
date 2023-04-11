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
            //transform.position = Vector3.Lerp(transform.position, GameManager.instance.player.transform.position, 0.03f);
            transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, 7f * Time.deltaTime);
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
            SoundManager.instance.PlaySE("Get Exp");
            gameObject.SetActive(false);
            onMagnet = false;

            // ����ġ ����(�ϴ� 1���� 3�� �����ϰ�)
            GameManager.instance.player.nowExp += 3;
            GameManager.instance.expBar.value = GameManager.instance.player.nowExp / GameManager.instance.player.needExpPerLV;

            // ���� ��
            if (GameManager.instance.player.nowExp >= GameManager.instance.player.needExpPerLV)
            {
                GameManager.instance.expBar.value = 0f;
                // �� ���� UI Ȱ��ȭ
                GameManager.instance.levelUpUI.SetActive(true);
                Time.timeScale = 0f;

                GameManager.instance.player.playerLV++;
                // ���� ����ġ 0���� �ʱ�ȭ
                GameManager.instance.player.nowExp = 0;
                
                // UI ����
                GameManager.instance.level.text = "Level: " + GameManager.instance.player.playerLV.ToString();
                // ����ġ �ʿ䷮ ����
                GameManager.instance.player.needExpPerLV += 10;           
            }
        }
    }
}
