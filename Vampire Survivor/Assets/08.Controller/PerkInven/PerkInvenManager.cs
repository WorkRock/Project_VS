using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*���
 * 1. �� ���� ����
 * 2. �� �κ��丮 ���
 * 3. �� �ʱ�ȭ
*/
public class PerkInvenManager : MonoBehaviour
{
    // �� ����Ʈ
    public List<Perk> perks;    

    // ���Ե��� �θ�(�ڱ� �ڽ�)
    public Transform slotParents;

    // ���� �迭
    public Slot_Perk[] slots;

    // ����
    public Slot_Perk slot;

    // �׽�Ʈ(synergy, special ���� � �ִ��� üũ)
    // -> ���� ȹ���� �� ī��Ʈ�� �÷��ش�?
    public int count_Special_BasicAtk;
    public int count_Special_Swap;

    private void OnValidate()
    {
        // �� ���� �ʱ�ȭ
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

    // ���Կ� �� �߰��ϱ�
    public void AddPerk(Perk _perk)
    {
        // ���� 9�� �̸��� ���� addPerk�� ����
        if (perks.Count >= 9)
            return;

        perks.Add(_perk);   // �� ����Ʈ�� �Ű������� ���� ���� �߰�
 
        // 
        if (GameManager.instance.perkValueCheck.swapCheck == false)
            GameManager.instance.perkValueCheck.swapCheck = true;
        Debug.Log("PerksCount : " + perks.Count);
        // ���κ��Ŵ����� �ڽ����� ������ �ϳ� ����
        Instantiate(slot, slotParents);
        slots = slotParents.GetComponentsInChildren<Slot_Perk>();
        FreshSlot();


        // �ߵ� ������ rtNone�� ��� �� ȹ��� �ٷ� �ߵ�
        if (_perk.requireTarget.ToString() == "rtNone")
        {
            GameManager.instance.perkValueCheck.addTargetY(perks.Count - 1);    // perk.Count - 1�� ���������� ���� ���� �ǹ�
            Debug.Log($"��ü ���ط��� {_perk.basicX} ��ŭ �����Ͽ����ϴ�.");
        }

        // ���� �Ծ��� �� ���� ���� special(Ưȭ)�� �������� üũ�ؼ� ī��Ʈ�� �÷���
        switch (_perk.special)
        {
            case Perk.Special.spBasicAtk:
                count_Special_BasicAtk++;
                break;
            case Perk.Special.spSkill:
                break;
            case Perk.Special.spSwap:
                count_Special_Swap++;
                break;
            case Perk.Special.spDmg:
                break;
            case Perk.Special.spUtil:
                break;
            case Perk.Special.spSummon:
                break;
            case Perk.Special.spHP:
                break;
        }
    }
}