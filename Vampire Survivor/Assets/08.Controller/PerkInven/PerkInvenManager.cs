using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*기능
 * 1. 퍽 정보 저장
 * 2. 퍽 인벤토리 기능
 * 3. 퍽 초기화
*/
public class PerkInvenManager : MonoBehaviour
{
    // 퍽 리스트
    public List<Perk> perks;    

    // 슬롯들의 부모(자기 자신)
    public Transform slotParents;

    // 슬롯 배열
    public Slot_Perk[] slots;

    // 슬롯
    public Slot_Perk slot;

   
    private void OnValidate()
    {
        // 퍽 슬롯 초기화
        slots = slotParents.GetComponentsInChildren<Slot_Perk>();
    }

    private void Awake()
    {
        FreshSlot();  
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //Debug.Log(perks[Random.Range(0, 3)].addTarget);
        //isSwap(0);
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

    // 슬롯에 퍽 추가하기
    public void AddPerk(Perk _perk)
    {
        // 퍽이 9개 미만일 때만 addPerk을 수행
        if (perks.Count >= 9)
            return;

        perks.Add(_perk);   // 퍽 리스트에 매개변수로 받은 퍽을 추가
 
        // 
        if (GameManager.instance.perkValueCheck.swapCheck == false)
            GameManager.instance.perkValueCheck.swapCheck = true;
        Debug.Log("PerksCount : " + perks.Count);
        // 퍽인벤매니저의 자식으로 슬롯을 하나 생성
        Instantiate(slot, slotParents);
        slots = slotParents.GetComponentsInChildren<Slot_Perk>();
        FreshSlot();


        // 발동 조건이 rtNone인 경우 퍽 획득시 바로 발동
        if (_perk.requireTarget.ToString() == "rtNone")
        {
            GameManager.instance.perkValueCheck.addTargetY(perks.Count - 1);    // perk.Count - 1은 마지막으로 먹은 퍽을 의미
            Debug.Log($"전체 피해량이 {_perk.basicX} 만큼 증가하였습니다.");
        } 
    }
}