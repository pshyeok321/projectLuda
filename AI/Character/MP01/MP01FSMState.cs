using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MP01FSMManager))]
public class MP01FSMState : MonoBehaviour
{
    protected MP01FSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<MP01FSMManager>();
    }

    public virtual void BeginState()
    {

    }

    public virtual void EndState()
    {

    }
}
