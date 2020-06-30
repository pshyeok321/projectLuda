using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP01IDLE : MP01FSMState
{
    private float _idleTime = 0.01f;
    [SerializeField]
    private float _time = 0.0f;
    public override void BeginState()
    {
        base.BeginState();

        // 적당히 랜덤으로 아이들타임을 진행시킨채로 초기화.
        //_time = Random.Range(0.0f, 0.5f);

    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        if (_manager._patrolcount > _manager.patrolSpots.Length)
        {
            _manager._patrolcount = 0;
        }

        _time += Time.deltaTime;
        if (_time >= _idleTime) {
            if (!_manager.isAlert) {
                _manager.SetState(MP01State.PATROL);
                _manager.SightLight[0].SetActive(true);
                _time = 0;
                return;
            }
            if (_manager.isAlert) {
                
                _manager.SetState(MP01State.ALERT);
                //_manager._alertCount++;
                _manager.SightLight[1].SetActive(true);
                _time = 0;
                return;
            }
        }

        

    }

}
