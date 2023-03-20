using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPerk : MonoBehaviour
{
    public PerkInvenManager perkInven;
    
    [SerializeField] public List<Perk> perks;

    public void InsertRandomPerk()
    {
        int ran = Random.Range(0, (perks.Count - 1));
        perkInven.AddPerk(perks[ran]);
    }
}
