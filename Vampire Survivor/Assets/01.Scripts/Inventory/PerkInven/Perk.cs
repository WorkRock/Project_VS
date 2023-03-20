using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Perk/Create New Perk")]
public class Perk : ScriptableObject
{
    public int id;
    public string perkName;

    public Sprite SynergyImage;
    public Sprite SpecialImage;

    public float basicX;
    public float incX;
    public float maxX;
    public float basicY;
    public float incY;
    public float maxY;

    public string perkExplan;

    public bool ugPerk;
    public Synergy synergy;
    public Special special;

    public enum Synergy
    {
        Fire,
        Electro,
        Ice,
        Physics,
        None
    }

    public enum Special
    {
        NormalAtk,
        SkillAtk,
        Swap,
        Damage,
        Util,
        Summon
    }
}
