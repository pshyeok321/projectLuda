using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneIDLE : DroneFSMState
{
    [SerializeField]
    private float _idleTime = 1.0f;
    private float _time = 0.0f;

    public override void BeginState()
    {
        base.BeginState();

        // 적당히 랜덤으로 아이들타임을 진행시킨채로 초기화.
        _time = Random.Range(0.0f, 0.5f);
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        if (GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS))
        {
            _manager.SetState(DroneState.CHASE);
            return;
        }

        _time += Time.deltaTime;
        if (_time > _idleTime)
        {
            
            _manager.SetState(DroneState.PATROL);
            _manager._patrolcount++;
            _time = 0;
        }
        if (_manager._patrolcount >= 10)
        {
            _manager._patrolcount = 1;
        }
    }
}
