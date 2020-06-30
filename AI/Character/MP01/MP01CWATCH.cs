using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MP01CWATCH : MP01FSMState
{

    float RunTime = 1f;
    float UITime = 3f;
    [SerializeField]
    float _time = 0;   
    
    public Vector3 playerTrans;

    public bool last1, last2;


    public override void BeginState()
    {
        base.BeginState();

        //playerT1 = new Vector3(1, 1, 1);
        //playerT2 = new Vector3(2, 2, 2);
        //playerT3 = new Vector3(3, 3, 3);

        //_manager.Emotion2.SetActive(true);
        _manager._UIs[2].gameObject.SetActive(true);
        
    }


    public override void EndState()
    {
        base.EndState();

        _time = 0;
        last1 = false;
        last2 = false;
        
        
    }

    private void Update()
    {
        playerTrans = new Vector3(_manager.PlayerTransform.position.x, transform.position.y, _manager.PlayerTransform.position.z);
        transform.LookAt(playerTrans);

        _time += Time.deltaTime;

        //_manager._UIs[2].fillAmount = _time;

        //if (_manager._UIs[2].fillAmount >= 1)
        //    _manager._UIs[2].fillAmount = 1;

        //1초 동안 플레이어 도망갈 시간 줌.
        if (_time >= RunTime)
        {
            _manager.Emotion1.SetActive(false);
            _manager.SetState(MP01State.CHASE);
            return;
        }
    }
}
