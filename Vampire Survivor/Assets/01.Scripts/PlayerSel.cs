using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Char/Create New Char")]
public class PlayerSel : ScriptableObject
{
    [Header("Player Stat")]
    public float cAllDmg;            // ��ü ���ط�
    public float cBasicAtkDmg;       // ��Ÿ ���ط�
    public float cSynergyDmg;        // �Ӽ� ���ط�
    public float cAtkSpeed;          // ���� �ӵ�
    public float cAtkRange;          // ���� ����
    public float cProjectileSpeed;   // ����ü �ӵ�
    public int cProjectileCount;     // ����ü ��
    public float cSkillCT;           // ��ų ��Ÿ��
    public float cSwapCT;            // ���� ��Ÿ��
    public float cGainUlt;           // �ñر� ������
    public int cPent;                // ����
    public float cMovementSpeed;     // �̵� �ӵ�
    public float cGainGold;          // ��� ȹ�淮
    public float cGainExp;           // ����ġ ȹ�淮
    public float cCri;               // ġ��Ÿ Ȯ��
    public float cMagnet;            // �ڼ���
    public int cRevive;              // ��Ȱ
    public float cMaxHP;             // �ִ� ü��
    public float cNowHP;             // ���� ü��
    public float cHPRegen;           // ü�� ���
    public float cReflect;           // ���ط� �ݻ�
    public float cGainDmg;           // �޴� ���ط�

    //public StartWeapon startWeapon;
    public int StartWeapon;

    /*
    public enum StartWeapon
    {
        Sword,
        Bow,
        Knife,
        Spear,
        Wand,
        Axe,
        Shield
    }
    */
}
