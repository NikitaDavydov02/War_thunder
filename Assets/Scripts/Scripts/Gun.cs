using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    [SerializeField]
    GunAudioManager audioManager;
    [SerializeField]
    private GameObject dimPrefab;
    private float _rot;
    public float maxRot;
    public float minRot;
    [SerializeField]
    public List<GameObject> curbPrefabs;
    private GameObject curb;
    public int curbTypeIndex=0;
    public float timeOfPer=5f;
    public float TimeSinseFire=10f;
    private GameObject dim;
    [SerializeField]
    private ModuleController controller;
    public string tankName;
    public Vector3 lastPopadaniePoint = new Vector3(0, 0, 0);
    [SerializeField]
    public Vector3 correctorVector;
    public float currentDistance = 0;
    // Use this for initialization
    void Start () {
        _rot = 0;
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void Rot(float delta)
    {
        if (!controller.alive)
            return;
        _rot += delta;
        _rot = Mathf.Clamp(_rot, minRot, maxRot);
        //Vector3 rot = transform.localEulerAngles;
        //Vector3 rot = transform.parent.parent.transform.eulerAngles;
        Vector3 rot = transform.localEulerAngles;
        rot.x = _rot;
        transform.localEulerAngles = rot;
    }
    public void Fire()
    {
        if (!controller.alive|| !controller.canFire)
            return;
        tankName = controller.gameObject.name;
        if (TimeSinseFire >= timeOfPer)
        {
            TimeSinseFire = 0;
            curb = Instantiate(curbPrefabs[curbTypeIndex]) as GameObject;
            curb.transform.position = transform.TransformPoint(Vector3.forward * 5f);
            //curb.transform.rotation = transform.rotation;
            curb.transform.eulerAngles = transform.eulerAngles + correctorVector;
            curb.name = tankName;
            curb.transform.GetChild(0).name = tankName;
            curb.transform.GetChild(1).name = tankName;
            curb.GetComponent<Curb>().gun = this;

            controller.lastCurb = curb;

            if (audioManager != null)
                audioManager.Shoot();
            StartCoroutine(Dim());
        }
    }
    private IEnumerator Dim()
    {
        dim = Instantiate(dimPrefab);
        dim.transform.position = transform.position;
        Vector3 gunRoot = transform.eulerAngles;
        dim.transform.eulerAngles = gunRoot;
        dim.transform.Translate(new Vector3(0, 0, 3.5f), Space.Self);
        yield return new WaitForSeconds(2f);
        Destroy(dim);
    }
}
