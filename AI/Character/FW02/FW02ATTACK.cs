using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FW02ATTACK : FW02FSMState
{
    public Vector3 playerTrans;
    public Vector3 TransH;
    bool isShoot = false;
    float _time = 0;
    float shootTime = 1.033333f;
    float attackTime = 2.666f;

    public Transform FirePos;
    bool isStart;

    GameObject eftFirst, eft;
    public Vector3 MyTrans;

    public Vector3 playerT;
    public override void BeginState()
    {
        base.BeginState();
        //GameLib.SimpleDamageProcess(transform, _manager.Stat.AttackRange,
        //          "Player", _manager.Stat);
      
    }

    public override void EndState()
    {
        base.EndState();
        isShoot = false;
        isStart = false;
    }

    private void Update()
    {
        _time += Time.deltaTime;
        MyTrans = new Vector3(transform.position.x, _manager.PlayerTransform.position.y + 0.8f, transform.position.z);

        // Player의 위치값을 Y축 고정 후 발사
        playerTrans = new Vector3(_manager.PlayerTransform.position.x, _manager.PlayerTransform.position.y + 1.3f, _manager.PlayerTransform.position.z);
        TransH = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);

        playerT = new Vector3(_manager.PlayerTransform.position.x, _manager.PlayerTransform.position.y, _manager.PlayerTransform.position.z);

        transform.LookAt(playerT);

        //eftFirst.transform.position = FirePos.position;

        // 2.6초 경과 시 Attack 애니메이션 종료와 함꼐 Chase 상태로 이동
        if (_time > attackTime)
        {
            _manager.SetState(FW02State.CHASE);
            _time = 0;
            return;
        }
        // 단 한번만 첫번째 이펙트 생성
            if (!isStart)
            {
                eftFirst = Instantiate(_manager.Effect_First, FirePos.position, FirePos.rotation);
                Destroy(eftFirst, 2f);
                isStart = true;
            }
            eftFirst.GetComponent<FW02EffectFirst>().SetManager(this.transform);
            if (_time > shootTime && !isShoot)
            {
                eft = Instantiate(_manager.Effect_Shoot, FirePos.position, FirePos.rotation);
                isShoot = true;

            }
            eft.GetComponent<FW02Effect>().SetManager(this.transform);
        //if (!GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS))
        //{
        //    _manager.SetState(FW02State.IDLE);

        //_manager.SightLight[2].SetActive(false);

        //    return;
        //}
        //transform.LookAt(_manager.PlayerTransform);

        //Debug.Log("ATtack");

        //Ray ray = new Ray(MyTrans, transform.forward);
        //Ray ray = new Ray(transform.position, MyTrans);
        Debug.DrawLine(TransH, playerTrans, Color.red, 1);
        //Debug.DrawRay(ray.origin, ray.direction * 50, Color.red);
        RaycastHit hitInfo;
        if(Physics.Linecast(TransH, playerTrans, out hitInfo, 1<<LayerMask.NameToLayer("Wall")))
        {
            _manager.isWall = true;
            _manager.isPlayer = false;
            _manager.SetState(FW02State.CHASE);
            return;
        }
        //if (Physics.Raycast(ray, out hitInfo, 20, 1 << LayerMask.NameToLayer("Wall")))
        //{
        //    _manager.isWall = true;
        //    _manager.isPlayer = false;
        //    _manager.SetState(FW02State.CHASE);
        //    return;
        //}
    }

    public void AttackCheck()
    {        
       
        Debug.Log("Attack");

        
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Effect")
    //    {
    //        _manager.SetState(FW02State.INCAP);
    //    }
    //}
}
