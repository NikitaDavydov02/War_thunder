using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : ForceCalculationManager
{
    public float generalLevel = 0;
    //public float generalLevelChangingSpeed = 1f;
    List<float> engineLevels = new List<float>();
    public float maxEleronAngle = 2;
    [SerializeField]
    private float destoingImpuls = 50f;

    [SerializeField]
    List<EngineForce> engines;
    [SerializeField]
    public GravityForce gravityForce;
    [SerializeField]
    public List<WingForce> wings;
    [SerializeField]
    List<ResistanceForce> resistanceForces;
    [SerializeField]
    Transform heightController;
    [SerializeField]
    Transform horizontalController;
    [SerializeField]
    Transform leftElleron;
    [SerializeField]
    Transform rightElleron;
    public float eleronAngle = 0;
    //public float eleronSensitivity;


    public float RotationPowerMultiplyer;
    //public float gasSensitivity;
    //public float heigtSensitivity;
    public float heightAngle = 0;
    //public float horizontalSensitivity;
    public float horizontAngle = 0;
    public Vector3 centerOfMassLocal;
    public Vector3 inertiaTensor;
    public float maxHeightAngle = 10;
    public float maxHorizontalAngle = 10;


    public Vector3 VelocityInLocalCoordinates = Vector3.zero;
    public Vector3 AngularVelocityInLocalCoordinates = Vector3.zero;

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
        for (int i = 0; i < engines.Count; i++)
        {
            engineLevels.Add(0);
            forceSources.Add(engines[i]);
        }
        foreach (ResistanceForce f in resistanceForces)
            forceSources.Add(f);
        foreach (WingForce w in wings)
            forceSources.Add(w);
        //forceSources.Add(gravityForce);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (inertiaTensor != rb.inertiaTensor)
            rb.inertiaTensor = inertiaTensor;
        base.Update();
        CountState();
        
        if (generalLevel > 1)
            generalLevel = 1;
        if (generalLevel < 0)
            generalLevel = 0;
        for (int i = 0; i < engineLevels.Count; i++)
            engineLevels[i] = generalLevel;

        //if (!ControlActive)
        //    vwrticalInput = 0;
        //Debug.Log("Input:" + vwrticalInput);
        //Debug.Log("Euler:" + heightController.localEulerAngles.x);

        //HorizontalController(horizontAngle);

        Debug.DrawLine(transform.position, transform.position + rb.velocity, Color.black);

        for (int i = 0; i < engineLevels.Count; i++)
            engines[i].Level = engineLevels[i];
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public void HorizontalController(float input)
    {
        
        //if (!ControlActive)
        //    horInput = 0;
        horizontalController.Rotate(input, 0, 0, Space.Self);
        horizontAngle +=input;
        if (horizontAngle < -maxHorizontalAngle || horizontAngle > maxHorizontalAngle)
        {
            horizontalController.Rotate(-input, 0, 0, Space.Self);
            horizontAngle -= input;
        }
    }
    public void HeightController(float input)
    {
        heightController.Rotate(input, 0, 0, Space.Self);
        heightAngle += input;
        if (heightAngle < -maxHeightAngle)
        {
            heightController.Rotate(-input, 0, 0, Space.Self);
            heightAngle -= input;
        }
        if (heightAngle > maxHeightAngle)
        {
            heightController.Rotate(-input, 0, 0, Space.Self);
            heightAngle -= input;
        }
    }
    public EleronPosition eleronPosition = EleronPosition.Neutral;
    public void Eleron(EleronPosition targetEleronPosition)
    {
        int act = targetEleronPosition - eleronPosition;

        leftElleron.Rotate(act*maxEleronAngle, 0, 0);
        rightElleron.Rotate(-act*maxEleronAngle, 0, 0);

        eleronPosition = targetEleronPosition;
    }
    public void Eleron(bool turnToTheLeft)
    {
        if (turnToTheLeft)
        {
            if (eleronPosition == EleronPosition.Left)
                return;
            leftElleron.Rotate(maxEleronAngle, 0, 0);
            rightElleron.Rotate(-maxEleronAngle, 0, 0);
            if (eleronPosition == EleronPosition.Neutral)
                eleronPosition = EleronPosition.Left;
            if (eleronPosition == EleronPosition.Right)
                eleronPosition = EleronPosition.Neutral;
        }
        else
        {
            if (eleronPosition == EleronPosition.Right)
                return;
            leftElleron.Rotate(-maxEleronAngle, 0, 0);
            rightElleron.Rotate(maxEleronAngle, 0, 0);
            if (eleronPosition == EleronPosition.Neutral)
                eleronPosition = EleronPosition.Right;
            if (eleronPosition == EleronPosition.Left)
                eleronPosition = EleronPosition.Neutral;
        }
    }
    //void FixedUpdate()
    //{


    //    List<Vector3> CurrentForceVectors;
    //    List<Vector3> AbsolutePointsOfForceApplying;
    //    foreach (IForce force in forceSources)
    //    {
    //        force.CountForce(out CurrentForceVectors, out AbsolutePointsOfForceApplying);
    //        for (int i = 0; i < CurrentForceVectors.Count; i++)
    //        {
    //            AddForce(CurrentForceVectors[i], AbsolutePointsOfForceApplying[i]);
    //        }

    //    }
    //    //Debug.Log("Force:" + ForceToCenterOfMass);
    //    rb.AddForce(ForceToCenterOfMass, ForceMode.Force);
    //    MomentInCoordinatesTranslatedToCenterOfMass *= -1;
    //    //Debug.Log("M: " + (MomentInCoordinatesTranslatedToCenterOfMass));
    //    rb.AddRelativeTorque(transform.InverseTransformDirection(MomentInCoordinatesTranslatedToCenterOfMass), ForceMode.Force);
    //    ForceToCenterOfMass = Vector3.zero;
    //    MomentInCoordinatesTranslatedToCenterOfMass = Vector3.zero;
    //}

    public void CountState()
    {
        AngularVelocityInLocalCoordinates = transform.InverseTransformDirection(rb.angularVelocity);
        VelocityInLocalCoordinates = transform.InverseTransformDirection(rb.velocity);
    }
    //}
    //private void AddForce(Vector3 forceInWorldCoordinates, Vector3 pointOfApplicationINWorldCoordinates)
    //{
    //    ForceToCenterOfMass += forceInWorldCoordinates;
    //    //Debug.Log("dF" + forceInWorldCoordinates);
    //    Vector3 r = pointOfApplicationINWorldCoordinates - rb.worldCenterOfMass;
    //    //Debug.Log("r" + r);
    //    Vector3 dM = -Vector3.Cross(r, forceInWorldCoordinates);
    //    //Debug.Log("dM: " + dM);
    //    MomentInCoordinatesTranslatedToCenterOfMass += dM;
    //    Debug.DrawLine(pointOfApplicationINWorldCoordinates, pointOfApplicationINWorldCoordinates + forceInWorldCoordinates, Color.red);
    //    Debug.DrawLine(pointOfApplicationINWorldCoordinates, pointOfApplicationINWorldCoordinates + dM, Color.blue);
    //    Debug.DrawLine(rb.worldCenterOfMass, rb.worldCenterOfMass + r, Color.green);
    //}
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision");
        if (collision.impulse.magnitude > destoingImpuls)
        {
            Debug.Log("Destroy");
            DestroyPlane();
        }
    }
    private void DestroyPlane()
    {
        GameObject explosion = Instantiate(Resources.Load("Prefabs/Explosion") as GameObject);
        explosion.transform.position = transform.position;
        AudioSource source = Instantiate(MainManager.musicManager.sourcePrefab);
        source.clip = Resources.Load("Music/Crash") as AudioClip;
        source.transform.position = transform.position;
        source.Play();
        InstantiateFire();

        Collider[] hits = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider hited in hits)
        {
            Module m = hited.gameObject.GetComponent<Module>();
            if (m != null)
            {
                m.Damage(10000,gameObject.name);
            }
        }
    }
    private void InstantiateFire()
    {
        Debug.Log("Instatniate fire");
        GameObject fire = Instantiate(Resources.Load("Prefabs/Fire") as GameObject);
        fire.transform.position = this.transform.position;
        //Vector3 scale = this.transform.localScale;
        //Vector3 newScale = new Vector3(1 / scale.x, 1 / scale.y, 1 / scale.z);
        fire.transform.SetParent(this.transform, false);
        fire.transform.localPosition = Vector3.zero;
        //fire.transform.localScale = newScale;
    }
}
public enum EleronPosition
{
    Right=-1,
    Neutral=0,
    Left = 1,
}