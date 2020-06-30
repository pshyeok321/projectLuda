using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneStat : CharacterStat
{
    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData.DroneMaxHp;
        _hp = _maxHp;
        _attackRange = statData.DroneAttackRange;
        _moveSpeed = statData.DroneMoveSpeed;
        _str = statData.DroneStr;
        _turnSpeed = statData.TurnSpeed;
        _turnDst = statData.turnDst;
        _stoppingDst = statData.stoppingDst;
    }
}
