using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Gun : MonoBehaviour {
    //REFACTORED_1
    [SerializeField]
    private GunAudioManager audioManager;
    
   
    [SerializeField]
    private List<GameObject> curbPrefabs;
    private int curbTypeIndex = 0;
    public float timeOfRecharging=5f;
    private float TimeSinseFire=10f;
    
    private GameObject smokePrefab;
    private GameObject smoke;
    public float smokeDistanceFromCenter = 3.5f;
    [SerializeField]
    protected ModuleController controller;
    public float recoilDistance = 0.5f;
    public float recoilTime = 1f;
    [SerializeField]
    private Transform recoilingPart;
    
    
    [SerializeField]
    protected GunType gunType = GunType.Gun;
    // Use this for initialization
    protected virtual void Start () {
        smokePrefab = Resources.Load("Prefabs/Smoke") as GameObject;
	}

    // Update is called once per frame
    protected virtual void Update () {
        TimeSinseFire += Time.deltaTime;
    }

    
    public Curb Fire()
    {
        if (!controller.alive ||!controller.CheckIfCanFire()|| MainManager.GameStatus != GameStatus.Playing)
            return null;
        
        if (TimeSinseFire >= timeOfRecharging)
        {
            TimeSinseFire = 0;
            GameObject curb = Instantiate(curbPrefabs[curbTypeIndex]) as GameObject;
            curb.transform.position = transform.TransformPoint(Vector3.forward * 5f);
            curb.transform.eulerAngles = transform.eulerAngles;
            if (controller != null)
            {
                curb.name = controller.gameObject.name + "_curb";
                curb.GetComponent<Curb>().Release(controller.gameObject.name);
                MainManager.PlayerFired(controller.gameObject, curb);
            }
                
            
            OnFired();

            if (audioManager != null)
                audioManager.Shoot();
            StartCoroutine(Smoke());
            StartCoroutine(Recoil());
            return curb.GetComponent<Curb>();
        }
        return null;
    }
    public event EventHandler Fired;
    private void OnFired()
    {
        EventHandler handler = Fired;
        if (handler != null)
            handler(this, new EventArgs());
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
        if (smokePrefab == null)
            yield break;
        smoke = Instantiate(smokePrefab);
        smoke.transform.position = transform.position;
        Vector3 gunRoot = transform.eulerAngles;
        smoke.transform.eulerAngles = gunRoot;
        smoke.transform.Translate(new Vector3(0, 0, smokeDistanceFromCenter), Space.Self);
        yield return new WaitForSeconds(2f);
        Destroy(smoke);
    }
    private IEnumerator Recoil()
    {
        if (recoilingPart == null)
            yield break;
        Vector3 delta = Vector3.zero;
        float velocity = recoilDistance / (recoilTime*0.1f);
        //velocity = 1f;
        for(float time = 0; time < recoilTime * 0.1f; time += Time.deltaTime)
        {
            recoilingPart.Translate(Vector3.back * Time.deltaTime * velocity, Space.Self);
            delta += Vector3.back * Time.deltaTime * velocity;
            yield return null;
        }
        velocity = recoilDistance / (recoilTime * 0.9f);
        for (float time = 0; time < recoilTime * 0.9f; time += Time.deltaTime)
        {
            recoilingPart.Translate(Vector3.forward * Time.deltaTime * velocity, Space.Self);
            delta += Vector3.forward * Time.deltaTime * velocity;
            yield return null;
        }
        recoilingPart.Translate(-delta, Space.Self);
    }
}
public enum GunType
{
    Gun,
    AutomaticGun,
}
