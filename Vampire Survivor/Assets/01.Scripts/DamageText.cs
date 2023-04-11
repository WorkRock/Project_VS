using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    float time;
    public float fadeTime = 2f;
    public float upSpeed = 2f;
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * upSpeed;   // �ؽ�Ʈ�� ���� �ö󰡸鼭 ���������
        time += Time.deltaTime;

        if(time < fadeTime)
        {
            GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1f - time / fadeTime);
        }
        else
        {
            time = 0;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1f);
    }
}
