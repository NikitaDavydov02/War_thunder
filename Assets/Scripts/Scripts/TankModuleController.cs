using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TankModuleController : ModuleController
{
    [SerializeField]
    private GameObject tower;
    [SerializeField]
    private GameObject gun;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        foreach (Module module in modules)
        {
            module.ModuleExplode += Explode;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Explode(object sender, EventArgs args)
    {
        Debug.Log("Tank exploded");
        tower.gameObject.AddComponent<Rigidbody>();
        Rigidbody r = tower.gameObject.GetComponent<Rigidbody>();
        r.mass = 20000;
        r.AddExplosionForce(3000000, tower.transform.position + new Vector3(0, -1, 0), 10);
        GameObject fire = Instantiate(Resources.Load("Prefabs/Fire") as GameObject);
        foreach (Module m in modules)
            if (m.nameOfModule == ModuleType.МеханизмПоворотаБашни)
                fire.transform.position = m.gameObject.transform.position;
        Die();
    }
}
