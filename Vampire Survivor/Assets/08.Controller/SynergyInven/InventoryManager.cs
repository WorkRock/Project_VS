using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // 시너지 리스트
    public List<Synergy> synergies;

    // 슬롯들의 부모(자기 자신)
    [SerializeField]
    private Transform slotParent;
    // 슬롯 배열
    [SerializeField]
    private Slot[] slots;
    // 슬롯
    [SerializeField]
    private Slot slot;

    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }

    void Awake()
    {
        FreshSlot();
    }

    public void FreshSlot()
    {
        int i = 0;
        for (; i < synergies.Count ; i++)
        {
            slots[i].synergy = synergies[i];
        }
        for (; i < slots.Length; i++)
        {
            slots[i].synergy = null;
        }
    }

    // 슬롯에 시너지 추가하기
    public void AddItem(Synergy _synergy)
    {
        synergies.Add(_synergy);   // 시너지 리스트에 매개변수로 받은 시너지를 추가
        // 시너지인벤토리 매니저의 자식으로 슬롯을 하나 생성
        Instantiate(slot, slotParent);
        slots = slotParent.GetComponentsInChildren<Slot>();
        FreshSlot();
    }
}
