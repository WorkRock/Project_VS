using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*기능
 * 1. 저장된 Perk들의 실행 조건 확인
 * 2. 실행 조건을 달성했을 시 Perk의 addTarget을 검사
 * 3. 조건에 맞는 value를 더해줌
 * 4. 유지시간이 존재할 시 이후 감소
 * 5. 액티브 Init
*/
public class PerkValueCheck : MonoBehaviour
{
    [SerializeField] private int[] Level;   // 레벨 별 증가 값을 계산하기 위한 퍽의 레벨 배열
    public bool swapCheck;          // 스왑을 했는지 체크, 
    public bool[] isCountRequire;   // 유지시간을 계산해야 하는 퍽인지 체크하는 배열
    private float[] cnt;            // 유지 시간을 계산하는 배열

    // 생성할 오브젝트
    GameObject lightning;
    bool isLightning;       // 낙뢰 실행 메소드가 겹치지 않게하기 위한 변수

    private void Start()
    {
        // 배열 초기화
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
        // 퍽 인벤토리에 저장된 퍽의 갯수만큼
        for(int i = 0; i < GameManager.instance.perkInven.perks.Count; i++)
        {
           // 저장된 퍽의 발동 조건을 검사한다.
           checkValue(i);
        }
        
        if(isLightning)
        {

        }

    }

    // 액티브 퍽일 때 실행하는 메소드
    void InitActive()
    {


        // isLightning 이면(낙뢰가 떨어지고 있으면 낙뢰를 생성 X)
        if (isLightning)
            return;

        int perActive = Random.Range(0, 100);
        if (perActive < 30)
        {
            // 근처에 적이 없을 때
            if (!GameManager.instance.player.scanner.nearestTarget)
            {
                isLightning = true;

                // 낙뢰 생성
                Debug.Log("30% 확률로 낙뢰 생성");
                // 화면 안 랜덤 위치에 낙뢰 생성
                float randomX = Random.Range(GameManager.instance.player.transform.position.x - 8f, GameManager.instance.player.transform.position.x + 9f);
                float randomY = Random.Range(GameManager.instance.player.transform.position.x - 4f, GameManager.instance.player.transform.position.x + 5f);

                lightning = GameManager.instance.pool.Get(21);
                lightning.transform.position = new Vector3(randomX, randomY);
                lightning.transform.rotation = Quaternion.identity;

                Invoke("LightningOff", 0.15f);
            }

            // 적이 있을 때
            else
            {
                isLightning = true;

                Debug.Log("30% 확률로 낙뢰 생성");
                Vector3 targetPos = GameManager.instance.player.scanner.nearestTarget.position;

                lightning = GameManager.instance.pool.Get(21);
                lightning.transform.position = targetPos;
                lightning.transform.rotation = Quaternion.identity;

                Invoke("LightningOff", 0.15f);
            }         
        }     
    }

    // 저장된 Perk들의 발동 조건을 체크한다.
    public void checkValue(int i)
    {
        //Debug.Log("CheckValue");
        // 퍽 인벤토리의 퍽들의 발동 조건을 검사한다.
        switch (GameManager.instance.perkInven.perks[i].requireTarget.ToString())
        {
            // 발동 조건: X
            case "rtNone":
                // 발동 조건이 rtNone인 경우 PerkInvenManager에서 퍽 획득시 바로 발동 
                break;
            // 발동 조건: 평타
            case "rtBasicAtk":
                // 평타 공격 주기가 돌아올 때
                if (GameManager.instance.weaponManager.invokeTime_Main > GameManager.instance.weaponManager.CT - 0.03f
                    || (GameManager.instance.weaponManager.subWeapon != null && GameManager.instance.weaponManager.invokeTime_Sub > GameManager.instance.weaponManager.CT_Sub - 0.03f))
                {
                    Debug.Log("평타 공격주기가 돌아왔습니다.");
                    isBasicAtk(i);                  
                }

                break;
            // 발동 조건: 스왑
            case "rtSwap":
                // swapCheck이 false이고 player의 canSwap이 false일때?
                if (swapCheck == false && GameManager.instance.player.canSwap == false)
                {
                    isSwap(i);
                }
                break;


                /*  rtNone,         // None (발동 조건 x, 퍽 획득 시 바로 발동)
                    rtBaiscAtk,     // 평타 공격시 발동
                    rtUseSkill,     // 스킬 공격시 발동
                    rtPyro,         // 연소 공격시 발동
                    rtElectro,      // 낙뢰 공격시 발동
                    rtIce,          // 빙결 공격시 발동
                    rtSwap,         // 스왑시 발동
                    rtNotUseSkill,  // 스킬 미사용시 발동
                    rtNotHit,       // 자동 (내가 안맞을 때 상시 발동?)
                    rtSynergyMix    // 속성 결합시 발동
                 */
        }
    }

