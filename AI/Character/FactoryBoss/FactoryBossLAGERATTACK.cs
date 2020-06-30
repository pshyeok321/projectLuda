using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBossLAGERATTACK : FactoryBossFSMState
{
    float delayTime = 10.0f;
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
    private GameObject LE;
    private GameObject ADE;

    Vector3 LagerTrans;
    public override void BeginState()
    {
        base.BeginState();

        startTime = Time.time;
        _playerTransY = 10f;
        //_playerTrans = new Vector3(1, 1, 1);

        LagerTrans = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {    
        _time += Time.deltaTime;
        
        transform.LookAt(_manager.PlayerTransform);

        if (!isLAZER)
        {
            LE = (GameObject) Instantiate(_manager.LagerEffect, LagerTrans, Quaternion.identity);
            Destroy(LE, delayTime);
            isLAZER = true;
        }

        if (_time > delayTime)
        {
            _manager.SetState(FactoryBossState.SLERPATTACK);
            isLAZER = false;
            _time = 0;
        }


        randAttack = Random.Range((int)0, (int)100);

        if (randAttack == 9 && !isRand) {

            //_playerTrans = _manager.PlayerTransform.position;

            t1.position = _manager.PlayerTransform.position;
            _manager._playerTrans = new Vector3(t1.position.x, _playerTransY, t1.position.z);

            ADE = (GameObject) Instantiate(_manager.AttackDelayEffect, _manager._playerTrans, Quaternion.identity);

            Destroy(ADE, 1f);
            isRand = true;
        }
        if (isRand) {
            Instantiate(_manager.SlerpEffect, _manager.SlerpEffect.transform.position, Quaternion.identity);
            isRand = false;
        }
    }
}
