using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP01ATTACK : MP01FSMState
{
    public Vector3 playerTrans;
    bool isShoot = false;
    float _time = 0;
    float attackTime = 1f;

    GameObject eft;
    public override void BeginState()
    {
        base.BeginState();
        //GameLib.SimpleDamageProcess(transform, _manager.Stat.AttackRange,
        //          "Player", _manager.Stat);

        //playerTrans = new Vector3(_manager.PlayerTransform.position.x, transform.position.y, _manager.PlayerTransform.position.z);
    }

    public override void EndState()
    {
        base.EndState();
        isShoot = false;
    }

    private void Update()
    {
        playerTrans = new Vector3(_manager.PlayerTransform.position.x, transform.position.y, _manager.PlayerTransform.position.z);
        transform.LookAt(playerTrans);
        _time += Time.deltaTime;
        if(_time>attackTime)
        {
            _manager.SetState(MP01State.IDLE);
            _time = 0;
            return;
        }
        //if (!GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS))
        //{
        //    _manager.SetState(MP01State.IDLE);
        if (!isShoot)
        {
            //GameObject eft = Instantiate(_manager.AttackEffect, transform.position, Quaternion.LookRotation(playerTrans));
            eft = Instantiate(_manager.AttackEffect, transform.position, transform.rotation);
            isShoot = true;
            Destroy(eft.gameObject, 5f);

        }
        eft.GetComponent<Balls>().SetManager(this.transform);

        _manager.SightLight[0].SetActive(false);
        _manager.SightLight[1].SetActive(false);
        //_manager.SightLight[2].SetActive(false);

        //    return;
        //}
        //transform.LookAt(_manager.PlayerTransform);

        //Debug.Log("ATtack");
    }
    
    public void AttackCheck()
    {        
       
        Debug.Log("Attack");

        
    }
}
