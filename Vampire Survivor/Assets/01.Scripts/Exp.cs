using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    // 자석 범위에 걸렸는지 체크
    public bool onMagnet;
   
    void Update()
    {
        // 자석 범위안에 걸린 경우 경험치를 플레이어 위치로 이동
        if(onMagnet)
        {
            //transform.position = Vector3.Lerp(transform.position, GameManager.instance.player.transform.position, 0.03f);
            transform.position = Vector3.MoveTowards(transform.position, GameManager.instance.player.transform.position, 7f * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 경험치 끌어오기
        if(collision.gameObject.tag == "Magnet")
        {
            onMagnet = true;
        }

        // 경험치 비활성화 -> 플레이어에서 안하는 이유: 플레이어 스크립트가 자석오브젝트의 상위에 있어서 자석에 닿은 즉시 없어져버림
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.instance.PlaySE("Get Exp");
            gameObject.SetActive(false);
            onMagnet = false;

            // 경험치 증가(일단 1개당 3씩 증가하게)
            GameManager.instance.player.nowExp += 3;
            GameManager.instance.expBar.value = GameManager.instance.player.nowExp / GameManager.instance.player.needExpPerLV;

            // 레벨 업
            if (GameManager.instance.player.nowExp >= GameManager.instance.player.needExpPerLV)
            {
                GameManager.instance.expBar.value = 0f;
                // 퍽 선택 UI 활성화
                GameManager.instance.levelUpUI.SetActive(true);
                Time.timeScale = 0f;

                GameManager.instance.player.playerLV++;
                // 현재 경험치 0으로 초기화
                GameManager.instance.player.nowExp = 0;
                
                // UI 갱신
                GameManager.instance.level.text = "Level: " + GameManager.instance.player.playerLV.ToString();
                // 경험치 필요량 갱신
                GameManager.instance.player.needExpPerLV += 10;           
            }
        }
    }
}
