using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS04IDLE : MS04FSMState
{
    [SerializeField]
    [Header("Idle->Patrol 전환 시간")]
    private float _idleTime = 1.0f;
    
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
        _time += Time.deltaTime;
        if (_time > _idleTime)
        {
            _manager.SetState(MS04State.PATROL);
        }      
    }
}
