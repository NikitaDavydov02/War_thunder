using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curb : MonoBehaviour {
    //REFACTORED_1
    public float speedScalyar = 100f;
    public Vector3 speedVector;
    protected float g = -9.8f;
    [SerializeField]
    private bool stop = false;
    [SerializeField]
    protected Detonator detonator;
    protected void Start () {
        Vector3 s = transform.forward*speedScalyar;
        speedVector = Vector3.ClampMagnitude(s, speedScalyar);
        if(detonator!=null)
            detonator.Detonate += Detonator_Detonate;
    }

    private void Detonator_Detonate(object sender, System.EventArgs e)
    {
        Stop();
    }

    protected void Update () {
        if (stop || MainManager.GameStatus != GameStatus.Playing)
            return;
        speedVector.y += g * Time.deltaTime;
        transform.Translate(speedVector*Time.deltaTime, Space.World);
	}
    private void Stop()
    {
        Debug.Log("Stop curb");
        stop = true;
        speedVector = Vector3.zero;
    }
    public void Release(string owner)
    {
        stop = false;
        detonator.Vzvesti();
        detonator.OwnerName = owner;
    }
    public void Release(string owner, Vector3 initialVelocity)
    {
        speedVector = initialVelocity;
        Release(owner);
    }
}
