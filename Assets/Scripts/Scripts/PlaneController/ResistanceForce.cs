using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistanceForce : MonoBehaviour, IForce
{
    private float k;
    public Primitive Primitive;
    public float length;
    public float diameter;
    //public List<Vector3> CurrentForceVector { get; private set; }

    //public List<Vector3> AbsolutePointOfForceApplying { get; private set; }
    public Vector3 velocity = Vector3.zero;
    private Vector3 lastPosition;
    public Rigidbody rb;
    bool rbFromParent = false;
    public float kParalel = 0.1f;
    public float kPerp = 1f;
    [SerializeField]
    public Vector3 deltaForceApplicationPoint = Vector3.zero;
    public Vector3 forwardDirectionVector = Vector3.forward;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.transform.parent.GetComponent<Rigidbody>();
            rbFromParent = true;
        }
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //velocity = rb.velocity;
        velocity = rb.velocity + Vector3.Cross(rb.angularVelocity, transform.position - rb.gameObject.transform.position) - MainWeatherManager.GetWind(transform.position);

        lastPosition = transform.position;
    }
    public void CountForce(out List<Vector3> CurrentForceVectors, out List<Vector3> AbsolutePointsOfForceApplying)
    {
        CurrentForceVectors = new List<Vector3>();
        AbsolutePointsOfForceApplying = new List<Vector3>();
        //Debug.Log("velostiy: " + velocity);
        if (velocity.magnitude == 0)
        {
            CurrentForceVectors.Add(Vector3.zero);
            AbsolutePointsOfForceApplying.Add(rb.worldCenterOfMass);
            return;
        }
        if (velocity.magnitude < 0.1f)
            return;
        float area = 0;
        Vector3 forceInGlobalCoordinates = Vector3.zero;
        if (Primitive == Primitive.Stick)
        {
            Vector3 stickAxis = transform.TransformDirection(1, 0, 0);
            Vector3 perpendicularVelosity = velocity - Vector3.Dot(velocity, stickAxis) * stickAxis;
            //Debug.DrawLine(transform.position, transform.position + velocity, Color.cyan);
            //Debug.DrawLine(transform.position, transform.position + perpendicularVelosity, Color.cyan);
            Vector3 direction = -perpendicularVelosity.normalized;
            //Debug.DrawLine(transform.TransformPoint(Vector3.zero), transform.TransformPoint(Vector3.zero) + direction, Color.grey);
            area = diameter * length;
            float k = 4.4f;
            float module = MainWeatherManager.AirDensity * perpendicularVelosity.magnitude * perpendicularVelosity.magnitude * k * area / 2;
            forceInGlobalCoordinates = direction * module;
        }
        if (Primitive == Primitive.Disk)
        {
            Vector3 diskAxis = transform.TransformDirection(0, 1, 0);
            Vector3 perpendicularVelosity = Vector3.Dot(velocity, diskAxis) * diskAxis;
            //Vector3 parallelVelosity = velocity - perpendicularVelosity;
            Vector3 direction = -perpendicularVelosity.normalized;
            // Vector3 direction2 = -parallelVelosity.normalized;

            area = Mathf.PI * diameter * diameter / 4;
            //float area2 = diameter * length;
            //float k2 =1.15f;
            float k = 1.15f;
            float module = MainWeatherManager.AirDensity * perpendicularVelosity.magnitude * perpendicularVelosity.magnitude * k * area / 2;
            //float module2 = MainManager.AirDensity * parallelVelosity.magnitude * parallelVelosity.magnitude * k2 * area2 / 2;
            forceInGlobalCoordinates = direction * module;
            //Debug.Log("RF:" + forceInGlobalCoordinates);
            //Debug.DrawLine(transform.TransformPoint(Vector3.zero), transform.TransformPoint(Vector3.zero) + forceInGlobalCoordinates*4, Color.cyan);
        }
        if (Primitive == Primitive.Cylinder)
        {
            Vector3 diskAxis = transform.TransformDirection(0, 1, 0);
            Vector3 perpendicularVelosity = Vector3.Dot(velocity, diskAxis) * diskAxis;
            Vector3 parallelVelosity = velocity - perpendicularVelosity;
            Vector3 direction = -perpendicularVelosity.normalized;
            Vector3 direction2 = -parallelVelosity.normalized;

            area = Mathf.PI * diameter * diameter / 4;
            float area2 = diameter * length;
            float k2 = 1.15f;
            float k = 1.15f;
            float module = MainWeatherManager.AirDensity * perpendicularVelosity.magnitude * perpendicularVelosity.magnitude * k * area / 2;
            float module2 = MainWeatherManager.AirDensity * parallelVelosity.magnitude * parallelVelosity.magnitude * k2 * area2 / 2;
            forceInGlobalCoordinates = direction * module + direction2 * module2;
            //Debug.Log("Resistance Force:" + forceInGlobalCoordinates);
            //Debug.DrawLine(transform.TransformPoint(Vector3.zero), transform.TransformPoint(Vector3.zero) + forceInGlobalCoordinates * 4, Color.cyan);
        }
        if (Primitive == Primitive.Airbus)
        {
            Vector3 parallelVelosity = Vector3.Dot(velocity, transform.TransformDirection(forwardDirectionVector)) * transform.TransformDirection(forwardDirectionVector);
            Vector3 perpendicularVelocity = velocity-parallelVelosity;
            Debug.DrawLine(transform.position, transform.position + parallelVelosity, Color.black);
            Debug.DrawLine(transform.position, transform.position + perpendicularVelocity, Color.black);
            Vector3 directionPerp = -perpendicularVelocity.normalized;
            Vector3 directionParal = -parallelVelosity.normalized;

            float areaPerpToFlow = Mathf.PI * diameter * diameter / 4;
            float areaParalToFlow = diameter * length;
            float modulePerp = MainWeatherManager.AirDensity * perpendicularVelocity.magnitude* perpendicularVelocity.magnitude * kPerp* areaParalToFlow/ 2;
            float moduleParal = MainWeatherManager.AirDensity * parallelVelosity.magnitude * parallelVelosity.magnitude * kParalel * areaPerpToFlow / 2;
            forceInGlobalCoordinates = modulePerp * directionPerp + moduleParal * directionParal;
            //Debug.Log("Resistance Force:" + forceInGlobalCoordinates);
            //Debug.DrawLine(transform.TransformPoint(Vector3.zero), transform.TransformPoint(Vector3.zero) + forceInGlobalCoordinates * 4, Color.cyan);
        }
        CurrentForceVectors.Add(forceInGlobalCoordinates);
        Vector3 pointOfApplication = Vector3.zero;
        if (rbFromParent)
            AbsolutePointsOfForceApplying.Add(transform.position);
        else
            AbsolutePointsOfForceApplying.Add(rb.worldCenterOfMass+transform.TransformDirection(deltaForceApplicationPoint));
    }
}
public enum Primitive
{
    Stick,
    Disk,
    Cylinder,
    Airbus
}
