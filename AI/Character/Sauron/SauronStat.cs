using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauronStat : CharacterStat
{
    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData.SauronMaxHp;
        _hp = _maxHp;
        _attackRange = statData.SauronAttackRange;
        _moveSpeed = statData.SauronMoveSpeed;
        _str = statData.SauronStr;
        _turnSpeed = statData.TurnSpeed;
    }
}
