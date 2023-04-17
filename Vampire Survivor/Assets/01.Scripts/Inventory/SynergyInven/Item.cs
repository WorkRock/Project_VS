using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    [Header("Same")]
    public string itemName;
    public Sprite itemImage;

    [Header("Main")]
    public int id_Main;
    public int prefId_Main;
    public int dmg_Main;
    public int Pent_Main;
    public float AtkSpeed_Main;   // 무기 쿨타임, atkSpeed
    public float animTime_Main;
    public AtkType atkType_Main;
    public int projectileCount_Main;
    public float projectileSpeed_Main;
    public int knockBack_Main;

    [Header("Sub")]
    public int id_Sub;
    public int prefId_Sub;
    public int dmg_Sub;
    public int Pent_Sub;   
    public float AtkSpeed_Sub;    // 무기 쿨타임, atkSpeed
    public float animTime_Sub;
    public AtkType atkType_Sub;
    public int projectileCount_Sub;
    public float projectileSpeed_Sub;
    public int knockBack_Sub;

    public enum AtkType
    {
        atkAllDir,      // 단검
        atkAllDir_Fix,  // 완드(주)
        atkHorDir,
        atkFollow,
        atkStatic,
        atkBoomerang    // 도끼(보조)
    }

}
