using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FW02PATROL : FW02FSMState
{

    private Vector3 destination;
    private bool isDown = false;
    //private bool isWall = false;    

    public float timer = 0;
    float fifteen = 10f;

    float _time = 0;
    float WatchTime = 1f;

    public Vector3 playerTrans; // 플레이어와 현재 몬스터의 위치값 계산을 위한 Vector3
    [Header("Patrol->Chase 인식 거리")]
    public float WatchDistance = 20f;
    public override void BeginState()
    {
        base.BeginState();
        
        DestiNation();
        IsDownCheck();

        //초기화
        //lastTransform.position = transform.position;
        //lastPlayerTransform.position = _manager.PlayerTransform.position;
        _manager.agent.destination = destination;
        _manager.agent.isStopped = false;
        // 플레이어 위치 세팅 Y값은 몬스터 기준
    }


    public override void EndState()
    {
        base.EndState();
        
        IsClockwiseCheck();
        _manager.agent.isStopped = true;
        _time = 0;
    }

    private void Update() {
        playerTrans = new Vector3(_manager.PlayerTransform.position.x, transform.position.y, _manager.PlayerTransform.position.z);
        //_manager.CC.CKMove(destination, _manager.Stat);
        //transform.LookAt(destination);
        if (Vector3.Distance(playerTrans, transform.position) < WatchDistance)
        {
            _time += Time.deltaTime;
            if (_time >= WatchTime)
            {
                _manager.SetState(FW02State.CHASESTART);
                return;
            }
        }


        if (Vector3.Distance(destination, transform.position) < 0.3f)
        {
            _manager.SetState(FW02State.IDLE);
            return;
        }

        //시야에 있다.
        //if (GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS)) {
        //    _manager.SetState(FW02State.ATTACK);
        //    return;
        //    //_manager.SetState(FW02State.WATCH;);
        //    //_manager.SightLight[0].SetActive(false);
        //    //_manager.SightLight[1].SetActive(true);
        //}
        //if (_manager.isAlertPatrol)
        //{
        
            //timer += Time.deltaTime;
            //_manager.SightLight[0].SetActive(false);
            //_manager.SightLight[1].SetActive(true);
            //if (timer >= fifteen)
            //{
                //timer = 0;
                //_manager.SightLight[0].SetActive(true);
                //_manager.SightLight[1].SetActive(false);
                //_manager.isAlertPatrol = false;
            //}
        //}

        
    }



    private void DestiNation()
    {
        destination = new Vector3(
              _manager.patrolSpots[_manager._patrolcount].position.x,
              transform.position.y,
              _manager.patrolSpots[_manager._patrolcount].position.z);
    }
    private void IsDownCheck()
    {
        if (!_manager.isClockwise)
        {
            if (_manager._patrolcount == _manager.patrolSpots.Length - 1)
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
            if (_manager._patrolcount > _manager.patrolSpots.Length - 1)
            {
                _manager._patrolcount = 0;
            }
        }
    }
    private void IsClockwiseCheck()
    {
        if (!_manager.isClockwise)
        {

            if ((_manager._patrolcount >= 0 || _manager._patrolcount < _manager.patrolSpots.Length - 1) && !isDown)
            {
                ++_manager._patrolcount;
            }
            if ((_manager._patrolcount >= 0 || _manager._patrolcount < _manager.patrolSpots.Length - 1) && isDown)
            {
                --_manager._patrolcount;
            }
        }
        else if (_manager.isClockwise)
        {
            ++_manager._patrolcount;
        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Effect")
    //    {
    //        _manager.SetState(FW02State.INCAP);
    //    }
    //}
}
