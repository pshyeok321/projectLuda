using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FW02Stat : CharacterStat
{
    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData.FW02MaxHp;
        _hp = _maxHp;
        _attackRange = statData.FW02AttackRange;
        _moveSpeed = statData.FW02MoveSpeed;
        _str = statData.FW02Str;
        _turnSpeed = statData.TurnSpeed;
        _turnDst = statData.turnDst;
        _stoppingDst = statData.stoppingDst;
    }
}
