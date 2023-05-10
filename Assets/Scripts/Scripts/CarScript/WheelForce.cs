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
    //public float frictionCoeffititient = 1f;

    //Connection with platform parameters
    public Vector3 basePointRelativeToPlatform;
    public float springInitialLength;

    //Interaction with enviroment
    //public Vector3 lastContactPosition { get; set; }
    //public Vector3 slipForce;
    //public Vector3 axisForceInGlobalCoordinates;
    //private float angularVelocity;

    // Engine force
    //public float force = 1f;
    ///public float engineLevel = 0;
    //public bool left = false;
    //public bool powered = false;

    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        //CurrentForceVectors = new List<Vector3>();


        Vector3 applicationPointInAbsoluteCoordinates = platformRb.transform.position + platformRb.transform.TransformDirection(basePointRelativeToPlatform);
        AbsolutePointsOfForceApplying = new List<Vector3>() { applicationPointInAbsoluteCoordinates };
        
        //Spring force
        float deltaSpring = (applicationPointInAbsoluteCoordinates - transform.position).magnitude - springInitialLength;
        float springVelocity = (deltaSpring-lastSpringDelta)/ Time.deltaTime;
        Vector3 springForce = new Vector3(0, (-deltaSpring * springK)-(springVelocity*damping), 0);
        CurrentForceVectors = new List<Vector3>() { transform.TransformDirection(springForce)};
        lastSpringDelta = deltaSpring;
        
        
        //Engine force
        


        

        
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
