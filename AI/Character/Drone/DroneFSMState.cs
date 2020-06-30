using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DroneFSMManager))]
[ExecuteInEditMode]
public class DroneFSMState : MonoBehaviour
{
    protected DroneFSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<DroneFSMManager>();
    }

    public virtual void BeginState()
    {

    }

    public virtual void EndState()
    {

    }
}
