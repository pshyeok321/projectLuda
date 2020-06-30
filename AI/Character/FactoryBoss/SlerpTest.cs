using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class SlerpTest : MonoBehaviour
{
    public Transform starPos;
    public Transform endPos;
    public float journeyTime = 1.0F;
    private float startTime;
    [SerializeField]
    private float speed;


    float _time;
    float _destorytime = 5f;
    public Vector3 vect;
    PlayerInfo playerinfo;
    [SerializeField]
    private FactoryBossFSMManager _manager;
    [SerializeField]
    private FactoryBossLAGERATTACK boss;

    private void Awake()
    {
        _manager = FactoryBossFSMManager.FindFactoryBossFSM();
        boss = _manager.GetComponent<FactoryBossLAGERATTACK>();
    }

    void Start() {
        startTime = Time.time;
        speed = 0.8f;

        playerinfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        //vect = boss._playerTrans;
        vect = _manager._playerTrans;

        
    }

    void Update() {
        if(transform.position.y < _manager.PlayerTransform.position.y)
        {
            Destroy(gameObject);
        }
        _time += Time.deltaTime;
        if (_time >= _destorytime)
        {
            Destroy(gameObject);
        }        
        //Vector3 center = (starPos.position + endPos.position) * 0.5F;
        //center -= new Vector3(0, 1, 0);

        //Vector3 riseRelCenter = starPos.position - center;
        //Vector3 setRelCenter = endPos.position - center;

        //float fracComplete = (Time.time - startTime) / journeyTime * speed;
        //transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete * speed);
        //transform.position += center;

        Vector3 cent = (starPos.position + vect) * 0.5f;
        cent -= new Vector3(0, 1, 0);
        Vector3 rise = starPos.position - cent;
        Vector3 set = vect - cent;

        float frac = (Time.time - startTime) / journeyTime * speed;
        transform.position = Vector3.Slerp(rise, set, frac * speed);
        transform.position += cent;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Destroy(gameObject);
            if (playerinfo._hp > 0)
                playerinfo._hp--;
            else
                return;
        }
        
    }
}
