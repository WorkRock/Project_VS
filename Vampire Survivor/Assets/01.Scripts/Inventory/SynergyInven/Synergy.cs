using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Synergy", menuName = "Synergy/Create New Synergy")]
public class Synergy : ScriptableObject
{
    public string synergyName;
    public Sprite synergyImage;

    public int prefId;
    public int dmg;
    public int Pent;
    public float AtkSpeed;   // ¹«±â ÄðÅ¸ÀÓ, atkSpeed
    public float animTime;
    
    public int projectileCount;
    public float projectileSpeed;
    public int knockBack;
}
