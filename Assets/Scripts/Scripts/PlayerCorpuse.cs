using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCorpuse : Corpuse {
    //REFACTORED_1
    public float inputSensentivity = 1300000;
    public float rotationalSensetivity = 2500000;

    private float lastAngleY;
    void Start () {
        base.Start();
    }

    // Update is called once per frame
    void Update () {
        if (!controller.alive)
            return;

        float aheadInput = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(0, 0, aheadInput);
        movement = transform.parent.transform.TransformDirection(movement);
        float turn = Input.GetAxis("Horizontal");

        //Move(movement);
        Move(aheadInput);

        Turn(turn);
        
    }
}
