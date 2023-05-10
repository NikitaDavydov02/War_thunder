using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneModuleController : ModuleController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        int diedCrew = 0;
        foreach (Module ec in crew)
        {
            if (ec.state == ModuleStates.Destroed)
            {
                diedCrew++;
                if (ec.nameOfModule == ModuleType.Пилот)
                {
                    canMove = false;
                    canFire = false;
                    canReloadGun = false;
                }
            }
        }
    }
}
