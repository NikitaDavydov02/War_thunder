using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneModuleController : ModuleController
{
    private float destoingImpuls = 20000f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        int diedCrew = 0;
        foreach (Module ec in crew)
        {
            if (ec.state == ModuleStates.Destroed)
            {
                diedCrew++;
                if (ec.nameOfModule == ModuleType.Пилот)
                {
                    //canMove = false;
                    //canFire = false;
                    //canReloadGun = false;
                }
            }
            
        }
        foreach(Module m in modules)
            if (m.nameOfModule == ModuleType.Крыло && m.state == ModuleStates.Destroed)
            {
                //Die();
            }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision: " + collision.impulse.magnitude);
        if (collision.impulse.magnitude > destoingImpuls)
        {
            Debug.Log("Destroy");
            Explode();
        }
    }
    public override void Explode()
    {
        GameObject explosion = Instantiate(Resources.Load("Prefabs/Explosion") as GameObject);
        explosion.transform.position = transform.position;
        AudioSource source = Instantiate(MainManager.musicManager.sourcePrefab);
        source.clip = Resources.Load("Music/Crash") as AudioClip;
        source.transform.position = transform.position;
        source.Play();
        InstantiateFire();

        Collider[] hits = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider hited in hits)
        {
            Module m = hited.gameObject.GetComponent<Module>();
            if (m != null)
            {
                m.Damage(10000, gameObject.name);
            }
        }
    }
    private void InstantiateFire()
    {
        Debug.Log("Instatniate fire");
        GameObject fire = Instantiate(Resources.Load("Prefabs/Fire") as GameObject);
        fire.transform.position = this.transform.position;
        //Vector3 scale = this.transform.localScale;
        //Vector3 newScale = new Vector3(1 / scale.x, 1 / scale.y, 1 / scale.z);
        fire.transform.SetParent(this.transform, false);
        fire.transform.localPosition = Vector3.zero;
        //fire.transform.localScale = newScale;
    }
}
