using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAICorpus : Corpuse {
    public Vector3 currentTargetPoint;
    public GameObject currentTargetObject;
    public bool taskComplited = true;
    public bool angleSelected = false;
    public float pogreshnost = 10f;
    public float anglePogreshnost = 5f;
    public int k = 0;
    public bool test = false;
    public float currentSpeed;
    public float currentEnemySpeed;
    private Vector3 lastPos;
    Vector3 lastEnemyPosition;
    private float lastAngleY;

    public Road currentRoad;
    public int endRoadPoint;
    public int startRoadPoint;
    public int currentTargetRoadPoint;
    public int storonaDvigeniya;
    public TankWanderingAI tankWanderingAI;
    public WanderingAIBank bankWanderingAI;

    public bool stop=false;

    // Use this for initialization
    void Start () {
        //lastPos = transform.position;
        taskComplited = true;
        anglePogreshnost = 2.5f;
        pogreshnost = 7.5f;
        Debug.Log("ЕС");
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit forwardHit;
        if(Physics.Raycast(new Ray(transform.parent.transform.position, transform.parent.transform.forward), out forwardHit))
        {
            if (forwardHit.transform.tag == "Tank" && forwardHit.distance<50f)
            {
                stop = true;
                Debug.Log("Замри: " + transform.parent.gameObject.name);
            }
            else
            {
                stop = false;
                Debug.Log("Отомри: " + transform.parent.gameObject.name);
            }
        }


        Vector2 xyTarget;
        if (currentTargetPoint!=Vector3.zero)
            xyTarget = new Vector2(currentTargetPoint.x, currentTargetPoint.z);
        else
        {
            if (currentTargetObject != null)
            {
                ModuleController moduleController = currentTargetObject.GetComponent<ModuleController>();
                if (moduleController != null)
                {
                    if (!moduleController.alive)
                    {
                        //tankWanderingAI.NewRoad();
                        taskComplited = true;
                        return;
                    }
                }
                xyTarget = new Vector2(currentTargetObject.transform.position.x, currentTargetObject.transform.position.z);
            }
            else
                return;
        }
        if (currentRoad != null)
        {
            currentTargetPoint = currentRoad.controlPoints[currentTargetRoadPoint];
            xyTarget = new Vector2(currentTargetPoint.x, currentTargetPoint.z);
        }
        Vector2 xyPos = new Vector2(transform.parent.transform.position.x, transform.parent.transform.position.z);
        Vector2 xyDelta = xyTarget - xyPos;
        float delta = Mathf.Abs(xyDelta.magnitude);
        if (delta < pogreshnost/1.2f)
        {
            taskComplited = true;
            //tankWanderingAI.NewRoad();
            if (currentRoad != null)
            {
                if (currentTargetRoadPoint != endRoadPoint)
                {
                    currentTargetRoadPoint += storonaDvigeniya;
                    taskComplited = false;
                }

             }
        }
        if (currentTargetObject != null)
        {
            if (delta < 100)
            {
                taskComplited = true;
                Debug.Log("Enemy is near");
            }
        }
        currentSpeed = Mathf.Abs((transform.position - lastPos).magnitude) / Time.deltaTime;
        //Vector3 enemyPeremeshenie;
        //if (tankWanderingAI.tower.target != null && lastEnemyPosition!=Vector3.zero)
           // Vector3 enemyPer = 
        currentRotSpeed = Mathf.Abs(transform.eulerAngles.y - lastAngleY) / Time.deltaTime;
        lastPos = transform.position;
        if (tankWanderingAI.tower.target != null)
            lastEnemyPosition = tankWanderingAI.tower.target.transform.position;
        lastAngleY = transform.eulerAngles.y;
        if (!taskComplited)
        {
            Vector2 direction = transform.parent.transform.TransformDirection(xyDelta);
            Vector2 xyForward;
            //Vector2 xyForward = new Vector3(transform.forward.x, transform.forward.z) * Mathf.Abs(xyDelta.magnitude);
            xyForward = new Vector3(transform.parent.transform.forward.x, transform.parent.transform.forward.z) * Mathf.Abs(xyDelta.magnitude);
            if (Mathf.Abs(xyForward.x-xyDelta.x)<=anglePogreshnost|| Mathf.Abs(xyForward.y - xyDelta.y) <= anglePogreshnost)
                angleSelected = true;
            else
                angleSelected = false;

            float distanceToPoint = Mathf.Abs(Vector3.Magnitude(currentTargetPoint - transform.position));
            if (currentRoad != null && distanceToPoint == 100f)
                angleSelected = false;

            if (angleSelected)
            {
                if (currentSpeed < maxSpeed)
                {
                    float ahead = 1000000 * Time.deltaTime * speed;
                    Vector3 movement = new Vector3(0, 0, ahead);
                    //movement = transform.TransformDirection(movement);
                    movement = transform.parent.transform.TransformDirection(movement);
                    RaycastHit raycastHit;
                    float distaseToTareget = Mathf.Abs(Vector3.Magnitude(bankWanderingAI.target.transform.position - transform.position));
                    //if(!tankWanderingAI.stop&& distaseToTareget>100f)
                    if (!stop && distaseToTareget > 100f)
                        Move(movement);
                }
            }
            else
            {
                float sinA = xyDelta.x / xyDelta.magnitude;
                float sinB = transform.parent.transform.forward.x / transform.parent.transform.forward.magnitude;
                float cosA = xyDelta.y / xyDelta.magnitude;
                float cosB = transform.parent.transform.forward.z / transform.parent.transform.forward.magnitude;
                float sin = (sinA * cosB) - (cosA * sinB);
                if (sin > 0)
                    k = 1;
                else
                    k = -1;
                Turn(k*60000);
                //Debug.Log("Turn");
            }
        }
    }
    public bool NewRoadTask(Road road, int startPoint, int endPoint)
    {
        //if (!taskComplited)
         //   return false;
        //else
        {
            currentTargetObject = null;
            currentRoad = road;
            endRoadPoint = endPoint;
            startRoadPoint = startPoint;
            currentTargetPoint = road.controlPoints[startPoint];
            taskComplited = false;
            if (endPoint > startPoint)
                storonaDvigeniya = 1;
            else
                storonaDvigeniya = -1;
            currentTargetRoadPoint = startPoint;
            return true;
        }
    }
    public bool NewPointTask(Vector3 newTarget)
    {
        if (!taskComplited)
            return false;
        else
        {
            currentTargetObject = null;
            currentTargetPoint = newTarget;
            taskComplited = false;
            return true;
        }
    }
    public bool NewGameObjectTask(GameObject newObject)
    {
        //if (!taskComplited)
         //   return false;
       // else
        {
            currentTargetObject = newObject;
            currentTargetPoint = new Vector3(0,0,0);
            taskComplited = false;

            //List<Road> allRoads = MainManager.roadManager.roads;
            //float minDistance = 100000;
            //Road roadWithEndPoint = null;
            //int endPointIndex = 0;
            //Road roadWithStartPoint = null;
            //int startPointIndex = 0;
            //foreach (Road road in allRoads)
            //{
            //    for(int i = 0; i < road.controlPoints.Count; i++)
            //    {
            //        float d = Mathf.Abs(Vector3.Magnitude(road.controlPoints[i] - currentTargetObject.transform.position));
            //        if (d < minDistance)
            //        {
            //            minDistance = d;
            //            roadWithEndPoint = road;
            //            endPointIndex = i;
            //        }
            //    }
            //}
            //minDistance = 100000;
            //foreach (Road road in allRoads)
            //{
            //    for (int i = 0; i < road.controlPoints.Count; i++)
            //    {
            //        float d = Mathf.Abs(Vector3.Magnitude(road.controlPoints[i] - transform.position));
            //        if (d < minDistance)
            //        {
            //            minDistance = d;
            //            roadWithStartPoint = road;
            //            startPointIndex = i;
            //        }
            //    }
            //}
            //currentRoad = roadWithEndPoint;
            //endRoadPoint = endPointIndex;
            //startRoadPoint = startPointIndex;
            //currentTargetPoint = roadWithEndPoint.controlPoints[endPointIndex];
            //taskComplited = false;
            //if (endPointIndex > startPointIndex)
            //    storonaDvigeniya = 1;
            //else
            //    storonaDvigeniya = -1;
            //currentTargetRoadPoint = startPointIndex;



            return true;
        }
    }

    //private bool KudaPovorachivat(Vector2 forward, Vector2 deltaV)
    //{
    //    
    //}
}
