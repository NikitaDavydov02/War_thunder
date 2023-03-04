using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAIGun : Gun {
    public Transform target;
    //{
    //    get
    //    {
    //        return target;
    //    }
    //    set
    //    {
    //        target = value;
    //        lastTargetPosition = target.position;
    //    }
    //}
    private Vector3 targetVelocity = Vector3.zero;
    private Vector3 lastTargetPosition = Vector3.zero;
    public bool angleSelected = false;
    public float anglePogreshnost = 5f;
    private float targetAngle = 0f;
    public float g = -9.8f;
    public float v = 792;
    public float rotSpeed = 0.1f;
    public float an;
    public float s;
    public float perelet;
    public bool test = false;

    // Use this for initialization
    void Start () {
        curbTypeIndex = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (target == null)
            return;
        TimeSinseFire += Time.deltaTime;
        Vector2 xyTarget = new Vector2(target.position.x, target.position.z);
        Vector2 xyPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 xyDelta = xyTarget - xyPos;
        //if (lastTargetPosition != Vector3.zero)
        //{
        //    float timeOfFlying = xyDelta.magnitude / v;
        //    Vector3 targetVelocity = (target.position - lastTargetPosition) / Time.deltaTime;
        //    Vector3 upregdenie = timeOfFlying * targetVelocity;
        //    Vector2 upregdenie2D = new Vector2(upregdenie.x, upregdenie.z);
        //    Debug.Log("timeOfFlying " + timeOfFlying);
        //    Debug.Log("Target velocityM " + targetVelocity.magnitude);
        //    Debug.Log("Target velocity " + targetVelocity);
        //    Debug.Log("UpregdenieM " + upregdenie2D.magnitude);
        //    Debug.Log("Upregdenie " + upregdenie2D);
        //    Debug.Log("XYDo " + xyDelta);
        //    xyDelta += upregdenie2D;
        //    Debug.Log("XYPosle " + xyDelta);
        //}
        //lastTargetPosition = target.position;
        //float heightDelta = target.position.y - transform.position.y + (xyDelta.magnitude / 100);
        float heightDelta = target.position.y - transform.position.y+1;
        perelet = (xyDelta.magnitude - (lastPopadaniePoint - transform.position).magnitude);
        s = Mathf.Abs(xyDelta.magnitude);
        float a = (g * s * s) / (2 * v * v);
        float b = s;
        float c = -heightDelta + ((g * s * s) / (2 * v * v));
        float D = (b * b) - (4 * a * c);
        float tg = (-b + Mathf.Sqrt(D)) / (2 * a);
        float angle = Mathf.Atan(tg);
        float angleInDeg = (180 * angle) / Mathf.PI;
        an= angleInDeg;
        float realAngle = 360 - transform.eulerAngles.x;
        if (realAngle > 50)
        {
            realAngle -= 360;
        }
        if (Mathf.Abs(realAngle - angleInDeg) <= anglePogreshnost)
            angleSelected = true;
        else
            angleSelected = false;
        if (!angleSelected)
        {
            if (realAngle > angleInDeg)
            {
                Rot(rotSpeed);
            }
            else
            {
                Rot(-rotSpeed);
            }
        }
        //Debug.Log(realAngle);
        
        //Vector3 Target = taret.transform.position;
        //Vector3 Pos = transform.position;
        //Vector3 Delta = Target - Pos;

        //Vector3 direction = transform.TransformDirection(Delta);
        //Vector3 Forward =transform.forward * Mathf.Abs(Delta.magnitude);
        //if (Mathf.Abs(Forward.x - Delta.x) <= anglePogreshnost && Mathf.Abs(Forward.y - Delta.y) <= anglePogreshnost && Mathf.Abs(Forward.y - Delta.y) <= anglePogreshnost)
        //    angleSelected = true;
        //else
        //    angleSelected = false;
    }
}
