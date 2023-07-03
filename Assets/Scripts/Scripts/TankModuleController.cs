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
    protected override void Start()
    {
        base.Start();
        foreach (Module module in modules)
        {
            module.ModuleExplode += Explode;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    private void Explode(object sender, EventArgs args)
    {
        if(tower.gameObject.GetComponent<Rigidbody>()==null)
            tower.gameObject.AddComponent<Rigidbody>();
        Rigidbody r = tower.gameObject.GetComponent<Rigidbody>();
        r.mass = 20000;
        r.AddExplosionForce(3000000, tower.transform.position + new Vector3(0, -1, 0), 10);
        foreach (Module m in modules)
        {
            if (m.nameOfModule == ModuleType.МеханизмПоворотаБашни)
                m.InstantiateFire();
            if (m.nameOfModule == ModuleType.Бензобак)
                m.InstantiateFire();
            if (m.nameOfModule == ModuleType.Боеукладка)
                m.InstantiateFire();
        }
        GameObject explosion = Instantiate(Resources.Load("Prefabs/Explosion") as GameObject);
        explosion.transform.position = transform.position;

        //Debug.Log("Tank controller call die after explosion");
        Die();
    }
}
