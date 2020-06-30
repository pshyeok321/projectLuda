using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP01INCAP : MP01FSMState
{
    float _time = 0;
    float IncapTime = 10f;
    public override void BeginState()
    {
        base.BeginState();
    }


    public override void EndState()
    {
        base.EndState();

    }
    private void Update()
    {
        _time += Time.deltaTime;

        if (_time > IncapTime)
        {
            _manager.SetState(MP01State.IDLE);
            return;
        }
    }

}
