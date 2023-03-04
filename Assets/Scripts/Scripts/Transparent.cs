using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparent : MonoBehaviour {
    [SerializeField]
    Material usualTexture;
    [SerializeField]
    Material transparentTexture;
    // Use this for initialization
    void Start () {
		
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
