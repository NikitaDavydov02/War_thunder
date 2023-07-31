using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Curb
{
    public Transform target;
    public float maxRotationalSpeed = 5f;
    public Vector3 lastTargetPosition = Vector3.zero;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (stop || MainManager.GameStatus != GameStatus.Playing)
            return;
        //Update speed vector
        if (target != null)
        {
            float distance = (target.position - transform.position).magnitude;
            float time = distance / speedVector.magnitude;
            Vector3 targetVelocity = Vector3.zero;
            if (lastTargetPosition != Vector3.zero)
                targetVelocity = (target.position-lastTargetPosition) / Time.deltaTime;
            Vector3 targetDirection = (target.position+time*targetVelocity - transform.position).normalized;
            Vector3 currentDirection = speedVector.normalized;
            Debug.DrawLine(transform.position, transform.position + targetDirection, Color.green);
            Debug.DrawLine(transform.position, transform.position + speedVector, Color.red);
            float angle = Vector3.Angle(currentDirection, targetDirection);
            float demandedSpeed = Mathf.Abs(angle) / Time.deltaTime;
            //if (//demandedSpeed <= maxRotationalSpeed)
                speedVector = speedVector.magnitude * targetDirection;
            transform.LookAt(target);
            //else
            //{
            //    Vector3 delta = (currentDirection - targetDirection) * maxRotationalSpeed / demandedSpeed;
            //    speedVector += delta;
            //}
            lastTargetPosition = transform.position;
        }
        

        transform.Translate(speedVector * Time.deltaTime, Space.World);
    }
}
