using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour {
    [SerializeField]
    List<Module> ecipag;
    [SerializeField]
    List<Module> boekomplekt;
    [SerializeField]
    Transform bank;
    [SerializeField]
    List<Module> pogaroopasnie;
    [SerializeField]
    Module motor;
    [SerializeField]
    GameObject firePrefab;
    [SerializeField]
    private List<Module> modules;
    private List<ModuleStates> lastStates;
    private GameObject fire;
    private GameObject fire2;
    public float TimeOfPogar = 0;
    private float force = 20f;
    public bool humanTankController = false;
    public bool alive { get; private set; }
    public bool blue = false;
    public string fatalityName;
    private bool hadAlreadyDie = false;
    private bool bankFly = false;
    public GameObject lastCurb;
    public bool canMove = true;
    public bool canFire = true;
    public bool canZaryajat = true;
    bool pogar = false;
    Module goryashayabk;


    [SerializeField]
    public GameObject tower;
    [SerializeField]
    public GameObject gun;
    public float repairTime = 0;
    //public string lastPopadanie;
    // Use this for initialization
    void Start () {
        alive = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (fire != null)
            {
                Destroy(fire);
                TimeOfPogar = 0;
                foreach (Module m in pogaroopasnie)
                {
                    m.pogar = false;
                }
            }
        }
        if(humanTankController)
            MainManager.userInterfaseManager.UpdateRepairTime(repairTime);
        Debug.Log("RT:" + repairTime);
        if (repairTime > 0)
            repairTime -= Time.deltaTime;
        if (repairTime < 0)
            repairTime = 0;
        if (Input.GetKeyDown(KeyCode.F))
        {
            List<Module> mustRepair = new List<Module>();
            List<float> timeOfRepair = new List<float>();
            if (motor.state >= ModuleStates.Crit)
            {
                mustRepair.Add(motor);
                timeOfRepair.Add(5);
            }
            foreach(Module m in pogaroopasnie)
            {
                if(m!=motor&&m.state >= ModuleStates.Crit)
                {
                    mustRepair.Add(m);
                    timeOfRepair.Add(4);
                }
            }
            foreach(Module human in ecipag)
            {
                if (human.state >= ModuleStates.Crit)
                {
                    mustRepair.Add(human);
                    timeOfRepair.Add(3);
                }
            }
            float SumTimeOfRepair = 0;
            foreach(int t in timeOfRepair)
                SumTimeOfRepair+=t;
            if (SumTimeOfRepair>0)
            {
                Debug.Log("в ремонт ");
                canMove = false;
                repairTime = SumTimeOfRepair;
                StartCoroutine(Repair(mustRepair, timeOfRepair, SumTimeOfRepair));
                canMove = true;
            }
        }
        if (fire != null)
        {
            fire.transform.position = motor.transform.position;
            //fire.transform.Translate(new Vector3(0, 1, 0), Space.Self);
        }
        if (fire2 != null)
        {
            fire2.transform.position = goryashayabk.transform.position; ;
            //fire2.transform.Translate(new Vector3(0, 2, 0), Space.Self);
            Debug.Log("Pogar!");
        }
        if (!alive)
        {
            if (!hadAlreadyDie)
            {
                if (fatalityName == MainManager.buttleManager.humanTank.name)
                {
                    hadAlreadyDie = true;
                    StartCoroutine(MainManager.userInterfaseManager.HumanDestroy());
                }
            }
            return;
        }
        int died = 0;
        foreach(Module ec in ecipag)
        {
            if (ec.state == ModuleStates.Destroed)
            {
                fatalityName = ec.lastDamag;
                died++;
                if (ec.nameOfModule == ModuleType.Мехвод)
                    canMove = false;
                if (ec.nameOfModule == ModuleType.Наводчик)
                    canFire = false;
                if (ec.nameOfModule == ModuleType.Заряжающий)
                    canZaryajat = false;
            }
        }
        if (ecipag.Count <= (died * 2))
            Die();
        foreach (Module b in boekomplekt)
        {
            if (b.state == ModuleStates.Destroed)
            {
                tower.gameObject.AddComponent<Rigidbody>();
                Rigidbody r = tower.gameObject.GetComponent<Rigidbody>();
                r.mass = 20000;
                r.AddExplosionForce(3000000, tower.transform.position+new Vector3(0,-1,0),10);
                if (fire2 == null)
                {
                    goryashayabk = b;
                    fire2 = Instantiate(firePrefab);
                    //fire2.transform.position = transform.position;
                }
                //r.AddForce(new Vector3(0, 10, 0));
                //bank.gameObject.AddComponent<Rigidbody>();
                //Rigidbody r = bank.gameObject.GetComponent<Rigidbody>();
                //r.AddExplosionForce(force, new Vector3(0, 0.2f , 0.5f), force, force ,ForceMode.Impulse);
                //r.mass = 12000;
                //r.AddRelativeForce(new Vector3(0, force, 0), ForceMode.VelocityChange);
                //r.AddForce(new Vector3(0, force, 0), ForceMode.Impulse);
                fatalityName = b.lastDamag;
                if (!bankFly)
                {
                    //b.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 100, 0));
                    //bankFly = true;
                }
                Die();
            }
        }
        foreach(Module m in pogaroopasnie)
        {
            if (m.nameOfModule == ModuleType.Двигатель&&m.state==ModuleStates.Destroed)
                canMove = false;
            if (m.pogar == true)
            {
                if (fire == null)
                {
                    fatalityName = m.lastDamag;
                    fire = Instantiate(firePrefab);
                    fire.transform.position = motor.transform.position;
                    //fire.transform.Translate(new Vector3(0, 1, 0), Space.Self);
                }
                TimeOfPogar += Time.deltaTime;
                if (TimeOfPogar > 10)
                {
                    Die();
                }
            }
        }
	}
    private void Die()
    {
        if (!alive)
            return;
        alive = false;
        AudioSource source = Instantiate(MainManager.musicManager.sourcePrefab);
        //Debug.Log(transform.parent.transform.position);
        source.clip = Resources.Load("Music/Crash") as AudioClip;
        source.transform.position = transform.position;
        source.Play();
        if (humanTankController)
        {
            Messenger.Broadcast(GameEvent.HUMANTANKDESTROIED);
        }
    }
    public void GameFinished()
    {
        alive = false;
    }
    private IEnumerator Repair(List<Module> repairModules, List<float> time, float sumTime)
    {
        //MainManager.userInterfaseManager.UpdateRepairTime(sumTime);
        for(int i = 0; i < repairModules.Count; i++)
        {
            yield return new WaitForSeconds(time[i]);

            if (repairModules[i].nameOfModule == ModuleType.Мехвод|| repairModules[i].nameOfModule == ModuleType.Двигатель)
                //canMove = true;
            if (repairModules[i].nameOfModule == ModuleType.Наводчик)
                canFire = true;
            if (repairModules[i].nameOfModule == ModuleType.Заряжающий)
                canZaryajat = true;

            sumTime -= time[i];
            //MainManager.userInterfaseManager.UpdateRepairTime(sumTime);
            repairModules[i].state = ModuleStates.Normal;
            repairModules[i].UpdateColor();
        }
        canMove = true;
    }
}
