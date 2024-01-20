using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Detonator : MonoBehaviour
{
    //REFACTORED_1
    [SerializeField]
    AudioSource source;

    [SerializeField]
    private float probitie = 100;
    public float ProbDecendingSpeed = 60f;

    private float timeSinceFire = -1;
    private bool vzveden = false;
    public float ExplosionDistance = 1f;

    private bool destroyed = false;
    public TypeOfCurp type;
    public float explosionRadius = 1.5f;
    
    //[SerializeField]
    //Curb curbScript;

    public float directDamage = 300f;
    public float impliciDamage = 10f;
    public float vzvodTime = 0.05f;
    
    private Vector3 lastPosition;
    [SerializeField]
    private float characteristicDamageLength = 1;

    public string OwnerName = "";
    // Use this for initialization
    void Start()
    {
        lastPosition = transform.position;
    }
    public void Vzvesti()
    {
        timeSinceFire = 0;
    }
    private bool TryToProbit(Vector3 point, GameObject hitObject)
    {
        //transform.position = hit.point;
        /*GameObject sph = GameObject.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
        sph.transform.localScale *= 0.1f;
        sph.transform.position = point;
        sph.GetComponent<SphereCollider>().isTrigger = true;*/

        Collider[] searchForBronya = Physics.OverlapSphere(point, 0.5f);
        Bronya b = null;
        Debug.Log("Searching for armor");
        foreach (Collider collider in searchForBronya)
        {
            b = collider.gameObject.GetComponent<Bronya>();
            if (b != null)
                break;
        }
        if (b != null)
        {
            Debug.Log("Bronya is found!!" + b.gameObject.name);
            Debug.Log("Bronya is not null");
            if (b.bronyaThickness >= probitie)
            {
                
                source.clip = Resources.Load("Music/Rikochet") as AudioClip;
                //source.volume = 0.2f;
                source.Play();
                Debug.Log("No penetration" + b.gameObject.name);
                return false;
            }
            else
            {
                Debug.Log("Probitie b!" + b.gameObject.name);

                source.clip = Resources.Load("Music/Probitie") as AudioClip;
                source.Play();
                return true;
            }
        }
        Debug.Log("Bronya is not found!!");
        return false;
        
    }
    private void DetectHit()
    {
        if (!vzveden)
            return;
        RaycastHit hit;
        Debug.DrawRay(lastPosition, transform.position - lastPosition, Color.red, 10f);
        if (Physics.Raycast(new Ray(lastPosition, transform.position - lastPosition), out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            Debug.Log("Hit raycasted " + hitObject.name);
            //Debug.Log("Detonator: Hit " + hitObject.name + "hit point " + hit.point);
            //Debug.Log("Hit distance " + hit.distance);
            if (!hitObject.gameObject.name.ToString().Contains(OwnerName) & hitObject.tag != "Curb" && hit.distance <= (transform.position - lastPosition).magnitude)
            //if (hitObject.tag != "Curb"&& hitObject.tag!="Terrain")
            {
                Debug.Log("Hit detected " + hitObject.name);
                
                // Debug.Log("Detonator: Hit " + hitObject.name + "hit point " + hit.point);
                //Debug.Log("Hit distance " + hit.distance);
                //Debug.Log("transform.position - lastPosition " + (transform.position - lastPosition).magnitude);
                // (hit.point - lastPosition).magnitude <= (transform.position -lastPosition).magnitude
                /*GameObject sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sph.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                sph.GetComponent<Collider>().isTrigger = true;
                sph.transform.position = hit.point;*/

                

                if (TryToProbit(hit.point, hitObject)|| type == TypeOfCurp.Bomb)
                {
                    transform.position = hit.point;
                    Debug.Log("Probitie!");
                    Damage();
                    
                }
                //else
                    //Debug.Log("No Probitie:(");

                destroyed = true;
                StartCoroutine(Die());
            }
        }
    }
    void Update()
    {
        if (destroyed)
            return;
        if (timeSinceFire >= vzvodTime && !vzveden)
        {
            vzveden = true;
            //Debug.Log("Ready");
        }
           
        if(timeSinceFire>=0)
            timeSinceFire += Time.deltaTime;
        if (timeSinceFire > 30)
        {
            Destroy(this.gameObject);
            return;
        }
        if (type != TypeOfCurp.Кумулятивный)
            probitie -= ProbDecendingSpeed * Time.deltaTime;

        DetectHit();

        lastPosition = transform.position;
    }
    private void Damage()
    {
        Debug.Log("Detonator: damage");
        Vector3 add = transform.TransformDirection(Vector3.forward * ExplosionDistance);
        //Debug.Log("Add: " + add.magnitude);
        //Debug.Log("Enter point: " + transform.position);
        //Debug.Log("Detonation center: " + add);

        List<Module> damagedModels = new List<Module>();
        if (type == TypeOfCurp.Фугасный||type==TypeOfCurp.Bomb)
        {
            Collider[] hits;
            Vector3 detonationCenter = transform.position+add;

            /*GameObject sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sph.transform.position = detonationCenter;
            sph.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            sph.GetComponent<Collider>().isTrigger = true;
            */

            hits = Physics.OverlapSphere(detonationCenter, explosionRadius);
            foreach (Collider hited in hits)
            {
                Module m = hited.gameObject.GetComponent<Module>();
                if (m != null)
                {
                    float distance = Vector3.Magnitude(m.transform.position - detonationCenter);
                    float realDamage = impliciDamage * Mathf.Exp(-distance/characteristicDamageLength);
                    m.Damage(realDamage, OwnerName);
                    //Debug.Log("Damage module: " + m.nameOfModule + " " + realDamage);
                    
                    damagedModels.Add(m);
                }
            }


        }
        if (type == TypeOfCurp.Бронебойный || type == TypeOfCurp.Кумулятивный)
        {
            Collider[] capsuleHits = Physics.OverlapCapsule(transform.position,transform.position + add, 0.2f);
            foreach (Collider hited in capsuleHits)
            {
                Module m = hited.gameObject.GetComponent<Module>();
                if (m != null)
                {
                    m.Damage(directDamage, OwnerName);
                    //m.controller.Killer = OwnerName;
                    //Debug.Log("Damage module: " + m.nameOfModule + " " + directDamage);
                    damagedModels.Add(m);
                }
            }
        }
        if (MainManager.buttleManager.clientTank!=null && OwnerName == MainManager.buttleManager.clientTank.name)
        {
            //Debug.Log("Display damaged modules");
            foreach (Module m in damagedModels)
            {
                if (m.state == ModuleStates.Damaged)
                    MainManager.userInterfaseManager.ModuleDamaged(m.nameOfModule.ToString());
                if (m.state == ModuleStates.Destroed)
                    MainManager.userInterfaseManager.ModuleDestroied(m.nameOfModule.ToString());
                if (m.state == ModuleStates.Crit)
                    MainManager.userInterfaseManager.ModuleCrit(m.nameOfModule.ToString());
            }
        }
        
    }
    public event EventHandler Detonate;

    private void OnDetonate()
    {
        EventHandler handler = Detonate;
        if (handler != null)
            handler(this, new EventArgs());
    }
    private IEnumerator Die()
    {
        
        OnDetonate();
        GameObject smoke = Instantiate(Resources.Load("Prefabs/Smoke")as GameObject);
        smoke.transform.position = transform.position;
        yield return new WaitForSeconds(1.5f);
        Destroy(smoke);
        Destroy(this.gameObject);
        yield return null;
    }
}