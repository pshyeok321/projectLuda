using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FW02FSMManager))]
public class FW02FSMState : MonoBehaviour
{
    protected FW02FSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<FW02FSMManager>();
    }

    public virtual void BeginState()
    {

    }

    public virtual void EndState()
    {

    }
}
