using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*���
 * 1. ����� Perk���� ���� ���� Ȯ��
 * 2. ���� ������ �޼����� �� Perk�� addTarget�� �˻�
 * 3. ���ǿ� �´� value�� ������
 * 4. �����ð��� ������ �� ���� ����
 * 5. ��Ƽ�� Init
*/
public class PerkValueCheck : MonoBehaviour
{
    [SerializeField] private int[] Level;   // ���� �� ���� ���� ����ϱ� ���� ���� ���� �迭
    public bool swapCheck;          // ������ �ߴ��� üũ, 
    public bool[] isCountRequire;   // �����ð��� ����ؾ� �ϴ� ������ üũ�ϴ� �迭
    private float[] cnt;            // ���� �ð��� ����ϴ� �迭

    // ������ ������Ʈ
    GameObject lightning;
    bool isLightning;       // ���� ���� �޼ҵ尡 ��ġ�� �ʰ��ϱ� ���� ����

    private void Start()
    {
        // �迭 �ʱ�ȭ
        cnt = new float[9];
        Level = new int[9];
        isCountRequire = new bool[9];
        for (int i = 0; i < isCountRequire.Length; i++)
        {
            isCountRequire[i] = false;
        }
        swapCheck = false;

    }

    void Update()
    {
        // �� �κ��丮�� ����� ���� ������ŭ
        for(int i = 0; i < GameManager.instance.perkInven.perks.Count; i++)
        {
           // ����� ���� �ߵ� ������ �˻��Ѵ�.
           checkValue(i);
        }
        
        if(isLightning)
        {

        }

    }

    // ��Ƽ�� ���� �� �����ϴ� �޼ҵ�
    void InitActive()
    {


        // isLightning �̸�(���ڰ� �������� ������ ���ڸ� ���� X)
        if (isLightning)
            return;

        int perActive = Random.Range(0, 100);
        if (perActive < 30)
        {
            // ��ó�� ���� ���� ��
            if (!GameManager.instance.player.scanner.nearestTarget)
            {
                isLightning = true;

                // ���� ����
                Debug.Log("30% Ȯ���� ���� ����");
                // ȭ�� �� ���� ��ġ�� ���� ����
                float randomX = Random.Range(GameManager.instance.player.transform.position.x - 8f, GameManager.instance.player.transform.position.x + 9f);
                float randomY = Random.Range(GameManager.instance.player.transform.position.x - 4f, GameManager.instance.player.transform.position.x + 5f);

                lightning = GameManager.instance.pool.Get(21);
                lightning.transform.position = new Vector3(randomX, randomY);
                lightning.transform.rotation = Quaternion.identity;

                Invoke("LightningOff", 0.15f);
            }

            // ���� ���� ��
            else
            {
                isLightning = true;

                Debug.Log("30% Ȯ���� ���� ����");
                Vector3 targetPos = GameManager.instance.player.scanner.nearestTarget.position;

                lightning = GameManager.instance.pool.Get(21);
                lightning.transform.position = targetPos;
                lightning.transform.rotation = Quaternion.identity;

                Invoke("LightningOff", 0.15f);
            }         
        }     
    }

    // ����� Perk���� �ߵ� ������ üũ�Ѵ�.
    public void checkValue(int i)
    {
        //Debug.Log("CheckValue");
        // �� �κ��丮�� �ܵ��� �ߵ� ������ �˻��Ѵ�.
        switch (GameManager.instance.perkInven.perks[i].requireTarget.ToString())
        {
            // �ߵ� ����: X
            case "rtNone":
                // �ߵ� ������ rtNone�� ��� PerkInvenManager���� �� ȹ��� �ٷ� �ߵ� 
                break;
            // �ߵ� ����: ��Ÿ
            case "rtBasicAtk":
                // ��Ÿ ���� �ֱⰡ ���ƿ� ��
                if (GameManager.instance.weaponManager.invokeTime_Main > GameManager.instance.weaponManager.CT - 0.03f
                    || (GameManager.instance.weaponManager.subWeapon != null && GameManager.instance.weaponManager.invokeTime_Sub > GameManager.instance.weaponManager.CT_Sub - 0.03f))
                {
                    Debug.Log("��Ÿ �����ֱⰡ ���ƿԽ��ϴ�.");
                    isBasicAtk(i);                  
                }

                break;
            // �ߵ� ����: ����
            case "rtSwap":
                // swapCheck�� false�̰� player�� canSwap�� false�϶�?
                if (swapCheck == false && GameManager.instance.player.canSwap == false)
                {
                    isSwap(i);
                }
                break;


                /*  rtNone,         // None (�ߵ� ���� x, �� ȹ�� �� �ٷ� �ߵ�)
                    rtBaiscAtk,     // ��Ÿ ���ݽ� �ߵ�
                    rtUseSkill,     // ��ų ���ݽ� �ߵ�
                    rtPyro,         // ���� ���ݽ� �ߵ�
                    rtElectro,      // ���� ���ݽ� �ߵ�
                    rtIce,          // ���� ���ݽ� �ߵ�
                    rtSwap,         // ���ҽ� �ߵ�
                    rtNotUseSkill,  // ��ų �̻��� �ߵ�
                    rtNotHit,       // �ڵ� (���� �ȸ��� �� ��� �ߵ�?)
                    rtSynergyMix    // �Ӽ� ���ս� �ߵ�
                 */
        }
    }

