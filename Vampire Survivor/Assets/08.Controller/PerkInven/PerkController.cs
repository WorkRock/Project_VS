using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkController : MonoBehaviour, IObjectPerk
{
    [Header("아이템")]
    public Perk perk;
    [Header("아이템 이미지")]
    public SpriteRenderer synergyImage;
    public SpriteRenderer specialImage;

    void Start()
    {
        synergyImage.sprite = perk.SynergyImage;
        specialImage.sprite = perk.SpecialImage;
        //perkImage.sprite = ;
    }



    public Perk ClickPerk()
    {
        return this.perk;
    }
}
