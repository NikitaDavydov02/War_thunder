using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpuse : MonoBehaviour {
    //REFACTORED_1
    [SerializeField]
    private CorpuseAudioManager audioManager;
    [SerializeField]
    private Rigidbody rigidbody;
    public float maxForce;
    public float rotForce;
    [SerializeField]
    protected ModuleController controller;

    //New fields
    [SerializeField]
    public List<Wheel> wheelsWithLeft;
    public float power = 10;
    public float suspensionDistance = 0.2f;

    public float currentTurnLevel = 0;

    public float extremumSlip = 1f;
    public float extremuFriction = 1f;
    public float assymptoticSlip = 2f;
    public float assymptoticFriction = 0.5f;
    public float stiffnes = 1f;
    protected void Start()
    {
        foreach (Wheel wheel in wheelsWithLeft)
        {
            wheel.wcol.suspensionDistance = suspensionDistance;

            
            WheelFrictionCurve curve = new WheelFrictionCurve();
            curve.asymptoteSlip = assymptoticSlip;
            curve.asymptoteValue = assymptoticFriction;
            curve.extremumSlip = extremumSlip;
            curve.extremumValue = extremuFriction;
            curve.stiffness = stiffnes;
            wheel.wcol.forwardFriction = curve;
            wheel.wcol.sidewaysFriction = curve;
            Debug.Log("Wheel" + wheel.wcol + " is assigned " + curve);
            //wheel.GetComponent<WheelCollider>().GetComponent<JointSpring>();
        }
    }
    //public float maxSpeed = 10f;
	
	// Update is called once per frame
	void Update () {
    }
    public void Move(float input)
    {
        //if (!controller.alive||!controller.canMove|| MainManager.GameStatus!=GameStatus.Playing)
        //    return;
        //rigidbody.AddRelativeForce(maxForce * Vector3.forward * input);
        //return;
        foreach (Wheel w in wheelsWithLeft)
        {
            float accelerationLevel = input;
            if (currentTurnLevel < 0)
            {
                if (w.left)
                    accelerationLevel *= currentTurnLevel;
                else
                    accelerationLevel =-currentTurnLevel;
            }

            if (currentTurnLevel > 0)
            {
                if (w.left)
                    accelerationLevel *= -currentTurnLevel;
                else
                    accelerationLevel = currentTurnLevel;
            }
            //w.Steer(horInput);
            if(w.left)
                Debug.Log("Left:" + accelerationLevel);
            else
                Debug.Log("Right:" + accelerationLevel);
            w.Accelerate(accelerationLevel * power);
            w.UpdatePosition();
        }

        audioManager.ChangePitch(input);
    }
    public void Turn(float turn)
    {
        //if (!controller.alive|| !controller.canMove|| MainManager.GameStatus != GameStatus.Playing)
        //    return;
        currentTurnLevel = turn;
        //rigidbody.AddTorque(transform.parent.transform.up * rotForce * turn);
    }
}
