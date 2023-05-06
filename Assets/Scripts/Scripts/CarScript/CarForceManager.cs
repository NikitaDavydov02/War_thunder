using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarForceManager : ForceCalculationManager
{
    [SerializeField]
    public List<WheelForce> wheels;
    [SerializeField]
    public GravityForce gravityForce;
    public Vector3 centerOfMassLocal;
    public Vector3 inertiaTensor;
    public float generalLevel = 0;
    public float inputSensetivity = 1f;
    public float horSensetivity = 1f;
    public float rotLevel = 0;
    public float slipCoeffitient = 2f;
    public float slipTreshhold = 0.1f;
    public float mass = 10f;
    [SerializeField]
    public ResistanceForce resistanceForce;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb.centerOfMass = centerOfMassLocal;
        Debug.Log("Inertia tensor" + rb.inertiaTensor);
        if (inertiaTensor != Vector3.zero)
            rb.inertiaTensor = inertiaTensor;

        //lastPosition = transform.position;
        generalLevel = 0;
        //for (int i = 0; i < engines.Count; i++)
        //{
        //    engineLevels.Add(0);
        //    forceSources.Add(engines[i]);
        //}
        foreach (WheelForce w in wheels)
            forceSources.Add(w);
        forceSources.Add(gravityForce);
        forceSources.Add(resistanceForce);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        foreach (WheelForce w in wheels)
        {
            RaycastHit hit;
            Vector3 origin = transform.position + transform.TransformDirection(w.basePointRelativeToPlatform);
            Vector3 dir = transform.TransformDirection(Vector3.down);
            Ray downRay = new Ray(origin, dir);
            if (Physics.Raycast(downRay, out hit, 3))
            {

                float distance = hit.distance - w.diametr / 2;
                w.transform.position = hit.point + transform.TransformDirection(Vector3.up * w.diametr / 2);
                //float xAngle = w.transform.eulerAngles.x;
                //Vector3 eulerAngles = transform.eulerAngles;
                //eulerAngles.x = xAngle;
                //w.transform.eulerAngles = eulerAngles;
                w.transform.localEulerAngles = transform.localEulerAngles;
            }
            float wheelLevel = generalLevel;
            //if (rotLevel < 0)
            //{
            //    if (w.left)
            //        wheelLevel = generalLevel * (1 - rotLevel);
            //}
            //if (rotLevel > 0)
            //{
            //    if (!w.left)
            //        wheelLevel = -generalLevel * (1 - rotLevel);
            //}
            //w.engineLevel = wheelLevel;
            w.axisForceInGlobalCoordinates = new Vector3(0, -mass * 9.81f / 4, 0);




            //Sliping force
            Vector3 slipingVelocity = (hit.point - w.lastContactPosition) / Time.deltaTime;
            Debug.DrawLine(hit.point, hit.point + slipingVelocity, Color.black);
            float paralelMovment = Vector3.Dot(transform.TransformDirection(Vector3.forward), slipingVelocity);

            Vector3 perpendicularSlip = slipingVelocity - (paralelMovment * transform.TransformDirection(Vector3.forward));
            Debug.DrawLine(hit.point, hit.point + perpendicularSlip, Color.black);
            if (perpendicularSlip.magnitude < slipTreshhold)
                perpendicularSlip = Vector3.zero;

            Vector3 slipForce = -perpendicularSlip.normalized * slipCoeffitient;
            w.slipForce = slipForce;
            w.lastContactPosition = hit.point;
        }
        
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        generalLevel += Input.GetAxis("Vertical") * inputSensetivity * Time.deltaTime;
        if (generalLevel > 1)
            generalLevel = 1;
        if (generalLevel < -1)
            generalLevel = -1;

        rotLevel += Input.GetAxis("Horizontal") * horSensetivity * Time.deltaTime;
        if (rotLevel > 1)
            rotLevel = 1;
        if (rotLevel < -1)
            rotLevel = -1;

        
        
    }
}
