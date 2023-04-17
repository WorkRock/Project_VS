using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Image synergyImage;
    [SerializeField] Text synergName;

    private Synergy _synergy;

    public Synergy synergy
    {
        get { return _synergy; }
        set
        {
            _synergy = value;
            if (_synergy != null)
            {
                synergName.text = synergy.synergyName;
                synergyImage.sprite = synergy.synergyImage;              
                synergyImage.color = new Color(1, 1, 1, 1);
            }
            else
            {
                synergName.text = "";
                synergyImage.color = new Color(1, 1, 1, 0);
            }
        }
    }
}
