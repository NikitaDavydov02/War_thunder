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
    public bool alive = true;
    private float repairTime = 0;
    List<Module> mustBeRepaired;
    //public bool canMove { get; protected set; } = true;
    //public bool canFire { get; protected set; } = true;
    //public bool canReloadGun { get; protected set; } = true;
    public string Killer { get; set; }


    
    protected virtual void Start () {
        alive = true;
        crew = new List<Module>();
        foreach (Module module in modules)
        {
            //module.ModuleDamaged += Module_ModuleDamaged;
            //module.ModuleExplode += Explode;
            if (module.IsHuman())
                crew.Add(module);
            module.controller = this;
        }

    }

    //private void Module_ModuleDamaged(object sender, EventArgs e)
    //{
        
    //    ModuleDamagedEventArgs args = e as ModuleDamagedEventArgs;
    //    Killer = args.Killer;
    //    Debug.Log("Module controller received killer" + Killer);
    //}

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
                    Explode();
                }
                    
            }
        }
        if (TimeUntilRepairingFinished > 0)
        {
            TimeUntilRepairingFinished -= Time.deltaTime;
            MainManager.userInterfaseManager.UpdateRepairTime(TimeUntilRepairingFinished);
        }
            
        if (TimeUntilRepairingFinished < 0)
        {
            TimeUntilRepairingFinished = 0;
            MainManager.userInterfaseManager.UpdateRepairTime(TimeUntilRepairingFinished);
            //Repairing finished
            foreach (Module m in mustBeRepaired)
                m.Repair();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            mustBeRepaired = new List<Module>();
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
                //Debug.Log("repair");
                //canMove = false;
                TimeUntilRepairingFinished = SumTimeOfRepair;
                //StartCoroutine(Repair(mustBeRepaired, timeOfRepairing, SumTimeOfRepair));
                //canMove = true;
            }
        }
        int diedCrew = 0;
        foreach(Module ec in crew)
        {
            if (ec.state == ModuleStates.Destroed)
            {
                diedCrew++;
                //if (ec.nameOfModule == ModuleType.Мехвод)
                //    canMove = false;
                //if (ec.nameOfModule == ModuleType.Наводчик)
                    //canFire = false;
                //if (ec.nameOfModule == ModuleType.Заряжающий)
                 //   canReloadGun = false;
            }
        }
        if (crew.Count <= (diedCrew * 2))
            Die();
        //foreach(Module m in modules)
        //{
        //    if (m.nameOfModule == ModuleType.Двигатель&&m.state==ModuleStates.Destroed)
        //        canMove = false;
        //}
	}
    
    protected void Die()
    {

        //Debug.Log("Die is called in module controller alive" + alive) ;
        if (!alive)
            return;
        MainManager.buttleManager.PlayerDied(this.gameObject, Killer);
        alive = false;
        Debug.Log("Module controller "+this.gameObject.name+" died ");
        AudioSource source = Instantiate(MainManager.musicManager.sourcePrefab);
        source.clip = Resources.Load("Music/Crash") as AudioClip;
        source.transform.position = transform.position;
        source.Play();
    }
    //private IEnumerator Repair(List<Module> repairModules, List<float> time, float sumTime)
    //{
    //    MainManager.userInterfaseManager.UpdateRepairTime(sumTime);
    //    //MainManager.userInterfaseManager.UpdateRepairTime(sumTime);
    //    for (int i = 0; i < repairModules.Count; i++)
    //    {
    //        MainManager.userInterfaseManager.UpdateRepairTime(sumTime);
    //        yield return new WaitForSeconds(time[i]);

    //        //if (repairModules[i].nameOfModule == ModuleType.Мехвод|| repairModules[i].nameOfModule == ModuleType.Двигатель)
    //        //    canMove = true;
    //        //if (repairModules[i].nameOfModule == ModuleType.Наводчик)
    //        //    canFire = true;
    //        //if (repairModules[i].nameOfModule == ModuleType.Заряжающий)
    //          //  canReloadGun= true;

    //        sumTime -= time[i];
    //        repairModules[i].Repair();
    //    }
    //    //canMove = true;
    //}
    public bool CheckIfCanMove()
    {
        if (TimeUntilRepairingFinished > 0)
            return false;
        foreach (Module m in modules)
        {
            if (m.nameOfModule == ModuleType.Двигатель && m.state == ModuleStates.Destroed)
                return false;
            if (m.nameOfModule == ModuleType.Мехвод && m.state == ModuleStates.Destroed)
                return false;
            if (m.nameOfModule == ModuleType.Пилот && m.state == ModuleStates.Destroed)
                return false;
        }

        return true;
    }
    public bool CheckIfCanFire()
    {
        if (TimeUntilRepairingFinished > 0)
            return false;
        foreach (Module m in modules)
        {
            if (m.nameOfModule == ModuleType.Орудие && m.state == ModuleStates.Destroed)
                return false;
            if (m.nameOfModule == ModuleType.Наводчик && m.state == ModuleStates.Destroed)
                return false;
        }

        return true;
    }
    public virtual void Explode()
    {
        
    }
}
