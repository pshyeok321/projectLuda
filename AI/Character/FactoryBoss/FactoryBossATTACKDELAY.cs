using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBossATTACKDELAY : FactoryBossFSMState
{
    public float delayTime = 3.0f;
    public float _time = 0.0f;
    private bool isADE;

    private GameObject ADE;

    public override void BeginState()
    {
        base.BeginState();

        isADE = false;
        int randPT = Random.Range(0, 3);
        _manager.randSpot = new int[3];

        //시계방향 회전
        if (randPT == 0)
        {
            _manager.randSpot[0] = Random.Range(0, _manager.AttackSpots.Length);
            _manager.randSpot[1] = Random.Range(_manager.randSpot[0] + 1, _manager.randSpot[0] + 5);
            _manager.randSpot[2] = Random.Range(_manager.randSpot[1] + 2, _manager.randSpot[1] + 5);
        }
        //시계반대방향 회전
        if (randPT == 1)
        {
            _manager.randSpot[0] = Random.Range(0, _manager.AttackSpots.Length);
            _manager.randSpot[1] = Random.Range(_manager.randSpot[0] - 5, _manager.randSpot[0] - 1);
            _manager.randSpot[2] = Random.Range(_manager.randSpot[1] - 5, _manager.randSpot[2] - 1);
        }
        //랜덤회전
        if (randPT == 2)
        {
            _manager.randSpot[0] = Random.Range(0, _manager.AttackSpots.Length);
            _manager.randSpot[1] = Random.Range(_manager.randSpot[0] + 1, _manager.randSpot[0] + 5);
            _manager.randSpot[2] = Random.Range(_manager.randSpot[1] - 5, _manager.randSpot[1] - 1);
        }

        if (_manager.randSpot[1] > 8)
        {
            _manager.randSpot[1] -= 8;
        }
        if (_manager.randSpot[2] > 8)
        {
            _manager.randSpot[2] -= 8;
        }
        if (_manager.randSpot[1] < 0)
        {
            _manager.randSpot[1] += 8;
        }
        if (_manager.randSpot[2] < 0)
        {
            _manager.randSpot[2] += 8;
        }
        Debug.Log(randPT + "번째 패턴");
        Debug.Log(_manager.randSpot[0]);
        Debug.Log(_manager.randSpot[1]);
        Debug.Log(_manager.randSpot[2]);
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        //Vector3 playerPosition = new Vector3(_manager.PlayerTransform.position.x, this.transform.position.y, _manager.PlayerTransform.position.z);
        //transform.LookAt(playerPosition);

        _time += Time.deltaTime;

        //if (!_manager.PT1)
        //{
            if (_time >= 0.2f && !isADE && _time < 0.4f)
            {
                ADE =  (GameObject)Instantiate(_manager.AttackDelayEffect, _manager.AttackSpots[_manager.randSpot[0]].position, Quaternion.identity);
                Destroy(ADE, 1f);
                isADE = true;
            }
            if (_time >= 0.4f && isADE && _time < 0.6f)
            {
                ADE = (GameObject)Instantiate(_manager.AttackDelayEffect, _manager.AttackSpots[_manager.randSpot[1]].position, Quaternion.identity);
                Destroy(ADE, 1f);
                isADE = false;
            }
            if (_time >= 0.6f && !isADE)
            {
                ADE = (GameObject)Instantiate(_manager.AttackDelayEffect, _manager.AttackSpots[_manager.randSpot[2]].position, Quaternion.identity);
                Destroy(ADE, 1f);
                isADE = true;
            }
        //}


        if (_time > delayTime)
        {
            //if (_manager.PT1)
            //{
            //    _manager.SetState(FactoryBossState.ATTACKONE);

            //    _manager.PT1 = false;
            //    _manager.PT2 = true;                
            //    return;
            //}
            //if (_manager.PT2 || _manager.PT3)
            //{
            _manager.SetState(FactoryBossState.ATTACKONE);
            _time = 0;
            //    _manager.PT1 = true;
            //    _manager.PT2 = false;                
            //    return;
            //}
            //_time = 0;
        }
    }
}






