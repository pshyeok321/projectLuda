using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS04INCAPEND : MS04FSMState
{
    float _time = 0;
    float IncapEndTime = 1f;
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

        if (_time > IncapEndTime)
        {
            _manager.SetState(MS04State.IDLE);
            return;
        }
    }
}
