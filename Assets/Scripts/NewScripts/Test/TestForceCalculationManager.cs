using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForceCalculationManager : ForceCalculationManager
{
    public Vector3 centerOfMassLocal;
    public Vector3 inertiaTensor;
    // Start is called before the first frame update
    [SerializeField]
    public GravityForce gravityForce;
    protected override void Start()
    {
        base.Start();
        rb.centerOfMass = centerOfMassLocal;
        Debug.Log("Inertia tensor" + rb.inertiaTensor);
        if (inertiaTensor != Vector3.zero)
            rb.inertiaTensor = inertiaTensor;

        forceSources.Add(gravityForce);
        gravityForce.InitForce(centerOfMassLocal);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
    }
}
