using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelForce : MonoBehaviour, IForce
{
    //Spring
    public float springK = 1f;
    public float damping = 200;
    private float lastSpringDelta = 0;

    //Physics and geometry parameters
    public Vector3 inertiaTensor = Vector3.zero;
    [SerializeField]
    private Rigidbody platformRb;
    public float diametr = 1f;
    //private Rigidbody wheelRb;
    public float frictionCoeffititient = 1f;

    //Connection with platform parameters
    public Vector3 basePointRelativeToPlatform;
    public float springInitialLength;

    //Interaction with enviroment
    public Vector3 lastContactPosition { get; set; }
    public Vector3 slipForce;
    public Vector3 axisForceInGlobalCoordinates;
    private float angularVelocity;

    // Engine force
    //public float force = 1f;
    public float engineLevel = 0;
    public bool left = false;
    public bool powered = false;

    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        CurrentForceVectors = new List<Vector3>();


        Vector3 applicationPointInAbsoluteCoordinates = platformRb.transform.position + platformRb.transform.TransformDirection(basePointRelativeToPlatform);
        AbsolutePointsOfForceApplying = new List<Vector3>() { applicationPointInAbsoluteCoordinates };
        
        //Spring force
        float deltaSpring = (applicationPointInAbsoluteCoordinates - transform.position).magnitude - springInitialLength;
        float springVelocity = (deltaSpring-lastSpringDelta)/ Time.deltaTime;
        Vector3 springForce = new Vector3(0, (-deltaSpring * springK)-(springVelocity*damping), 0);
        CurrentForceVectors = new List<Vector3>() { transform.TransformDirection(springForce)};
        

        //Slipping force
        CurrentForceVectors.Add((slipForce));
        AbsolutePointsOfForceApplying.Add(applicationPointInAbsoluteCoordinates);

        //Roll force
        Vector3 frictionForceInRelativeCoordinates = Vector3.zero;
        float normalAxisForce = transform.InverseTransformDirection(axisForceInGlobalCoordinates).y;
        float tangetAxisForce = transform.InverseTransformDirection(axisForceInGlobalCoordinates).z;
        //Debug.Log("Velocity wheel" + wheelRb.velocity);
        //Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(new Vector3(0, -normalAxisForce, 0)), Color.yellow);
        //Debug.DrawLine(transform.position, transform.position + transform.TransformDirection(new Vector3(0, 0, tangetAxisForce)), Color.yellow);
        if (normalAxisForce > 0)
            return;
        float maxFrictionForce = -normalAxisForce * frictionCoeffititient;
        //Debug.DrawLine(transform.position, transform.position + platformRb.velocity, Color.white);
        Vector3 frictionForceInWorldCoordinates = Vector3.zero;
        //if (platformRb.velocity.magnitude <0.1f)
        if(Mathf.Abs(engineLevel)<0.1f || !powered)
        {
            if (Mathf.Abs(tangetAxisForce) <= maxFrictionForce)
                frictionForceInRelativeCoordinates = new Vector3(0, 0, -tangetAxisForce);
            else
                frictionForceInRelativeCoordinates = new Vector3(0, 0, -maxFrictionForce);
            
        }
        else
        {
            if(engineLevel>0)
                frictionForceInRelativeCoordinates = new Vector3(0, 0, maxFrictionForce*engineLevel);
            else if(platformRb.transform.InverseTransformDirection(platformRb.velocity).z>0)
                frictionForceInRelativeCoordinates = new Vector3(0, 0, maxFrictionForce * engineLevel);
        }
                
        frictionForceInWorldCoordinates = transform.TransformDirection(frictionForceInRelativeCoordinates);
        //else
        //{
        //    if (Vector3.Dot(platformRb.velocity , transform.TransformDirection(Vector3.forward)) > 0)
        //    {
        //        frictionForceInRelativeCoordinates = -Vector3.forward * maxFrictionForce;
        //    }
        //    else
        //    {
        //        frictionForceInRelativeCoordinates = Vector3.forward * maxFrictionForce;
        //    }
        //    frictionForceInWorldCoordinates = transform.TransformDirection(frictionForceInRelativeCoordinates);
        //}


        //Vector3 velocityOfContactPoint = platformRb.velocity;
        //Debug.DrawLine(transform.position, transform.position + velocityOfContactPoint, Color.cyan);
        // Vector3 moment = -Vector3.Cross((lastContactPosition - transform.position), frictionForceInWorldCoordinates);
        // Debug.DrawLine(transform.position, transform.position + moment, Color.blue);
        // wheelRb.AddTorque(moment);
        //transform.Rotate(new Vector3(angularVelocity*Time.deltaTime,0,0), Space.Self);
        angularVelocity = platformRb.velocity.magnitude / (lastContactPosition - transform.position).magnitude;
        Vector3 velocityOfContactPoint = platformRb.velocity -Vector3.Cross((lastContactPosition - transform.position),Vector3.right*angularVelocity);
        Debug.DrawLine(transform.position, transform.position + velocityOfContactPoint, Color.cyan);
        //Debug.Log("Point Velocity:" + velocityOfContactPoint.magnitude);

        //Output forces
        CurrentForceVectors.Add(frictionForceInWorldCoordinates);
        AbsolutePointsOfForceApplying.Add(applicationPointInAbsoluteCoordinates);
        
        //Engine force
        


        //Vector3 velocityInContactPoint = - Vector3.Cross((lastContactPosition - transform.position), wheelRb.angularVelocity);
        //Debug.DrawLine(transform.position, transform.position + (lastContactPosition - transform.position), Color.black);
        //Debug.DrawLine(transform.position, transform.position + wheelRb.angularVelocity, Color.cyan);
        //Debug.Log("Abgular velocity" + wheelRb.angularVelocity);
        //Debug.DrawLine(transform.position, velocityInContactPoint, Color.yellow);



        //Updating parameners
        lastSpringDelta = deltaSpring;
    }

    // Start is called before the first frame update
    void Start()
    {
        //wheelRb = gameObject.GetComponent<Rigidbody>();
        //if (inertiaTensor != Vector3.zero)
        //    wheelRb.inertiaTensor = inertiaTensor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
