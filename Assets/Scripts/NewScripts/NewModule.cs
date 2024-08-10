using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewModule : MonoBehaviour
{
    //REFACTORED
    public ModuleType nameOfModule;
    public ModuleStates state { get; private set; }
    public float maxHp { get; private set; }
    public float currentHP { get; private set; }
    public bool IsFiring { get; private set; }
    public float TimeOfFiring { get; private set; }
    
    public float flamaProbability = 0;
    public float flamePropgationVelosity = 0.2f;
    public float flameRadius = 0;
    public bool explosive = false;
    private List<Material> materials;
    private GameObject fire;
   // public ModuleController controller;


    void Start()
    {
        materials = new List<Material>();
        materials.Add(Resources.Load("Materials/UsualModule") as Material);
        materials.Add(Resources.Load("Materials/Damaged") as Material);
        materials.Add(Resources.Load("Materials/Crit") as Material);
        materials.Add(Resources.Load("Materials/Destroyed") as Material);
        state = ModuleStates.Normal;
        TimeOfFiring = 0;
        //timeStillCrit = null;
        if (nameOfModule == ModuleType.Бензобак)
            maxHp = 200;
        else if (nameOfModule == ModuleType.Боеукладка)
            maxHp = 150;
        else if (nameOfModule == ModuleType.Двигатель)
            maxHp = 300;
        else if (IsHuman())
            maxHp = 100;
        else if (nameOfModule == ModuleType.МеханизмПоворотаБашни)
            maxHp = 150;
        else if (nameOfModule == ModuleType.Оптика)
            maxHp = 10;
        else if (nameOfModule == ModuleType.Орудие)
            maxHp = 175;
        else if (nameOfModule == ModuleType.Рация)
            maxHp = 75;
        else if (nameOfModule == ModuleType.Трансмиссия)
            maxHp = 250;
        else if (nameOfModule == ModuleType.Крыло)
            maxHp = 50;
        else maxHp = 100;
        currentHP = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFiring)
        {
            currentHP = 0;
            UpdateColor();
            TimeOfFiring += Time.deltaTime;
            flameRadius += Time.deltaTime * flamePropgationVelosity;
            if (fire != null)
                fire.transform.position = this.transform.position;
            Collider[] capsuleHits = Physics.OverlapSphere(transform.position, flameRadius);
            foreach(Collider collider in capsuleHits)
            {
                NewModule module = collider.gameObject.GetComponent<NewModule>();
                if (module != null && transform.root==module.gameObject.transform.root)
                {
                    if (module.flamaProbability != 0)
                        module.SetFire();
                    else
                        module.Damage(1000);
                }
            }
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
    public void Damage(float damage)
    {
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
        if (flamaProbability!=0)
        {
            float random = UnityEngine.Random.RandomRange(0, 1);
            if (random < flamaProbability)
                SetFire();
        }
        if (explosive && state == ModuleStates.Destroed)
        {
            OnModuleExplode();
        }
        //OnModuleDamaged();
    }
    public void SetFire()
    {
        if (!IsFiring)
        {
            InstantiateFire();
            IsFiring = true;
        }
    }
    private void InstantiateFire()
    {
        //  return;
        //Debug.Log("Instatniate fire");
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
    public void Repair()
    {
        state = ModuleStates.Normal;
        UpdateColor();
    }
    public void Extinguish()
    {
        IsFiring = false;
        TimeOfFiring = 0;
        flameRadius = 0;
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
