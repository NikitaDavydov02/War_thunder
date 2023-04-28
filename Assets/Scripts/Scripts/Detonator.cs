using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    [SerializeField]
    Curb curbScript;

    public float directDamage = 300f;
    public float impliciDamage = 10f;
    public float vzvodTime = 0.05f;
    
    private Vector3 lastPosition;
    [SerializeField]
    private float characteristicDamageLength = 1;
    // Use this for initialization
    void Start()
    {
        lastPosition = transform.position;
    }
    public void Vzvesti()
    {
        timeSinceFire = 0;
        Debug.Log("Vzveden method");
    }
    // Update is called once per frame
    void Update()
    {
        if (destroyed)
            return;
        if (timeSinceFire >= vzvodTime)
        {
            vzveden = true;
            Debug.Log("Vzv");
        }
           
        if(timeSinceFire>=0)
            timeSinceFire += Time.deltaTime;
        if (timeSinceFire > 10)
        {
            Destroy(this.gameObject.transform.parent.gameObject);
            return;
        }
        if (type != TypeOfCurp.Кумулятивный)
            probitie -= ProbDecendingSpeed * Time.deltaTime;

        //Detect hiting
        RaycastHit hit;
        Debug.DrawLine(lastPosition, transform.position, Color.red, 10f);
        if (Physics.Raycast(new Ray(lastPosition, transform.position - lastPosition), out hit)&&vzveden)
        {
            Debug.Log("Hit!");
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.tag != "Curb" && (hit.point - lastPosition).magnitude <= 5*(transform.position - lastPosition).magnitude)
            {
                // (hit.point - lastPosition).magnitude <= (transform.position -lastPosition).magnitude
                //GameObject sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sph.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                //sph.GetComponent<Collider>().isTrigger = true;
                //sph.transform.position = hit.point;

                //sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sph.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                //sph.GetComponent<Collider>().isTrigger = true;
                //sph.transform.position = lastPosition;

                //sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sph.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                //sph.GetComponent<Collider>().isTrigger = true;
                //sph.transform.position = transform.position;
                //Debug.Log("Curb hited something " + hitObject.name);
                bool probil = false;
                transform.position = hit.point;
                
                Collider[] searchForBronya = Physics.OverlapSphere(hit.point, 0.05f);
                Bronya b = null;
                foreach (Collider collider in searchForBronya)
                {
                    b = collider.gameObject.GetComponent<Bronya>();
                    if (b != null)
                        break;
                }
                if (b != null)
                {
                    if (b.bronyaThickness >= probitie)
                    {
                        probil = false;
                        source.clip = Resources.Load("Music/Rikochet") as AudioClip;
                        source.volume = 0.2f;
                        source.Play();
                    }
                    else
                    {
                        source.clip = Resources.Load("Music/Probitie") as AudioClip;
                        source.Play();
                        probil = true;
                    }
                }

                if (probil||type==TypeOfCurp.Bomb)
                {
                    if (hitObject.tag == "Tank")
                    {
                        string[] s = gameObject.transform.parent.name.Split('_');
                        hitObject.GetComponent<ModuleController>().Killer = s[0];
                    }
                    Damage();
                }
                StartCoroutine(Die());
            }
            
        }
        lastPosition = transform.position;
    }
    private void Damage()
    {

        Vector3 add = transform.TransformDirection(Vector3.forward * ExplosionDistance);

        List<Module> damagedModels = new List<Module>();
        if (type == TypeOfCurp.Фугасный||type==TypeOfCurp.Bomb)
        {
            Collider[] hits;
            Vector3 detonationCenter = transform.position+add;
            GameObject sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sph.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            sph.GetComponent<Collider>().isTrigger = true;
            sph.transform.position = detonationCenter;


            hits = Physics.OverlapSphere(detonationCenter, explosionRadius);
            foreach (Collider hited in hits)
            {
                Module m = hited.gameObject.GetComponent<Module>();
                if (m != null)
                {
                    float distance = Vector3.Magnitude(m.transform.position - detonationCenter);
                    float realDamage = impliciDamage * Mathf.Exp(-distance/characteristicDamageLength);
                    m.Damage(realDamage);
                    damagedModels.Add(m);
                }
            }


        }
        if (type == TypeOfCurp.Бронебойный || type == TypeOfCurp.Кумулятивный)
        {
            Collider[] capsuleHits = Physics.OverlapCapsule(transform.position, transform.position + add, 0.2f);
            foreach (Collider hited in capsuleHits)
            {
                Module m = hited.gameObject.GetComponent<Module>();
                if (m != null)
                {
                    m.Damage(directDamage);
                    damagedModels.Add(m);
                }
            }
        }
        if (gameObject.transform.parent.name == MainManager.buttleManager.clientTank.name)
        {
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
    private IEnumerator Die()
    {
        destroyed = true;
        if(curbScript!=null)
            curbScript.Stop();
        GameObject smoke = Instantiate(Resources.Load("Prefabs/Smoke")as GameObject);
        smoke.transform.position = transform.position;
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject.transform.parent.gameObject);
        yield return null;
        Destroy(smoke);
    }
}