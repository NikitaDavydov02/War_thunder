using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModuleController : MonoBehaviour {
    //REFACTORED_1
    [SerializeField]
    protected List<Module> modules;
    
    
    protected List<Module> crew;

    public float TimeUntilRepairingFinished { get; private set; } = 0;
    public bool alive { get; private set; } = true;
    public bool canMove { get; protected set; } = true;
    public bool canFire { get; protected set; } = true;
    public bool canReloadGun { get; protected set; } = true;
    public string Killer { get; set; }


    
    protected virtual void Start () {
        alive = true;
        crew = new List<Module>();
        foreach (Module module in modules)
        {
            //module.ModuleExplode += Explode;
            if (module.IsHuman())
                crew.Add(module);
            module.controller = this;
        }

    }

    // Update is called once per frame
    protected virtual void Update () {
        if (!alive || MainManager.GameStatus!=GameStatus.Playing)
            return;
        bool extinguish = false;
        if (Input.GetKeyDown(KeyCode.Alpha7))
            extinguish = true;
        foreach (Module module in modules)
        {
            if (module.IsFiring)
            {
                if (extinguish)
                    module.Extinguish();
                if (module.TimeOfFiring > 10)
                {
                    Debug.Log("Die from fire");
                    Die();
                }
                    
            }
        }
        if (TimeUntilRepairingFinished > 0)
            TimeUntilRepairingFinished -= Time.deltaTime;
        if (TimeUntilRepairingFinished < 0)
            TimeUntilRepairingFinished = 0;

        if (Input.GetKeyDown(KeyCode.F))
        {
            List<Module> mustBeRepaired = new List<Module>();
            List<float> timeOfRepairing = new List<float>();
            foreach(Module module in modules)
                if(module.state >= ModuleStates.Crit)
                {
                    mustBeRepaired.Add(module);
                    timeOfRepairing.Add(5);
                }
            
            float SumTimeOfRepair = 0;
            foreach(int t in timeOfRepairing)
                SumTimeOfRepair+=t;
            if (SumTimeOfRepair>0)
            {
                canMove = false;
                TimeUntilRepairingFinished = SumTimeOfRepair;
                StartCoroutine(Repair(mustBeRepaired, timeOfRepairing, SumTimeOfRepair));
                canMove = true;
            }
        }
        int diedCrew = 0;
        foreach(Module ec in crew)
        {
            if (ec.state == ModuleStates.Destroed)
            {
                diedCrew++;
                if (ec.nameOfModule == ModuleType.Мехвод)
                    canMove = false;
                if (ec.nameOfModule == ModuleType.Наводчик)
                    canFire = false;
                if (ec.nameOfModule == ModuleType.Заряжающий)
                    canReloadGun = false;
            }
        }
        if (crew.Count <= (diedCrew * 2))
            Die();
        foreach(Module m in modules)
        {
            if (m.nameOfModule == ModuleType.Двигатель&&m.state==ModuleStates.Destroed)
                canMove = false;
        }
	}
    
    protected void Die()
    {
        

        if (!alive)
            return;
        MainManager.buttleManager.PlayerDied(this.gameObject, Killer);
        alive = false;
        Debug.Log("Module controller "+this.gameObject.name+": Die " + alive);
        AudioSource source = Instantiate(MainManager.musicManager.sourcePrefab);
        source.clip = Resources.Load("Music/Crash") as AudioClip;
        source.transform.position = transform.position;
        source.Play();
    }
    private IEnumerator Repair(List<Module> repairModules, List<float> time, float sumTime)
    {
        MainManager.userInterfaseManager.UpdateRepairTime(sumTime);
        //MainManager.userInterfaseManager.UpdateRepairTime(sumTime);
        for (int i = 0; i < repairModules.Count; i++)
        {
            MainManager.userInterfaseManager.UpdateRepairTime(sumTime);
            yield return new WaitForSeconds(time[i]);

            if (repairModules[i].nameOfModule == ModuleType.Мехвод|| repairModules[i].nameOfModule == ModuleType.Двигатель)
                canMove = true;
            if (repairModules[i].nameOfModule == ModuleType.Наводчик)
                canFire = true;
            if (repairModules[i].nameOfModule == ModuleType.Заряжающий)
                canReloadGun= true;

            sumTime -= time[i];
            repairModules[i].Repair();
        }
        canMove = true;
    }
}
