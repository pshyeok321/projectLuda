using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class Lager : MonoBehaviour
{
    PlayerInfo playerinfo;
    // Start is called before the first frame update
    void Start()
    {
        playerinfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (playerinfo._hp > 0)
                playerinfo._hp--;
            else
                return;
        }
    }
}
