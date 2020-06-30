using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBossATTACKTWO : FactoryBossFSMState {

    private float delayTime = 8.0f;
    public float _time = 0.0f;
    public BoxCollider _box;

    private bool isBox;
    private bool isRand;
    public float turnspeed = 3f;

    float frame15 = 0.5f;
    float frame20 = 0.6666f;
    
    public override void BeginState()
    {
        base.BeginState();
        Debug.Log("AttackTwo 들어옴");
        isBox = false;
        Vector3 dir = _manager.AttackSpots[0].position - transform.position;
    }

    public override void EndState()
    {
        base.EndState();
        Debug.Log("AttackTwo 나감");
    }
    private void LateUpdate()
    {
        if (_time < frame20) {
            Vector3 direction = _manager.AttackSpots[0].position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnspeed * Time.deltaTime);
        }
        if (_time > frame20*2 && _time < frame20*3)
        {
            Vector3 direction = _manager.AttackSpots[1].position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnspeed * Time.deltaTime);
        }

        if (_time > frame20*4 &&  _time < frame20*5)
         {
            Vector3 direction = _manager.AttackSpots[2].position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnspeed * Time.deltaTime);
        }
      
    }
    private void Update()
    {
         
        _time += Time.deltaTime;

       
        // 이 3개는 주먹으로 땅 칠 때 주먹 박스(투명하게 처리할 것임)
        if (_time >= frame20 && !isBox && _time < frame20*2)
        {
            Instantiate(_box, _manager.AttackSpots[0].position, Quaternion.identity);
            isBox = true;
        }
        if (_time >= frame20*3 && isBox && _time < frame20*4)
        {
            Instantiate(_box, _manager.AttackSpots[1].position, Quaternion.identity);

            isBox = false;
        }

        if (_time >= frame20*5 && !isBox && _time < frame20*6)
        {
            Instantiate(_box, _manager.AttackSpots[2].position, Quaternion.identity);
            isBox = true;
        }

        // 이 3개는 땅 쳤을 때 생성 되는 3개의 고리(따로 RingEffect에 스크립트가 생성 될 예정)
        if (_time >= frame15+frame20 && isBox && _time < frame20*2)
        {
            Instantiate(_manager.RingEffet, _manager.AttackSpots[0].position, Quaternion.identity);
            isBox = false;
        }
        if (_time >= frame15+frame20*3 && !isBox && _time < frame20*4)
        {
            Instantiate(_manager.RingEffet, _manager.AttackSpots[1].position, Quaternion.identity);
            isBox = true;
        }
        if (_time >= frame15+frame20*5 && isBox)
        {
            Instantiate(_manager.RingEffet, _manager.AttackSpots[2].position, Quaternion.identity);
            isBox = false;
        }

        if (_time > frame20*8)
        {
            if (_manager.PT1)
            {
                _manager.SetState(FactoryBossState.LAGERATTACK);
                _time = 0;
            }
            if (!_manager.PT1)
            {
                _manager.SetState(FactoryBossState.LAGERATTACK);
                _time = 0;
            }
        }
        //transform.LookAt(_manager.PlayerTransform);

        // 시야에 없으면서 동시에 추적 시간 5초가 넘었을 시에 아이들로 돌아온다?
        // 복귀타이머 -> 복귀
        // 추적상태에서 attack으로 넘어갈 때 2초 후 공격 시작하는 형식으로 그 사이 애니메이션 작동

        //if (!GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS))
        //{
        //    _manager.SetState(FactoryBossState.IDLE);
        //    return;
        //}

        //if (Vector3.Distance(_manager.PlayerTransform.position, transform.position) < _manager.Stat.AttackRange)
        //{
        //    _manager.SetState(FactoryBossState.ATTACK);
        //    return;
        //}

        //_manager.CC.CKMove(_manager.PlayerTransform.position, _manager.Stat);
    }
}
