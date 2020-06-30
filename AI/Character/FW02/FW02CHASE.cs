using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FW02CHASE : FW02FSMState
{
    private Vector3 destination;


    float _time = 0;
    float ChaseTime = 20f;

    public Vector3 playerTrans; // 플레이어와 현재 몬스터의 위치값 계산을 위한 Vector3
    public Vector3 playerAttackTrans;
    public Vector3 MyTrans;
    public Vector3 TransH;
    [Header("Chase->Attack 공격 거리")]
    public float AttackDistance = 10f;
    public override void BeginState()
    {
        base.BeginState();
       
        //초기화
        //_manager.agent.destination = destination;
        //_manager.agent.isStopped = false;
        // 플레이어 위치 세팅 Y값은 몬스터 기준
    }


    public override void EndState()
    {
        base.EndState();

        _manager.agent.isStopped = true;
        //_time = 0;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        //Ray ray = new Ray(MyTrans, transform.forward);

        //Debug.DrawRay(TransH, MyTrans * 50, Color.red);
       
      
       

        playerTrans = new Vector3(_manager.PlayerTransform.position.x, transform.position.y, _manager.PlayerTransform.position.z);
        playerAttackTrans = new Vector3(_manager.PlayerTransform.position.x, _manager.PlayerTransform.position.y + 0.8f, _manager.PlayerTransform.position.z);
        MyTrans = new Vector3(transform.position.x, _manager.PlayerTransform.position.y + 1.4f, transform.position.z);
        TransH = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        _manager.agent.destination = playerTrans;
        _manager.agent.isStopped = false;
        //_manager.CC.CKMove(destination, _manager.Stat);
        //transform.LookAt(destination);
        if (_manager.isPlayer)
        {
            if (Vector3.Distance(playerTrans, transform.position) < AttackDistance)
            {
                _manager.SetState(FW02State.ATTACK);
                return;
            }
        }
        //20초 지나면 다시 patrol
        if(_time >= ChaseTime)
        {
            _manager.SetState(FW02State.CHASEEND);
            _time = 0;
            return;
        }




        Debug.DrawLine(TransH, MyTrans, Color.red, 1);
        RaycastHit hitInfo;
        //RaycastHit hitPlayer;
        if (Physics.Linecast(TransH, playerAttackTrans, out hitInfo, 1 << LayerMask.NameToLayer("Wall")))
        {
            _manager.isWall = true;
        }
        if (!Physics.Linecast(TransH, playerAttackTrans, out hitInfo, 1 << LayerMask.NameToLayer("Wall")))
        {
            _manager.isWall = false;
        }
        //if (Physics.Raycast(ray, out hitInfo, 20, 1 << LayerMask.NameToLayer("Wall")))
        //{
        //    _manager.isWall = true;         
        //}
        //if (!Physics.Raycast(ray, out hitInfo, 20, 1 << LayerMask.NameToLayer("Wall")))
        //{
        //    _manager.isWall = false;
        //}
        if (_manager.isWall)
        {
            if (Physics.Linecast(TransH, playerAttackTrans, out hitInfo, 1 << LayerMask.NameToLayer("Player")))
            {
                _manager.isPlayer = false;
            }
            //if (Physics.Raycast(ray, out hitPlayer, 20, 1 << LayerMask.NameToLayer("Player")))
            //{
            //    //_manager.isWall = false;
            //    _manager.isPlayer = false;
            //}
        }
        if (!_manager.isWall)
        {
            if (Physics.Linecast(TransH, playerAttackTrans, out hitInfo, 1 << LayerMask.NameToLayer("Player")))
            {
                _manager.isPlayer = true;
            }
        }

    }


}
