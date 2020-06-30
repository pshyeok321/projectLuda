using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FOFSMManager))]
public class FOFSMState : MonoBehaviour
{
    protected FOFSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<FOFSMManager>();
    }

    public virtual void BeginState()
    {

    }

    public virtual void EndState()
    {

    }

    public virtual void AttackBehavior()
    {

    }
}
