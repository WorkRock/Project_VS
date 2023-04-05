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
    public int count_Main;
    public float atkSpeed_Main;
    public float CT_Main;
    public float animTime_Main;
    public AtkType atkType_Main;
    public int projectileCount_Main;

    [Header("Sub")]
    public int id_Sub;
    public int prefId_Sub;
    public int dmg_Sub;
    public int count_Sub;
    public float atkSpeed_Sub;
    public float CT_Sub;
    public float animTime_Sub;
    public AtkType atkType_Sub;
    public int projectileCount_Sub;

    public enum AtkType
    {
        atkAllDir,      // �ܰ�
        atkAllDir_Fix,  // �ϵ�(��)
        atkHorDir,
        atkFollow,
        atkStatic,
        atkBoomerang    // ����(����)
    }

}
