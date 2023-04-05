using System.Collections.Generic;
using UnityEngine;
public class PlayerData : Singleton<PlayerData>
{
    protected int _charNum;

    protected int _Gold;

    #region Lobby UG
    // Lobby UG
    protected int _UGLv_AllDmg;
    protected int _UGLv_BasicAtkDmg;
    protected int _UGLv_SynergyDmg;
    protected int _UGLv_AtkSpeed;
    protected int _UGLv_AtkRange;
    protected int _UGLv_ProjectileSpeed;
    protected int _UGLv_ProjectileCount;
    protected int _UGLv_SkillCT;
    protected int _UGLv_SwapCT;
    protected int _UGLv_GainUlt;
    protected int _UGLv_Pent;
    protected int _UGLv_MovementSpeed;
    protected int _UGLv_GainGold;
    protected int _UGLv_GainExp;
    protected int _UGLv_Cri;
    protected int _UGLv_Magnet;
    protected int _UGLv_Revive;
    protected int _UGLv_MaxHP;
    protected int _UGLv_HPRegen;
    protected int _UGLv_Reflect;
    protected int _UGLv_GainDMG;
    #endregion

    #region Player Stat
    // Player Stat
    protected int playerCharNum;
    protected float allDmg;
    protected float AllDmg;
    protected float BasicAtkDmg;
    protected float SynergyDmg;
    protected float AtkSpeed;
    protected float AtkRange;
    protected float ProjectileSpeed;
    protected float ProjectileCount;
    protected float SkillCT;
    protected float SwapCT;
    protected float GainUlt;
    protected float Pent;
    protected float MovementSpeed;
    protected float GainGold;
    protected float GainExp;
    protected float Cri;
    protected float Magnet;
    protected float Revive;
    protected float MaxHP;
    protected float HPRegen;
    protected float Reflect;
    protected float GainDMG; 
    #endregion

    public static int charNum
    {
        get { return GetInstance()._charNum; }
        set { GetInstance()._charNum = value; }
    }

    public static int Gold
    {
        get { return GetInstance()._Gold; }
        set { GetInstance()._Gold = value; }
    }

    public static int UGLv_AllDmg
    {
        get { return GetInstance()._UGLv_AllDmg; }
        set { GetInstance()._UGLv_AllDmg = value; }
    }

    public static int UGLv_BasicAtkDmg
    {
        get { return GetInstance()._UGLv_BasicAtkDmg; }
        set { GetInstance()._UGLv_BasicAtkDmg = value; }
    }
    public static int UGLv_SynergyDmg
    {
        get { return GetInstance()._UGLv_SynergyDmg; }
        set { GetInstance()._UGLv_SynergyDmg = value; }
    }
    public static int UGLv_AtkSpeed
    {
        get { return GetInstance()._UGLv_AtkSpeed; }
        set { GetInstance()._UGLv_AtkSpeed = value; }
    }
    public static int UGLv_AtkRange
    {
        get { return GetInstance()._UGLv_AtkRange; }
        set { GetInstance()._UGLv_AtkRange = value; }
    }
    public static int UGLv_ProjectileSpeed
    {
        get { return GetInstance()._UGLv_ProjectileSpeed; }
        set { GetInstance()._UGLv_ProjectileSpeed = value; }
    }
    public static int UGLv_ProjectileCount
    {
        get { return GetInstance()._UGLv_ProjectileCount; }
        set { GetInstance()._UGLv_ProjectileCount = value; }
    }
    public static int UGLv_SkillCT
    {
        get { return GetInstance()._UGLv_SkillCT; }
        set { GetInstance()._UGLv_SkillCT = value; }
    }

    public static int UGLv_SwapCT
    {
        get { return GetInstance()._UGLv_SwapCT; }
        set { GetInstance()._UGLv_SwapCT = value; }
    }
    public static int UGLv_GainUlt
    {
        get { return GetInstance()._UGLv_GainUlt; }
        set { GetInstance()._UGLv_GainUlt = value; }
    }
    public static int UGLv_Pent
    {
        get { return GetInstance()._UGLv_Pent; }
        set { GetInstance()._UGLv_Pent = value; }
    }
    public static int UGLv_MovementSpeed
    {
        get { return GetInstance()._UGLv_MovementSpeed; }
        set { GetInstance()._UGLv_MovementSpeed = value; }
    }
    public static int UGLv_GainGold
    {
        get { return GetInstance()._UGLv_GainGold; }
        set { GetInstance()._UGLv_GainGold = value; }
    }
    public static int UGLv_GainExp
    {
        get { return GetInstance()._UGLv_GainExp; }
        set { GetInstance()._UGLv_GainExp = value; }
    }
    public static int UGLv_Cri
    {
        get { return GetInstance()._UGLv_Cri; }
        set { GetInstance()._UGLv_Cri = value; }
    }
    public static int UGLv_Magnet
    {
        get { return GetInstance()._UGLv_Magnet; }
        set { GetInstance()._UGLv_Magnet = value; }
    }
    public static int UGLv_Revive
    {
        get { return GetInstance()._UGLv_Revive; }
        set { GetInstance()._UGLv_Revive = value; }
    }
    public static int UGLv_MaxHP
    {
        get { return GetInstance()._UGLv_MaxHP; }
        set { GetInstance()._UGLv_MaxHP = value; }
    }
    public static int UGLv_HPRegen
    {
        get { return GetInstance()._UGLv_HPRegen; }
        set { GetInstance()._UGLv_HPRegen = value; }
    }
    public static int UGLv_Reflect
    {
        get { return GetInstance()._UGLv_Reflect; }
        set { GetInstance()._UGLv_Reflect = value; }
    }
    public static int UGLv_GainDMG
    {
        get { return GetInstance()._UGLv_GainDMG; }
        set { GetInstance()._UGLv_GainDMG = value; }
    }
            
}