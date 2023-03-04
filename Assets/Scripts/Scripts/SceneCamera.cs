using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCamera : MonoBehaviour {
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform gun;
    //[SerializeField]
    //private Material tex;
    public float rotSpeeed = 1.5f;
    private float _rotY;
    private Vector3 _offset;
    private Vector3 _memoryOffset;
    public Vector3 _zoomOffset;
    private bool inZoom = false;
    public float vertSpeed = 3f;
    public float maxYOffset = 5f;
    public float minYOffset = -15f;
    public float maxDistance = 20f;
    public float minDistance = 5f;
    public float zoomSpeed = 5f;
    private float distance;
    [SerializeField]
    public ModuleController controller;
    public float rotXYSpeed = 10f;
    private float lastDeathRotXY = 0;
    [SerializeField]
    ButtleManager buttleManager;
    private bool  bulletCam =false;
    private Transform lastBullet;
    // Use this for initialization
    void Start () {
        this.GetComponent<Camera>().farClipPlane = 4000;
        _rotY = transform.eulerAngles.y;
        _offset = new Vector3(0, -5, 10);
        distance = _offset.magnitude;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (!inZoom)
        {
            float deltaVert = Input.GetAxis("Mouse Y");
            Vector3 o = _offset;
            o.y += deltaVert * Time.deltaTime;
            if (o.y > maxYOffset)
                o.y = maxYOffset;
            if (o.y < minYOffset)
                o.y = minYOffset;
            _offset = o;

            distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
            if (distance > maxDistance) distance = maxDistance;
            if (distance < minDistance) distance = minDistance;
            float k = distance / _offset.magnitude;
            _offset *= k;
            Quaternion rotation;
            if (!bulletCam)
            {
                if (controller.alive)
                    rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);
                else
                {
                    lastDeathRotXY += Input.GetAxis("Mouse X") * rotXYSpeed * Time.deltaTime;
                    rotation = Quaternion.Euler(0, target.eulerAngles.y + lastDeathRotXY, 0);
                }
                transform.position = target.position - (rotation * _offset);
                transform.LookAt(target);
            }
            else
            {
                rotation = Quaternion.Euler(0, lastBullet.eulerAngles.y, 0);
                transform.position = lastBullet.position - (rotation * _offset);
                transform.LookAt(lastBullet);
            }
        }
        else
        {
            if (!controller.alive)
            {
                if (inZoom)
                {
                    Zoom();
                    Messenger.Broadcast(GameEvent.PRICEL);
                }
                return;
            }
            transform.position = gun.position;
            Vector3 gunRoot = gun.eulerAngles;
            transform.eulerAngles = gunRoot;
            transform.Translate(new Vector3(0, 0, 5f), Space.Self);
        }
	}

    private bool targetGot = false;
    void Update()
    {
        if (!targetGot)
        {
            target = MainManager.buttleManager.humanTank.transform;
            if (target != null)
            {
                controller = target.transform.gameObject.GetComponent<ModuleController>();
                target = controller.tower.transform;
                gun = controller.gun.transform;
                targetGot = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            Zoom();
            Messenger.Broadcast(GameEvent.PRICEL);
        }
        else
        {
            bulletCam = false;
        }
    }

    private void Zoom()
    {
        Camera cam = this.gameObject.GetComponent<Camera>();
        if (cam != null)
        {
            if (inZoom)
                cam.fieldOfView = 60;
            else
                cam.fieldOfView = 3;
        }
        inZoom = !inZoom;
    }
    public void NewTargetForCamara(int index)
    {
        Debug.Log("New Target");
        if (controller.alive)
            return;
        Debug.Log("Index of new Target:" + index);
        Debug.Log(buttleManager.allred.Count);
        Debug.Log(buttleManager.allblue.Count);
        if (index<buttleManager.allred.Count)
        {
            target = buttleManager.allred[index].transform;
        }
        else
        {
            if(index - buttleManager.allred.Count< buttleManager.allblue.Count)
            target = buttleManager.allblue[index - buttleManager.allblue.Count].transform;
        }
    }
}
