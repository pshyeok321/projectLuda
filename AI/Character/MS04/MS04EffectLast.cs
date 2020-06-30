using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS04EffectLast : MonoBehaviour
{
    float time = 0;
    float destorytime = 2f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Manager").GetComponentInChildren<DroneSound>().PlayDroneSkillEMPEnd(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time>= destorytime)
        {
            Destroy(gameObject);
        }
    }
}
