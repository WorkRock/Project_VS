using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    // 퍽리스트_Basic : 33개
    public List<Perk> perks;
    // 퍽리스트_UG : 33개
    public List<Perk> perkList_UG;

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
        // 중복되지 않는 랜덤한 숫자를 저장할 배열
        int[] randomIndex = new int[3];
        
        // 랜덤 퍽 3개를 뽑기 위한 반복
        for (int i = 0; i < 3; i++)
        {
            randomIndex[i] = Random.Range(0, perks.Count);
            //중복제거를 위한 for문 
            for (int j = 0; j < i; j++) 
            {
                /*현재 randomIndex[]에 저장된 랜덤숫자와 이전에 randomIndex[]에 들어간 숫자를 비교
                 ※예를 들어
                 배열 randomIndex[3]에 숫자 6이 들어갔을때 이전에 완성된 배열 randomIndex[0], randomIndex[1], randomIndex[2]와 비교하여
                 숫자 6이 중복되지 않을시 다음 randomIndex[4]으로 넘어가고, 중복된다면 다시 randomIndex[3]에 중복되지   
                 않는 숫자를 넣기 위하여 i를 한번 감소한후 처음 for문으로 돌아가 다시 randomIndex[3]을 채운다
                 */
                if (randomIndex[i] == randomIndex[j])
                {
                    i--;
                }
            }
        }
        for(int i = 0; i < 3; i++)
        {
            // 랜덤리스트에 뽑은 퍽을 넣어줌
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

    // 슬롯을 클릭했을 때 호출 되는 함수 (여기서 Time.timeScale을 다시 1로 해준다)
    public void AddPerkCall(int num)
    {
        // 퍽 인벤토리에 클릭한 퍽을 넣어줌
        GameManager.instance.perkInven.AddPerk(randomPerks[num]);
        // 레벨업 ui 비활성화 및 시간 초기화
        GameManager.instance.levelUpUI.SetActive(false);
        Time.timeScale = 1f;
    }
}