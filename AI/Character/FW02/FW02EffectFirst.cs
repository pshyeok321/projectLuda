using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FW02EffectFirst : MonoBehaviour
{
    [SerializeField]
    FW02FSMManager _manager;
    [SerializeField]
    FW02ATTACK attack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = attack.FirePos.position;
    }
    public void SetManager(Transform init)
    {
        _manager = init.root.GetComponentInChildren<FW02FSMManager>();
        attack = _manager.GetComponentInChildren<FW02ATTACK>();
    }
}