    // # �ߵ� ������ rtBasicAtk�� �� ȣ��
    private void isBasicAtk(int i)
    {
        Debug.Log("isBasicAtk");

        
        // 1. �����ð� X, ��Ƽ�� �� O �ΰ�?
        if (!GameManager.instance.perkInven.perks[i].isCount && GameManager.instance.perkInven.perks[i].isActive)
        {
            InitActive();       
        }

        // 2. �����ð� O, ��Ƽ�� X �ΰ�?
        
    }

    private void LightningOff()
    {
        Debug.Log("���� ����");
        isLightning = false;
        lightning.SetActive(false);
    }

    // # �ߵ� ������ rtSwap�� �� ȣ��
    private void isSwap(int i)
    {
        Debug.Log("isSwap");
        swapCheck = true;
        // �� �κ��� ���� �����ð� O�̰� ������ ���� X�� ��� : ���� �ð����� ���� ����
        if (GameManager.instance.perkInven.perks[i].isCount && !GameManager.instance.perkInven.perks[i].isActive)
        {
            // �ش� ���� isCountRequire(�����ð��� ����ؾ� �ϴ°�?)�� true�� ������ش�.
            isCountRequire[i] = true;
            // �ش� ���� ��� Y�� ����
            addTargetY(i);
        }

        //
        else isCountRequire[i] = false;
    }

    // # 

    public void addTargetX(int i)
    {

    }

    public void minusTargetX(int i)
    {

    }

    // ��� Y�� ���������ִ� �Լ� 
    public void addTargetY(int i)
    {
        // �� �κ�.���� �������Y�� �˻�
        switch (GameManager.instance.perkInven.perks[i].addTargetY.ToString())
        {
            // # ���� �ӵ�
            case "atAtkSpeed":
                GameManager.instance.weaponManager.CT
                    = GameManager.instance.weaponManager.mainWeapon.CT_Main *
                    (1 - (GameManager.instance.perkInven.perks[i].basicY + (GameManager.instance.perkInven.perks[i].addY * Level[i])));
                Debug.Log("CT : " + GameManager.instance.weaponManager.CT);
                break;
            // # ��ü ���ط�
            case "atAllDmg":
                GameManager.instance.player.AllDmg += GameManager.instance.perkInven.perks[i].basicX;
                Debug.Log("AllDmg : " + GameManager.instance.player.AllDmg);
                break;
        }
    }

    // �������״� ��� Y�� ���� ������� ���ҽ����ִ� �Լ�(FixedUpdate���� �ð���� �� ȣ��)
    public void minusTargetY(int i)
    {
        // �� �κ�.���� �������Y�� �˻�
        switch (GameManager.instance.perkInven.perks[i].addTargetY.ToString())
        {
            case "atAtkSpeed":
                GameManager.instance.weaponManager.CT
                    /= 1 - (GameManager.instance.perkInven.perks[i].basicY + (GameManager.instance.perkInven.perks[i].addY * Level[i]));
                Debug.Log("CT(minus) : " + GameManager.instance.weaponManager.CT);
                break;
        }
    }


    private void FixedUpdate()
    {
        // ���κ��� �ִ� ���� ������ŭ �ݺ�
        for (int i = 0; i < GameManager.instance.perkInven.perks.Count; i++)
        {
            // �����ð��� ����ؾ� �ϴ� ���̶��
            if (isCountRequire[i])
            {
                // �ش� ���� ���� �ð��� ���
                cnt[i] += Time.deltaTime;
            }

            // �����ð��� ����� �ʿ� ���� ���̶�� �ð� = 0
            else
            {
                cnt[i] = 0;
                continue;
            }

            // �����ð��� ����ؾ� �ϴ� ���̰�, ���� �����ð��� �ش� ���� �� �����ð�(�⺻ �����ð� + ������ ���� �ð�)���� Ŀ����
            if (isCountRequire[i] && cnt[i] > GameManager.instance.perkInven.perks[i].basicX + GameManager.instance.perkInven.perks[i].addX * Level[i])
            {
                // �����ð� �ʱ�ȭ, �����ð� ��� �ʿ� X, �������״� ������ �ʱ�ȭ
                cnt[i] = 0;
                isCountRequire[i] = false;
                minusTargetY(i);
            }
        }
    }
}
