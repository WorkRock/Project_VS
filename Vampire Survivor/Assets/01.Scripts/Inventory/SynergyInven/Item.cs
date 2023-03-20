using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
public class Item : ScriptableObject
{
    public int id;
    public int prefId;
    public string itemName;
    
    public int dmg;
    public int count;
    public float atkSpeed;
    public float CT;
    public float animTime;
    
    public Sprite itemImage;
    public AtkType atkType;

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
