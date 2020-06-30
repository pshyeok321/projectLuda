using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS04AnimEvent : MonoBehaviour
{
    MS04FSMManager _manager;
    [SerializeField]
    MS04ATTACK _attackCp;
    private void Awake()
    {
        _manager = transform.root.GetComponent<MS04FSMManager>();
    }

    void HitCheck()
    {
        if (null != _attackCp)
        {
            _attackCp.AttackCheck();
        }
    }
}
