using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPhysics : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> SpringConnectionPoints;
    private List<float> lastLength;
    private List<float> levels;
    public float initialLengthsToGround;
    public float springK;
    public float damping;
    private Rigidbody rb;
    public float slipCoeffitient = 100f;
    public float slipTreshhold = 0.1f;
    public float generalLevel = 0f;
    public float rotLevel = 0f;
    public float enginePower = 10f;
    public float rotationPower = 0.05f;
    public Vector3 centerOfMass = Vector3.zero;
    public Vector3 inertiaTensor = Vector3.zero;
    [SerializeField]
    public Gun gun;
    public float impuls = 100;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (centerOfMass != Vector3.zero)
            rb.centerOfMass = centerOfMass;
        if (inertiaTensor != Vector3.zero)
            rb.inertiaTensor = inertiaTensor;
        lastLength = new List<float>();
        levels = new List<float>();
        for (int i = 0; i < SpringConnectionPoints.Count; i++)
        {
            lastLength.Add(initialLengthsToGround);
            levels.Add(0);
        }
        if(gun!=null)
            gun.Fired += Gun_Fired;
    }

    private void Gun_Fired(object sender, System.EventArgs e)
    {
        Vector3 force = transform.TransformDirection(Vector3.back * impuls);
        Vector3 point = transform.position + transform.TransformDirection(0, 2, 0);
        rb.AddForceAtPosition(force, point, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        generalLevel = Input.GetAxis("Vertical");
        rotLevel = Input.GetAxis("Horizontal");
        for (int i=0;i<SpringConnectionPoints.Count;i++)
        {
            float trackLevel = generalLevel;
            if (rotLevel < 0)
            {
                if (SpringConnectionPoints[i].x<0)
                    trackLevel = (rotLevel) * rotationPower;
                else
                    trackLevel = -1 * (rotLevel) * rotationPower;
            }
            if (rotLevel > 0)
            {
                if (SpringConnectionPoints[i].x > 0)
                    trackLevel = -1 * (rotLevel) * rotationPower;
                else
                    trackLevel = (rotLevel) * rotationPower;
            }
            levels[i] = trackLevel;

            RaycastHit hit;
            Vector3 origin = transform.position + transform.TransformDirection(SpringConnectionPoints[i]);
            Vector3 dir = transform.TransformDirection(Vector3.down);
            Ray downRay = new Ray(origin, dir);
            if (Physics.Raycast(downRay, out hit))
            {
                Debug.Log(hit.collider.gameObject);
                Debug.DrawLine(origin, hit.point, Color.cyan);
                float currentDistance = hit.distance;
                if (currentDistance > 2)
                    currentDistance = 2;
                if (currentDistance < 0)
                    currentDistance = 0;
                float deltaSpring = currentDistance - initialLengthsToGround;
                float springVelocity = (currentDistance - lastLength[i]) / Time.deltaTime;
                if (springVelocity > 10|| springVelocity < -10)
                    springVelocity = 0;
                Vector3 springForce = new Vector3(0, (-deltaSpring * springK) - (springVelocity * damping), 0);
                //Vector3 springForce = new Vector3(0, (-deltaSpring * springK), 0);
                lastLength[i] = currentDistance;
                Vector3 force = transform.TransformDirection(springForce);
                Debug.Log(force);
                Debug.DrawLine(origin, origin + force, Color.red);
                rb.AddForceAtPosition(force, origin);
                Debug.Log("Spring " + i + " force is " + force);

                //Slip force
                Vector3 velocity = rb.velocity - Vector3.Cross(origin - rb.transform.position, rb.angularVelocity);
                //Debug.DrawLine(origin, origin + velocity, Color.black);

                float paralelMovment = Vector3.Dot(transform.TransformDirection(Vector3.forward), velocity);
                Vector3 perpendicularSlip = velocity;
                if (levels[i] !=0 && rotLevel==0)
                    perpendicularSlip = velocity - (paralelMovment * transform.TransformDirection(Vector3.forward));
                perpendicularSlip.y = 0;
                //float slipAceleration = (perpendicularSlip - lastSlipVelocity).magnitude / Time.deltaTime;
                //lastSlipVelocity = perpendicularSlip;

                Debug.DrawLine(origin, origin + velocity * 5, Color.black);
                Debug.DrawLine(origin, origin + perpendicularSlip * 5, Color.black);

                if (perpendicularSlip.magnitude < slipTreshhold)
                    perpendicularSlip = Vector3.zero;
                Vector3 slipForce = -perpendicularSlip.normalized * slipCoeffitient;
                Debug.DrawLine(origin, origin + slipForce, Color.red);
                //slipForce += transform.TransformDirection(Vector3.forward) * engineForce;

                rb.AddForceAtPosition(slipForce, origin);

                Vector3 engineForce = enginePower * levels[i] * transform.TransformDirection(Vector3.forward);
                rb.AddForceAtPosition(engineForce, origin);
                Debug.DrawLine(origin, origin + engineForce, Color.red);
                //CurrentForceVectors.Add(slipForce, origin);
                //AbsolutePointsOfForceApplying.Add(transform.position);
            }
        }
           
        
        //Vector3 applicationPointInAbsoluteCoordinates = platformRb.transform.position + platformRb.transform.TransformDirection(basePointRelativeToPlatform);
        //AbsolutePointsOfForceApplying = new List<Vector3>() { applicationPointInAbsoluteCoordinates };

        //Spring force
        
    }
}
