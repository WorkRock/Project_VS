using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviour
{
    [Header("Inventory")]
    public PerkInvenManager perkInven;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ∆‹ »πµÊ
        if (collision.gameObject.tag == "Perk")
        {
            // 1. ∆‹ ¿Œ∫•≈‰∏Æ ∞ªΩ≈
            Perk perk = collision.gameObject.GetComponent<PerkController>().perk;
            perkInven.AddPerk(perk);
            collision.gameObject.SetActive(false);


            
            /*
            // 2. «√∑π¿ÃæÓ Ω∫≈» ¡ı∞°
            GameManager.instance.player.atk += perk.;
            atkSpeed += perk.plusAtkSpeed;
            atkRange += perk.plusRange;
            critical += perk.plusCriticalPossiblity;
            criticalDmg += perk.plusCriticalDmg;
            projectile += perk.plusProjectile;
            coolTime -= perk.plusCT;
            moveSpeed += perk.plusMoveSpeed;
            maxHP += perk.plusHp;
            defense += perk.plusDefense;

            atk_Text.text = "Atk   " + atk.ToString();
            atkSpeed_Text.text = "Atk Speed    " + atkSpeed.ToString();
            atkRange_Text.text = "Atk Range    " + atkRange.ToString();
            critical_Text.text = "Critical    " + critical.ToString();
            criticalDmg_Text.text = "Critical Dmg    " + criticalDmg.ToString();
            projectile_Text.text = "Projectile    " + projectile.ToString();
            coolTime_Text.text = "Cool Time    " + coolTime.ToString();
            moveSpeed_Text.text = "Move Speed    " + moveSpeed.ToString();
            maxHP_Text.text = "Max HP    " + maxHP.ToString();
            defense_Text.text = "Defense    " + defense.ToString();
            */
            
        }
    }
}
