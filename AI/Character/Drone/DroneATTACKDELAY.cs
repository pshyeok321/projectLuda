using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneATTACKDELAY : DroneFSMState
{
    private float _idleTime = 1.0f;
    private float _time = 0.0f;

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
        

        if (Vector3.Distance(_manager.PlayerTransform.position, transform.position) >= _manager.Stat.AttackRange)
        {
            _manager.SetState(DroneState.CHASE);
            return;
        }
        //transform.LookAt(_manager.PlayerTransform);
        if (_time >= _idleTime)
        {
            if (Vector3.Distance(_manager.PlayerTransform.position, transform.position) < _manager.Stat.AttackRange)
            {
                _time = 0;
                _manager.SetState(DroneState.ATTACK);
                Debug.Log("ATTACKDELAY"); // 이쪽에 애니메이션 한번만 작동.
                return;
            }
        }
    }
}
