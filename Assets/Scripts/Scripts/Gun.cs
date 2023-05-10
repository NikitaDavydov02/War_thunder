using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    //REFACTORED_1
    [SerializeField]
    private GunAudioManager audioManager;
    [SerializeField]
    private GameObject smokePrefab;
   
    [SerializeField]
    private List<GameObject> curbPrefabs;
    private int curbTypeIndex = 0;
    public float timeOfRecharging=5f;
    private float TimeSinseFire=10f;
    private GameObject smoke;
    [SerializeField]
    protected ModuleController controller;
    
    public float smokeDistanceFromCenter = 3.5f;
    [SerializeField]
    protected GunType gunType = GunType.Gun;
    // Use this for initialization
    protected void Start () {
        smokePrefab = Resources.Load("Prefabs/Smoke") as GameObject;
	}

    // Update is called once per frame
    protected void Update () {
        TimeSinseFire += Time.deltaTime;
    }

    
    public void Fire()
    {
        //if (!controller.alive|| !controller.canFire|| MainManager.GameStatus != GameStatus.Playing)
            //return;
        
        if (TimeSinseFire >= timeOfRecharging)
        {
            TimeSinseFire = 0;
            GameObject curb = Instantiate(curbPrefabs[curbTypeIndex]) as GameObject;
            curb.transform.position = transform.TransformPoint(Vector3.forward * 5f);
            curb.transform.eulerAngles = transform.eulerAngles;
            curb.name = controller.gameObject.name+"_curb";
            curb.GetComponent<Curb>().Release(controller.gameObject.name);
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
        if (smokePrefab == null)
            yield return null;
        smoke = Instantiate(smokePrefab);
        smoke.transform.position = transform.position;
        Vector3 gunRoot = transform.eulerAngles;
        smoke.transform.eulerAngles = gunRoot;
        smoke.transform.Translate(new Vector3(0, 0, smokeDistanceFromCenter), Space.Self);
        yield return new WaitForSeconds(2f);
        Destroy(smoke);
    }
}
public enum GunType
{
    Gun,
    AutomaticGun,
}
