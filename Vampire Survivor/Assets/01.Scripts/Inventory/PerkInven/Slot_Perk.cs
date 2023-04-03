using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot_Perk : MonoBehaviour
{
    public Text perkName;
    public Text perkLevel;
    public Image synergyImage;
    public Image specialImage;

    private Perk _perk;

    public Perk perk
    {
        get { return _perk; }
        set
        {
            _perk = value;
            if (_perk != null)
            {
                perkName.text = perk.perkName;
                perkLevel.text = perk.perkLevel;
                synergyImage.sprite = perk.SynergyImage;
                specialImage.sprite = perk.SpecialImage;
                synergyImage.color = new Color(1, 1, 1, 1);
                specialImage.color = new Color(1, 1, 1, 1);
            }
            else
            {
                perkName.text = "";
                perkLevel.text = "";
                synergyImage.color = new Color(1, 1, 1, 0);
                specialImage.color = new Color(1, 1, 1, 0);
            }
        }
    }
}
