using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FactoryBossFSMManager))]
public class FactoryBossFSMState : MonoBehaviour
{
    protected FactoryBossFSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<FactoryBossFSMManager>();
    }

    public virtual void BeginState()
    {

    }

    public virtual void EndState()
    {

    }
}
