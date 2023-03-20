using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkController : MonoBehaviour, IObjectPerk
{
    [Header("������")]
    public Perk perk;
    [Header("������ �̹���")]
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
