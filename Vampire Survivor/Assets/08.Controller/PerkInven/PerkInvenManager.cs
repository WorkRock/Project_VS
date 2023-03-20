using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkInvenManager : MonoBehaviour
{
    public List<Perk> perks;

    [SerializeField]
    private GameObject SlotFullAlert;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot_Perk[] slots;

    [SerializeField]
    private int[] slotLevel;
    public List<Sprite> chessSprites = new List<Sprite>();

    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot_Perk>();
    }

    void Awake()
    {
        FreshSlot();
    }

    public void FreshSlot()
    {
        int i = 0;
        for (; i < perks.Count; i++)
        {
            slots[i].perk = perks[i];
            var chessImage = slots[i].transform.Find("ChessImage").GetComponent<Image>();

            if (slotLevel[i] == 0)
                chessImage.sprite = chessSprites[0];
            else
                chessImage.sprite = chessSprites[slotLevel[i] - 1];
        }
        for (; i < slots.Length; i++)
        {
            var chessImage = slots[i].transform.Find("ChessImage").GetComponent<Image>();
            chessImage.sprite = null;
            slots[i].perk = null;
            //slotLevel[i] = 0;
        }
    }

    public void AddPerk(Perk _perk)
    {
        if (perks.Count < slots.Length)
        {
            perks.Add(_perk);
            Array.Resize<int>(ref slotLevel, slotLevel.Length + 1);
            FreshSlot();
        }
        else
        {
            SlotFullAlert.SetActive(true);
        }
    }

    public void DeletePerk(Perk _perk)
    {
        for (int i = 0; i < perks.Count; i++)
        {
            if (perks[i] == _perk)
            {
                perks.RemoveAt(i);
                FreshSlot();
                break;
            }

        }
    }
}