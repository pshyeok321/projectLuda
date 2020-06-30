using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneATTACK : DroneFSMState
{

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
        if (Vector3.Distance(_manager.PlayerTransform.position, transform.position) >= _manager.Stat.AttackRange)
        {
            _manager.SetState(DroneState.ATTACKCANCLE);
            return;
        }
        //transform.LookAt(_manager.PlayerTransform);
    }
    public void AttackCheck()
    {
        GameLib.SimpleDamageProcess(transform, _manager.Stat.AttackRange,
            "Player", _manager.Stat);
    }
}
