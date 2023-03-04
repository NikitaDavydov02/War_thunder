using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oskolok : MonoBehaviour {
    public Vector3 rotation;
    public float speed = 1f;
    float timeOfLife = 0;
    public int damage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // transform.eulerAngles = rotation;
        //RaycastHit hit;
        //if(Physics.Raycast(new Ray(transform.position,Vector3.forward),out hit))
        //{
        //    //if(hit.Hi)
        //}
        // transform.Translate(0,0, speed * Time.deltaTime);
        transform.position += (Vector3.forward*0.0001f*Time.deltaTime);
        timeOfLife += Time.deltaTime;
        if (timeOfLife > 2f)
            Destroy(this.gameObject);
	}
}
