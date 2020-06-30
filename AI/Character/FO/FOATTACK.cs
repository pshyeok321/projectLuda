using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOATTACK : FOFSMState
{
    float interval = 2.4f;
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
        AttackPattern();
        
    }
    public override void AttackBehavior()
    {
        base.AttackBehavior();
        GameLib.SimpleDamageProcess(transform, _manager.Stat.AttackRange,
            "Player", _manager.Stat);
    }

    void AttackPattern()
    {
        int randPattern = Random.Range(0, 2);

        if (_manager.PlayerTransform.position.x >= _manager.list[0].transform.position.x - interval) // 0보다 클 때 0~5
        {
            if (randPattern == 0)
            {
                Instantiate(_manager.Ball, _manager.BallsPos[0].position, Quaternion.identity);
                Instantiate(_manager.Ball, _manager.BallsPos[1].position, Quaternion.identity);

                _manager.SetState(FOState.IDLE);
            }
            if (randPattern == 1)
            {
                Instantiate(_manager.Ball, _manager.BallsPos[0].position, Quaternion.identity);
                Instantiate(_manager.Ball, _manager.BallsPos[2].position, Quaternion.identity);

                _manager.SetState(FOState.IDLE);
            }
        }

        if (_manager.PlayerTransform.position.x >= _manager.list[1].transform.position.x - interval &&
            _manager.PlayerTransform.position.x < _manager.list[0].transform.position.x - interval) //-5보다 클 때 -5~0 그리고 0보다 작을때
        {
            if (randPattern == 0)
            {
                Instantiate(_manager.Ball, _manager.BallsPos[0].position, Quaternion.identity);
                Instantiate(_manager.Ball, _manager.BallsPos[1].position, Quaternion.identity);

                _manager.SetState(FOState.IDLE);
            }
            if (randPattern == 1)
            {
                Instantiate(_manager.Ball, _manager.BallsPos[1].position, Quaternion.identity);
                Instantiate(_manager.Ball, _manager.BallsPos[2].position, Quaternion.identity);

                _manager.SetState(FOState.IDLE);
            }
        }

        if (_manager.PlayerTransform.position.x >= _manager.list[2].transform.position.x - interval * 10f &&
            _manager.PlayerTransform.position.x < _manager.list[1].transform.position.x - interval) //-11보다 클 때 -11~-5 그리고 -5보다 작을때
        {
            if (randPattern == 0)
            {
                Instantiate(_manager.Ball, _manager.BallsPos[1].position, Quaternion.identity);
                Instantiate(_manager.Ball, _manager.BallsPos[2].position, Quaternion.identity);

                _manager.SetState(FOState.IDLE);
            }
            if (randPattern == 1)
            {
                Instantiate(_manager.Ball, _manager.BallsPos[0].position, Quaternion.identity);
                Instantiate(_manager.Ball, _manager.BallsPos[2].position, Quaternion.identity);

                _manager.SetState(FOState.IDLE);
            }
        }
    }
}