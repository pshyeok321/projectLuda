using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS04INCAP : MS04FSMState
{
    float _time = 0;
    float IncapTime = 5f;
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

        if(_time > IncapTime)
        {
            _manager.SetState(MS04State.INCAPEND);
            return;
        }
    }
}
