using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankController : TankController
{
    public float verticalRotationSensetivity = 1f;
    public float horizontalRotationSensetivity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Gas(Input.GetAxis("Vertical"));
        Rotate(Input.GetAxis("Horizontal"));
        RotateTower(Input.GetAxis("Mouse X"));
        RotateGun(Input.GetAxis("Mouse Y"));
        if (Input.GetKey(KeyCode.Space))
            Fire();
    }
}
