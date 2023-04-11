using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int Gold;

    public bool[] isClassGet = new bool[7];

    public bool isKnightGet;
    public bool isArcherGet;
    public bool isThiefGet;
    public bool isMagicianGet;
    public bool isSpearWarriorGet;
    public bool isShieldGuyGet;
    public bool isBabarianGet;

    public int UGLv_AllDmg;
    public int UGLv_BasicAtkDmg;
    public int UGLv_SynergyDmg;
    public int UGLv_AtkSpeed;
    public int UGLv_AtkRange;
    public int UGLv_ProjectileSpeed;
    public int UGLv_ProjectileCount;
    public int UGLv_SkillCT;
    public int UGLv_SwapCT;
    public int UGLv_GainUlt;
    public int UGLv_Pent;
    public int UGLv_MovementSpeed;
    public int UGLv_GainGold;
    public int UGLv_GainExp;
    public int UGLv_Cri;
    public int UGLv_Magnet;
    public int UGLv_Revive;
    public int UGLv_MaxHP;
    public int UGLv_HPRegen;
    public int UGLv_Reflect;
    public int UGLv_GainDMG;

}
public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;
    string path;

    void Awake()
    {
        if (instance == null) //instance가 null. 즉, 시스템상에 존재하고 있지 않을때
        {
            instance = this; //내자신을 instance로 넣어줍니다.
            DontDestroyOnLoad(gameObject); //OnLoad(씬이 로드 되었을때) 자신을 파괴하지 않고 유지
        }
        else
        {
            if (instance != this) //instance가 내가 아니라면 이미 instance가 하나 존재하고 있다는 의미
                Destroy(this.gameObject); //둘 이상 존재하면 안되는 객체이니 방금 AWake된 자신을 삭제
        }
    }

    void Start()
    {
        path = Path.Combine(Application.dataPath, "database.json");
        JsonLoad();
    }

    public void JsonLoad()
    {
        if (path == null) path = Path.Combine(Application.dataPath, "database.json");

        SaveData saveData = new SaveData();

        if (!File.Exists(path))
        {
            JsonReset();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                PlayerData.isKnightGet = saveData.isKnightGet;
                PlayerData.isArcherGet = saveData.isArcherGet;
                PlayerData.isThiefGet = saveData.isThiefGet;
                PlayerData.isMagicianGet = saveData.isMagicianGet;
                PlayerData.isSpearWarriorGet = saveData.isSpearWarriorGet;
                PlayerData.isShieldGuyGet = saveData.isShieldGuyGet;
                PlayerData.isBabarianGet = saveData.isBabarianGet;
                PlayerData.Gold = saveData.Gold;
                PlayerData.UGLv_AllDmg = saveData.UGLv_AllDmg;
                PlayerData.UGLv_BasicAtkDmg = saveData.UGLv_BasicAtkDmg;
                PlayerData.UGLv_SynergyDmg = saveData.UGLv_SynergyDmg;
                PlayerData.UGLv_AtkSpeed = saveData.UGLv_AtkSpeed;
                PlayerData.UGLv_AtkRange = saveData.UGLv_AtkRange;
                PlayerData.UGLv_ProjectileSpeed = saveData.UGLv_ProjectileSpeed;
                PlayerData.UGLv_ProjectileCount = saveData.UGLv_ProjectileCount;
                PlayerData.UGLv_SkillCT = saveData.UGLv_SkillCT;
                PlayerData.UGLv_GainUlt = saveData.UGLv_GainUlt;
                PlayerData.UGLv_Pent = saveData.UGLv_Pent;
                PlayerData.UGLv_MovementSpeed = saveData.UGLv_MovementSpeed;
                PlayerData.UGLv_GainGold = saveData.UGLv_GainGold;
                PlayerData.UGLv_GainExp = saveData.UGLv_GainExp;
                PlayerData.UGLv_Cri = saveData.UGLv_Cri;
                PlayerData.UGLv_Magnet = saveData.UGLv_Magnet;
                PlayerData.UGLv_Revive = saveData.UGLv_Revive;
                PlayerData.UGLv_MaxHP = saveData.UGLv_MaxHP;
                PlayerData.UGLv_HPRegen = saveData.UGLv_HPRegen;
                PlayerData.UGLv_Reflect = saveData.UGLv_Reflect;
                PlayerData.UGLv_GainDMG = saveData.UGLv_GainDMG;
            }
        }
    }

    public void JsonReset()
    {
        PlayerData.isKnightGet = true;
        PlayerData.isArcherGet = false;
        PlayerData.isThiefGet = false;
        PlayerData.isMagicianGet = false;
        PlayerData.isSpearWarriorGet = false;
        PlayerData.isShieldGuyGet = false;
        PlayerData.isBabarianGet = false;
        PlayerData.Gold = 0;
        PlayerData.UGLv_AllDmg = 0;
        PlayerData.UGLv_BasicAtkDmg = 0;
        PlayerData.UGLv_SynergyDmg = 0;
        PlayerData.UGLv_AtkSpeed = 0;
        PlayerData.UGLv_AtkRange = 0;
        PlayerData.UGLv_ProjectileSpeed = 0;
        PlayerData.UGLv_ProjectileCount = 0;
        PlayerData.UGLv_SkillCT = 0;
        PlayerData.UGLv_GainUlt = 0;
        PlayerData.UGLv_Pent = 0;
        PlayerData.UGLv_MovementSpeed = 0;
        PlayerData.UGLv_GainGold = 0;
        PlayerData.UGLv_GainExp = 0;
        PlayerData.UGLv_Cri = 0;
        PlayerData.UGLv_Magnet = 0;
        PlayerData.UGLv_Revive = 0;
        PlayerData.UGLv_MaxHP = 0;
        PlayerData.UGLv_HPRegen = 0;
        PlayerData.UGLv_Reflect = 0;
        PlayerData.UGLv_GainDMG = 0;
        JsonSave();
    }

    public void JsonSave()
    {
        SaveData saveData = new SaveData();

        saveData.isKnightGet = PlayerData.isKnightGet;
        saveData.isArcherGet = PlayerData.isArcherGet;
        saveData.isThiefGet = PlayerData.isThiefGet;
        saveData.isMagicianGet = PlayerData.isMagicianGet;
        saveData.isSpearWarriorGet = PlayerData.isSpearWarriorGet;
        saveData.isShieldGuyGet = PlayerData.isShieldGuyGet;
        saveData.isBabarianGet = PlayerData.isBabarianGet;
        saveData.Gold = PlayerData.Gold;
        saveData.UGLv_AllDmg = PlayerData.UGLv_AllDmg;
        saveData.UGLv_BasicAtkDmg = PlayerData.UGLv_BasicAtkDmg;
        saveData.UGLv_SynergyDmg = PlayerData.UGLv_SynergyDmg;
        saveData.UGLv_AtkSpeed = PlayerData.UGLv_AtkSpeed;
        saveData.UGLv_AtkRange = PlayerData.UGLv_AtkRange;
        saveData.UGLv_ProjectileSpeed = PlayerData.UGLv_ProjectileSpeed;
        saveData.UGLv_ProjectileCount = PlayerData.UGLv_ProjectileCount;
        saveData.UGLv_SkillCT = PlayerData.UGLv_SkillCT;
        saveData.UGLv_GainUlt = PlayerData.UGLv_GainUlt;
        saveData.UGLv_Pent = PlayerData.UGLv_Pent;
        saveData.UGLv_MovementSpeed = PlayerData.UGLv_MovementSpeed;
        saveData.UGLv_GainGold = PlayerData.UGLv_GainGold;
        saveData.UGLv_GainExp = PlayerData.UGLv_GainExp;
        saveData.UGLv_Cri = PlayerData.UGLv_Cri;
        saveData.UGLv_Magnet = PlayerData.UGLv_Magnet;
        saveData.UGLv_Revive = PlayerData.UGLv_Revive;
        saveData.UGLv_MaxHP = PlayerData.UGLv_MaxHP;
        saveData.UGLv_HPRegen = PlayerData.UGLv_HPRegen;
        saveData.UGLv_Reflect = PlayerData.UGLv_Reflect;
        saveData.UGLv_GainDMG = PlayerData.UGLv_GainDMG;

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, json);
    }
}
