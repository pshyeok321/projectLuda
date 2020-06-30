using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneATTACKCANCLE : DroneFSMState
{


    [SerializeField]
    [TextArea(2, 5)]
    string comment = "";

    [SerializeField]
    private float idleTime = 1.0f;
    private float time = 0.0f;

    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {
        time += Time.deltaTime;

        //transform.LookAt(_manager.PlayerTransform);
        if (time >= idleTime)
        {
            time = 0;
            Debug.Log("ATTACKCANCLE"); // 이쪽에 애니메이션 한번만 작동.
            _manager.SetState(DroneState.PATROL);
        }
    }
}