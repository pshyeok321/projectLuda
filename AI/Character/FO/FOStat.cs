using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOStat : CharacterStat
{
    //public GameObject throwableSlime;
    protected override void Awake()
    {
        base.Awake();
        _maxHp = statData.FOMaxHp;
        _hp = _maxHp;
        _attackRange = statData.FOAttackRange;
        _moveSpeed = statData.FOMoveSpeed;
        _str = statData.FOStr;
    }
}
