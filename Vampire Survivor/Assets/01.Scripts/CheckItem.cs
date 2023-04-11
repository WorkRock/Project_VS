using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckItem : MonoBehaviour
{
    // 적이 맞은 공격이 1. 어떤 무기인지 2. main인지 sub인지 체크하기 위해 scriptableObject를 연결
    public Item item;

    public int dmg;
}
