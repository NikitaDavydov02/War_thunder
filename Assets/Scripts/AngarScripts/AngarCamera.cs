using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngarCamera : MonoBehaviour {
    private float rotY;
    private float rotX;
    private float distance;
    public float maxDistance = 20;
    public float zoomSpeed;
    public float minDistance = 1;
    public float maxRotX = 60;
    public float minRotX = 0;
    private Vector3 offset;
    [SerializeField]
    Vector3 targetPoint;
    public float yRotSpeed;
    public float xRotSpeed;
    private Transform target;
    // Use this for initialization
    void Start () {
        rotY = transform.eulerAngles.y;
        rotX = transform.eulerAngles.x;
        offset = targetPoint - transform.position;
        distance = offset.magnitude;
    }

    // Update is called once per frame
    private bool targetGeted = false;
	void Update () {
        //rotY += Input.GetAxis("Mouse X") * yRotSpeed * Time.deltaTime;
        //rotX += Input.GetAxis("Mouse Y") * xRotSpeed * Time.deltaTime;
        //if (rotX > maxRotX) rotX = maxRotX;
        //if (rotX < minRotX) rotX = minRotX;
        //Quaternion rotation = Quaternion.Euler(0, rotY, 0);
        //transform.position = targetPoint - (rotation * offset);
        //transform.LookAt(targetPoint);
        //Vector3 rot = transform.localEulerAngles;
        //rot.x = rotX;
        //transform.localEulerAngles = rot;
        //distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        //if (distance > maxDistance) distance = maxDistance;
        //if (distance < minDistance) distance = minDistance;
        //float k = distance / offset.magnitude;
        //offset *= k;
        while (!targetGeted||target==null)
        {
            if (AngarTankManager.currentModel != null)
            {
                target = AngarTankManager.currentModel.transform;
                targetGeted = true;
            }
            return;
        }
        rotY += Input.GetAxis("Mouse X") * yRotSpeed * Time.deltaTime;
        rotX += Input.GetAxis("Mouse Y") * xRotSpeed * Time.deltaTime;
        if (rotX > maxRotX) rotX = maxRotX;
        if (rotX < minRotX) rotX = minRotX;
        Quaternion rotation = Quaternion.Euler(-rotX, rotY, 0);
        transform.position = target.position - (rotation * offset);
        transform.LookAt(target);

        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        if (distance > maxDistance) distance = maxDistance;
        if (distance < minDistance) distance = minDistance;
        float k = distance / offset.magnitude;
        offset *= k;
    }
}
