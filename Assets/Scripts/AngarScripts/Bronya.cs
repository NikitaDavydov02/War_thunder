using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bronya : MonoBehaviour {
    public float bronya;
    public bool prioritet = false;
    public bool list = false;
    [SerializeField]
    Material usualTexture;
    [SerializeField]
    Material bronyaTexture;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (MainManager.buttleManager != null)
            return;
        if (Input.GetKeyDown(KeyCode.B))
        {
            GetComponent<Renderer>().material = bronyaTexture;
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            GetComponent<Renderer>().material = usualTexture;
        }
    }

    void OnTriggerIn(Collider other)
    {
        if (bronya == 0)
        {
            Debug.Log("Oi v meny popali!");
        }
        Detonator detonator = other.gameObject.GetComponent<Detonator>();
        if (detonator != null)
        {
            if (detonator.probitie < bronya)
            {
                Destroy(detonator.transform.parent.gameObject);
                Debug.Log("Ne voshel "+gameObject.name);
            }
            else
            {
                detonator.probitie -= bronya;
                Debug.Log("Voshel " + gameObject.name);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (list)
            return;
        Detonator detonator = other.gameObject.GetComponent<Detonator>();
        if (detonator != null)
        {
            Destroy(detonator.transform.parent.gameObject);
        }
    }
}
