using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankGun : TankGun {
    //REFACTORED_1
    public float sensetivityvert = 9f;
	// Use this for initialization
	void Start () {
    }
	void Awake()
    {
    }
    void OnDestroy()
    {
    }
    
	// Update is called once per frame
	public void Update () {
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
        

        float rot = Input.GetAxis("Mouse Y")*sensetivityvert;
        Rot(rot);
        if (Input.GetKey(KeyCode.Space))
            Fire();
        //if (Input.GetMouseButton(0) && gunType == GunType.AutomaticGun)
            //Fire();
    }
}
