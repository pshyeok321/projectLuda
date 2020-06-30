using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class MS04EffectTwo : MonoBehaviour
{
    float time = 0f;
    float hackingtime = 5f;

    bool isExit;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isExit)
        {
            time += Time.deltaTime;
        }
        if (time >= hackingtime)
        {
            playerController._IsLock = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //player skill locked
            playerController._IsLock = true;

            time = 0;
            isExit = false;            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isExit = true;
        }
    }
}
