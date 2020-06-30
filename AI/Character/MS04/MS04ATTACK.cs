using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS04ATTACK : MS04FSMState
{
    [Header("Attack->Idle 전환 시간")]
    public float _waitTime = 3f;

    float _time = 0;
    Vector3 Effecttransform;
    float f;



    public override void BeginState()
    {
        Effecttransform = new Vector3(transform.position.x, transform.position.y+0.1f, transform.position.z);
        base.BeginState();
        Instantiate(_manager.EMP, Effecttransform, Quaternion.identity); // 공격 위치 도착시 일단 시작 effect 생성.

        transform.GetComponentInChildren<Renderer>().material.shader = Shader.Find("Custom/EMP_light_Range");
        f = transform.GetComponentInChildren<Renderer>().material.GetFloat("_RG");
        //Debug.Log("_RG의 값" + f);
    }

    public override void EndState()
    {
        base.EndState();
        transform.GetComponentInChildren<Renderer>().material.SetFloat("_RG", 0.01f);

    }

    private void Update()
    {
        _time += Time.deltaTime;
        //1초동안 0.01->7.01이 되야함. 1초동안 7올라야하니까 7배 해주면 됨.
        if(_time>=0.3f)
            transform.GetComponentInChildren<Renderer>().material.SetFloat("_RG", (_time-0.3f) * 7);

        if (_time >= _waitTime)
        {
            _manager.SetState(MS04State.IDLE);
            _time = 0;            
            return;
        }      
        //transform.LookAt(_manager.PlayerTransform);

    }
    
    public void AttackCheck()
    {        
        GameLib.SimpleDamageProcess(transform, _manager.Stat.AttackRange,
                   "Player", _manager.Stat);
        Debug.Log("EMPAttack");
        
        
    }
}
