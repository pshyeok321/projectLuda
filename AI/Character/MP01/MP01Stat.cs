using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP01Stat : CharacterStat
{
    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData.MP01MaxHp;
        _hp = _maxHp;
        _attackRange = statData.MP01AttackRange;
        _moveSpeed = statData.MP01MoveSpeed;
        _str = statData.MP01Str;
        _turnSpeed = statData.TurnSpeed;
        _turnDst = statData.turnDst;
        _stoppingDst = statData.stoppingDst;
    }
}
