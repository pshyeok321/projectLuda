﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS04DEAD : MS04FSMState
{
    public override void BeginState()
    {
        base.BeginState();
        Destroy(gameObject, _manager.MyStatData.AutoDestroyTime);
    }

    public override void EndState()
    {
        base.EndState();
    }

    private void Update()
    {

    }
}
