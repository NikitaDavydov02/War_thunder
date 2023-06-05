using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSystem : Gun
{
    public float maxDistance;
    


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        foreach(GameObject enemy in MainManager.buttleManager.allred)
        {
            if((enemy.transform.position - transform.position).magnitude <= maxDistance)
            {
                ReleaseRocket(enemy.transform);
                
            }
        }
    }
    private void ReleaseRocket(Transform target)
    {
        Curb curb = Fire();
        if(curb!=null && curb is Rocket)
        {
            Debug.Log("Rocket releaesd");
            Rocket rocket = curb as Rocket;
            rocket.target = target;
        }
        

    }
}
