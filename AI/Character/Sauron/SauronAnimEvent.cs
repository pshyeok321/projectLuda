using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauronAnimEvent : MonoBehaviour
{
    SauronFSMManager _manager;
    SauronATTACK _attackCp;
    private void Awake()
    {
        _manager = transform.root.GetComponent<SauronFSMManager>();
        _attackCp = _manager.GetComponent<SauronATTACK>();
    }

    void HitCheck()
    {
        if (null != _attackCp)
        {
            _attackCp.AttackCheck();
        }
    }
}
