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
    private bool inZoom = false;
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
    private Transform target;


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
        if (!inZoom)
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
        else
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
	}

    void Update()
    {
        if (target == null)
        {
            if (playerAndHisFiredCurbs.Count != 0)
                target = playerAndHisFiredCurbs[0];
            else
                return;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchZoom();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (inZoom)
                ZoomOut();
            int indexOfTarget = playerAndHisFiredCurbs.IndexOf(target);
            if (indexOfTarget == playerAndHisFiredCurbs.Count - 1)
                indexOfTarget = 0;
            else
                indexOfTarget++;
            target = playerAndHisFiredCurbs[indexOfTarget];
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

        if (inZoom)
            ZoomOut();
        else
            ZoomIn();
    }
    public void ZoomOut()
    {
        cam.fieldOfView = 60;
        inZoom = false;
        MainManager.userInterfaseManager.Pricel(false);
    }
    public void ZoomIn()
    {
        cam.fieldOfView = 3;
        inZoom = true;
        MainManager.userInterfaseManager.Pricel(true);
    }
}
