using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //public Vector3 speedVector;
    //private float g = -9.8f;
    //public bool stop { get; private set; } = true;
    void Start()
    {
    }

    void Update()
    {
        //if (stop || MainManager.GameStatus != GameStatus.Playing)
        //    return;
        //speedVector.y += g * Time.deltaTime;
        //transform.Translate(speedVector * Time.deltaTime, Space.World);
    }
    public void Stop()
    {
        //stop = true;
        // = Vector3.zero;
    }
    public void Release()
    {
        //speedVector = Vector3.zero;
        //stop = false;
    }
}
