using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBossAnimEvent : MonoBehaviour
{
    FactoryBossFSMManager _manager;
    FactoryBossATTACKTWO _attackCp;
    private void Awake()
    {
        _manager = transform.root.GetComponent<FactoryBossFSMManager>();
        _attackCp = _manager.GetComponent<FactoryBossATTACKTWO>();
    }

    void HitCheck()
    {
        //if (null != _attackCp)
        //{
        //    _attackCp.AttackCheck();
        //}
    }
}   
