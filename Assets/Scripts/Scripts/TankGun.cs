using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGun : Gun, IVerticalRotatable {
    //REFACTORED_1
    public float sensetivityvert = 9f;

    private float _rot;

    //Maximum vertical rotations of Gun
    public float maxRot;
    public float minRot;
    public float verticalRotationSpeed = 1f;

    [SerializeField]
    private bool createAimingMark = false;
    private GameObject aimingMark;
    // Use this for initialization
    void Start () {
        base.Start();
        _rot = 0;
        if (createAimingMark)
        {
            Debug.Log("Aiming mark is created");
            aimingMark = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            aimingMark.name = "Aiming mark";
           // aimingMark.gameObject.tag = "Aiming mark";
            aimingMark.transform.position = new Vector3(0, 1000, 0);
            aimingMark.transform.localScale = new Vector3(5, 5, 5);
            aimingMark.GetComponent<MeshRenderer>().material.color = Color.green;
            aimingMark.layer = LayerMask.NameToLayer("Ignore Raycast");
            aimingMark.GetComponent<SphereCollider>().isTrigger = true;
        }

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
        //;
        if (createAimingMark)
        {
            
            Vector3 start = transform.position + 10f * transform.TransformDirection(Vector3.forward);
            Ray ray = new Ray(start, transform.TransformDirection(Vector3.forward));
            Debug.DrawLine(start, transform.position + 100f * transform.TransformDirection(Vector3.forward), Color.black);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 5000))
            {
                aimingMark.transform.position = hitInfo.point;
            }
            else
            {
                aimingMark.transform.position = transform.position + 5000f * transform.TransformDirection(Vector3.forward);
            }

            aimingMark.transform.localScale = Vector3.one*(aimingMark.transform.position - transform.position).magnitude / 100f;
        }
       

        //float rot = Input.GetAxis("Mouse Y")*sensetivityvert;
        //Rotate(rot);
        //if (Input.GetKey(KeyCode.Space))
        //    Fire();
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
    public void RotateToAnAngle(float angle)
    {
        if (!controller.alive || MainManager.GameStatus != GameStatus.Playing)
            return;
        if(Mathf.Abs(angle)> verticalRotationSpeed * Time.deltaTime)
        {
            Rotate(angle / Mathf.Abs(angle));
            return;
        }
        _rot += angle;
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
