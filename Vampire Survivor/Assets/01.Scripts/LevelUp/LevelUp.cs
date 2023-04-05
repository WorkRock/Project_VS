using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    // ��� �ܸ���Ʈ���� ��Ƴ��� ����
    public List<Perk> perks;

    // ������ �� 3�� �̾� ������ �ܸ���Ʈ
    public List<Perk> randomPerks;

    // ���Ե��� �θ�(�ڱ� �ڽ�)
    public Transform slotParents;

    // ���� �迭
    public Slot_Perk[] slots;

    // ����
    public Slot_Perk slot;



    private void OnValidate()
    {
        // �� ���� �ʱ�ȭ(3��)
        slots = slotParents.GetComponentsInChildren<Slot_Perk>();
    }

    private void Awake()
    {
        //slots = new Slot_Perk[3];
        //FreshSlot(); 
    }

    // ���� ����
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

    // ������ â�� Ȱ��ȭ �ɶ�(�������� �� ��) perks(��� �� ����Ʈ)���� ������ 3���� ���� �̾� ǥ���Ѵ�
    private void OnEnable()
    {
        // 1. ���� �� 3�� �̱�
        for(int i = 0; i < 3; i++)
        {
            int random = Random.Range(0, perks.Count);
            // ��������Ʈ�� ���� ���� �־���
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