using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAI : MonoBehaviour
{
    [SerializeField]
    private float treshholdRoll = 2f;
    [SerializeField]
    private float treshholdEleronVSWheelCorrection = 10f;
    [SerializeField]
    private float treshholdPitch = 2f;
    [SerializeField]
    private PlaneController planeController;
    [SerializeField]
    private float acceptableCourseError = 5f;
    [SerializeField]
    private float acceptableAltitudeError = 50f;

    private float currentRoll = 0f;
    private float currentPitch= 0f;
    private float currentAltitude = 0f;
    private Vector3 currentVelocity = Vector3.zero;
    private float currentCourse = 0f;
    public float fireDistance = 300f;

    public float maxUp = 0.1f;
    public float neutralHeightWheel = -0.3f;
    public float maxDown = 1f;
    public float maxRoll = 30f;
    public float maxHorizontalInput = 1f;

    [SerializeField]
    private Gun gun;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //base.Start();
    }
    bool tookOf = false;

    // Update is called once per frame
    void Update()
    {
        //base.Update();
        planeController.generalLevel = 0.8f;

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
        //Debug.Log("Course: " + currentCourse);

        if (currentAltitude < 300f && !tookOf)
        {
            StabalizePitch(10f);
            StabilazeRoll(0f);
        }
        else
        {
            tookOf = true;
            Transform target = MainManager.buttleManager.allred[0].transform;
            Vector3 targetSpeed = target.gameObject.GetComponent<Rigidbody>().velocity;
            Vector3 course = target.position - transform.position;
            Vector3 upr = targetSpeed * course.magnitude / 750f;
            course += upr;
            
            if (course.magnitude < fireDistance)
                gun.Fire();
            StabalizeCourse(course);
            //StabalizePitch(0f);
        }
    }
    private void StabilazeRoll(float roll = 0)
    {
        Debug.Log("Stabilize roll");
        //Debug.Log("Roll: " + currentRoll);
        
        if (currentRoll > roll + treshholdRoll)
            planeController.Eleron(EleronPosition.Right);
        else if (currentRoll < roll - treshholdRoll)
            planeController.Eleron(EleronPosition.Left);
        else
            planeController.Eleron(EleronPosition.Neutral);
    }
    private void StabalizePitch(float pitch=0f)
    {
        //Debug.Log("Stabilize pitch");
        //Debug.Log("Input" + Input.GetAxis("Mouse Y"));
        //Debug.Log("Pitch: " + currentPitch);
       // Debug.Log("Targe pitch:" + pitch);

        if (currentPitch > pitch + treshholdPitch)
        {
            planeController.HeightController(maxDown);
            //Debug.Log("Down");
        }
        else if (currentPitch < pitch - treshholdPitch)
        {
            planeController.HeightController(maxUp);
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
    private void StabalizeCourse(float targerCourse)
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
        else if(Mathf.Abs(delta)!=0)
        {
            //planeController.Eleron(EleronPosition.Neutral);
            StabilazeRoll(0f);
            float fraction = Mathf.Abs(delta / treshholdEleronVSWheelCorrection);
            fraction = 1f;
            if (delta > acceptableCourseError)
            {
                Debug.Log("Wheel left");
                planeController.HorizontalController(-maxHorizontalInput*fraction);
            }
            else if (delta < -acceptableCourseError)
            {
                Debug.Log("Wheel right");
                planeController.HorizontalController(maxHorizontalInput*fraction);
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
    private void StabalizeCourse(Vector3 targerCourse)
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
    private void TurnToTheLeft()
    {
        //Debug.Log("Turn to the left");
        if (currentRoll > maxRoll)
            planeController.Eleron(EleronPosition.Right);
        else
            planeController.Eleron(EleronPosition.Left);
    }
    private void TurnToTheRight()
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
