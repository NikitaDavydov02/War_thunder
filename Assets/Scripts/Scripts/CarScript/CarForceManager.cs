using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarForceManager : ForceCalculationManager
{
    [SerializeField]
    public List<WheelForce> wheels;
    [SerializeField]
    public List<TracksForce> tracks;
    [SerializeField]
    public GravityForce gravityForce;
    public Vector3 centerOfMassLocal;
    public Vector3 inertiaTensor;
    public float generalLevel = 0;
    //public float inputSensetivity = 1f;
    //public float horSensetivity = 1f;
    private float rotLevel = 0;
    //public float slipCoeffitient = 2f;
    //public float slipTreshhold = 0.1f;
    //public float mass = 10f;
    [SerializeField]
    public ResistanceForce resistanceForce;
    //[SerializeField]
    //private Rigidbody carRb;
    public float maWheelDistance = 1.5f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //rb = carRb;
        rb.centerOfMass = centerOfMassLocal;
        Debug.Log("Inertia tensor" + rb.inertiaTensor);
        if (inertiaTensor != Vector3.zero)
            rb.inertiaTensor = inertiaTensor;

        
        generalLevel = 0;
        
        foreach (WheelForce w in wheels)
            forceSources.Add(w);
        
        foreach (TracksForce t in tracks)
            forceSources.Add(t);
        forceSources.Add(gravityForce);
        forceSources.Add(resistanceForce);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        foreach(TracksForce t in tracks)
        {
            t.forceFromPlatformInWorldCoordinates = new Vector3(0, -rb.mass * 9.81f / wheels.Count, 0);
            float trackLevel = generalLevel;
            if (rotLevel < 0)
            {
                if (t.left)
                    trackLevel = (rotLevel);
                else
                    trackLevel = -1 * (rotLevel);
            }
            if (rotLevel > 0)
            {
                if (!t.left)
                    trackLevel = -1 * (rotLevel);
                else
                    trackLevel = (rotLevel);
            }
            t.engineLevel = trackLevel;
        }
        foreach (WheelForce w in wheels)
        {
            RaycastHit hit;
            Vector3 origin = transform.position + transform.TransformDirection(w.basePointRelativeToPlatform);
            Vector3 dir = transform.TransformDirection(Vector3.down);
            Ray downRay = new Ray(origin, dir);
            if (Physics.Raycast(downRay, out hit, 3))
            {

                float distance = hit.distance - w.diametr / 2;
                //if (distance >= maWheelDistance)
                    //w.transform.position = origin + dir.normalized * maWheelDistance;
                //else
                    w.transform.position = hit.point + transform.TransformDirection(Vector3.up * w.diametr / 2);
               
                w.transform.localEulerAngles = transform.localEulerAngles;
            }
        }
        if (rb.velocity.magnitude < 10.1f)
            rb.velocity = Vector3.zero;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        generalLevel = Input.GetAxis("Vertical");
        if (generalLevel > 1)
            generalLevel = 1;
        if (generalLevel < -1)
            generalLevel = -1;

        rotLevel = 0;
        if (Input.GetKey(KeyCode.A))
            rotLevel = -1;
        if (Input.GetKey(KeyCode.D))
            rotLevel = 1;
        //rotLevel += Input.GetAxis("Horizontal") * horSensetivity * Time.deltaTime;
        if (rotLevel > 1)
            rotLevel = 1;
        if (rotLevel < -1)
            rotLevel = -1;

        
        
    }
}
