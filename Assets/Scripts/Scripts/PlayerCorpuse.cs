using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCorpuse : Corpuse {
    [SerializeField]
    CorpuseAudioManager audioManager;
    public float currentSpeed;
    private Vector3 lastPos;
    private float lastAngleY;
    public float puti { get; private set; }
    [SerializeField]
    GameObject oskolokPrefab;
    public int oskolkov;
    //[SerializeField]
    // Use this for initialization
    void Start () {
        puti = 0;
    }

    // Update is called once per frame
    void Update () {
        Debug.Log("Alive: " + alive);
        if (!alive)
            return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < oskolkov; i++)
            {
                GameObject osk = Instantiate(oskolokPrefab) as GameObject;
                //osk.transform.position = new Vector3(1407, 33, 3915);
                osk.transform.position = transform.position;
                osk.gameObject.GetComponent<Oskolok>().rotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            }
        }

        float aheadInput = Input.GetAxis("Vertical") * Time.deltaTime * speed * 1300000;
        Debug.Log("Aheadinput!" + aheadInput);
        Vector3 movement = new Vector3(0, 0, aheadInput);
        movement = transform.parent.transform.TransformDirection(movement);
        float turn = Input.GetAxis("Horizontal")*2500000*Time.deltaTime;
        currentSpeed = Mathf.Abs((transform.position - lastPos).magnitude) / Time.deltaTime;
        currentRotSpeed = Mathf.Abs(transform.eulerAngles.y - lastAngleY) / Time.deltaTime;
        float s;
        if (lastPos != Vector3.zero)
            s = (transform.position - lastPos).magnitude;
        else
            s = 0;
        puti += s;
        //Debug.Log(puti);
        lastPos = transform.position;
        lastAngleY = transform.eulerAngles.y;
        if (movement != new Vector3(0, 0, 0)&&(currentSpeed < maxSpeed))
        {
            Debug.Log("Move!" + movement);
            Move(movement);
            if (audioManager != null)
                audioManager.ChangeGaz(true);
        }
        else
        {
            if (audioManager != null)
                audioManager.ChangeGaz(false);
        }
        Turn(turn);
        
    }
    private bool alive = true;
    void Awake()
    {
        Messenger.AddListener(GameEvent.HUMANTANKDESTROIED, ThisTankDestroied);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.HUMANTANKDESTROIED, ThisTankDestroied);
    }

    private void ThisTankDestroied()
    {
        MainManager.buttleResult.SetPuti(puti);
        alive = false;
    }
}
