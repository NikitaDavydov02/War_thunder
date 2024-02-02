using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : MonoBehaviour
{
    public float appropriatePositionDelta = 20f;
    public float fireDistance = 2000f;
    public float targetStopDistance = 20f;
    [SerializeField]
    Gun gun;
    public float gunDir = 1f;

    TankController tankController;
    Rigidbody rb;
    //
    public bool IsRed = true;
    public TankState tankState = TankState.Waiting;
    public bool IsActive = false;
    
    //public float 
    public GameObject currentTarget = null;
    public GameObject folowingTarget;
    Queue<Vector3> currentRoute;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        tankController = gameObject.GetComponent<TankController>();
        rb = gameObject.GetComponent<Rigidbody>();
        //GoToRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        //Check for threat
        if (IsActive)
        {
            GameObject possibleFolowingTarget = SearchForAnAppropriateTarget(false);
            if (possibleFolowingTarget != null && folowingTarget!=possibleFolowingTarget)
            {
                FolowTarget(possibleFolowingTarget);
            }

        }

        GameObject enemy = SearchForAnAppropriateTarget();
        if (enemy != null)
        {
            if (TargetIsSeen(enemy))
            {
                currentTarget = enemy;
                ////Debug.Log("Atack");
            }
        }
        
        if (tankState == TankState.Waiting)
            tankController.Gas(0);
        if (tankState == TankState.FolowTarget)
        {
            FolowTargetStep();

        }
        if (tankState == TankState.GoingToPosition)
        {
            if (currentRoute == null || currentRoute.Count == 0)
                tankState = TankState.Waiting;
            GoingByRoute();
        }
        if (currentTarget!=null)
        {
            if (currentTarget.GetComponent<ModuleController>().alive)
            {
                if (SetGunOnTarget(currentTarget) && (currentTarget.transform.position - transform.position).magnitude < fireDistance)
                {
                    tankController.Fire();
                }
            }
            else
            {
                currentTarget = null;
            }
            
        }

        if (folowingTarget != null)
        {
            FolowTargetStep();
        }
        else if (currentRoute != null && currentRoute.Count !=0)
            GoingByRoute();
    }
    private void FolowTargetStep()
    {
        if (folowingTarget == null)
        {
            tankState = TankState.Waiting;
            return;
        }
        if (folowingTarget.GetComponent<PlaneModuleController>() != null)
            return;
        Queue<Vector3> newPath = BuiltRoad(folowingTarget.transform.position);
        newPath.Dequeue();
        if (newPath != null && newPath.Count != 0)
            currentRoute = newPath;
        /*if (currentRoute == null || currentRoute.Count == 0)
        {
            Queue<Vector3> newPath = BuiltRoad(folowingTarget.transform.position);
            if (newPath != null && newPath.Count != 0)
                currentRoute = newPath;
        }*/
        GoingByRoute();
    }
    private void GoingByRoute()
    {
        if (currentRoute == null||currentRoute.Count==0)
        {
            tankState = TankState.Waiting;
            ////Debug.Log("Wait");
            return;
        }
        Vector3 nextPoint = currentRoute.Peek();
        if (FolowPosition(nextPoint)||(currentRoute.Count==1 && (transform.position - nextPoint).magnitude<=targetStopDistance))
        {
            currentRoute.Dequeue();
            if (currentRoute.Count == 0)
            {
                tankController.Gas(0);
                tankState = TankState.Waiting;
                ////Debug.Log("Wait");
                return;
            }
        }
    }
    public void GoToRandomPosition()
    {
        tankState = TankState.GoingToPosition;
        //Debug.Log("Go to random position");
        currentRoute = MainManager.mapAIManager.GetRoadToRandomPosotion(transform.position, IsRed);
        //Debug
        ////Debug.Log("Route is choosen");
        List<Vector3> path = new List<Vector3>();
        foreach (Vector3 p in currentRoute)
            path.Add(p);
        for (int i = 0; i < path.Count - 1; i++)
            Debug.DrawLine(path[i], path[i + 1], Color.green);
    }
    public void GoToPosition(Vector3 pos)
    {
        tankState = TankState.GoingToPosition;
        Queue<Vector3>  path = BuiltRoad(pos);

       if(path!=null && path.Count != 0)
        {
            currentRoute = path;
        }
        else
        {
            tankState = TankState.Waiting;
        }
    }
    private Queue<Vector3> BuiltRoad(Vector3 pos)
    {
        Queue<Vector3> output = MainManager.mapAIManager.FindPath(transform.position, pos);
        

        List<Vector3> path = new List<Vector3>();
        foreach (Vector3 p in output)
            path.Add(p);
        //for (int i = 0; i < path.Count - 1; i++)
            //Debug.DrawLine(path[i], path[i + 1], Color.green, 10);
        return output;
    }
    private bool TargetIsSeen(GameObject target)
    {
        if (target == null)
            return false;
        RaycastHit hit;
        Ray t = new Ray(transform.position + Vector3.up * 6f, target.transform.position + 2*Vector3.up - (transform.position + Vector3.up * 6f));
        
        if(Physics.Raycast(t,out hit))
        {
            //Debug.DrawLine(t.origin, hit.point, Color.cyan);
            ////Debug.Log("Hit seen ray: " + hit.collider.gameObject.name);
            GameObject root = hit.collider.transform.root.gameObject;
            if (root.name == target.name)
            {
                ////Debug.Log("Seen");
                return true;
            }
                
        }
        return false;
    }
    private bool SetGunOnTarget(GameObject target)
    {
        if (target == null)
            return false;
        Vector3 targetVelocity = target.GetComponent<Rigidbody>().velocity;
        //targetVelocity = Vector3.zero;
        Vector3 targetDirection = MainManager.mapAIManager.CalculateGunDirectionOnTarget(target.transform.position - transform.position, gun.curbPrefabs[gun.curbTypeIndex].GetComponent<Curb>().speedScalyar, targetVelocity);

        float distance = (target.transform.position - gun.transform.position).magnitude;
        Vector3 currentGunDirection = gun.transform.InverseTransformDirection(gun.transform.TransformDirection(Vector3.forward));
        Vector3 targetGunDirection = gun.transform.InverseTransformDirection(targetDirection);
        Debug.DrawLine(gun.transform.position, gun.transform.position + targetDirection * distance, Color.black);
        Debug.DrawLine(gun.transform.position, gun.transform.position + gun.transform.TransformDirection(Vector3.forward * distance), Color.yellow);
        Debug.DrawLine(gun.transform.position, gun.transform.position + gun.transform.TransformDirection(targetGunDirection) * distance*0.99f, Color.green);
        /*Vector3 currentGunDirection = gun.transform.InverseTransformDirection(gun.transform.TransformDirection(Vector3.forward));
        Debug.DrawLine(gun.transform.position, gun.transform.position + gun.transform.TransformDirection(Vector3.forward*6), Color.green);
        Vector3 targetGunDirection = gun.transform.InverseTransformDirection(target.transform.position - gun.transform.position);
        Debug.DrawLine(gun.transform.position, gun.transform.position + (target.transform.position - gun.transform.position), Color.cyan);
        float timeOfFlying = targetGunDirection.magnitude / gun.curbPrefabs[gun.curbTypeIndex].GetComponent<Curb>().speedScalyar;
        Vector3 targetVelocity = target.GetComponent<Rigidbody>().velocity;
        Vector3 delta = new Vector3(0,-2-9.81f * timeOfFlying * timeOfFlying / 2, 0)-targetVelocity*timeOfFlying;
        targetGunDirection = gun.transform.InverseTransformDirection(-delta+target.transform.position - gun.transform.position);
        Debug.DrawLine(gun.transform.position, gun.transform.position + (-delta + target.transform.position - gun.transform.position), Color.cyan);*/



        Vector3 crossCourse = Vector3.Cross(targetGunDirection.normalized, currentGunDirection.normalized);
        //Debug.Log("Cross gun: " + crossCourse);
        if (crossCourse.magnitude > maxAppropriateCrossProduc(distance,2f))
        {           
            float fraction = 1f;
            float angle = Vector3.Angle(targetGunDirection.normalized, currentGunDirection.normalized);
            if (angle < 5)
            {
                fraction = Mathf.Abs(angle / 5);
            }
            ////Debug.Log("Not fire");
            ////Debug.Log("Fraction: "+fraction);
            if (crossCourse.y > 0)
                tankController.RotateTower(-1*fraction);
            else
                tankController.RotateTower(1 * fraction);
            //Vertial rotation
            if (crossCourse.x > 0)
            {
                //Up
                ////Debug.Log("Rotate up");
                tankController.RotateGun(-gunDir * fraction);
            }
            else
            {
                ////Debug.Log("Rotate down");
                tankController.RotateGun(gunDir * fraction);
            }
                
            return false;
        }
        else
        {
            ////Debug.Log("fire");
            return true;
        }
    }
    private GameObject SearchForAnAppropriateTarget(bool checkToBeSeen =true)
    {
        List<GameObject> targets = new List<GameObject>();
        if (IsRed)
        {
            foreach (GameObject g in MainManager.buttleManager.allblue)
                if (g.GetComponent<ModuleController>().alive)
                    targets.Add(g);
        }
        else
        {
            foreach (GameObject g in MainManager.buttleManager.allred)
                if (g.GetComponent<ModuleController>().alive)
                    targets.Add(g);
        }
        if (targets.Count == 0)
            return null;
        GameObject nearestTarget = null;
        float disctanceToTarget = 100000;
        foreach(GameObject g in targets)
            if ((g.transform.position - transform.position).magnitude < disctanceToTarget && ((TargetIsSeen(g)&&checkToBeSeen)||!checkToBeSeen))
            {
                disctanceToTarget = (g.transform.position - transform.position).magnitude;
                nearestTarget = g;
            }
        return nearestTarget;
    }
    public void FolowTarget(GameObject target)
    {
        tankState = TankState.FolowTarget;
        folowingTarget = target;
        currentRoute = null;
        //Debug.Log("Folow target comand received");
        //s
    }
    private bool FolowPosition(Vector3 pos)
    {
        if ((transform.position - pos).magnitude < appropriatePositionDelta)
            return true;
        Vector3 currentCourse = transform.TransformDirection(Vector3.forward);
        Vector3 targetCourse = pos - transform.position;
        Vector3 courseCross = Vector3.Cross(targetCourse.normalized, currentCourse.normalized);
        ////Debug.Log("Cross: " + courseCross);
        ////Debug.Log("Max cross: " + maxAppropriateCrossProduc(targetCourse.magnitude, appropriatePositionDelta));
        if(Mathf.Abs(courseCross.y) > maxAppropriateCrossProduc(targetCourse.magnitude,appropriatePositionDelta))
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
            ////Debug.Log("Gas");
            //Gas
            tankController.Rotate(0);
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
    GoingToPosition,
    FolowTarget,
}

