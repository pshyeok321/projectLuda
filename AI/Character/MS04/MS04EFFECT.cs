using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class MS04EFFECT : MonoBehaviour
{
    public GameObject EMPLoop;
    public GameObject EMPLast;
    [SerializeField]
    float time = 0;
    float starttime = 1.5f;
    float looptime = 1.98f; // 루프 이펙트가 시작되는 시간
    //float destorytime = 3.98f; // 마지막 이펙트가 사라지는 시간
    //float destorytime = 1.98f; // 마지막 이펙트가 사라지는 시간
    float destorytime = 0.98f; // 마지막 이펙트가 사라지는 시간
    float hackingtime = 5f; // 해킹당해있는 시간
    bool isloop;
    bool islast;

    bool isExit; // 플레이어가 해킹당한곳으로 부터 벗어났을 때

    [SerializeField]
    float time2 = 0; // 시간함수
    [SerializeField]
    private PlayerController playerController;

    SphereCollider sphere;
    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sphere = GetComponent<SphereCollider>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        sphere.enabled = false;
        GameObject.FindGameObjectWithTag("Manager").GetComponentInChildren<DroneSound>().PlayDroneSkillEMPStart(this.gameObject);

    }
    private void OnDisable()
    {
        playerController._IsLock = false;
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time>= starttime)
        {
            sphere.enabled = true;
        }
        if(time >+ looptime && !isloop)
        {
            isloop = true;
            Instantiate(EMPLoop, transform.position, Quaternion.identity);
            
        }
        if (time >= looptime+destorytime && !islast)
        {
            islast = true;
            Instantiate(EMPLast, transform.position, Quaternion.identity);
        }
        if(time>= looptime + destorytime)
        {
            
            sphere.enabled = false;
        }

        if(time >= destorytime + looptime + hackingtime)
        {
            Destroy(gameObject);
        }

        if (isExit)
        {
            time2 += Time.deltaTime;            
        }
        if (time2 >= hackingtime)
        {
            playerController._IsLock = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //player skill locked
            playerController._IsLock = true;

            time2 = 0;
            isExit = false;          
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isExit = true;            
        }
    }
}
