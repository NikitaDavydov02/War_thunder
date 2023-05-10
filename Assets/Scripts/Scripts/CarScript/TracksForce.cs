using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksForce : MonoBehaviour, IForce
{
    //public float slipCoeffitient = 1f;
    //public float slipTreshhold = 1f;
    public float frictionCoeffitient = 1f;
    public Vector3 forceFromPlatformInWorldCoordinates;
    private Vector3 lastPosition;
    [SerializeField]
    private Rigidbody rb;
    private Vector3 velocity;
    
    public float engineLevel = 0f;
    public float dynamicFrictionVelocityTreshhold = 0.1f;
    public float engineForce = 1f;
    public bool left = false;
    public float normalizationSlope = 1f;

    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        CurrentForceVectors = new List<Vector3>();
        AbsolutePointsOfForceApplying = new List<Vector3>();
        /*velocity = (transform.position - lastPosition) / Time.deltaTime;
        float paralelMovment = Vector3.Dot(transform.TransformDirection(Vector3.forward), velocity);

        Vector3 perpendicularSlip = velocity - (paralelMovment * transform.TransformDirection(Vector3.forward));
        if (perpendicularSlip.magnitude < slipTreshhold)
            perpendicularSlip = Vector3.zero;

        Vector3 slipForce = -perpendicularSlip.normalized * perpendicularSlip.magnitude * slipCoeffitient;

        CurrentForceVectors = new List<Vector3>() { slipForce };
        AbsolutePointsOfForceApplying = new List<Vector3>() { transform.position };*/



        //FrictionForce
        Vector3 frictionForceInRelativeCoordinates = Vector3.zero;
        float normalAxisForce = transform.InverseTransformDirection(forceFromPlatformInWorldCoordinates).y;
        Debug.DrawLine(transform.position, transform.position + forceFromPlatformInWorldCoordinates, Color.green);
        Vector3 forceFromPlatformInLocalCoordinated = transform.InverseTransformDirection(forceFromPlatformInWorldCoordinates);
        Vector2 tangentForceInLocalCoordinates= new Vector2(forceFromPlatformInLocalCoordinated.x, forceFromPlatformInLocalCoordinated.z);


        if (normalAxisForce > 0)
        {
            //You are flying
            return;
        }
            
        float maxFrictionForce = -normalAxisForce * frictionCoeffitient;

        Vector3 frictionForceInWorldCoordinates = Vector3.zero;
        
        //if (Mathf.Abs(engineLevel) < 0.1f)
        {
            Vector3 planarVelocity = transform.InverseTransformDirection(velocity);
            planarVelocity.y = 0;
            //Debug.Log("Planar velocity" + planarVelocity);
            Debug.DrawLine(transform.position, transform.position + 5*transform.TransformDirection(planarVelocity), Color.black);
            if (planarVelocity.magnitude >= dynamicFrictionVelocityTreshhold)
            {
                //Dynamic friction
                //frictionForceInRelativeCoordinates = -planarVelocity*planarVelocity.magnitude* maxFrictionForce/normalizationSlope;
                frictionForceInRelativeCoordinates = -planarVelocity.normalized * maxFrictionForce;
                Debug.Log("Dinamic frition");
            }
            else
            {
                //Static friction
                if (tangentForceInLocalCoordinates.magnitude <= maxFrictionForce)
                {
                    frictionForceInRelativeCoordinates = -forceFromPlatformInLocalCoordinated;
                    frictionForceInRelativeCoordinates.y = 0;
                    Debug.Log("Static friction not max");
                }
                else
                {
                    frictionForceInRelativeCoordinates = -forceFromPlatformInLocalCoordinated;
                    frictionForceInRelativeCoordinates.y = 0;
                    frictionForceInRelativeCoordinates = frictionForceInRelativeCoordinates.normalized * maxFrictionForce;
                    Debug.Log("Static friction max");
                }
            }
            
                
            //if (Mathf.Abs(tangetAxisForce) <= maxFrictionForce)
            //    frictionForceInRelativeCoordinates = new Vector3(0, 0, -tangetAxisForce);
            //else
            //    frictionForceInRelativeCoordinates = new Vector3(0, 0, -maxFrictionForce);

        }
        //else
        {
            if (engineLevel > 0)
                frictionForceInRelativeCoordinates.z=engineForce * engineLevel;
            //else if(platformRb.transform.InverseTransformDirection(platformRb.velocity).z>0)
            //else
            //    frictionForceInRelativeCoordinates = new Vector3(0, 0, maxFrictionForce * engineLevel);
        }

        frictionForceInWorldCoordinates = transform.TransformDirection(frictionForceInRelativeCoordinates);
        CurrentForceVectors.Add(frictionForceInWorldCoordinates);
        AbsolutePointsOfForceApplying.Add(transform.position);
    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity = rb.velocity - Vector3.Cross(transform.position - rb.transform.position, rb.angularVelocity);
        //velocity = (transform.position-lastPosition) / Time.deltaTime;
        
        //velocity.y = 0;
        lastPosition = transform.position;
    }
}
