using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    // 모든 퍽리스트들을 담아놓을 변수
    public List<Perk> perks;

    // 레벨업 시 3개 뽑아 저장할 퍽리스트
    public List<Perk> randomPerks;

    // 슬롯들의 부모(자기 자신)
    public Transform slotParents;

    // 슬롯 배열
    public Slot_Perk[] slots;

    // 슬롯
    public Slot_Perk slot;



    private void OnValidate()
    {
        // 퍽 슬롯 초기화(3개)
        slots = slotParents.GetComponentsInChildren<Slot_Perk>();
    }

    private void Awake()
    {
        //slots = new Slot_Perk[3];
        //FreshSlot(); 
    }

    // 슬롯 갱신
    public void FreshSlot()
    {
        int i = 0;
        for (; i < perks.Count; i++)
        {
            slots[i].perk = perks[i];
        }
        for (; i < slots.Length; i++)
        {
            slots[i].perk = null;
        }
    }

    // 레벨업 창이 활성화 될때(레벨업을 할 때) perks(모든 퍽 리스트)에서 랜덤한 3개의 퍽을 뽑아 표출한다
    private void OnEnable()
    {
        // 1. 랜덤 퍽 3개 뽑기
        for(int i = 0; i < 3; i++)
        {
            int random = Random.Range(0, perks.Count);
            // 랜덤리스트에 뽑은 퍽을 넣어줌
            randomPerks[i] = perks[random];
            slots[i].perk = randomPerks[i];     
        }   
    }

    private void OnDisable()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].perk = null;
        }
    }

    public void AddPerkCall(int num)
    {
        GameManager.instance.perkInven.AddPerk(randomPerks[num]);
    }
}