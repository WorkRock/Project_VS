using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� ���� ��ĵ�ؼ� ���Ÿ� ������ �߻��ϴ� ��ũ��Ʈ
public class Scanner : MonoBehaviour
{
    // ��ĵ�� ����
    public float scanRange;
    // ���̾�
    public LayerMask targetLayer;
    // ��ĵ ����� ���� �迭
    public RaycastHit2D[] targets;
    // ��ĵ ��� �� ���� ����� ��ǥ�� ���� ����
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        // CircleCastAll(1, 2, 3, 4, 5): ������ ĳ��Ʈ�� ��� ��� ����� ��ȯ�ϴ� �Լ�
        /* 1. ĳ���� ���� ��ġ
         * 2. ���� ������
         * 3. ĳ���� ����
         * 4. ĳ���� ����
         * 5. ��� ���̾�
         */
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        // ���������� ���� ����� ��ǥ���� ������Ʈ
        nearestTarget = GetNearest();
    }

    // ���� ����� ���� ��ȯ�ϴ� �Լ�
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos); // Vector3.Distance: ���� A�� B������ �Ÿ��� ������ִ� �Լ�
            
            // �ݺ����� ���� ������ �Ÿ��� ����� �Ÿ�(100)���� ������ ��ü
            if(curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;       
            }
        }

        return result;
    }
}
