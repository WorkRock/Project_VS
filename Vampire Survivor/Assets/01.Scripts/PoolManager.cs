using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리팹들을 보관할 변수 배열
    public GameObject[] prefabs;

    // 풀 담당을 하는 리스트 배열(프리팹 하나당 리스트도 하나로 1:1대응)
    List<GameObject>[] pools;

    void Awake()
    {
        // 1. 리스트 배열 초기화: 풀의 크기는 프리팹의 크기와 같으므로 프리팹 배열의 크기로 초기화
        pools = new List<GameObject>[prefabs.Length];
        
        // 2. 리스트 초기화: 모든 오브젝트 풀 리스트를 초기화
        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 놀고 있는(비활성화 된) 게임오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;
                // 활성화
                select.SetActive(true);
                // 하나 발견했으면 종료
                break;
            }
        }

        // 놀고 있는 게임 오브젝트가 없으면?
        if (select == null)
        {
            // 새롭게 생성하고 select 변수에 할당 -> 즉 점점 등장하는 오브젝트의 수가 증가한다?
            // -> instantiate 두번째 인자로 자신의 transform을 주게 되면 자식으로 생성 되기 때문에 hierarchy가 깔끔해진다.
            select = Instantiate(prefabs[index], transform);
            // 생성된 오브젝트는 해당 오브젝트 풀 리스트에 Add 함수로 추가
            pools[index].Add(select);
        }
        return select;
    }    
}
