using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : MonoBehaviour
{
    public float appropriatePositionDelta = 20f;

    TankController tankController;
    Rigidbody rb;
    public TankState tankState = TankState.Waiting;
    public float fireDistance = 2000f;
    //public float 
    public GameObject currentTarget = null;
    [SerializeField]
    Gun gun;
    Queue<Vector3> currentRoute;
    // Start is called before the first frame update
    void Start()
    {
        tankController = gameObject.GetComponent<TankController>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tankState == TankState.Waiting)
        {
            GameObject target = SearchForAnAppropriateTarget();
            if(TargetIsSeen(target))
            {
                currentTarget = target;
                tankState = TankState.Atacking;
            }
        }
        if (tankState == TankState.Exploring)
        {
            //Find trget
            //if thread -> atack it or hide
            //Go to unexplored point

        }
        if (tankState == TankState.GoingToPosition)
        {
            Vector3 nextPoint = currentRoute.Peek();
            if (FolowPosition(nextPoint))
            {
                currentRoute.Dequeue();
                if (currentRoute.Count == 0)
                {
                    tankState = TankState.Waiting;
                    return;
                }
            }
        }
        if (tankState == TankState.Atacking)
        {
            if (currentTarget == null)
            {
                tankState = TankState.Waiting;
                return;
            }
            if (SetGunOnTarget(currentTarget) && (currentTarget.transform.position - transform.position).magnitude < fireDistance)
            {
                tankController.Fire();
            }
        }
    }
    public void GoToRandomPosition()
    {
        tankState = TankState.GoingToPosition;
        currentRoute = MainManager.mapAIManager.GetRoadToRandomPosotion(transform.position);
    }
    public void GoToPosition(Vector3 pos)
    {
        tankState = TankState.GoingToPosition;
        currentRoute = MainManager.mapAIManager.FindPath(transform.position, pos);
    }
    private bool TargetIsSeen(GameObject target)
    {
        return true;
        RaycastHit hit;
        Ray t = new Ray(transform.position + Vector3.up * 6f, target.transform.position + Vector3.up - (transform.position + Vector3.up * 6f));
        
        if(Physics.Raycast(t,out hit))
        {
            Debug.DrawLine(t.origin, hit.point, Color.cyan);
            Debug.Log("Hit seen ray: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.name == target.name)
                return true;
        }
        return false;
    }
    private bool SetGunOnTarget(GameObject target)
    {
        //Vector3 currentGunDirection = gun.transform.TransformDirection(Vector3.forward);
        Vector3 currentGunDirection = transform.InverseTransformDirection(gun.transform.TransformDirection(Vector3.forward));
        Debug.Log("Current gun direction: " + currentGunDirection.normalized);
        Vector3 targetGunDirection = transform.InverseTransformDirection(target.transform.position - gun.transform.position);
        float timeOfFlying = targetGunDirection.magnitude / gun.curbPrefabs[gun.curbTypeIndex].GetComponent<Curb>().speedScalyar;
        Vector3 delta = new Vector3(0,2-9.81f * timeOfFlying * timeOfFlying / 2, 0);
        targetGunDirection = transform.InverseTransformDirection(-delta+target.transform.position - gun.transform.position);
        Debug.Log("Target direction: " + targetGunDirection.normalized);
        
        //Vector3 delta = new Vector3(0, -9.81f * timeOfFlying * timeOfFlying / 2, 0);
        //targetGunDirection = targetGunDirection + delta;


        Vector3 crossCourse = Vector3.Cross(targetGunDirection.normalized, currentGunDirection.normalized);
        Debug.Log("Cross: " + crossCourse);
        if (crossCourse.magnitude > maxAppropriateCrossProduc(targetGunDirection.magnitude,2f))
        {            float fraction = 1f;
            float angle = Vector3.Angle(targetGunDirection.normalized, currentGunDirection.normalized);
            if (angle < 1)
            {
                fraction = angle / 1;
            }
            Debug.Log("Not fire");
            Debug.Log("Fraction: "+fraction);
            if (crossCourse.y > 0)
                tankController.RotateTower(-1*fraction);
            else
                tankController.RotateTower(1 * fraction);
            //Vertial rotation
            if (crossCourse.x > 0)
            {
                //Up
                tankController.RotateGun(1 * fraction);
            }
            else
                tankController.RotateGun(-1 * fraction);
            return false;
        }
        else
        {
            Debug.Log("fire");
            return true;
        }
    }
    private GameObject SearchForAnAppropriateTarget()
    {
        List<GameObject> targets = new List<GameObject>();
        foreach (GameObject g in MainManager.buttleManager.allred)
            if (g.GetComponent<ModuleController>().alive && g.GetComponent<Technic>().Type != TechnicsType.Plane)
                targets.Add(g);
        if (targets.Count == 0)
            return null;
        GameObject nearestTarget = null;
        float disctanceToTarget = 100000;
        foreach(GameObject g in targets)
            if ((g.transform.position - transform.position).magnitude < disctanceToTarget)
            {
                disctanceToTarget = (g.transform.position - transform.position).magnitude;
                nearestTarget = g;
            }
        return nearestTarget;
    }
    private bool FolowPosition(Vector3 pos)
    {
        if ((transform.position - pos).magnitude < appropriatePositionDelta)
            return true;
        Vector3 currentCourse = transform.TransformDirection(Vector3.forward);
        Vector3 targetCourse = pos - transform.position;
        Vector3 courseCross = Vector3.Cross(targetCourse.normalized, currentCourse.normalized);
        if(courseCross.magnitude> maxAppropriateCrossProduc(targetCourse.magnitude,appropriatePositionDelta))
        {
            //Rotate corpuse
            if (courseCross.y > 0)
            {
                //Turn to the left
                tankController.Rotate(-1);
            }
            else
            {
                tankController.Rotate(1);
            }
        }
        else
        {
            //Gas
            tankController.Gas(1);
        }
        return false;
    }
    public float maxAppropriateCrossProduc(float distance, float size)
    {
        Vector3 v = new Vector3(distance, size,0);
        Vector3 v2 = new Vector3(1, 0,0);
        Vector3 cross = Vector3.Cross(v.normalized, v2);
        return Mathf.Abs(cross.magnitude);
    }
}
public enum TankState { 
    Waiting,
    Exploring,
    GoingToPosition,
    Atacking,
}

