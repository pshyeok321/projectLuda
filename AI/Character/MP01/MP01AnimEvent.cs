using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP01AnimEvent : MonoBehaviour
{
    MP01FSMManager _manager;
    
    public MP01ATTACK _attackCp;
    private void Awake()
    {
        
        _manager = transform.root.GetComponent<MP01FSMManager>();
        //_attackCp = _manager.GetComponent<MP01ATTACK>();
    }

    void HitCheck()
    {
        if (null != _attackCp)
        {
            _attackCp.AttackCheck();
        }
    }
}
