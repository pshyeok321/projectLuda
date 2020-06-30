using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class Balls : MonoBehaviour
{
    float speed = 30f;
    PlayerBehaviour player;
    public Vector3 pos;
    public MP01FSMManager _manager;
    MP01ATTACK attack;


    private void Start()
    {
        Debug.Log("SoundPlay");
        GameObject.FindGameObjectWithTag("Manager").
            GetComponentInChildren<DroneSound>().DroneAttackSFX(this.gameObject);
    }

    void Update()
    {
        //if(_manager != null)
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);

        transform.LookAt(pos);
    }

    public void SetManager(Transform init)
    {
        _manager = init.root.GetComponentInChildren<MP01FSMManager>();
        attack = _manager.GetComponentInChildren<MP01ATTACK>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        pos = attack.playerTrans;
        //GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        //GetComponent<Rigidbody>().AddForce(pos * speed);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            player.PlayerDamaged();
            Destroy(gameObject);
        }
    }
}
