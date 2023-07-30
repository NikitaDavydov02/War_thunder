using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bronya : MonoBehaviour {
    //REFACTORED
    [SerializeField]
    public float bronyaThickness;
    //public bool list { get; private set; }= false;
    private Material usualTexture;
    //[SerializeField]
    private Material bronyaTexture;
    // Use this for initialization
    void Start () {
        usualTexture = gameObject.GetComponent<MeshRenderer>().material;
        if (bronyaThickness < 50)
            bronyaTexture = Resources.Load("Materials/ThickArmor") as Material;
        else if(bronyaThickness < 100)
            bronyaTexture = Resources.Load("Materials/MiddleArmor") as Material;
        else
        {
            bronyaTexture = Resources.Load("Materials/HeavyArmor") as Material;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (MainManager.buttleManager != null)
            return;
        /*if (Input.GetKeyDown(KeyCode.B))
        {
            GetComponent<Renderer>().material = bronyaTexture;
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            GetComponent<Renderer>().material = usualTexture;
        }*/
    }
    public void ShowThickness(bool show)
    {
        if (show)
        {
            GetComponent<Renderer>().material = bronyaTexture;
        }
        else
        {
            GetComponent<Renderer>().material = usualTexture;
        }
    }

}
