using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpuse : MonoBehaviour {
    [SerializeField]
    Rigidbody rigidbody;
    [SerializeField]
    public float speed;
    public float rotSpeed;
    [SerializeField]
    private ModuleController controller;
    public float maxSpeed = 10f;
    public float currentRotSpeed;
    public float maxRotSpeed = 5f;
    
    //public float currentSpeed;
    //private Vector3 lastPos;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //currentSpeed = Mathf.Abs((transform.position - lastPos).magnitude) / Time.deltaTime;
        //Debug.Log("LP: " + lastPos);
        //Debug.Log("CP: " + transform.position);
        //Debug.Log(currentSpeed);
        //lastPos = transform.position;
    }
    public void Move(Vector3 movement)
    {
        if (!controller.alive||controller.TimeOfPogar>0 || !controller.canMove)
            return;
        rigidbody.AddForce(movement);
    }
    public void Turn(float turn)
    {
        if (!controller.alive || currentRotSpeed > maxRotSpeed|| !controller.canMove)
            return;
        rigidbody.AddTorque(transform.parent.transform.up * rotSpeed * turn);
    }
}
