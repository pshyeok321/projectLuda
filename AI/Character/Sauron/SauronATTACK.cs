using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauronATTACK : SauronFSMState
{

    public override void BeginState()
    {
        base.BeginState();
        GameLib.SimpleDamageProcess(transform, _manager.Stat.AttackRange,
                     "Player", _manager.Stat);
        Debug.Log("Damage");
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        if (!GameLib.DetectCharacter3(_manager.Sight, _manager.PlayerCS, true))
        {
            _manager.SetState(SauronState.IDLE);
            _manager.Sight.transform.Rotate(Vector3.left * 180);

            if ((_manager._sightCount % 2) == 1)
            {
                _manager.SightLight[2].SetActive(false);
            }
            if ((_manager._sightCount % 2) == 0)
            {
                _manager.SightLight[3].SetActive(false);
            }
        }
        //transform.LookAt(_manager.PlayerTransform);
    }    

    public void AttackCheck()
    {
        GameLib.SimpleDamageProcess(transform, _manager.Stat.AttackRange,
                     "Player", _manager.Stat);
        Debug.Log("Damage");
    }
}
