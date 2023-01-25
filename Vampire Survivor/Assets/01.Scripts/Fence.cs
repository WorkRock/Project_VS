using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    void OnEnable()
    {
        transform.position = GameManager.instance.player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 플레이어 히트 사운드 재생
            SoundManager.instance.PlaySE("Player Hit");
            GameManager.instance.player.GetComponent<Animator>().SetTrigger("isHit");
            GameManager.instance.player.player_Hp--;

            GameManager.instance.player.HurtEffectOn();
        }
    }
}
