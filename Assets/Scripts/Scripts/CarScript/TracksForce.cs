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
    private Vector3 lastSlipVelocity;
    
    
    public float engineLevel = 0f;
    public float dynamicFrictionVelocityTreshhold = 0.1f;
    public float engineForce = 1f;
    public bool left = false;
    public float normalizationSlope = 1f;

    //
    //Slip force
    public float slipCoeffitient = 10f;
    public float slipTreshhold = 0f;

    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        CurrentForceVectors = new List<Vector3>();
        AbsolutePointsOfForceApplying = new List<Vector3>();



        //FrictionForce
        Vector3 frictionForceInRelativeCoordinates = Vector3.zero;
        Vector3 frictionForceInWorldCoordinates = Vector3.zero;
        float normalAxisForce = transform.InverseTransformDirection(forceFromPlatformInWorldCoordinates).y;
        Debug.DrawLine(transform.position, transform.position + forceFromPlatformInWorldCoordinates, Color.green);
        Vector3 forceFromPlatformInLocalCoordinated = transform.InverseTransformDirection(forceFromPlatformInWorldCoordinates);
        Vector2 tangentForceInLocalCoordinates= new Vector2(forceFromPlatformInLocalCoordinated.x, forceFromPlatformInLocalCoordinated.z);
        float maxFrictionForce = -normalAxisForce * frictionCoeffitient;

        
        //engineLevel = 0;
        if (engineLevel > 0)
            frictionForceInRelativeCoordinates =Vector3.forward *engineForce * engineLevel;
        frictionForceInWorldCoordinates = transform.TransformDirection(frictionForceInRelativeCoordinates);
        Debug.Log("Engine force inrelative"+frictionForceInRelativeCoordinates);
        CurrentForceVectors.Add(frictionForceInWorldCoordinates);
        AbsolutePointsOfForceApplying.Add(transform.position);

        // if (normalAxisForce > 0)
        //{
        //You are flying
        //return;
        // }



        //Slip force
        float paralelMovment = Vector3.Dot(transform.TransformDirection(Vector3.forward), velocity);
        Vector3 perpendicularSlip = velocity - (paralelMovment * transform.TransformDirection(Vector3.forward));
        perpendicularSlip.y = 0;
        float slipAceleration = (perpendicularSlip - lastSlipVelocity).magnitude / Time.deltaTime;
        lastSlipVelocity = perpendicularSlip;

        //if (perpendicularSlip.magnitude < slipTreshhold)
        //    perpendicularSlip = Vector3.zero;
        Debug.DrawLine(transform.position, transform.position + velocity*5, Color.black);
        Debug.DrawLine(transform.position, transform.position + perpendicularSlip*5, Color.black);

        Vector3 slipForce = -perpendicularSlip.normalized * 1000f;
        //slipForce += transform.TransformDirection(Vector3.forward) * engineForce;

        CurrentForceVectors.Add(slipForce);
        AbsolutePointsOfForceApplying.Add(transform.position);



       

        /*//if (Mathf.Abs(engineLevel) < 0.1f)
        {
            Vector3 planarVelocity = transform.InverseTransformDirection(velocity);
            planarVelocity.y = 0;
            //Debug.Log("Planar velocity" + planarVelocity);
            Debug.DrawLine(transform.position, transform.position + 5*transform.TransformDirection(planarVelocity), Color.black);
            if (planarVelocity.magnitude >= dynamicFrictionVelocityTreshhold)
            {
                //Dynamic friction
                //frictionForceInRelativeCoordinates = -planarVelocity*planarVelocity.magnitude* maxFrictionForce/normalizationSlope;
                //frictionForceInRelativeCoordinates = -planarVelocity.normalized * maxFrictionForce;
                frictionForceInRelativeCoordinates = -planarVelocity.normalized*forceFromPlatformInLocalCoordinated.magnitude;
                frictionForceInRelativeCoordinates.y = 0;
                Debug.Log("Dinamic frition");
            }
            else
            {
                Debug.Log("Static frition");
                //frictionForceInRelativeCoordinates = -forceFromPlatformInLocalCoordinated;
                //frictionForceInRelativeCoordinates.y = 0;
                //normalizationSlope = dynamicFrictionVelocityTreshhold * forceFromPlatformInLocalCoordinated.magnitude;
                frictionForceInRelativeCoordinates = -planarVelocity / normalizationSlope;
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
        */
        //else
        //{

        //else if(platformRb.transform.InverseTransformDirection(platformRb.velocity).z>0)
        //else
        //    frictionForceInRelativeCoordinates = new Vector3(0, 0, maxFrictionForce * engineLevel);
        //}


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
