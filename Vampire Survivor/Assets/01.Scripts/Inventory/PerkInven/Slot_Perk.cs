using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Perk : MonoBehaviour
{
    [SerializeField] Image image_Synergy;
    [SerializeField] Image image_Special;
    [SerializeField] Text text;
    //[SerializeField] GameObject Equip;
    private Perk _perk;
    public Perk perk
    {
        get { return _perk; }
        set
        {
            _perk = value;
            if (_perk != null)
            {
                image_Synergy.sprite = perk.SynergyImage;
                image_Special.sprite = perk.SpecialImage;
                text.text = perk.perkName;
                image_Synergy.color = new Color(1, 1, 1, 1);
                image_Special.color = new Color(1, 1, 1, 1);
            }
            else
            {
                text.text = "";
                image_Synergy.color = new Color(1, 1, 1, 0);
                image_Special.color = new Color(1, 1, 1, 0);
            }
        }
    }
}
