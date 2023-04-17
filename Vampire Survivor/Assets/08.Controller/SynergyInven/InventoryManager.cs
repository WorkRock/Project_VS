using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // �ó��� ����Ʈ
    public List<Synergy> synergies;

    // ���Ե��� �θ�(�ڱ� �ڽ�)
    [SerializeField]
    private Transform slotParent;
    // ���� �迭
    [SerializeField]
    private Slot[] slots;
    // ����
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

    // ���Կ� �ó��� �߰��ϱ�
    public void AddItem(Synergy _synergy)
    {
        synergies.Add(_synergy);   // �ó��� ����Ʈ�� �Ű������� ���� �ó����� �߰�
        // �ó����κ��丮 �Ŵ����� �ڽ����� ������ �ϳ� ����
        Instantiate(slot, slotParent);
        slots = slotParent.GetComponentsInChildren<Slot>();
        FreshSlot();
    }
}
