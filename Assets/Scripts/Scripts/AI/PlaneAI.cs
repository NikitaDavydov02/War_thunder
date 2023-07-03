using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAI : MonoBehaviour
{
    [SerializeField]
    private float treshholdRoll = 2f;
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
    

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //base.Start();
    }

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
        Debug.Log("Course: " + currentCourse);

        if (currentAltitude < 200f)
        {
            StabalizePitch(10f);
            StabilazeRoll(0f);
        }
        else
        {
            Vector3 course = new Vector3(320, 2, -776) - transform.position;
            StabalizeCourse(course);
            StabalizePitch(0f);
        }
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
        //Debug.Log("Pitch: " + currentPitch);

        if (currentPitch > pitch + treshholdPitch)
            planeController.HeightController(-1f);
        else if (currentPitch < pitch - treshholdPitch)
            planeController.HeightController(1f);
        else
            planeController.HeightController(0f);
        //    planeController.Eleron(false);
        //if (currentRoll < -treshholdRoll)
        //    planeController.Eleron(true);
        //if(currentRoll>5f)
    }
    private void StabalizeCourse(float targerCourse)
    {
        float delta = currentCourse - targerCourse;
        Debug.Log("Course delta: " + delta);
        if (delta > acceptableCourseError)
            TurnToTheLeft();
        else if (delta < -acceptableCourseError)
            TurnToTheRight();
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
        Debug.Log("Course delta: " + deltaAzimut);
        if (deltaAzimut > acceptableCourseError)
            TurnToTheLeft();
        else if (deltaAzimut < -acceptableCourseError)
            TurnToTheRight();
        else
            StabilazeRoll(0f);
    }
    private void TurnToTheLeft()
    {
        Debug.Log("Turn to the left");
        if (currentRoll > 20f)
            planeController.Eleron(EleronPosition.Right);
        else
            planeController.Eleron(EleronPosition.Left);
    }
    private void TurnToTheRight()
    {
        Debug.Log("Turn to the right");
        if (currentRoll < -20f)
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
        Debug.Log("Go up. Current pitch:" + currentPitch);
        if (currentPitch > 20f)
            planeController.HeightController(0.3f);
        else
            planeController.HeightController(-0.3f);
    }
    private void GoDown()
    {
        Debug.Log("Go down. Current pitch:" + currentPitch);
        if (currentPitch < -20f)
            planeController.HeightController(-0.3f);
        else
            planeController.HeightController(0.3f);
    }
}
