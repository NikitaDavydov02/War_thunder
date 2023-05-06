using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    // Start is called before the first frame update
    public bool powered = false;
    public bool rotatable = false;
    public float maxAngle = 90f;
    public float offset = 0f;
    private float turnAngle;
    public WheelCollider wcol;
    private Transform wmesh;
    public bool left = true;
    
    private void Start()
    {
        //wcol = GetComponentInChildren<WheelCollider>();
        Debug.Log(gameObject.name);
        wmesh = transform.Find("mesh_Wheel");
    }
    public void Steer(float steerInput)
    {
        if (!rotatable)
            return;
        turnAngle = steerInput * maxAngle + offset;
        wcol.steerAngle = turnAngle;
    }

    public void Accelerate(float powerInput)
    {
        if (powered) wcol.motorTorque = powerInput;
        else wcol.brakeTorque = 0;
    }
    public void UpdatePosition()
    {
        Vector3 pos = transform.position;
        Quaternion rot= transform.rotation;
        wcol.GetWorldPose(out pos, out rot);
        wmesh.transform.position = pos;
        wmesh.transform.rotation = rot;
    }
}