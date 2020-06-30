using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP01LOSTTARGET : MP01FSMState
{
    float timer;
    float waitTime = 5f;
    MP01ALERT alert;
    public override void BeginState()
    {
        alert = _manager.GetComponent<MP01ALERT>();
        base.BeginState();     
    }


    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {            
            alert.isVector3 = true;
            _manager.SetState(MP01State.ALERT);
            timer = 0;
            return;
        }

        if (GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS))
        {
            _manager.isAlertPatrol = true;
            _manager.SetState(MP01State.WATCH);
            return;
        }
    }
}
