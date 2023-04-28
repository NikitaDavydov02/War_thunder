using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curb : MonoBehaviour {
    //REFACTORED_1
    public float speedScalyar = 100f;
    protected Vector3 speedVector;
    protected float g = -9.8f;
    public bool stop { get; protected set; } = false;
    [SerializeField]
    protected Detonator detonator;
    protected void Start () {
        if (detonator.type == TypeOfCurp.Bomb)
        {
            stop = true;
        }
        else
        {
            detonator.Vzvesti();
        }
        Vector3 s = transform.forward*speedScalyar;
        speedVector = Vector3.ClampMagnitude(s, speedScalyar);
    }

    protected void Update () {
        if (stop || MainManager.GameStatus != GameStatus.Playing)
            return;
        speedVector.y += g * Time.deltaTime;
        transform.Translate(speedVector*Time.deltaTime, Space.World);
	}
    public void Stop()
    {
        stop = true;
        speedVector = Vector3.zero;
    }
}
