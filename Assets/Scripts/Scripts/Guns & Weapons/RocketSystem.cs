using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSystem : Gun
{
    public float maxDistance;
    public bool redIsEnemy = true;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.T))
        {
            List<GameObject> enemies = MainManager.buttleManager.allred;
            if (!redIsEnemy)
                enemies = MainManager.buttleManager.allblue;
            foreach (GameObject enemy in enemies)
            {

                if ((enemy.transform.position - transform.position).magnitude <= maxDistance && enemy.GetComponent<ModuleController>().alive)
                {
                    Debug.Log("RocketSystem: Rocket releaese initiated");
                    ReleaseRocket(enemy.transform);

                }
            }
        }
        
    }
    private void ReleaseRocket(Transform target)
    {
        Curb curb = Fire();
        if(curb!=null && curb is Rocket)
        {
            Debug.Log("RocketSystem: Rocket releaesd towards: " + target.name);
            Rocket rocket = curb as Rocket;
            rocket.target = target;
            rocket.transform.LookAt(target);
        }
        

    }
}
