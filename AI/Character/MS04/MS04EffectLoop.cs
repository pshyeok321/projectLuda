using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS04EffectLoop : MonoBehaviour
{
    float time = 0;
    //float looptime = 3.98f;
    float looptime = 0.98f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Manager").GetComponentInChildren<DroneSound>().PlayDroneSkillEMPLoop(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time >= looptime)
        {
            GameObject.FindGameObjectWithTag("Manager").GetComponentInChildren<DroneSound>().StopDroneSkillEMPLoop(this.gameObject);
            Destroy(gameObject);

        }
    }
}
