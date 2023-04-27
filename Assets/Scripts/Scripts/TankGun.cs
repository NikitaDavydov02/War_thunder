using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGun : Gun
{
    private float _rot;

    //Maximum vertical rotations of Gun
    public float maxRot;
    public float minRot;
    public float verticalRotationSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        _rot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    public void Rot(float input)
    {
        if (!controller.alive || MainManager.GameStatus != GameStatus.Playing)
            return;
        _rot += input * verticalRotationSpeed * Time.deltaTime;
        _rot = Mathf.Clamp(_rot, minRot, maxRot);
        Vector3 rot = transform.localEulerAngles;
        rot.x = _rot;
        transform.localEulerAngles = rot;
    }
}
