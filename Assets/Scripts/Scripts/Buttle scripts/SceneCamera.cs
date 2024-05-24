using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCamera : MonoBehaviour {
    //REFACTORED_1
    private ModuleController controller;
    private Transform gun;
    private TechnicsType typeOfTarget;

    private List<Transform> playerAndHisFiredCurbs;
    
    
    //[SerializeField]
    //private Material tex;
    public float rotSpeeed = 1.5f;
    public Vector3 _zoomOffset;


    private float _rotY;
    private Vector3 _offset;
    //private Vector3 _memoryOffset;
    [SerializeField]
    private CameraState camState;
    public float vertSpeed = 3f;
    public float maxYOffset = 5f;
    public float minYOffset = -15f;
    public float maxDistance = 20f;
    public float minDistance = 5f;
    public float zoomSpeed = 5f;
    private float distance;
    public float rotXYSpeed = 10f;
    private float lastDeathRotXY = 0;
    private Camera cam;
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float zoomIn = 14f;
    [SerializeField]
    private float zoomOut = 60f;


    //private bool  bulletCam =false;
    //private Transform lastBullet;
    // Use this for initialization
    void Awake () {
        playerAndHisFiredCurbs = new List<Transform>();
        cam = this.GetComponent<Camera>();
        cam.farClipPlane = 4000;
        _rotY = transform.eulerAngles.y;
        _offset = new Vector3(0, -5, 10);
        distance = _offset.magnitude;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (target==null)
            return;
        if (camState==CameraState.ZoomOut)
        {

            distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
            if (distance > maxDistance) distance = maxDistance;
            if (distance < minDistance) distance = minDistance;
            float k = distance / _offset.magnitude;
            _offset *= k;

            Quaternion rotation = Quaternion.identity;
            if (typeOfTarget != TechnicsType.Plane)
            {
                float deltaVert = Input.GetAxis("Mouse Y");
                Vector3 o = _offset;
                o.y += deltaVert * Time.deltaTime;
                if (o.y > maxYOffset)
                    o.y = maxYOffset;
                if (o.y < minYOffset)
                    o.y = minYOffset;
                _offset = o;
            }
            if (typeOfTarget == TechnicsType.Plane)
            {
                //???/
                _offset = new Vector3(0, 3, -10);
                transform.position = target.transform.position + target.transform.TransformDirection(_offset);
                //transform.position = target.transform.position + _offset;
                transform.LookAt(target);
                Vector3 euler = transform.localEulerAngles;
                euler.z = target.localEulerAngles.z;
                transform.localEulerAngles = euler;
            }
            if (typeOfTarget == TechnicsType.Tank)
            {
                float deltaVert = Input.GetAxis("Mouse Y");
                Vector3 o = _offset;
                o.y += deltaVert * Time.deltaTime;
                if (o.y > maxYOffset)
                    o.y = maxYOffset;
                if (o.y < minYOffset)
                    o.y = minYOffset;
                _offset = o;
                if (controller != null && controller.alive)
                {
                    rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);
                    //Debug.Log("Attached"+ target.eulerAngles.y);
                }
                else
                {
                    lastDeathRotXY += Input.GetAxis("Mouse X") * rotXYSpeed * Time.deltaTime;
                    rotation = Quaternion.Euler(0, target.eulerAngles.y + lastDeathRotXY, 0);
                }
                transform.position = target.position - (rotation * _offset);
                transform.LookAt(target);
            }
        }
        if(camState==CameraState.ZoomIn)
        {
            if (!controller.alive)
            {
                ZoomOut();
                return;
            }
            transform.position = gun.position;
            Vector3 gunRoot = gun.eulerAngles;
            transform.eulerAngles = gunRoot;
            transform.Translate(new Vector3(0, 0, 5f), Space.Self);
        }
        if (camState == CameraState.BombSight)
        {
            transform.position = target.transform.position + Vector3.down * 4f;
            Vector3 velocity = target.transform.GetComponent<Rigidbody>().velocity;
            float h = target.transform.position.y;
            float timeOfFlying = (velocity.y+Mathf.Sqrt(velocity.y* velocity.y+2*9.81f*h)) / (9.81f);
            Vector3 horVelocity = velocity;
            horVelocity.y = 0;
            Vector3 targetPos = target.transform.position + horVelocity * timeOfFlying + Vector3.down * h;
            transform.LookAt(targetPos);
        }
	}
    public void BombSight()
    {
        
        cam.fieldOfView = zoomIn;
        camState = CameraState.BombSight;
        Debug.Log("Zoom bomb!" + camState);
        MainManager.userInterfaseManager.Pricel(true);
    }

    void Update()
    {
        if (target == null)
        {
            if (playerAndHisFiredCurbs.Count != 0)
                target = playerAndHisFiredCurbs[playerAndHisFiredCurbs.Count-1];
            else
                return;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchZoom();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (camState == CameraState.ZoomIn)
                ZoomOut();
            int indexOfTarget = playerAndHisFiredCurbs.IndexOf(target);
            if (indexOfTarget == playerAndHisFiredCurbs.Count - 1)
                indexOfTarget = 0;
            else
                indexOfTarget++;
            //target = playerAndHisFiredCurbs[indexOfTarget];
            if(indexOfTarget==0)
                target = playerAndHisFiredCurbs[playerAndHisFiredCurbs.Count - 1];
            else
                target = playerAndHisFiredCurbs[0];
        }

        List<Transform> transformToRemove = new List<Transform>();
        foreach (Transform t in playerAndHisFiredCurbs)
            if (t == null)
                transformToRemove.Add(t);
        foreach(Transform t in transformToRemove)
            playerAndHisFiredCurbs.Remove(t);
    }
    public void AddFiredCurb(Transform curb)
    {
        playerAndHisFiredCurbs.Add(curb);
    }
    public void SetTargetForCamera(TechnicsType type, Transform target, Transform gun=null, ModuleController controller=null)
    {
        this.target = target;
        this.controller = controller;
        this.typeOfTarget = type;
        if(type==TechnicsType.Tank)
            this.gun = gun;
        playerAndHisFiredCurbs.Clear();
        playerAndHisFiredCurbs.Add(target);
    }
    private void SwitchZoom()
    {

        if (camState != CameraState.ZoomOut)
            ZoomOut();
        else
        {
            ModuleController mc = target.gameObject.GetComponent<ModuleController>();
            //Debug.Log("Module controller: " + mc.name + "   " + (mc is PlaneModuleController));

            if (mc is PlaneModuleController)
                BombSight();
            else
                ZoomIn();
        }
    }
    public void ZoomOut()
    {
        
        cam.fieldOfView = zoomOut;
        camState = CameraState.ZoomOut;
        Debug.Log("Zoom out!" + camState);
        MainManager.userInterfaseManager.Pricel(false);
    }
    public void ZoomIn()
    {
        
        cam.fieldOfView = zoomIn;
        camState = CameraState.ZoomIn;
        Debug.Log("Zoom in!" + camState);
        MainManager.userInterfaseManager.Pricel(true);
    }
}
public enum CameraState
{
    ZoomOut,
    ZoomIn,
    BombSight,
}
