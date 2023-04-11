using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Perk/Create New Perk")]
public class Perk : ScriptableObject
{
    [Header("Perk Info")]
    public int perkID;
    public Sprite perkImage;    // 퍽 이미지 -> 퍽 고유 아이콘을 각각 만들것인가??
    public Sprite SynergyImage; // 퍽 속성 이미지
    public Sprite SpecialImage; // 퍽 특화 이미지
    
    [Header("String")]
    public string perkName;
    public string perkInfo;
    public string perkLevel;    // Basic Or Upgrade Perk?
    
    [Header("Bool")]
    //public bool ugPerk;   // 업그레이드된 퍽인지 체크 -> 업그레이드 퍽은 별도의 퍽으로 만들거라서 이 변수는 필요 없나?
    public bool isActive;   // 프리팹을 생성하는 퍽인가?
    public bool isCount;    // 유지시간이 존재하는 퍽인가?
   
    [Header("Plus Data")]
    public float basicX;    // 기본 X
    public float addX;      // 1포인트 당 증가량 X  
    public float basicY;    // 기본 Y
    public float addY;      // 1포인트 당 증가량 Y

    [Header("Enum")]
    public Synergy synergy;
    public Special special;
    public Active active;
    public AddTarget addTargetX;
    public AddTarget addTargetY;
    public RequireTarget requireTarget;

    // 퍽 속성 목록
    public enum Synergy
    {
        syPyro,     // 불
        syElectro,  // 번개
        syIce,      // 얼음
        syPhysics,  // 물리
        syWind,     // 바람
        syNone      // 무
    }

    // 퍽 특화 목록
    public enum Special
    {
        spBasicAtk, // 평타
        spSkill,    // 스킬
        spSwap,     // 스왑  
        spDmg,      // 데미지
        spUtil,     // 유틸
        spSummon,   // 소환
        spHP        // 체력
    }

    // 액티브 목록
    public enum Active
    {
        aPyro,              // 연소
        aElectro,           // 낙뢰
        aIce,               // 빙결
        aFireRing,          // 불꽃링 생성
        aFireBall,          // 화염구 투척
        aSummon_Electro,    // 소환(번개)
        aSummon_Ice,        // 소환(얼음)
        aSummon_Wind,       // 소환(바람)
        aSpellShield,       // 보호막
        aIceBall,           // 얼음 투사체
        None                // 없음
    }

    // 발동 조건 목록
    public enum RequireTarget
    {
        rtNone,         // None (발동 조건 x, 퍽 획득 시 바로 발동)
        rtBasicAtk,     // 평타 공격시 발동
        rtUseSkill,     // 스킬 공격시 발동
        rtPyro,         // 연소 공격시 발동
        rtElectro,      // 낙뢰 공격시 발동
        rtIce,          // 빙결 공격시 발동
        rtSwap,         // 스왑시 발동
        rtNotUseSkill,  // 스킬 미사용시 발동
        rtNotHit,       // 자동 (내가 안맞을 때 상시 발동?)
        rtSynergyMix,   // 속성 결합시 발동
        rtKillEnemy,    // 적 처치시 발동
        rtGainExp       // 경험치 획득시
    }

    // 증가 대상 목록
    public enum AddTarget
    {
        atAllDmg,                   // 전체 피해량
        atBasicAtkDmg,              // 평타 피해량
        atSynergyDmg,               // 속성 피해량
        atAtkSpeed,                 // 공격 속도
        atAtkRange,                 // 공격 범위
        atProjectileSpeed,          // 투사체 속도
        atProjectileCount,          // 투사체 수
        atSkillCT,                  // 스킬 쿨타임
        atSwapCT,                   // 스왑 쿨타임
        atGainUlt,                  // 궁극기 충전량
        atPent,                     // 관통
        atMovementSpeed,            // 이동 속도
        atGainGold,                 // 골드 획득량
        atGainExp,                  // 경험치 획득량
        atCri,                      // 치명타 확률
        atMagnet,                   // 자석력
        atRevive,                   // 부활
        atMaxHP,                    // 최대 체력
        atHPRegen,                  // 체력 재생
        atReflect,                  // 피해량 반사
        atGainDMG,                  // 받는 피해량
        atSynergyPercent_Pyro,      // 속성 부착 확률(Pyro)
        atSynergyPercent_Ice,       // 속성 부착 확률(Ice)
        atStatUpTime,               // 증가량 유지 시간
        atActiveTime,               // 액티브 유지 시간
        atGift,                     // 보상
        atActiveDmg_Per,            // 액티브 데미지 측정(공격력 %)
        atActiveDmg,                // 액티브 데미치 측정
        atSynergyDmg_Pyro,          // 속성 피해량_Pyro
        atSynergyDmg_Ice,           // 속성 피해량_Ice
        atActivePer,                // 액티브 발동 확률
        atActiveProjectile,         // 액티브 투사체 수
        atActiveCT,                 // 액티브 발동 쿨타임
        atActiveMission_BasicAtk,   // 액티브 발동 횟수(평타)
        atSkillCT_Rolling,          // 스킬 쿨타임(구르기)
        akSkillRange_Rolling,       // 스킬 범위(구르기)
        atSkillDmg,                 // 스킬 피해량 증가
        atGainMax,                  // 증가량 max
        atSwapMaxCT,                // 스왑 최대 쿨타임 감소
        atSkillMaxCT,               // 스킬 최대 쿨타임 감소
        atFixHPRegen,               // 체력 회복
        atNone                      // 증가 대상 x
    }

    
}
