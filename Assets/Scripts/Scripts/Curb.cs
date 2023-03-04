using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curb : MonoBehaviour {
    public float speedScalyar = 100f;
    public Vector3 speedVector;
    public float g = -9.8f;
    public float horSpeed = 0;
    public Gun gun;
	// Use this for initialization
	void Start () {
        Vector3 s = transform.forward*speedScalyar;
        speedVector = Vector3.ClampMagnitude(s, speedScalyar);
	}
	
	// Update is called once per frame
	void Update () {
        speedVector.y += g * Time.deltaTime;
        transform.Translate(speedVector*Time.deltaTime, Space.World);
	}
}
