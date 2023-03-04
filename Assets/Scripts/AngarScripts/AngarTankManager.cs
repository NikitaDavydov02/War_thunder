using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngarTankManager : MonoBehaviour {
    public TankLibrary tankLibrary;
	// Use this for initialization
    public static GameObject currentModel;
	void Start () {
        tankLibrary = this.gameObject.GetComponent<TankLibrary>();
        //GameObject modelPrefab = tankLibrary.GetHumanTankByName("Т-34");
        GameObject modelPrefab = tankLibrary.GetHumanTankByName("Т-34");
        currentModel = Instantiate(modelPrefab) as GameObject;
        currentModel.transform.position = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void InstantiateTank(string name)
    {
        Destroy(currentModel);
        GameObject modelPrefab = tankLibrary.GetHumanTankByName(name);
        currentModel = Instantiate(modelPrefab) as GameObject;
        currentModel.transform.position = new Vector3(0, 0, 0);
    }
}
