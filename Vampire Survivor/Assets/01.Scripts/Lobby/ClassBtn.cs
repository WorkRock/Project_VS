using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassBtn : MonoBehaviour
{
    public int value;


    public void clickEvent()
    {
        LobbyManager.instance.InitPrefab(value);
    }
}
