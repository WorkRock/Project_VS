using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    // �ܸ���Ʈ_Basic : 33��
    public List<Perk> perks;
    // �ܸ���Ʈ_UG : 33��
    public List<Perk> perkList_UG;

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
        // �ߺ����� �ʴ� ������ ���ڸ� ������ �迭
        int[] randomIndex = new int[3];
        
        // ���� �� 3���� �̱� ���� �ݺ�
        for (int i = 0; i < 3; i++)
        {
            randomIndex[i] = Random.Range(0, perks.Count);
            //�ߺ����Ÿ� ���� for�� 
            for (int j = 0; j < i; j++) 
            {
                /*���� randomIndex[]�� ����� �������ڿ� ������ randomIndex[]�� �� ���ڸ� ��
                 �ؿ��� ���
                 �迭 randomIndex[3]�� ���� 6�� ������ ������ �ϼ��� �迭 randomIndex[0], randomIndex[1], randomIndex[2]�� ���Ͽ�
                 ���� 6�� �ߺ����� ������ ���� randomIndex[4]���� �Ѿ��, �ߺ��ȴٸ� �ٽ� randomIndex[3]�� �ߺ�����   
                 �ʴ� ���ڸ� �ֱ� ���Ͽ� i�� �ѹ� �������� ó�� for������ ���ư� �ٽ� randomIndex[3]�� ä���
                 */
                if (randomIndex[i] == randomIndex[j])
                {
                    i--;
                }
            }
        }
        for(int i = 0; i < 3; i++)
        {
            // ��������Ʈ�� ���� ���� �־���
            randomPerks[i] = perks[randomIndex[i]];
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

    // ������ Ŭ������ �� ȣ�� �Ǵ� �Լ� (���⼭ Time.timeScale�� �ٽ� 1�� ���ش�)
    public void AddPerkCall(int num)
    {
        // �� �κ��丮�� Ŭ���� ���� �־���
        GameManager.instance.perkInven.AddPerk(randomPerks[num]);
        // ������ ui ��Ȱ��ȭ �� �ð� �ʱ�ȭ
        GameManager.instance.levelUpUI.SetActive(false);
        Time.timeScale = 1f;
    }
}