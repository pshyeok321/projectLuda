using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SauronFSMManager))]
public class SauronFSMState : MonoBehaviour
{
    protected SauronFSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<SauronFSMManager>();
    }

    public virtual void BeginState()
    {

    }

    public virtual void EndState()
    {

    }
}
