using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
    //public bool pointIsRed = true;
    public List<GameObject> blue;
    public List<GameObject> red;
    public float scoreOfRed = 100;
    public float speed = 1f;
    [SerializeField]
    private ButtleManager buttleManager;
    public float radius;
    [SerializeField]
    private AudioSource pointAudioSource;
    [SerializeField]
    private AudioClip zahvatClip;
    private bool audioIsPlaying=false;
    [SerializeField]
    Material normalMaterial;
    [SerializeField]
    Material blueMaterial;
    [SerializeField]
    Material redMaterial;
    private bool pointZahvachena = false;
    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material = normalMaterial;

    }
	
	// Update is called once per frame
	void Update () {
        if (buttleManager.gameFinished)
            return;
        blue.Clear();
        red.Clear();
        foreach(GameObject b in buttleManager.blue)
        {
            Vector2 xy = new Vector2(b.transform.position.x - transform.position.x, b.transform.position.z - transform.position.z);
            if (xy.magnitude <= radius)
            {
                blue.Add(b);
            }
        }
        foreach (GameObject r in buttleManager.red)
        {
            Vector2 xy = new Vector2(r.transform.position.x - transform.position.x, r.transform.position.z - transform.position.z);
            if (xy.magnitude <= radius)
            {
                red.Add(r);
                Debug.Log("R: " + r + xy.magnitude);
            }
        }
        float delta = red.Count - blue.Count;
        scoreOfRed += Time.deltaTime * speed * delta;
        if (delta != 0)
        {
            if (!audioIsPlaying)
            {
                pointAudioSource.clip = zahvatClip;
                pointAudioSource.Play();
                audioIsPlaying = true;
            }
        }
        else
        {
            pointAudioSource.Stop();
            audioIsPlaying = false;
        }
        if (scoreOfRed > 100)
        {
            if (GetComponent<Renderer>().material==redMaterial)
                return;
            GetComponent<Renderer>().material = redMaterial;
            buttleManager.Point(true);
            pointAudioSource.Stop();
            pointZahvachena = true;
        }
        if (scoreOfRed < -100)
        {
            if (GetComponent<Renderer>().material == blueMaterial)
                return;
            GetComponent<Renderer>().material = blueMaterial;
            buttleManager.Point(false);
            pointAudioSource.Stop();
            pointZahvachena = true;
        }
    }

    void OnTriigerEnter(Collider other)
    {
        Debug.Log("aaa");
    }
    
}
