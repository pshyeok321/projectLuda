using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MS04FSMManager))]
public class MS04FSMState : MonoBehaviour
{
    protected MS04FSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<MS04FSMManager>();
    }

    public virtual void BeginState()
    {

    }

    public virtual void EndState()
    {

    }
}
