using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FW02CHASESTART : FW02FSMState
{
    float _time = 0;
    float StartTime = 0.4f;

    public Vector3 playerTrans; // 플레이어와 현재 몬스터의 위치값 계산을 위한 Vector3

    public override void BeginState()
    {
        base.BeginState();

    }
    public override void EndState()
    {
        base.EndState();
        _time = 0;
        _manager.agent.isStopped = true;
    }
    private void FixedUpdate()
    {
        playerTrans = new Vector3(_manager.PlayerTransform.position.x, transform.position.y, _manager.PlayerTransform.position.z);
        _manager.agent.destination = playerTrans;
        _manager.agent.isStopped = false;

        _time += Time.deltaTime;
        if (_time >= StartTime)
        {
            _manager.SetState(FW02State.CHASE);
            return;
        }
    }
}
