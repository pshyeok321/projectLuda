using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBossIDLE : FactoryBossFSMState {
    private float idleTime = 3.0f;
    private float time = 0.0f;
    [SerializeField]
    private int _randPT;

    
    public override void BeginState()
    {
        base.BeginState();
        
        // 적당히 랜덤으로 아이들타임을 진행시킨채로 초기화.
        time = Random.Range(0.0f, 0.5f);

        //_randPT = Random.Range(0, 3);
        _randPT = Random.Range(0, 3);
        if (_randPT == 0)
        {
            _manager.PT1 = true;
            _manager.PT2 = false;
            _manager.PT3 = false;
        }
        if (_randPT == 1)
        {
            _manager.PT1 = false;
            _manager.PT2 = true;
            _manager.PT3 = false;
        }
        if (_randPT == 2)
        {
            _manager.PT1 = false;
            _manager.PT2 = false;
            _manager.PT3 = true;
        }        
    }

    public override void EndState()
    {
        
        base.EndState();
    }

    private void Update()
    {     
        time += Time.deltaTime;

        if (time > idleTime)
        {
            Vector3 playerPosition = new Vector3(_manager.PlayerTransform.position.x, this.transform.position.y, _manager.PlayerTransform.position.z);


            transform.LookAt(playerPosition);
            int randPattern = Random.Range(0, 3);

            switch (randPattern)
            {
                case 0:
                    _manager.SetState(FactoryBossState.ATTACKDELAY);
                    _manager.PT1 = true;
                    _manager.PT2 = false;
                    _manager.PT3 = false;                    
                    break;
                case 1:
                    _manager.SetState(FactoryBossState.ATTACKDELAY);
                    _manager.PT1 = false;
                    _manager.PT2 = true;
                    _manager.PT3 = false;
                    break;
                case 2:
                    _manager.SetState(FactoryBossState.ATTACKDELAY);
                    _manager.PT1 = false;
                    _manager.PT2 = false;
                    _manager.PT3 = true;
                    break;
                default:
                    Debug.Log("없는 패턴");
                    break;
            }
            time = 0;
        }
    }
}

