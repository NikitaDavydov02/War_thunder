using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : Tower {
    //REFACTORED_1
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!controller.alive)
            return;
        float rotY = Input.GetAxis("Mouse X");
        Rotate(rotY);
    }
    void Awake()
    {
    }
    void OnDestroy()
    {
    }

}
