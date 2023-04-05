using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int Gold;

    public int UGLv_AllDmg;
    public int UGLv_BasicAtkDmg;
    public int UGLv_SynergyDmg;
    public int UGLv_AtkSpeed;
    public int UGLv_AtkRange;
    public int UGLv_ProjectileSpeed;
    /*
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
    */
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
        Debug.Log(path);
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
                PlayerData.Gold = saveData.Gold;
                PlayerData.UGLv_AllDmg = saveData.UGLv_AllDmg;
                PlayerData.UGLv_BasicAtkDmg = saveData.UGLv_BasicAtkDmg;
                PlayerData.UGLv_SynergyDmg = saveData.UGLv_SynergyDmg;
                PlayerData.UGLv_AtkSpeed = saveData.UGLv_AtkSpeed;
                PlayerData.UGLv_AtkRange = saveData.UGLv_AtkRange;
                PlayerData.UGLv_ProjectileSpeed = saveData.UGLv_ProjectileSpeed;
            }
        }

        Debug.Log("Data Load");
    }

    public void JsonReset()
    {
        PlayerData.Gold = 0;
        PlayerData.UGLv_AllDmg = 0;
        PlayerData.UGLv_BasicAtkDmg = 0;
        PlayerData.UGLv_SynergyDmg = 0;
        PlayerData.UGLv_AtkSpeed = 0;
        PlayerData.UGLv_AtkRange = 0;
        PlayerData.UGLv_ProjectileSpeed = 0;
        JsonSave();
        Debug.Log("Data Reset");
    }

    public void JsonSave()
    {
        SaveData saveData = new SaveData();

        saveData.Gold = PlayerData.Gold;
        saveData.UGLv_AllDmg = PlayerData.UGLv_AllDmg;
        saveData.UGLv_BasicAtkDmg = PlayerData.UGLv_BasicAtkDmg;
        saveData.UGLv_SynergyDmg = PlayerData.UGLv_SynergyDmg;
        saveData.UGLv_AtkSpeed = PlayerData.UGLv_AtkSpeed;
        saveData.UGLv_AtkRange = PlayerData.UGLv_AtkRange;
        saveData.UGLv_ProjectileSpeed = PlayerData.UGLv_ProjectileSpeed;

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, json);
        Debug.Log("Data Save");
    }
}
