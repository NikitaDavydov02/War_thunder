using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Module : MonoBehaviour {
    //REFACTORED
    public ModuleType nameOfModule;
    public ModuleStates state { get; private set; }
    public float maxHp { get; private set; }
    public float currentHP { get; private set; }
    public bool IsFiring { get; private set; }
    public float TimeOfFiring { get; private set; }
    public bool flameable = false;
    public bool explosive = false;
    private List<Material> materials;
    private GameObject fire;
    public ModuleController controller;


    void Start () {
        materials = new List<Material>();
        materials.Add(Resources.Load("Materials/UsualModule")as Material);
        materials.Add(Resources.Load("Materials/Damaged") as Material);
        materials.Add(Resources.Load("Materials/Crit") as Material);
        materials.Add(Resources.Load("Materials/Destroyed") as Material);
        state = ModuleStates.Normal;
        TimeOfFiring = 0;
        //timeStillCrit = null;
        if (nameOfModule == ModuleType.Бензобак)
            maxHp = 200;
        if (nameOfModule == ModuleType.Боеукладка)
            maxHp = 150;
        if (nameOfModule == ModuleType.Двигатель)
            maxHp = 300;
        if (IsHuman())
            maxHp = 100;
        if (nameOfModule == ModuleType.МеханизмПоворотаБашни)
            maxHp = 150;
        if (nameOfModule == ModuleType.Оптика)
            maxHp = 10;
        if (nameOfModule == ModuleType.Орудие)
            maxHp = 175;
        if (nameOfModule == ModuleType.Рация)
            maxHp = 75;
        if (nameOfModule == ModuleType.Трансмиссия)
            maxHp = 250;
        currentHP = maxHp;
    }
	
	// Update is called once per frame
	void Update () {
        if (IsFiring)
        {
            TimeOfFiring += Time.deltaTime;
            fire.transform.position = this.transform.position;
        }

	}

    public bool IsHuman()
    {
        if (nameOfModule == ModuleType.Мехвод ||
            nameOfModule == ModuleType.Наводчик ||
            nameOfModule == ModuleType.Радист ||
            nameOfModule == ModuleType.Командир ||
            nameOfModule == ModuleType.Заряжающий ||
            nameOfModule == ModuleType.Пилот)
            return true;
        return false;
    }
    public void Damage(float damage, string killerName)
    {
        Debug.Log("Module " + nameOfModule + " damaged by " + killerName);
        //OnModuleDamaged(killerName);
        controller.Killer = killerName;
        currentHP -= damage;
        if (currentHP < 0)
            currentHP = 0;
        if (state != ModuleStates.Destroed)
        {
            if (maxHp * 0.66f < currentHP)
                state = ModuleStates.Normal;
            else if (maxHp * 0.33f < currentHP && currentHP <= maxHp * 0.66f)
                state = ModuleStates.Damaged;
            else if (0 < currentHP && maxHp * 0.33f >= currentHP)
                state = ModuleStates.Crit;
            else
                state = ModuleStates.Destroed;
        }
        UpdateColor();
        if (flameable)
        {
            int random = UnityEngine.Random.RandomRange(0, 10);
            if (random == 1)
            {
                if (!IsFiring)
                {
                    InstantiateFire();
                }
                IsFiring = true;

            }
        }
        if (explosive && state== ModuleStates.Destroed)
        {
           OnModuleExplode();
           //InstantiateFire();
        }
        //OnModuleDamaged();
    }
    public void InstantiateFire()
    {
        Debug.Log("Instatniate fire");
        fire = Instantiate(Resources.Load("Prefabs/Fire") as GameObject);
        fire.transform.position = this.transform.position;
        //Vector3 scale = this.transform.localScale;
        //Vector3 newScale = new Vector3(1 / scale.x, 1 / scale.y, 1 / scale.z);
        fire.transform.SetParent(this.transform, false);
        fire.transform.localPosition = Vector3.zero;
        //fire.transform.localScale = newScale;
    }
    private void OnModuleExplode()
    {
        EventHandler eventHandler = ModuleExplode;
        if (eventHandler != null)
            eventHandler(this, new EventArgs());
    }
    public event EventHandler ModuleExplode;
    //private void OnModuleDamaged()
    //{
    //    EventHandler eventHandler = ModuleDamaged;
    //    if (eventHandler != null)
    //        eventHandler(this, new ModuleDamagedEventArgs(nameOfModule,state));
    //}
    //public event EventHandler ModuleDamaged;
    public void Repair()
    {
        state = ModuleStates.Normal;
        UpdateColor();
    }
    public void Extinguish()
    {
        IsFiring = false;
        TimeOfFiring = 0;
        Destroy(fire);
    }
    private void UpdateColor()
    {
        if (materials.Count > 0)
        {
            if (IsHuman())
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform t = transform.GetChild(i);
                    t.gameObject.GetComponent<Renderer>().material = materials[(int)state];
                }
            }
            else
                GetComponent<Renderer>().material = materials[(int)state];
        }
    }
}
public class ModuleDamagedEventArgs:EventArgs
{
  public string Killer { get; private set; }
//    public ModuleStates moduleState { get; private set; }
    public ModuleDamagedEventArgs(string killer)
    {
        Killer = killer;
    }
}
