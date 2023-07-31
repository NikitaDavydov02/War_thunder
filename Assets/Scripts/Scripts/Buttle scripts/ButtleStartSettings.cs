using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleStartSettings : MonoBehaviour {
	//REFACTORED_1
	
    public string playerTechnicName = "";
	public ButtleType buttleType = ButtleType.AgainstBots;
	public Regime regime = Regime.OnePoint;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
