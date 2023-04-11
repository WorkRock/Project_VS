using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Perk/Create New Perk")]
public class Perk : ScriptableObject
{
    [Header("Perk Info")]
    public int perkID;
    public Sprite perkImage;    // �� �̹��� -> �� ���� �������� ���� ������ΰ�??
    public Sprite SynergyImage; // �� �Ӽ� �̹���
    public Sprite SpecialImage; // �� Ưȭ �̹���
    
    [Header("String")]
    public string perkName;
    public string perkInfo;
    public string perkLevel;    // Basic Or Upgrade Perk?
    
    [Header("Bool")]
    //public bool ugPerk;   // ���׷��̵�� ������ üũ -> ���׷��̵� ���� ������ ������ ����Ŷ� �� ������ �ʿ� ����?
    public bool isActive;   // �������� �����ϴ� ���ΰ�?
    public bool isCount;    // �����ð��� �����ϴ� ���ΰ�?
   
    [Header("Plus Data")]
    public float basicX;    // �⺻ X
    public float addX;      // 1����Ʈ �� ������ X  
    public float basicY;    // �⺻ Y
    public float addY;      // 1����Ʈ �� ������ Y

    [Header("Enum")]
    public Synergy synergy;
    public Special special;
    public Active active;
    public AddTarget addTargetX;
    public AddTarget addTargetY;
    public RequireTarget requireTarget;

    // �� �Ӽ� ���
    public enum Synergy
    {
        syPyro,     // ��
        syElectro,  // ����
        syIce,      // ����
        syPhysics,  // ����
        syWind,     // �ٶ�
        syNone      // ��
    }

    // �� Ưȭ ���
    public enum Special
    {
        spBasicAtk, // ��Ÿ
        spSkill,    // ��ų
        spSwap,     // ����  
        spDmg,      // ������
        spUtil,     // ��ƿ
        spSummon,   // ��ȯ
        spHP        // ü��
    }

    // ��Ƽ�� ���
    public enum Active
    {
        aPyro,              // ����
        aElectro,           // ����
        aIce,               // ����
        aFireRing,          // �Ҳɸ� ����
        aFireBall,          // ȭ���� ��ô
        aSummon_Electro,    // ��ȯ(����)
        aSummon_Ice,        // ��ȯ(����)
        aSummon_Wind,       // ��ȯ(�ٶ�)
        aSpellShield,       // ��ȣ��
        aIceBall,           // ���� ����ü
        None                // ����
    }

    // �ߵ� ���� ���
    public enum RequireTarget
    {
        rtNone,         // None (�ߵ� ���� x, �� ȹ�� �� �ٷ� �ߵ�)
        rtBasicAtk,     // ��Ÿ ���ݽ� �ߵ�
        rtUseSkill,     // ��ų ���ݽ� �ߵ�
        rtPyro,         // ���� ���ݽ� �ߵ�
        rtElectro,      // ���� ���ݽ� �ߵ�
        rtIce,          // ���� ���ݽ� �ߵ�
        rtSwap,         // ���ҽ� �ߵ�
        rtNotUseSkill,  // ��ų �̻��� �ߵ�
        rtNotHit,       // �ڵ� (���� �ȸ��� �� ��� �ߵ�?)
        rtSynergyMix,   // �Ӽ� ���ս� �ߵ�
        rtKillEnemy,    // �� óġ�� �ߵ�
        rtGainExp       // ����ġ ȹ���
    }

    // ���� ��� ���
    public enum AddTarget
    {
        atAllDmg,                   // ��ü ���ط�
        atBasicAtkDmg,              // ��Ÿ ���ط�
        atSynergyDmg,               // �Ӽ� ���ط�
        atAtkSpeed,                 // ���� �ӵ�
        atAtkRange,                 // ���� ����
        atProjectileSpeed,          // ����ü �ӵ�
        atProjectileCount,          // ����ü ��
        atSkillCT,                  // ��ų ��Ÿ��
        atSwapCT,                   // ���� ��Ÿ��
        atGainUlt,                  // �ñر� ������
        atPent,                     // ����
        atMovementSpeed,            // �̵� �ӵ�
        atGainGold,                 // ��� ȹ�淮
        atGainExp,                  // ����ġ ȹ�淮
        atCri,                      // ġ��Ÿ Ȯ��
        atMagnet,                   // �ڼ���
        atRevive,                   // ��Ȱ
        atMaxHP,                    // �ִ� ü��
        atHPRegen,                  // ü�� ���
        atReflect,                  // ���ط� �ݻ�
        atGainDMG,                  // �޴� ���ط�
        atSynergyPercent_Pyro,      // �Ӽ� ���� Ȯ��(Pyro)
        atSynergyPercent_Ice,       // �Ӽ� ���� Ȯ��(Ice)
        atStatUpTime,               // ������ ���� �ð�
        atActiveTime,               // ��Ƽ�� ���� �ð�
        atGift,                     // ����
        atActiveDmg_Per,            // ��Ƽ�� ������ ����(���ݷ� %)
        atActiveDmg,                // ��Ƽ�� ����ġ ����
        atSynergyDmg_Pyro,          // �Ӽ� ���ط�_Pyro
        atSynergyDmg_Ice,           // �Ӽ� ���ط�_Ice
        atActivePer,                // ��Ƽ�� �ߵ� Ȯ��
        atActiveProjectile,         // ��Ƽ�� ����ü ��
        atActiveCT,                 // ��Ƽ�� �ߵ� ��Ÿ��
        atActiveMission_BasicAtk,   // ��Ƽ�� �ߵ� Ƚ��(��Ÿ)
        atSkillCT_Rolling,          // ��ų ��Ÿ��(������)
        akSkillRange_Rolling,       // ��ų ����(������)
        atSkillDmg,                 // ��ų ���ط� ����
        atGainMax,                  // ������ max
        atSwapMaxCT,                // ���� �ִ� ��Ÿ�� ����
        atSkillMaxCT,               // ��ų �ִ� ��Ÿ�� ����
        atFixHPRegen,               // ü�� ȸ��
        atNone                      // ���� ��� x
    }

    
}
