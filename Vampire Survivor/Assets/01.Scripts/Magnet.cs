using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float magnetRange;
    public float maxMagnetRange;
    CircleCollider2D circle;

    private void Awake()
    {
        circle = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        magnetRange = 0.8f;
        circle.radius = magnetRange;
    }

    private void Update()
    {
        // 자석 범위 증가
        if (Input.GetKeyDown(KeyCode.Q))
        {
            magnetRange += 0.2f;
            circle.radius = magnetRange;
            if (magnetRange >= maxMagnetRange)
                magnetRange = maxMagnetRange;
        }
    }
}
