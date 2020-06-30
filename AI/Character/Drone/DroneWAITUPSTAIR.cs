using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneWAITUPSTAIR : DroneFSMState
{
    public DroneGOUPSTAIR go;

    //Vector3 listPosition;
    public override void BeginState()
    {
        base.BeginState();
        go = _manager.GetComponent<DroneGOUPSTAIR>();

        //listPosition = new Vector3(go.list[1].transform.position.x, go.list[1].transform.position.y + 1f, go.list[1].transform.position.z);
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        if (transform.position.y + 1f >= go.list[1].transform.position.y)
        {
            transform.position = go.list[1].transform.position;
        }
        if (_manager.PlayerTransform.position.y <= transform.position.y)
        {
            _manager.SetState(DroneState.IDLE);
            return;
        }
    }    

}