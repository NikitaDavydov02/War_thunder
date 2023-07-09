using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField]
    TankPhysics tankPhysics;
    [SerializeField]
    TankModuleController moduleController;
    [SerializeField]
    Tower tower;
    [SerializeField]
    TankGun tankGun;

    //public float horizontalRotationSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
       // tankPhysics = GetComponent<TankPhysics>();
        //moduleController = GetComponent<TankModuleController>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Gas(float input)
    {
        if (!moduleController.alive)
            return;
        tankPhysics.generalLevel = input;
    }
    public void Rotate(float input)
    {
        if (!moduleController.alive)
            return;
        tankPhysics.rotLevel = input;
    }
    public void RotateTower(float input)
    {
        if (!moduleController.alive)
            return;
        tower.Rotate(input);
    }
    public void RotateGun(float input)
    {
        if (!moduleController.alive)
            return;
        tankGun.Rotate(input);
    }
    public void Fire()
    {
        tankGun.Fire();
    }
}
