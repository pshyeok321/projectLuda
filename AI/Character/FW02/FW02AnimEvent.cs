using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FW02AnimEvent : MonoBehaviour
{
    FW02FSMManager _manager;
    
    public FW02ATTACK _attackCp;
    private void Awake()
    {
        
        _manager = transform.root.GetComponent<FW02FSMManager>();
        //_attackCp = _manager.GetComponent<FW02ATTACK>();
    }

    void HitCheck()
    {
        if (null != _attackCp)
        {
            _attackCp.AttackCheck();
        }
    }
}
