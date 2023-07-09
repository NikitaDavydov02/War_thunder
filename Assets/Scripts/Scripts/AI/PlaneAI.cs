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
    public float maxPitch = 20f;
    public float maxHorizontalInput = 1f;
    public float targetAndCourseAtackTreshhold = 0.95f;

    [SerializeField]
    private Gun gun;

    private Rigidbody rb;

    public PlaneStates planeState = PlaneStates.Landed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //base.Start();
    }
    private Transform target = null;
    private ModuleController targetController = null;

    // Update is called once per frame
    void Update()
    {
        //base.Update();
        //planeController.generalLevel = 0.8f;

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


        if (planeState == PlaneStates.Landed)
            planeState = PlaneStates.TakingOff;
        if (planeState == PlaneStates.TakingOff)
        {
            planeController.generalLevel = 1f;
            StabalizePitch(10f);
            StabilazeRoll(0f);
            if(currentAltitude >= 300f)
            {
                planeState = PlaneStates.FolowingTarget;
                target = MainManager.buttleManager.GetTargetForBlue();
                if (target == null)
                {
                    planeState = PlaneStates.GoingToTheField;
                    return;
                }
                targetController = target.gameObject.GetComponent<ModuleController>();
            }
        }
        if (planeState == PlaneStates.FolowingTarget)
        {
            Vector3 targetSpeed = target.gameObject.GetComponent<Rigidbody>().velocity;
            Vector3 course = target.position - transform.position;
            Vector3 upr = targetSpeed * course.magnitude / 750f;
            course += upr;
            FolowPosition(target.position + upr);
            Vector3 currentSpeed = rb.velocity;
            float dot = Vector3.Dot(currentSpeed.normalized, course.normalized);

            if (course.magnitude < fireDistance && dot>targetAndCourseAtackTreshhold)
                gun.Fire();
            if (!targetController.alive)
            {
                target = MainManager.buttleManager.GetTargetForBlue();
                if (target == null)
                {
                    planeState = PlaneStates.GoingToTheField;
                    return;
                }
                targetController = target.gameObject.GetComponent<ModuleController>();
            }
            if (transform.position.y < 50f)
            {
                planeState = PlaneStates.TakingOff;
            }
        }
        if (planeState == PlaneStates.GoingToTheField)
        {
            FolowPosition(MainManager.buttleManager.GetBlueAirport());
        }
        
       /* if (currentAltitude < 300f && !tookOf)
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
        }*/
    }
    private void FolowPosition(Vector3 position)
    {
        Vector3 course = position - transform.position;
        StabalizeCourse(course);
    }
   
    private void StabilazeRoll(float roll = 0)
    {
        //Debug.Log("Stabilize roll");
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
    private void StabalizeCourseHorizontal(Vector3 targerCourse)
    {
        Vector3 currentHorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float angle = Mathf.Abs(Vector3.Angle(currentHorizontalVelocity, targerCourse));
        //Debug.Log("Course delta: " + angle);
        Vector3 cross = Vector3.Cross(targerCourse, currentHorizontalVelocity);
        if (angle > treshholdEleronVSWheelCorrection)
        {
           // Debug.Log("Eleron");
            if (cross.y > 0)
                TurnToTheLeft();
            else if (cross.y < 0)
                TurnToTheRight();
        }
        else if(angle != 0)
        {
            //planeController.Eleron(EleronPosition.Neutral);
            StabilazeRoll(0f);
            float fraction = Mathf.Abs(angle / treshholdEleronVSWheelCorrection);
            fraction = 1f;
            if (angle < acceptableCourseError)
            {
                planeController.HorizontalController(0);
                //Debug.Log("Wheel neutral");
            }
            else if(cross.y > 0)
            {
                //Debug.Log("Wheel left");
                planeController.HorizontalController(-maxHorizontalInput * fraction);
            }
            else
            {
                //Debug.Log("Wheel right");
                planeController.HorizontalController(maxHorizontalInput * fraction);
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
        StabalizeCourseHorizontal(targetCourseHorizontal);
        //Stabilize course


        //Stabilize pitch

        float targetPitch = Vector3.Angle(targetCourseHorizontal, targerCourse);
        if (targerCourse.y < 0)
            targetPitch = -targetPitch;
        
        StabalizePitch(targetPitch);
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
    private void StabalizeAltitude(float targetAltitude)
    {
        float delta = targetAltitude-currentAltitude;
        //Debug.Log("Course delta: " + delta);
        if (delta > acceptableAltitudeError)
            GoUp();
        else if (delta < -acceptableAltitudeError)
            GoDown();
        else
            StabalizePitch();
    }
    private void GoUp()
    {
       // Debug.Log("Go up. Current pitch:" + currentPitch);
        if (currentPitch > maxPitch)
            planeController.HeightController(maxDown);
        else
            planeController.HeightController(maxUp);
    }
    private void GoDown()
    {
        //Debug.Log("Go down. Current pitch:" + currentPitch);
        if (currentPitch < -maxPitch)
            planeController.HeightController(maxUp);
        else
            planeController.HeightController(maxDown);
    }
}
public enum PlaneStates
{
    Landed,
    TakingOff,
    FolowingTarget,
    GoingToTheField,
    //AtackTarget,
}
