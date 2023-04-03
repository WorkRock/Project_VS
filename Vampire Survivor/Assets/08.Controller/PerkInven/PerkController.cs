using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkController : MonoBehaviour, IObjectPerk
{
    [Header("Perk Info")]
    public Perk perk;
    
    public Perk ClickPerk()
    {
        return this.perk;
    }
}
