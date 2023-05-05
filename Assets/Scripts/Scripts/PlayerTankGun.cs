using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankGun : PlayerGun, IVerticalRotatable {
    //REFACTORED_1
    public float sensetivityvert = 9f;

    private float _rot;

    //Maximum vertical rotations of Gun
    public float maxRot;
    public float minRot;
    public float verticalRotationSpeed = 1f;
    // Use this for initialization
    void Start () {
        base.Start();
        _rot = 0;
    }
	void Awake()
    {
    }
    void OnDestroy()
    {
    }
    
	// Update is called once per frame
	public void Update () {
        base.Update();
        

        float rot = Input.GetAxis("Mouse Y")*sensetivityvert;
        Rotate(rot);
        if (Input.GetKey(KeyCode.Space))
            Fire();
        //if (Input.GetMouseButton(0) && gunType == GunType.AutomaticGun)
            //Fire();
    }
    public void Rotate(float input)
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
public interface IVerticalRotatable
{
    public void Rotate(float input);
}
