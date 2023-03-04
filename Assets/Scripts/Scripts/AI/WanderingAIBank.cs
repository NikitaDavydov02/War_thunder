using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAIBank : Tower {
    [SerializeField]
    private WanderingAIGun gun;
    public bool angleSelected = false;
    public float anglePogreshnost = 0.5f;
    public float currentAnglePogreshnost;
    private int k = 1;
    public bool test = false;
    public Transform target;
    public float farMnogtel = 3;
    public float nearMnogtel = 150;
    public float farSpeedMnogtel = 1;
    public float neaSpeedrMnogtel = 1;
    public Vector3 startPosition;
    public TankWanderingAI tankWanderingAI;
    private Vector3 lastTargetPosition;
    // Use this for initialization
    void Start () {
        currentAnglePogreshnost = anglePogreshnost;
        startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null)
            return;
        //if (!target.GetComponent<ModuleController>().alive)
        //    tankWanderingAI.NewRoad();
        Vector2 xyTarget = new Vector2(target.position.x, target.position.z);
        Vector2 xyPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 xyDelta = xyTarget - xyPos;
        if (lastTargetPosition != Vector3.zero)
        {
            float timeOfFlying = xyDelta.magnitude / gun.v;
            Vector3 targetVelocity = (target.position - lastTargetPosition) / Time.deltaTime;
            Vector3 upregdenie = timeOfFlying * targetVelocity;
            Vector2 upregdenie2D = new Vector2(upregdenie.x, upregdenie.z);
            Debug.Log("timeOfFlying " + timeOfFlying);
            Debug.Log("Target velocityM " + targetVelocity.magnitude);
            Debug.Log("Target velocity " + targetVelocity);
            Debug.Log("UpregdenieM " + upregdenie2D.magnitude);
            Debug.Log("Upregdenie " + upregdenie2D);
            Debug.Log("XYDo " + xyDelta);
            xyDelta += upregdenie2D;
            Debug.Log("XYPosle " + xyDelta);
        }
        lastTargetPosition = target.position;
        if (xyDelta.magnitude > 100)
        {
            currentAnglePogreshnost = anglePogreshnost;
        }
        if (xyDelta.magnitude > 1000)
        {
            currentAnglePogreshnost = farMnogtel * anglePogreshnost;
        }
        else
        {
            if(xyDelta.magnitude <= 100)
                currentAnglePogreshnost = nearMnogtel * anglePogreshnost;
        }
        Vector2 direction = transform.TransformDirection(xyDelta);
        Vector2 xyForward = new Vector3(transform.forward.x, transform.forward.z) * Mathf.Abs(xyDelta.magnitude);
        float p = currentAnglePogreshnost * xyDelta.magnitude;
        if (test)
        {
            //Debug.Log("");
            //Debug.Log(Mathf.Abs(xyForward.x - xyDelta.x));
            //Debug.Log(Mathf.Abs(xyForward.y - xyDelta.y));
        }
        if ((Mathf.Abs(xyForward.x - xyDelta.x) <= p) && (Mathf.Abs(xyForward.y - xyDelta.y)) <= p)
            angleSelected = true;
        else
            angleSelected = false;
        if (angleSelected)
        {
            if (gun.angleSelected)
                gun.Fire();
        }
        else
        {
            float sinA = xyDelta.x / xyDelta.magnitude;
            float sinB = transform.forward.x / transform.forward.magnitude;
            float cosA = xyDelta.y / xyDelta.magnitude;
            float cosB = transform.forward.z / transform.forward.magnitude;
            float sin = (sinA * cosB) - (cosA * sinB);
            if (sin > 0)
                k = -1;
            else
                k = 1;
            if (xyDelta.magnitude > 1000)
            {
                Rotate(k * rotSpeed * farSpeedMnogtel);
            }
            else
                Rotate(k * rotSpeed);
        }
    }

    public bool SetTargetForBank(Transform target)
    {
        if (this.target == null)
        {
            this.target = target;
            gun.target = target;
            return true;
        }
        if (!this.target.GetComponent<ModuleController>().alive)
            tankWanderingAI.NewRoad();
        this.target = target;
        gun.target = target;
        return true;
    }
}
