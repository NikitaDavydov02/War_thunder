using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleResult : MonoBehaviour {
    private int frags = 0;
    public int serebro { get; private set; }
    public int expirience { get; private set; }
    public int shoots { get; private set; }
    public float puti { get; private set; }
    public bool win { get; private set; }
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
        serebro = Random.Range(1000, 2000);
        expirience = Random.Range(100, 200);
        shoots = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AddShoot()
    {
        shoots++;
    }
    public void AddFrag()
    {
        frags++;
        serebro += Random.Range(1000, 5000);
        expirience += Random.Range(300, 500);
    }
    public int GetFrags()
    {
        Destroy(this.gameObject);
        return frags;
    }
    public void SetPuti(float puti)
    {
        this.puti = puti;
    }
    public void SetWin(bool win)
    {
        this.win = win;
    }
}
