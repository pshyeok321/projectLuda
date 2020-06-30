using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP01ALERT : MP01FSMState
{
    float _UItime = 0;
    float UITime = 5f;
    [SerializeField]
    float _guardTime = 0;
    float GuardTime = 15f;

    float _time = 0;

    public Vector3 lastPlayerVector;
    public Vector3 lastTrans;
    MP01WATCH watch;


    public bool isPlayer = false;
    public bool isVector3 = false;
    public float timer = 0;
    float waitTime = 5f;


    public override void BeginState() {
        base.BeginState();
        //?UI 5초간 출력
        _manager.SightLight[1].SetActive(true);
        _manager.Emotion1.SetActive(true);
        watch = _manager.GetComponent<MP01WATCH>();
        _manager.isAlert = true;

        lastPlayerVector = watch.lastplayervector;
        lastTrans = watch.lastvector;
    }


    public override void EndState() {
        base.EndState();

        ++_manager._alertCount;
        _UItime = 0;
        _manager.isAlert = false;
        isPlayer = false;
        timer = 0;
        isVector3 = false;

        _manager.agent.isStopped = true;
    }

    private void Update() {
        
        _UItime += Time.deltaTime;
        if (_UItime >= UITime) {
          //  _manager.Emotion1.SetActive(false);
        }
        _guardTime += Time.deltaTime;
        if (_guardTime >= GuardTime) {
            _manager.SetState(MP01State.PATROL);
            _manager.isAlert = false;
            _guardTime = 0;
            return;
        }
        if (!isPlayer)
        {
            _manager.agent.destination = lastPlayerVector;
            _manager.agent.stoppingDistance = 0.3f;
            _manager.agent.isStopped = false;
            //_manager.CC.CKMove(lastPlayerVector, _manager.Stat);
            //transform.LookAt(lastPlayerVector);
        }
        if (isVector3)
        {
            _manager.agent.destination = lastTrans;
            _manager.agent.isStopped = false;
            //_manager.CC.CKMove(lastTrans, _manager.Stat);
            //transform.LookAt(lastTrans);
            if (Vector3.Distance(lastTrans, transform.position) < 0.3f && !_manager.isAlertPatrol)
            {
                _manager.isAlertPatrol = true;
                _manager.SetState(MP01State.PATROL);
                return;
            }
        }
        if (GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS)){
            _manager.isAlertPatrol = true;
            _manager.SetState(MP01State.WATCH);
            return;
        }

        if (Vector3.Distance(lastPlayerVector, transform.position) < 0.54f && !isVector3) {
            _manager.SetState(MP01State.LOSTTARGET);
            isPlayer = true;
            return;
            //if (isPlayer)
            //{                
            //    timer += Time.deltaTime;
            //    if (timer > waitTime)
            //    {
            //        isVector3 = true;                   
            //    }
            //}            
        }
        _manager._UIs[2].fillAmount = _manager._UIs[2].fillAmount - (_UItime / 50f);
        if (_manager._UIs[2].fillAmount >= 1)
        {
            _manager._UIs[2].fillAmount = 1;
        }
        if (_manager._UIs[2].fillAmount <= 0)
        {
            _manager._UIs[2].fillAmount = 0;
        }
    }
}
