using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewModuleContoller : MonoBehaviour
{
    //REFACTORED_1
    [SerializeField]
    protected List<NewModule> modules;
    protected List<NewModule> crew;
    [SerializeField]
    private int minCrew = 2;

    public float TimeUntilRepairingFinished { get; private set; } = 0;
    public bool alive = true;
    private float repairTime = 0;
    List<NewModule> mustBeRepaired;

    public string Killer { get; set; }

    protected virtual void Start()
    {
        alive = true;
        crew = new List<NewModule>();
        foreach (NewModule module in modules)
        {
            if (module.IsHuman())
                crew.Add(module);
        }

    }
    protected virtual void Update()
    {
        if (!alive || MainManager.GameStatus != GameStatus.Playing)
            return;
       /* foreach (NewModule module in modules)
        {
            if (module.IsFiring)
            {
                if (module.TimeOfFiring > 10)
                {
                    Debug.Log("Die from fire");

                    Die();
                }
            }
        }*/
        if (TimeUntilRepairingFinished > 0)
        {
            TimeUntilRepairingFinished -= Time.deltaTime;
            MainManager.userInterfaseManager.UpdateRepairTime(TimeUntilRepairingFinished);
        }
        if (TimeUntilRepairingFinished < 0)
        {
            TimeUntilRepairingFinished = 0;
            MainManager.userInterfaseManager.UpdateRepairTime(0);
            //Repairing finished
            foreach (NewModule m in mustBeRepaired)
                m.Repair();
        }

        
        int diedCrew = 0;
        foreach (NewModule ec in crew)
            if (ec.state == ModuleStates.Destroed)
                diedCrew++;
        if (crew.Count - diedCrew <= minCrew)
            Die();
       
    }
    public void Repair()
    {
        mustBeRepaired = new List<NewModule>();
        List<float> timeOfRepairing = new List<float>();
        foreach (NewModule module in modules)
            if (module.state >= ModuleStates.Crit)
            {
                mustBeRepaired.Add(module);
                timeOfRepairing.Add(5);
            }

        float SumTimeOfRepair = 0;
        foreach (int t in timeOfRepairing)
            SumTimeOfRepair += t;
        if (SumTimeOfRepair > 0)
            TimeUntilRepairingFinished = SumTimeOfRepair;
    }
    protected void Die()
    {
        if (!alive)
            return;
        alive = false;
        MainManager.buttleManager.PlayerDied(this.gameObject,"");
        Debug.Log("NewModule controller " + this.gameObject.name + " died ");
        /*AudioSource source = Instantiate(MainManager.musicManager.sourcePrefab);
        source.clip = Resources.Load("Music/Crash") as AudioClip;
        source.transform.position = transform.position;
        source.Play();*/
    }
    //private IEnumerator Repair(List<NewModule> repairModules, List<float> time, float sumTime)
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
}
