using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Char/Create New Char")]
public class PlayerSel : ScriptableObject
{
    [Header("Player Stat")]
    public float cAllDmg;            // 전체 피해량
    public float cBasicAtkDmg;       // 평타 피해량
    public float cSynergyDmg;        // 속성 피해량
    public float cAtkSpeed;          // 공격 속도
    public float cAtkRange;          // 공격 범위
    public float cProjectileSpeed;   // 투사체 속도
    public int cProjectileCount;     // 투사체 수
    public float cSkillCT;           // 스킬 쿨타임
    public float cSwapCT;            // 스왑 쿨타임
    public float cGainUlt;           // 궁극기 충전량
    public int cPent;                // 관통
    public float cMovementSpeed;     // 이동 속도
    public float cGainGold;          // 골드 획득량
    public float cGainExp;           // 경험치 획득량
    public float cCri;               // 치명타 확률
    public float cMagnet;            // 자석력
    public int cRevive;              // 부활
    public float cMaxHP;             // 최대 체력
    public float cNowHP;             // 현재 체력
    public float cHPRegen;           // 체력 재생
    public float cReflect;           // 피해량 반사
    public float cGainDmg;           // 받는 피해량

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
