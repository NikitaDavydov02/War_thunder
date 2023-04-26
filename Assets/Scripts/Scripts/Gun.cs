using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    //REFACTORED_1
    [SerializeField]
    private GunAudioManager audioManager;
    [SerializeField]
    private GameObject smokePrefab;
    private float _rot;

    //Maximum vertical rotations of Gun
    public float maxRot;
    public float minRot;
    [SerializeField]
    private List<GameObject> curbPrefabs;
    private int curbTypeIndex = 0;
    public float timeOfRecharging=5f;
    private float TimeSinseFire=10f;
    private GameObject smoke;
    [SerializeField]
    private ModuleController controller;
    public float verticalRotationSpeed = 1f;
    public float smokeDistanceFromCenter = 3.5f;
    // Use this for initialization
    void Start () {
        _rot = 0;
	}
	
	// Update is called once per frame
	public virtual void Update () {
        TimeSinseFire += Time.deltaTime;
    }

    public void Rot(float input)
    {
        if (!controller.alive || MainManager.GameStatus!=GameStatus.Playing)
            return;
        _rot += input * verticalRotationSpeed * Time.deltaTime;
        _rot = Mathf.Clamp(_rot, minRot, maxRot);
        Vector3 rot = transform.localEulerAngles;
        rot.x = _rot;
        transform.localEulerAngles = rot;
    }
    public void Fire()
    {
        if (!controller.alive|| !controller.canFire|| MainManager.GameStatus != GameStatus.Playing)
            return;
        
        if (TimeSinseFire >= timeOfRecharging)
        {
            TimeSinseFire = 0;
            GameObject curb = Instantiate(curbPrefabs[curbTypeIndex]) as GameObject;
            curb.transform.position = transform.TransformPoint(Vector3.forward * 5f);
            curb.transform.eulerAngles = transform.eulerAngles;
            curb.name = controller.gameObject.name;
            MainManager.PlayerFired(controller.gameObject, curb);

            if (audioManager != null)
                audioManager.Shoot();
            StartCoroutine(Smoke());
        }
    }
    protected void SwitchCurb(int index)
    {
        if (MainManager.GameStatus != GameStatus.Playing)
            return;
        if (curbPrefabs.Count > index && index>=0)
        {
            curbTypeIndex = index;
            TimeSinseFire = 0;
        }
    }
    private IEnumerator Smoke()
    {
        smoke = Instantiate(smokePrefab);
        smoke.transform.position = transform.position;
        Vector3 gunRoot = transform.eulerAngles;
        smoke.transform.eulerAngles = gunRoot;
        smoke.transform.Translate(new Vector3(0, 0, smokeDistanceFromCenter), Space.Self);
        yield return new WaitForSeconds(2f);
        Destroy(smoke);
    }
}
