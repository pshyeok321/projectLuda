using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class FW02Effect : MonoBehaviour
{
    PlayerBehaviour player;
    [Header("스피드값조절")]
    public float speed = 30f;
    [SerializeField]
    FW02FSMManager _manager;
    [SerializeField]
    FW02ATTACK attack;

    public Vector3 pos;

    public Transform startPos;
    float _time = 0;
    float destoryTime = 1.5f;
    public GameObject LastEffect;

    public Vector3 dis;

    public Transform startTrans;

    Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        //startTrans.position;
        rigid = GetComponent<Rigidbody>();

        GameObject.FindGameObjectWithTag("Manager").GetComponentInChildren<DroneSound>().DroneAttackSFX(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //Quaternion rotation = Quaternion.identity;
        //rotation.eulerAngles = new Vector3(0, transform.rotation.y, transform.rotation.z);
        //rotation *= rotation;

        _time += Time.deltaTime;

        
        

        if (_time < 0.1f)
        {
            transform.LookAt(pos);
            
        }
        dis = attack.playerTrans - startPos.position;//startTrans.position;
        //dis.Normalize();
        Quaternion.LookRotation(dis);
        //rigid.velocity = transform.forward * 5f * Time.deltaTime;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (_time > destoryTime)
        {
            Destroy(gameObject);
        }
    }

    public void SetManager(Transform init)
    {

        _manager = init.root.GetComponentInChildren<FW02FSMManager>();
        attack = _manager.GetComponentInChildren<FW02ATTACK>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        pos = attack.playerTrans;
        startPos = attack.FirePos;
        //Vector3.MoveTowards(pos, 5f * Time.deltaTime);
        //GetComponent<Rigidbody>().AddForce(transform.forward * speed);

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            player.PlayerDamaged();
            Destroy(this.gameObject);
            //플레이어한테 맞았을 때의 이펙트
            //Instantiate(gameObject, transform.position, transform.rotation);
        }

        else
        {
            GameObject obj = Instantiate(LastEffect, transform.position, transform.rotation);
            Destroy(obj.gameObject, 2f);
            Destroy(gameObject);            
        }
    }

}

