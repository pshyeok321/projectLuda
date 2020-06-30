using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatData : ScriptableObject
{
    // 시체가 자동으로 사라지는 시간
    public float AutoDestroyTime = 3f;
    // 모든 캐릭터가 도는 속도
    public float TurnSpeed = 10f;

    public float turnDst = 5;

    public float stoppingDst = 10;

    // 플레이어의 최대 체력
    public int PlayerMaxHp = 100;
    // 플레이어의 공격 범위
    public float PlayerAttackRange = 50f;
    // 플레이어의 이동 속도
    public float PlayerMoveSpeed = 50f;
    // 플레이어의 힘
    public int PlayerStr = 10;
    // 플레이어 턴 속도
    public float PlayerTurnSpeed = 180f;


    
    // 슬라임
    public int SlimeMaxHp = 20;
    public float SlimeAttackRange = 50f;
    public float SlimeMoveSpeed = 50f;
    public int SlimeStr = 1;

    // 드론
    public int DroneMaxHp = 20;
    public float DroneAttackRange = 5f;
    public float DroneMoveSpeed = 3f;
    public int DroneStr = 1;

    // EMP드론
    public int MS04MaxHp = 20;
    public float MS04AttackRange = 5f;
    public float MS04MoveSpeed = 2f;
    public int MS04Str = 0;

    // 어택드론
    public int MP01MaxHp = 20;
    public float MP01AttackRange = 5f;
    public float MP01MoveSpeed = 4f;
    public int MP01Str = 1;

    // FW02
    public int FW02MaxHp = 20;
    public float FW02AttackRange = 5f;
    public float FW02MoveSpeed = 4f;
    public int FW02Str = 1;

    // 사우론
    public int SauronMaxHp = 20;
    public float SauronAttackRange = 5f;
    public float SauronMoveSpeed = 5f;
    public int SauronStr = 1;
    
    //1페이지
    // 공장장
    public int FOMaxHp = 20;
    public float FOAttackRange = 50f;
    public float FOMoveSpeed = 50f;
    public int FOStr = 1;
    public float FOHillWindDamageRate = 2f;
    public float FOHillWindRange = 7f;
    public float FOHillWindTurnSpeed = 10f;
    public float FORushDamageRate = 3f;
    public float FORushTime = 1.5f;
    public float FORushSpeed = 0.2f;
    public int FOSommonMany = 5;
    public float FOSkillRate = 0.1f;


    //2페이지
    // 공장장
    public int FactoryBossMaxHp = 20;
    public float FactoryBossAttackRange = 50f;
    public float FactoryBossMoveSpeed = 50f;
    public int FactoryBossStr = 1;
    public float FactoryBossHillWindDamageRate = 2f;
    public float FactoryBossHillWindRange = 7f;
    public float FactoryBossHillWindTurnSpeed = 10f;
    public float FactoryBossRushDamageRate = 3f;
    public float FactoryBossRushTime = 1.5f;
    public float FactoryBossRushSpeed = 0.2f;
    public int FactoryBossSommonMany = 5;
    public float FactoryBossSkillRate = 0.1f;

   


    // 고블린
    public int GoblinMaxHp = 20;
    public float GoblinAttackRange = 50f;
    public float GoblinMoveSpeed = 50f;
    public int GoblinStr = 1;
    public float GoblinHillWindDamageRate = 2f;
    public float GoblinHillWindRange = 7f;
    public float GoblinHillWindTurnSpeed = 10f;
    public float GoblinRushDamageRate = 3f;
    public float GoblinRushTime = 1.5f;
    public float GoblinRushSpeed = 0.2f;
    public int GoblinSommonMany = 5;
    public float GoblinSkillRate = 0.1f;

}
