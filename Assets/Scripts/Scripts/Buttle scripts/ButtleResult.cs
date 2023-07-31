using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleResult {
    //REFACTORED
    public int frags { get; private set; } = 0;
    public int silver { get; private set; }
    public int expirience { get; private set; }
    public int shoots { get; private set; }
    public bool Win { get; set; }
    // Use this for initialization
    public ButtleResult() {
        //DontDestroyOnLoad(gameObject);
        silver = Random.Range(1000, 2000);
        expirience = Random.Range(100, 200);
        shoots = 0;
        frags = 0;
    }
	
	// Update is called once per frame
	
    public void AddShoot()
    {
        shoots++;
    }
    public void AddFrag()
    {
        frags++;
        silver += Random.Range(1000, 5000);
        expirience += Random.Range(300, 500);
    }
}
