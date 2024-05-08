using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnicsManager : MonoBehaviour {
    //REFACTORED
    private TechnicsLibrary technicsLibrary;
	// Use this for initialization
    private static GameObject currentModel;
	void Start () {
        technicsLibrary = this.gameObject.GetComponent<TechnicsLibrary>();
       // GameObject modelPrefab = technicsLibrary.GetRandomHumanTank();
       // currentModel = Instantiate(modelPrefab) as GameObject;
       // currentModel.transform.position = new Vector3(0, 0, 0);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject InstantiateTechnic(string name)
    {
        Destroy(currentModel);
        GameObject modelPrefab = technicsLibrary.GetHumanTechnicByName(name);
        if (modelPrefab == null)
            return null;
        currentModel = Instantiate(modelPrefab) as GameObject;
        currentModel.transform.position = new Vector3(0, 1f, 0);
        return currentModel;
    }
}
