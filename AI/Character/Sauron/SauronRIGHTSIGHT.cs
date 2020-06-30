using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauronRIGHTSIGHT : SauronFSMState
{
    private float _sightTime = 3.0f;
    public float _time = 0.0f;
    //GameObject obj_A;
    private float _animTime = 0.8f;
    public override void BeginState()
    {
        //obj_A = ObjPooling._objPool.GetPooledObject_A();


        base.BeginState();

    }


    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (_time > _animTime)
        {
            _manager.SightLight[0].SetActive(true);
            _manager.Sight.transform.gameObject.SetActive(true);
        }

        if (GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS))
        {
            _manager.SetState(SauronState.ATTACK);

            _manager.SightLight[0].SetActive(false);

            _manager.SightLight[2].SetActive(true);            
        }

        if (_time > _sightTime)
        {
            _manager.SetState(SauronState.IDLE);
            _manager.Sight.transform.Rotate(Vector3.left * 180);
            _time = 0;
            _manager.SightLight[0].SetActive(false);
            
        }




    }


}
