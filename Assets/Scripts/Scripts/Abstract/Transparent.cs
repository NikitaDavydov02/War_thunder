using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparent : MonoBehaviour {
    //Refactored
    private Material usualTexture;
    private Material transparentTexture;
    // Use this for initialization
    void Start () {
        usualTexture = gameObject.GetComponent<MeshRenderer>().material;
        transparentTexture = Resources.Load("Materials/Transparent") as Material;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.O))
        {
            GetComponent<Renderer>().material = transparentTexture;
        }
        if (Input.GetKeyUp(KeyCode.O))
        {
            GetComponent<Renderer>().material = usualTexture;
        }
    }
}
