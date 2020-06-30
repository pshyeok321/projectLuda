using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSCAN : DroneFSMState
{
    private float _scanTime = 5.0f;
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

        if(_time >= _scanTime)
        {
            Debug.Log("SCAN");
            _manager.SetState(DroneState.PATROL);
            _time = 0f;
            _manager.isScan = false;
            _manager._objTypeCount++;
            if (_manager._objTypeCount >= _manager.Obj.Count)
            {
                _manager._objTypeCount = 0;
                //_manager.list.Clear();
            }
        }

        
        //스캔을 해봅시다!
        //우선 _manager에서 스캔에 대한 정보를 가져옵니다.
        // 그리고 위치를 알아야 하는데 위치를 모릅니다.
        // 그리고 리스트를 사용해야 하는데...
      
        //if (!GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS))
        //{
        //    _manager.SetState(DroneState.IDLE);
        //    return;
        //}

        //if (Vector3.Distance(_manager.PlayerTransform.position, transform.position) < _manager.Stat.AttackRange)
        //{
        //    _manager.SetState(DroneState.ATTACKDELAY);
        //    return;
        //}

        //_manager.CC.CKMove(_manager.PlayerTransform.position, _manager.Stat);
    }
}
