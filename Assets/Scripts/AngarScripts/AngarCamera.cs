using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngarCamera : MonoBehaviour {
    //REFACTORED
    public float maxDistance = 20;
    public float zoomSpeed;
    public float minDistance = 1;
    public float maxRotX = 60;
    public float minRotX = 0;
    public float yRotSpeed;
    public float xRotSpeed;
    public Transform target { get; set; }

    private float rotY;
    private float rotX;
    private float distance;
    private Vector3 offset;
    // Use this for initialization
    void Start () {
        rotY = transform.eulerAngles.y;
        rotX = transform.eulerAngles.x;
        offset = Vector3.zero - transform.position;
        distance = offset.magnitude;
    }

	void Update () {

        if (target == null)
            return;
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
