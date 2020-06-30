using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauronSIGHT : SauronFSMState
{
    private float _sightTime = 3.0f;
    private float _time = 0.0f;
    //GameObject obj_A;

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

        if ((_manager._sightCount % 2) == 1)
        {
            _manager.SightLight[0].SetActive(true);
        }
        if ((_manager._sightCount % 2) == 0)
        {
            _manager.SightLight[1].SetActive(true);            
        }

        if (GameLib.DetectCharacter2(_manager.Sight, _manager.PlayerCS))
        {
            _manager.SetState(SauronState.ATTACK);

            if ((_manager._sightCount %2) ==  1)
            {
                _manager.SightLight[0].SetActive(false);

                _manager.SightLight[2].SetActive(true);
            }
            if ((_manager._sightCount %2) == 0)
            {
                _manager.SightLight[1].SetActive(false);

                _manager.SightLight[3].SetActive(true);
            }
        }

        _time += Time.deltaTime;
        if(_time > _sightTime)
        {
            _manager.SetState(SauronState.IDLE);
            _manager.Sight.transform.Rotate(Vector3.left * 180);
            _time = 0;

            if ((_manager._sightCount % 2)== 1)
            {
                _manager.SightLight[0].SetActive(false);
            }
            if ((_manager._sightCount % 2)== 0)
            {
                _manager.SightLight[1].SetActive(false);
            }
        }




    }


}
