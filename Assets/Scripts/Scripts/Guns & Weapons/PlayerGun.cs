using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCurb(0);
            MainManager.userInterfaseManager.SwitchCurb();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchCurb(1);
            MainManager.userInterfaseManager.SwitchCurb();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchCurb(2);
            MainManager.userInterfaseManager.SwitchCurb();
        }

        if (Input.GetKey(KeyCode.Space))
            Fire();
    }
}
