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
    [SerializeField]
    private float criticalAngle = 5f;

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
    public float fireErrorMultiplyer = 10f;
    public float GunErrorVert = 2f;

    [SerializeField]
    private List<Gun> gun;

    private Rigidbody rb;

    [SerializeField]
    public PlaneStates planeState = PlaneStates.Landed;
    public bool IsRed = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //base.Start();
    }
    [SerializeField]
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
        ////Debug.Log("Course: " + currentCourse);
        if (transform.position.y < 75f)
        {
            planeState = PlaneStates.TakingOff;
            Debug.Log("Take off!");
        }

        if (planeState == PlaneStates.Landed)
            planeState = PlaneStates.TakingOff;
        if (planeState == PlaneStates.TakingOff)
        {
            planeController.generalLevel = 1f;
            StabalizePitch(10f);
            StabilazeRoll(0f);
            if(currentAltitude >= 500f)
            {
                
                
                if (target == null || !targetController.alive)
                {
                    if (IsRed)
                        target = MainManager.buttleManager.GetTargetForRed();
                    else
                        target = MainManager.buttleManager.GetTargetForBlue();
                    if(target==null)
                        planeState = PlaneStates.GoingToTheField;
                    else
                        targetController = target.gameObject.GetComponent<ModuleController>();
                    planeState = PlaneStates.FolowingTarget;
                    if (planeController.ReleaseableBombs && target!=null && target.GetComponent<TankController>() != null)
                        planeState = PlaneStates.BombingTarget;
                }
                else
                    planeState = PlaneStates.FolowingTarget;

            }
        }
        if (planeState == PlaneStates.BombingTarget)
        {
            Vector3 targetSpeed = target.gameObject.GetComponent<Rigidbody>().velocity;
            float distance = (target.position - gun[0].transform.position).magnitude;
            float planeFlightTime = distance / currentVelocity.magnitude;
           

            float relativeAltitude = transform.position.y - target.position.y;
            float bombFlightTime = Mathf.Sqrt(2 * relativeAltitude / 9.81f);

            Vector3 targetDelta = targetSpeed * bombFlightTime;
            Vector3 followingPosition = target.position + targetDelta + Vector3.up * 1000;
            FolowPosition(followingPosition);

            Vector3 targetPlaneDelta = target.position - transform.position;
            targetPlaneDelta.y = 0;
            Vector3 horVelocity = currentVelocity;
            horVelocity.y = 0;

            float criticalHorizontalDistance = horVelocity.magnitude * bombFlightTime;

            Debug.Log("Bombing: relativeAltitude = " + relativeAltitude);
            Debug.Log("Bombing: bombFlighTime = " + bombFlightTime);
            Debug.Log("Bombing: criticalDistance = " + criticalHorizontalDistance);
            Debug.DrawLine(transform.position, followingPosition, Color.cyan);
            Debug.Log("Bombing: distance ahead = " + targetPlaneDelta.magnitude);
            if (targetPlaneDelta.magnitude <= criticalHorizontalDistance)
                planeController.ReleaseBombs();
                //Release

        }
        if (planeState == PlaneStates.FolowingTarget)
        {
            Vector3 targetSpeed = target.gameObject.GetComponent<Rigidbody>().velocity;
            //Vector3 course = target.position - gun.transform.position;
            float distance = (target.position - gun[0].transform.position).magnitude;
            Vector3 course = MainManager.mapAIManager.CalculateGunDirectionOnTarget(target.position - gun[0].transform.position + Vector3.up * GunErrorVert, 750, targetSpeed) * distance;
            //float delta = (9.81f / 2) * (course.magnitude / 750) * (course.magnitude / 750);
            //Vector3 upr = targetSpeed * course.magnitude / 750f + Vector3.up*delta;
           // Debug.DrawLine(gun.transform.position, gun.transform.position + course, Color.cyan);
            //course += upr;
            //Debug.DrawLine(gun.transform.position, gun.transform.position + course, Color.cyan);
            FolowPosition(gun[0].transform.position + course);
            Vector3 currentSpeed = rb.velocity;
            float dot = Vector3.Dot(currentSpeed.normalized, course.normalized);
            float angle = Vector3.Angle(transform.TransformDirection(Vector3.forward), course.normalized);
            float maxAngle = maxAngleCount(course.magnitude);
           // //Debug.Log("Max angle: " + maxAngle);
            float minDot = Mathf.Cos(maxAngle * Mathf.Deg2Rad);
            ////Debug.Log("Angle: " + angle);
            if (course.magnitude < fireDistance && angle< fireErrorMultiplyer * maxAngle)
                foreach(Gun g in gun)
                    g.Fire();
            if (!targetController.alive)
            {
                if (IsRed)
                    target = MainManager.buttleManager.GetTargetForRed();
                else
                    target = MainManager.buttleManager.GetTargetForBlue();
                if (target == null)
                {
                    planeState = PlaneStates.GoingToTheField;

                    return;
                }
                targetController = target.gameObject.GetComponent<ModuleController>();
            }
            if (transform.position.y < 75f)
            {
                planeState = PlaneStates.TakingOff;
                Debug.Log("Take off!");
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
    private float maxAngleCount(float distance)
    {
        return Mathf.Atan2(5, distance)*Mathf.Rad2Deg;
    }
    private void FolowPosition(Vector3 position)
    {
        Vector3 course = position - gun[0].transform.position;
        StabalizeCourse(course);
    }
   
    private void StabilazeRoll(float roll = 0)
    {
        ////Debug.Log("Stabilize roll");
        ////Debug.Log("Roll: " + currentRoll);
        
        if (currentRoll > roll + treshholdRoll)
            planeController.Eleron(-1);
        else if (currentRoll < roll - treshholdRoll)
            planeController.Eleron(1);
        else
            planeController.Eleron(0);
    }
    private void StabalizePitch(float pitch=0f, float maxError=0)
    {
        ////Debug.Log("Stabilize pitch");
        ////Debug.Log("Input" + Input.GetAxis("Mouse Y"));
        ////Debug.Log("Pitch: " + currentPitch);
        ////Debug.Log("Targe pitch:" + pitch);
        float delta = Mathf.Abs(currentPitch - pitch);
        ////Debug.Log("Pitch delta: " + delta);
        ////Debug.Log("Max error: " + maxError);
        float fraction = delta / 15;
        if (maxError == 0)
            maxError = treshholdPitch;

        if (currentPitch > pitch + maxError)
        {
            planeController.HeightController((maxDown)*fraction);
            ////Debug.Log("Down");
        }
        else if (currentPitch < pitch - maxError)
        {
            planeController.HeightController((maxUp ) * fraction);
            ////Debug.Log("Up");
        }
        else
        {
            planeController.HeightController(neutralHeightWheel);
            ////Debug.Log("Neutral");
        }
            
        //    planeController.Eleron(false);
        //if (currentRoll < -treshholdRoll)
        //    planeController.Eleron(true);
        //if(currentRoll>5f)
    }
    private void StabalizeCourseHorizontal(Vector3 targerCourseInPlaneCoordinates, float maxError=0)
    {


        Vector3 currentHorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        //float angle = Mathf.Abs(Vector3.Angle(currentHorizontalVelocity, targerCourse));
        float angle = Mathf.Abs(Vector3.Angle(Vector3.forward, targerCourseInPlaneCoordinates));
        ////Debug.Log("Horizontal delta: " + angle);
        Vector3 cross = Vector3.Cross(Vector3.forward, currentHorizontalVelocity);
        if (angle > treshholdEleronVSWheelCorrection)
        {
           // //Debug.Log("Eleron");
            if (targerCourseInPlaneCoordinates.x < 0)
                TurnToTheLeft(1);
            else if (targerCourseInPlaneCoordinates.x > 0)
                TurnToTheRight(1);
        }
        else if(angle != 0)
        {
            //planeController.Eleron(EleronPosition.Neutral);
            StabilazeRoll(0f);
            float fraction = Mathf.Abs(angle / treshholdEleronVSWheelCorrection);
            fraction = 1f;
            if (maxError == 0)
                maxError = acceptableCourseError;
            if (angle < maxError)
            {
                planeController.HorizontalController(0);
                ////Debug.Log("Wheel neutral");
            }
            else if(targerCourseInPlaneCoordinates.x < 0)
            {
                ////Debug.Log("Wheel left");
                planeController.HorizontalController(-maxHorizontalInput * fraction);
            }
            else
            {
                ////Debug.Log("Wheel right");
                planeController.HorizontalController(maxHorizontalInput * fraction);
            }
        }
        else
            StabilazeRoll(0f);
    }
    private void StabalizeCourse(Vector3 targerCourse)
    {
        float maxAngleError = maxAngleCount(targerCourse.magnitude);

        Vector3 targetCourseInPlaneCoordinates = gun[0].transform.InverseTransformDirection(targerCourse);
        ////Debug.Log("Target course in world coordinates: " + targerCourse);
        ////Debug.Log("Target course in plane coordinates: " + targetCourseInPlaneCoordinates);
        Vector3 targetCourseHorizontal = new Vector3(targetCourseInPlaneCoordinates.x, 0, targetCourseInPlaneCoordinates.z);
        float deltaAzimut = Vector3.Angle(Vector3.forward, targetCourseHorizontal);
        ////Debug.Log"Delta azimut: " + deltaAzimut);
        //if (Vector3.Cross(targetCourseHorizontal, Vector3.forward).y < 0)
            //deltaAzimut = -deltaAzimut;

       // float targetAzimut = Vector3.Angle(Vector3.forward, targetCourseHorizontal);
        if (targetCourseHorizontal.x < 0)
            deltaAzimut = -deltaAzimut;

        Vector3 targetCourseHorizontalInWorldCoordinates = new Vector3(targerCourse.x, 0, targerCourse.z);
        float targetPitch = Vector3.Angle(targetCourseHorizontalInWorldCoordinates, targerCourse);
        if (targerCourse.y < 0)
            targetPitch = -targetPitch;
        if (Mathf.Abs(deltaAzimut) < criticalAngle && Mathf.Abs(targetPitch)< criticalAngle)
        {
            transform.LookAt(gun[0].transform.position + targerCourse);
            Debug.Log("Stabalize");
            return;
        }
         
        StabalizeCourseHorizontal(targetCourseHorizontal, maxAngleError);

        /*Vector3 targetCourseHorizontal = new Vector3(targerCourse.x, 0, targerCourse.z);
        Vector3 horizontalVelocuty = new Vector3(currentVelocity.x, 0, currentVelocity.z);
        float deltaAzimut = Vector3.Angle(targetCourseHorizontal, horizontalVelocuty);
        //Vector3 deltaAzimutVector = horizontalVelocuty - targetCourseHorizontal;
        if (Vector3.Cross(targetCourseHorizontal, horizontalVelocuty).y < 0)
            deltaAzimut = -deltaAzimut;
        float targetAzimut = Vector3.Angle(Vector3.forward, targetCourseHorizontal);
        if (targetCourseHorizontal.x < 0)
            targetAzimut = -targetAzimut;
        StabalizeCourseHorizontal(targetCourseHorizontal, maxAngleError);*/
        //Stabilize course


        //Stabilize pitch

        
        
        
        StabalizePitch(targetPitch, maxAngleError);
    }
    private void TurnToTheLeft(float degree)
    {
        ////Debug.Log("Turn to the left");
        if (currentRoll > maxRoll)
        {
            //planeController.HorizontalController(maxHorizontalInput);
            planeController.Eleron(-1);
        }
        else
        {
            planeController.HorizontalController(-maxHorizontalInput);
            planeController.Eleron(1);
        }
            
    }
    private void TurnToTheRight(float degree)
    {
        ////Debug.Log("Turn to the right");
        if (currentRoll < -maxRoll)
            planeController.Eleron(1);
        else
        {
            planeController.HorizontalController(maxHorizontalInput);
            planeController.Eleron(-1);
        }
            
    }
    private void StabalizeAltitude(float targetAltitude)
    {
        float delta = targetAltitude-currentAltitude;
        ////Debug.Log("Course delta: " + delta);
        if (delta > acceptableAltitudeError)
            GoUp();
        else if (delta < -acceptableAltitudeError)
            GoDown();
        else
            StabalizePitch();
    }
    private void GoUp()
    {
       // //Debug.Log("Go up. Current pitch:" + currentPitch);
        if (currentPitch > maxPitch)
            planeController.HeightController(maxDown);
        else
            planeController.HeightController(maxUp);
    }
    private void GoDown()
    {
        ////Debug.Log("Go down. Current pitch:" + currentPitch);
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
    BombingTarget,
    //AtackTarget,
}