    // # 발동 조건이 rtBasicAtk일 때 호출
    private void isBasicAtk(int i)
    {
        Debug.Log("isBasicAtk");

        
        // 1. 유지시간 X, 액티브 퍽 O 인가?
        if (!GameManager.instance.perkInven.perks[i].isCount && GameManager.instance.perkInven.perks[i].isActive)
        {
            InitActive();       
        }

        // 2. 유지시간 O, 액티브 X 인가?
        
    }

    private void LightningOff()
    {
        Debug.Log("낙뢰 끄기");
        isLightning = false;
        lightning.SetActive(false);
    }

    // # 발동 조건이 rtSwap일 때 호출
    private void isSwap(int i)
    {
        Debug.Log("isSwap");
        swapCheck = true;
        // 퍽 인벤의 퍽이 유지시간 O이고 프리팹 생성 X인 경우 : 일정 시간동안 스탯 증가
        if (GameManager.instance.perkInven.perks[i].isCount && !GameManager.instance.perkInven.perks[i].isActive)
        {
            // 해당 퍽의 isCountRequire(유지시간을 계산해야 하는가?)를 true로 만들어준다.
            isCountRequire[i] = true;
            // 해당 퍽의 대상 Y를 증가
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

    // 대상 Y를 증가시켜주는 함수 
    public void addTargetY(int i)
    {
        // 퍽 인벤.퍽의 증가대상Y를 검사
        switch (GameManager.instance.perkInven.perks[i].addTargetY.ToString())
        {
            // # 공격 속도
            case "atAtkSpeed":
                GameManager.instance.weaponManager.CT
                    = GameManager.instance.weaponManager.mainWeapon.CT_Main *
                    (1 - (GameManager.instance.perkInven.perks[i].basicY + (GameManager.instance.perkInven.perks[i].addY * Level[i])));
                Debug.Log("CT : " + GameManager.instance.weaponManager.CT);
                break;
            // # 전체 피해량
            case "atAllDmg":
                GameManager.instance.player.AllDmg += GameManager.instance.perkInven.perks[i].basicX;
                Debug.Log("AllDmg : " + GameManager.instance.player.AllDmg);
                break;
        }
    }

    // 증가시켰던 대상 Y의 값을 원래대로 감소시켜주는 함수(FixedUpdate에서 시간계산 후 호출)
    public void minusTargetY(int i)
    {
        // 퍽 인벤.퍽의 증가대상Y를 검사
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
        // 퍽인벤의 있는 퍽의 개수만큼 반복
        for (int i = 0; i < GameManager.instance.perkInven.perks.Count; i++)
        {
            // 유지시간을 계산해야 하는 퍽이라면
            if (isCountRequire[i])
            {
                // 해당 퍽의 유지 시간을 계산
                cnt[i] += Time.deltaTime;
            }

            // 유지시간을 계산할 필요 없는 퍽이라면 시간 = 0
            else
            {
                cnt[i] = 0;
                continue;
            }

            // 유지시간을 계산해야 하는 퍽이고, 현재 유지시간이 해당 퍽의 총 유지시간(기본 유지시간 + 레벨별 증가 시간)보다 커지면
            if (isCountRequire[i] && cnt[i] > GameManager.instance.perkInven.perks[i].basicX + GameManager.instance.perkInven.perks[i].addX * Level[i])
            {
                // 유지시간 초기화, 유지시간 계산 필요 X, 증가시켰던 스탯을 초기화
                cnt[i] = 0;
                isCountRequire[i] = false;
                minusTargetY(i);
            }
        }
    }
}
