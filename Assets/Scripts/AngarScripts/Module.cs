using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {
    public ModuleType nameOfModule;
    public ModuleStates state;
    private float timeStillCrit;
    public bool pogar = false;
    [SerializeField]
    List<Material> materials;
    public bool moduleIsHuman = false;
    public string lastDamag;
    public float hp;
    public float currentHP;
    // Use this for initialization
    void Start () {
        state = ModuleStates.Normal;
        timeStillCrit = 1;
        if (nameOfModule == ModuleType.Бензобак)
            hp = 200;
        if (nameOfModule == ModuleType.Боеукладка)
            hp = 150;
        if (nameOfModule == ModuleType.Двигатель)
            hp = 300;
        if (nameOfModule == ModuleType.Заряжающий|| nameOfModule == ModuleType.Командир|| nameOfModule == ModuleType.Мехвод|| nameOfModule == ModuleType.Наводчик|| nameOfModule == ModuleType.Радист)
            hp = 100;
        if (nameOfModule == ModuleType.МеханизмПоворотаБашни)
            hp = 150;
        if (nameOfModule == ModuleType.Оптика)
            hp = 10;
        if (nameOfModule == ModuleType.Орудие)
            hp = 175;
        if (nameOfModule == ModuleType.Рация)
            hp = 75;
        if (nameOfModule == ModuleType.Трансмиссия)
            hp = 250;
        currentHP = hp;
    }
	
	// Update is called once per frame
	void Update () {
        timeStillCrit += Time.deltaTime;
	}

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Point>())
            return;
        if (other.gameObject.GetComponent<Oskolok>())
        {
            Debug.Log("Ohohosheniki hoho!");
            Crite(other.gameObject.name);
            if(state!=ModuleStates.Destroed)
                Destroy(other.gameObject);
        }
        //Crite(other.gameObject.name);
    }
    public void Crite(string name, bool destroy=false)
    {
        lastDamag = name;
            timeStillCrit = 0;
            bool stateChanged = false;
            if (state != ModuleStates.Destroed)
            {
                state++;
                stateChanged = true;
                if (destroy)
                {
                    state = ModuleStates.Destroed;
                    Debug.Log("Destroy!");
                }
            }
            else
            {
                state = ModuleStates.Destroed;
            }
            if (lastDamag == "Т-34" && stateChanged)
            {
                if (state == ModuleStates.Damaged)
                    StartCoroutine(MainManager.userInterfaseManager.ModuleDamaged(nameOfModule.ToString()));
                if (state == ModuleStates.Crit)
                    StartCoroutine(MainManager.userInterfaseManager.ModuleCrit(nameOfModule.ToString()));
                if (state == ModuleStates.Destroed)
                    StartCoroutine(MainManager.userInterfaseManager.ModuleDestroied(nameOfModule.ToString()));
            }
            if (materials.Count > 0)
            {
                if (moduleIsHuman)
                {
                    for(int i = 0; i < transform.childCount; i++)
                    {
                        Transform t = transform.GetChild(i);
                        t.gameObject.GetComponent<Renderer>().material = materials[(int)state];
                    }
                }
                else
                    GetComponent<Renderer>().material = materials[(int)state];
            }
            if(nameOfModule==ModuleType.Бензобак|| nameOfModule == ModuleType.Двигатель)
            {
                int random = Random.RandomRange(0, 10);
                if (random == 1)
                {
                    pogar = true;
                }
            }
    }

    public void UpdateColor()
    {
        if (state == ModuleStates.Damaged)
            StartCoroutine(MainManager.userInterfaseManager.ModuleDamaged(nameOfModule.ToString()));
        if (state == ModuleStates.Crit)
            StartCoroutine(MainManager.userInterfaseManager.ModuleCrit(nameOfModule.ToString()));
        if (state == ModuleStates.Destroed)
            StartCoroutine(MainManager.userInterfaseManager.ModuleDestroied(nameOfModule.ToString()));
    }

    public void AlternativeCrite(string name,float damage)
    {
        lastDamag = name;
        timeStillCrit = 0;
        bool stateChanged = false;
        currentHP -= damage;
        if (state != ModuleStates.Destroed)
        {
            if (hp * 0.66f < currentHP)
                state = ModuleStates.Normal;
            else if (hp * 0.33f < currentHP && currentHP <= hp * 0.66f)
                state = ModuleStates.Damaged;
            else if (0 < currentHP && hp * 0.33f >= currentHP)
                state = ModuleStates.Crit;
            else
                state = ModuleStates.Destroed;
            stateChanged = true;
        }
        else
        {
            state = ModuleStates.Destroed;
        }
        if (lastDamag == "Т-34" && stateChanged)
        {
            if (state == ModuleStates.Damaged)
                StartCoroutine(MainManager.userInterfaseManager.ModuleDamaged(nameOfModule.ToString()));
            if (state == ModuleStates.Crit)
                StartCoroutine(MainManager.userInterfaseManager.ModuleCrit(nameOfModule.ToString()));
            if (state == ModuleStates.Destroed)
                StartCoroutine(MainManager.userInterfaseManager.ModuleDestroied(nameOfModule.ToString()));
        }
        if (materials.Count > 0)
        {
            if (moduleIsHuman)
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
        if (nameOfModule == ModuleType.Бензобак || nameOfModule == ModuleType.Двигатель)
        {
            int random = Random.RandomRange(0, 10);
            if (random == 1)
            {
                pogar = true;
            }
        }
    }
}
