using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBossSLERPATTACK : FactoryBossFSMState
{
    [SerializeField]
    float delayTime = 7.0f;
    public float _time = 0.0f;
    private bool isLAZER = false;

    [SerializeField]
    int randAttack;
    bool isRand;
    public Transform t1;
    float _waitTIme = 1f;
    float _timer = 0;
    float startTime;
    //public Vector3 _playerTrans;
    float _playerTransY;

    float turnspeed = 4;

    private GameObject ADE;
    public override void BeginState()
    {
        base.BeginState();

        startTime = Time.time;
        _playerTransY = 10f;
        //_playerTrans = new Vector3(1, 1, 1);
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {

        //transform.LookAt(_manager.PlayerTransform);

        Vector3 direction = _manager.PlayerTransform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, turnspeed * Time.deltaTime);

        _time += Time.deltaTime;
        if (_time > delayTime)
        {
            _manager.SetState(FactoryBossState.IDLE);
            isLAZER = false;
            _time = 0;
        }


        randAttack = Random.Range((int)0, (int)20);

        if (randAttack == 9 && !isRand)
        {

            float randX = Random.Range(-10, 11);
            float randZ = Random.Range(-10, 11);

            t1.position = _manager.PlayerTransform.position;
            _manager._playerTrans = new Vector3(t1.position.x + randX, _playerTransY, t1.position.z + randZ);

            ADE = (GameObject)Instantiate(_manager.AttackDelayEffect, _manager._playerTrans, Quaternion.identity);
            Destroy(ADE, 1f);
            isRand = true;
        }
        if (isRand)
        {
            Instantiate(_manager.SlerpEffect, _manager.SlerpEffect.transform.position, Quaternion.identity);
            isRand = false;
        }
    }
}
