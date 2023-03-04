using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {
    public static UIManager userInterfaseManager;
    public static MusicManager musicManager;
    public static ButtleResult buttleResult;
    public static ButtleManager buttleManager;
    public static TankLibrary tankLibrary;
    public static RoadManager roadManager;
    //public static ButtleManager buttleManager;
    // Use this for initialization
    void Start () {
        userInterfaseManager = GetComponent<UIManager>();
        musicManager = GetComponent<MusicManager>();
        buttleResult = GetComponent<ButtleResult>();
        tankLibrary = GetComponent<TankLibrary>();
        buttleManager = GetComponent<ButtleManager>();
        roadManager = GetComponent<RoadManager>();
        //buttleManager = GetComponent<ButtleManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
