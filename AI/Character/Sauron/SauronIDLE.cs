using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauronIDLE : SauronFSMState
{
    private float _idleTime = 0.01f;
    private float _time = 0.0f;
    
    public override void BeginState()
    {
        base.BeginState();

        // 적당히 랜덤으로 아이들타임을 진행시킨채로 초기화.
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        //_manager.Sight.enabled = false;
        _manager.Sight.transform.gameObject.SetActive(false);

        if (GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS))
        {            
        }
        
        _time += Time.deltaTime;
        if (_time > _idleTime)
        {
            _manager._sightCount++;

            if ((_manager._sightCount % 2) == 1)
            {
                _manager.SetState(SauronState.RIGHTSIGHT);
            }
            if ((_manager._sightCount % 2) == 0)
            {
                _manager.SetState(SauronState.LEFTSIGHT);
            }
            
            _time = 0;
        }
        


    }
}
