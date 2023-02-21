using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;

    float timer;

    // 웨이브 시간을 체크할 변수
    public float waveTime;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();   
    }

    void Update()
    {
        timer += Time.deltaTime;
     
        if (timer > 0.5f)
        {
            waveTime += timer;

            // 1웨이브: 0번 적 생성
            if (waveTime <= 60f)
            {
                Spawn(0);
                timer = 0f;
            }
            // 2웨이브: 1번 적 생성
            else if (waveTime > 60f && waveTime <= 120f)
            {
                Spawn(1);
                timer = 0f;
            }
            // 3웨이브: 2번 적 생성 
            else if (waveTime > 120f && waveTime <= 180f)
            {
                Spawn(2);
                timer = 0f;
            }
            // 4웨이브: 3번 적 생성   
            else if (waveTime > 180f && waveTime <= 240f)
            {
                Spawn(3);
                timer = 0f;
            }
            // 5웨이브: 4번 적 생성
            else if (waveTime > 240f && waveTime <= 300f)
            {
                Spawn(4);
                timer = 0f;
                // 5웨이브가 마지막 웨이브 타임이므로 waveTime 0으로 초기화
                waveTime = 0f;
            }
               
        }
    }

    /*
    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(Random.Range(0, 5));
        // 자식 오브젝트만 선택되도록 랜덤 시작은 1부터(자기 자신도 spawnPoint에 포함됨)
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
    */

    // 웨이브마다 다른 적을 스폰하는 함수 (일단 5웨이브 주기로 돈다)
    void Spawn(int wave)
    {
        GameObject enemy = GameManager.instance.pool.Get(wave);
        // Random.Range 시작값을 1로 한 이유: 0으로 하면 자기 자신도 포함됨, 자식 오브젝트만 선택되도록 할 것.
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}
