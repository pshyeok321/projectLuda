using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBossStat : CharacterStat
{
    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData.FactoryBossMaxHp;
        _hp = _maxHp;
        _attackRange = statData.FactoryBossAttackRange;
        _moveSpeed = statData.FactoryBossMoveSpeed;
        _str = statData.FactoryBossStr;
    }
}
