using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneStateMachine :StateMachine
{
    //States
    private LandedState landedState;
    private TakingOffState takingOffState;
    //------------------------------------

    [SerializeField]
    private float maxRollError = 2f;
    [SerializeField]
    private float treshholdEleronVSWheelCorrection = 10f;
    [SerializeField]
    private float maxPitchError = 2f;
    [SerializeField]
    private PlaneController planeController;
    [SerializeField]
    private float acceptableCourseError = 5f;
    [SerializeField]
    private float acceptableAltitudeError = 50f;

    public float currentRoll { get; private set; } = 0f;
    public float currentPitch { get; private set; } = 0f;
    public float currentAltitude { get; private set; } = 0f;
    public Vector3 currentVelocity { get; private set; } = Vector3.zero;
    public float currentCourse { get; private set; } = 0f;
    public float fireDistance = 300f;

    public float maxUpHeightWheel = 0.1f;
    public float neutralHeightWheel = -0.3f;
    public float maxDownHeightWheel = 1f;
    public float maxRoll = 30f;
    public float maxHorizontalWheelInput = 1f;

    [SerializeField]
    private Gun gun;

    private Rigidbody rb;
    // Start is called before the first frame update
    private void Awake()
    {
        landedState = new LandedState(this, gameObject.transform);
        takingOffState = new TakingOffState(this, planeController);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentRoll = transform.localEulerAngles.z;
        if (currentRoll > 180f)
            currentRoll = currentRoll - 360f;

        currentPitch = transform.localEulerAngles.x;
        if (currentPitch > 180)
            currentPitch = currentPitch - 360;
        currentPitch = -currentPitch;

        currentAltitude = transform.position.y;
        currentVelocity = rb.velocity;
        Vector3 planeVelocuty = new Vector3(currentVelocity.x, 0, currentVelocity.z);
        currentCourse = Vector3.Angle(Vector3.forward, planeVelocuty);
        if (planeVelocuty.x < 0)
            currentCourse = -currentCourse;
    }

    public void StabilazeRoll(float roll = 0)
    {
        Debug.Log("Stabilize roll");
        //Debug.Log("Roll: " + currentRoll);

        if (currentRoll > roll + maxRollError)
            planeController.Eleron(EleronPosition.Right);
        else if (currentRoll < roll - maxRollError)
            planeController.Eleron(EleronPosition.Left);
        else
            planeController.Eleron(EleronPosition.Neutral);
    }
    public void StabalizePitch(float pitch = 0f)
    {
        //Debug.Log("Stabilize pitch");
        //Debug.Log("Input" + Input.GetAxis("Mouse Y"));
        //Debug.Log("Pitch: " + currentPitch);
        // Debug.Log("Targe pitch:" + pitch);

        if (currentPitch > pitch + maxPitchError)
        {
            planeController.HeightController(maxDownHeightWheel);
            //Debug.Log("Down");
        }
        else if (currentPitch < pitch - maxPitchError)
        {
            planeController.HeightController(maxUpHeightWheel);
            //Debug.Log("Up");
        }
        else
        {
            planeController.HeightController(neutralHeightWheel);
            //Debug.Log("Neutral");
        }

        //    planeController.Eleron(false);
        //if (currentRoll < -treshholdRoll)
        //    planeController.Eleron(true);
        //if(currentRoll>5f)
    }
    public void StabalizeCourse(float targerCourse)
    {
        float delta = currentCourse - targerCourse;
        Debug.Log("Course delta: " + delta);
        if (Mathf.Abs(delta) > treshholdEleronVSWheelCorrection)
        {
            Debug.Log("Eleron");
            if (delta > 0)
                TurnToTheLeft();
            else if (delta < 0)
                TurnToTheRight();
        }
        else if (Mathf.Abs(delta) != 0)
        {
            //planeController.Eleron(EleronPosition.Neutral);
            StabilazeRoll(0f);
            float fraction = Mathf.Abs(delta / treshholdEleronVSWheelCorrection);
            fraction = 1f;
            if (delta > acceptableCourseError)
            {
                Debug.Log("Wheel left");
                planeController.HorizontalController(-maxHorizontalWheelInput * fraction);
            }
            else if (delta < -acceptableCourseError)
            {
                Debug.Log("Wheel right");
                planeController.HorizontalController(maxHorizontalWheelInput * fraction);
            }
            else
            {
                planeController.HorizontalController(0);
                Debug.Log("Wheel neutral");
            }

        }
        else
            StabilazeRoll(0f);
    }
    public void StabalizeCourse(Vector3 targerCourse)
    {
        Vector3 targetCourseHorizontal = new Vector3(targerCourse.x, 0, targerCourse.z);
        Vector3 horizontalVelocuty = new Vector3(currentVelocity.x, 0, currentVelocity.z);
        float deltaAzimut = Vector3.Angle(targetCourseHorizontal, horizontalVelocuty);
        //Vector3 deltaAzimutVector = horizontalVelocuty - targetCourseHorizontal;
        if (Vector3.Cross(targetCourseHorizontal, horizontalVelocuty).y < 0)
            deltaAzimut = -deltaAzimut;
        float targetAzimut = Vector3.Angle(Vector3.forward, targetCourseHorizontal);
        if (targetCourseHorizontal.x < 0)
            targetAzimut = -targetAzimut;
        StabalizeCourse(targetAzimut);
        //Debug.Log("Course delta: " + deltaAzimut);
        //if (deltaAzimut > acceptableCourseError)
        //    TurnToTheLeft();
        //else if (deltaAzimut < -acceptableCourseError)
        //    TurnToTheRight();
        //else
        //    StabilazeRoll(0f);

        float targetPitch = Vector3.Angle(targetCourseHorizontal, targerCourse);
        if (targerCourse.y < 0)
            targetPitch = -targetPitch;

        StabalizePitch(targetPitch);
        //float deltaPitch = currentPitch - targetPitch;

        //if (deltaPitch > acceptableCourseError)
        //    GoDown();
        //else if (deltaPitch < -acceptableCourseError)
        //    GoUp();
        //else
        //    StabalizePitch(0f);
    }
    public void TurnToTheLeft()
    {
        //Debug.Log("Turn to the left");
        if (currentRoll > maxRoll)
            planeController.Eleron(EleronPosition.Right);
        else
            planeController.Eleron(EleronPosition.Left);
    }
    public void TurnToTheRight()
    {
        //Debug.Log("Turn to the right");
        if (currentRoll < -maxRoll)
            planeController.Eleron(EleronPosition.Left);
        else
            planeController.Eleron(EleronPosition.Right);
    }
    //private void StabalizeAltitude(float targetAltitude)
    //{
    //    float delta = targetAltitude-currentAltitude;
    //    //Debug.Log("Course delta: " + delta);
    //    if (delta > acceptableAltitudeError)
    //        GoUp();
    //    else if (delta < -acceptableAltitudeError)
    //        GoDown();
    //    else
    //        StabalizePitch();
    //}
    //private void GoUp()
    //{
    //    Debug.Log("Go up. Current pitch:" + currentPitch);
    //    if (currentPitch > 20f)
    //        planeController.HeightController(-1f);
    //    else
    //        planeController.HeightController(1f);
    //}
    //private void GoDown()
    //{
    //    Debug.Log("Go down. Current pitch:" + currentPitch);
    //    if (currentPitch < -20f)
    //        planeController.HeightController(1f);
    //    else
    //        planeController.HeightController(-1f);
    //}
}
