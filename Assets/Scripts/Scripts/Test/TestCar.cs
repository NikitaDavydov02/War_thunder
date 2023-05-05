using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCar : MonoBehaviour
{
    private float verInput;
    private float horInput;
    public float power = 10;
    [SerializeField]
    private List<Wheel> wheels;
    public float suspensionDistance = 0.2f;
    public float spring = 100f;
    public float force = 100f;
    private void Start()
    {
        foreach(Wheel wheel in wheels)
        {
            wheel.wcol.suspensionDistance = suspensionDistance;
            //wheel.GetComponent<WheelCollider>().GetComponent<JointSpring>();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddRelativeForce(0, 0, -force);
        }
        ProcessInput();
       // Vector3 diff =transform.position - gravityTarget.position; 
    }

    void FixedUpdate()
    {
        ProcessForces();
        //ProcessGravity();
    }
    void ProcessInput()
    {
       verInput = Input.GetAxis("Vertical"); 
       horInput = Input.GetAxis("Horizontal");
    }
    void ProcessForces()
    {
        //Vector3 force = new Vector3(0f, 0f, verInput power); //rb.AddRelativeForce(force);
        //Vector3 rforce = new Vector3(0f, horInput" torque, ef); //rb.AddRelative Torque (rforce);
        foreach (Wheel w in wheels)
        {
            w.Steer(horInput);
            w.Accelerate(verInput*power);
            w.UpdatePosition();
        }
            
    }
    void ProcessGravity()
    {
            //Vector3 diff transform.position rb.AddForce(-diff.normalized
    
            //gravityTarget.position;
            //gravity(rb.mass));
    }
}
