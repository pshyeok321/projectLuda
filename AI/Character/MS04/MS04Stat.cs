using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS04Stat : CharacterStat
{
    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData.MS04MaxHp;
        _hp = _maxHp;
        _attackRange = statData.MS04AttackRange;
        _moveSpeed = statData.MS04MoveSpeed;
        _str = statData.MS04Str;
        _turnSpeed = statData.TurnSpeed;
        _turnDst = statData.turnDst;
        _stoppingDst = statData.stoppingDst;
    }
}
