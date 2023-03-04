using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonator : MonoBehaviour
{
    [SerializeField]
    private GameObject dimPrefab;
    private Vector3 lastPosition;
    public float probitie = 100;
    public float ProbSpeed = 60f;
    private float time;
    private float timeVzvod=0;
    public bool vzveden = false;
    public bool probil = false;
    public TypeOfCurp type;
    public float radius = 1.5f;
    private bool bolisheNeKituu = false;
    [SerializeField]
    GameObject oskolokPrefab;
    public int oskolkov;
    public int oskolokDamage = 50;
    public float oskolokSpeed;
    Curb curbScript;
    public float pryamoyDamage = 300f;
    public float vzvodTime = 0.002f;

    // Use this for initialization
    void Start()
    {
        lastPosition = transform.position;
        curbScript = transform.parent.gameObject.GetComponent<Curb>();
    }

    // Update is called once per frame
    void Update()
    {
        timeVzvod += Time.deltaTime;
        if (timeVzvod >= 0.05f)
            vzveden = true;
        time += Time.deltaTime;
        if (time > 10)
        {
            Destroy(this.gameObject.transform.parent.gameObject);
            return;
        }
        if (type != TypeOfCurp.Кумулятивный)
            probitie -= ProbSpeed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(new Ray(lastPosition, transform.position - lastPosition), out hit)&&!bolisheNeKituu)
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.tag == "Terrain")
            {
                //transform.parent.transform.position = hit.point;
                if(gameObject.transform.parent.gameObject.name=="Т-34")
                    Debug.Log("Kh!");
                StartCoroutine(Die());
                Debug.Log("Terrain");
                //DieVoid();
                //Curb curb = transform.parent.transform.gameObject.GetComponent<Curb>();
                //curb.gun.lastPopadaniePoint = hit.point;
            }
            if (hitObject.tag == "Tank")
            {
                bolisheNeKituu = true;
                transform.parent.transform.position = hit.point;
                Curb cu = transform.parent.GetComponent<Curb>();
                cu.g = 0;
                cu.speedVector *= 0.01f;
                Collider[] s = Physics.OverlapCapsule(hit.point - cu.speedVector, hit.point + cu.speedVector, 0.01f);
                bool probil = true;

                //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                //sphere.GetComponent<Collider>().isTrigger = true;
                //sphere.transform.position = hit.point;
                Debug.Log("vstavil");

                foreach (Collider hited in s)
                {
                    Bronya b = hited.gameObject.GetComponent<Bronya>();
                    if (b != null)
                    {
                        if (hitObject.name == MainManager.buttleManager.humanTank.name)
                            Debug.Log("Chekau: "+b.bronya);
                        AudioSource source = Instantiate(MainManager.musicManager.sourcePrefab);
                        source.volume = 0.5f;
                        source.transform.position = transform.position;

                        if (b.bronya >= probitie)
                        {
                            probil = false;
                            source.clip = Resources.Load("Music/Rikochet") as AudioClip;
                            source.volume = 0.2f;
                            source.Play();

                            //DieVoid();
                            StartCoroutine(Die());
                            return;
                        }
                        else
                        {
                            if (hitObject.name == MainManager.buttleManager.humanTank.name)
                                Debug.Log("probitie" + b.bronya);
                            source.clip = Resources.Load("Music/Probitie") as AudioClip;
                            source.Play();
                            //Debug.Log("Probitie!");
                        }
                    }
                }
                Vector3 speed = curbScript.speedVector;
                float putiDoVzryva = vzvodTime * Vector3.Magnitude(curbScript.speedVector);
                Vector3 add = Vector3.ClampMagnitude(speed, putiDoVzryva);
                if (type == TypeOfCurp.Фугасный)
                {
                    Collider[] hits;
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    hits = Physics.OverlapSphere(transform.position+add, radius/(i+1));
                    //    foreach (Collider hited in hits)
                    //    {
                    //        Module m = hited.gameObject.GetComponent<Module>();
                    //        if (m != null)
                    //        {
                    //            //m.Crite(name, false);
                    //            //m.AlternativeCrite(name, oskolokDamage);
                    //        }
                    //    }
                    //}
                    Vector3 detonationCenter = transform.position + add;
                    GameObject sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sph.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    sph.GetComponent<Collider>().isTrigger = true;
                    sph.transform.position = detonationCenter;

                    hits = Physics.OverlapSphere(detonationCenter, radius);
                    foreach (Collider hited in hits)
                    {
                        Module m = hited.gameObject.GetComponent<Module>();
                        if (m != null)
                        {
                            //m.Crite(name, false);
                            float distance = Vector3.Magnitude(m.transform.position - detonationCenter);
                            float realDamage = oskolokDamage * Mathf.Exp(-distance)*5;
                           //Debug.Log("D=" + distance + "  Exp=" + Mathf.Exp(-distance)+ "  Damage=" +realDamage);
                            m.AlternativeCrite(name, realDamage);
                        }
                    }
                    foreach (Collider h in s)
                    {
                        Debug.Log(h.gameObject.name);
                        Module mo = h.gameObject.GetComponent<Module>();
                        if (mo != null)
                        {
                            mo.AlternativeCrite(name, pryamoyDamage);
                        }
                    }





                    //for (int i = 0; i < oskolkov; i++)
                    //{
                    //    GameObject osk = Instantiate(oskolokPrefab) as GameObject;
                    //    osk.name = name;
                    //    //osk.transform.position = new Vector3(1407, 33, 3915);
                    //    osk.transform.position = transform.position + add;
                    //    osk.transform.eulerAngles = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
                    //    //osk.gameObject.GetComponent<Oskolok>().rotation = new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
                    //    Oskolok oskolocScript = osk.GetComponent<Oskolok>();
                    //    oskolocScript.speed = oskolokSpeed;
                    //    oskolocScript.damage = oskolokDamage;
                    //}

                }
                if (type == TypeOfCurp.Бронебойный || type == TypeOfCurp.Кумулятивный)
                {
                    //Curb cu = transform.parent.GetComponent<Curb>();
                    //cu.g = 0;
                    //cu.speedVector *= 0.01f;
                    //RaycastHit hhit;
                    //Collider[] s = Physics.OverlapCapsule(hit.point - cu.speedVector, hit.point + cu.speedVector, 0.01f);
                    //foreach (Collider hited in s)
                    //{
                    //    Bronya b = hited.gameObject.GetComponent<Bronya>();
                    //    if (b != null)
                    //    {
                    //        if (b.bronya >= probitie)
                    //        {
                    //            Debug.Log("Ne probil!");
                    //            Die();
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            Debug.Log("Probitie!");
                    //        }
                    //    }
                    //}
                    foreach (Collider hited in s)
                    {
                        Debug.Log(hited.gameObject.name);
                        Module m = hited.gameObject.GetComponent<Module>();
                        if (m != null)
                        {
                            m.Crite(name,false);
                        }
                    }
                    //DieVoid();
                }
                StartCoroutine(Die());
            }
        }
        ChangeLastPosition();
    }
    private void ChangeLastPosition()
    {
        lastPosition = transform.position;
    }
    private IEnumerator Die()
    {
        GameObject dim = Instantiate(dimPrefab);
        dim.transform.position = transform.position;
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject.transform.parent.gameObject);
        yield return null;
        Destroy(dim);
    }
    private void DieVoid()
    {
        Destroy(this.gameObject.transform.parent.gameObject);
    }
}