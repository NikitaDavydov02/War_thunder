using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlaneController : PlaneController
{
    public float generalLevelChangingSpeed = 1f;
    public float eleronSensitivity;
    public float gasSensitivity;
    public float heigtSensitivity;
    public float horizontalSensitivity;
    public bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (!active)
            return;
        /*if (Input.GetKeyDown(KeyCode.A))
            Eleron(true);
        if (Input.GetKeyUp(KeyCode.A))
            Eleron(false);
        if (Input.GetKeyDown(KeyCode.D))
            Eleron(false);
        if (Input.GetKeyUp(KeyCode.D))
            Eleron(true);*/
        if (Input.GetKeyDown(KeyCode.A))
           Eleron(1);
       if (Input.GetKeyUp(KeyCode.A))
           Eleron(0);
       if (Input.GetKeyDown(KeyCode.D))
           Eleron(-1);
       if (Input.GetKeyUp(KeyCode.D))
           Eleron(0);

        if (Input.GetKey(KeyCode.W))
            generalLevel += generalLevelChangingSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            generalLevel -= generalLevelChangingSpeed * Time.deltaTime;
        float verticalInput = Input.GetAxis("Mouse Y") * Time.deltaTime * heigtSensitivity;
        HeightController(verticalInput);
        float horInput = -Input.GetAxis("Mouse X") * Time.deltaTime * horizontalSensitivity;
        HorizontalController(horInput);

        if (Input.GetKeyDown(KeyCode.R))
            ReleaseBombs();
    }
}
