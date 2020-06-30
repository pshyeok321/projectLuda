using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS04PATROL : MS04FSMState
{
    
    private Vector3 destination;
    private bool isDown = false;
  
    public override void BeginState()
    {
        base.BeginState();

        DestiNation();
        IsDownCheck();
    }


    public override void EndState()
    {
        base.EndState();

        IsClockwiseCheck();
        
    }

    private void Update()
    {
        transform.LookAt(destination);
        if (!_manager.isChangeMS04)
        {
            if (Vector3.Distance(destination, transform.position) < 1f)
            {
                _manager.SetState(MS04State.ATTACK);
                return;
            }
        }

        if (_manager.isChangeMS04)
        {
            if (Vector3.Distance(destination, transform.position) <1f)
            {
                _manager.SetState(MS04State.IDLE);
                return;
            }
        }
        _manager.CC.CKMove(destination, _manager.Stat);
    }



    private void DestiNation()
    {
        destination = new Vector3(
              _manager.patrolSpots[_manager._patrolcount].position.x,
              _manager.patrolSpots[_manager._patrolcount].position.y,
              _manager.patrolSpots[_manager._patrolcount].position.z);
    }

    private void IsDownCheck()
    {
        if (!_manager.isClockwise)
        {
            if (_manager._patrolcount == _manager.patrolSpots.Length -1)
            {
                isDown = true;
            }
            if (_manager._patrolcount == 0)
            {
                isDown = false;
            }
        }
        else if (_manager.isClockwise)
        {
            if (_manager._patrolcount > _manager.patrolSpots.Length -1)
            {
                _manager._patrolcount = 0;
            }
        }
    }
    private void IsClockwiseCheck()
    {
        if (!_manager.isClockwise)
        {

            if ((_manager._patrolcount >= 0 || _manager._patrolcount < _manager.patrolSpots.Length -1) && !isDown)
            {
                ++_manager._patrolcount;
            }
            if ((_manager._patrolcount >= 0 || _manager._patrolcount < _manager.patrolSpots.Length -1) && isDown)
            {
                --_manager._patrolcount;
            }
        }
        else if (_manager.isClockwise)
        {
            ++_manager._patrolcount;
        }


    }
 
}
