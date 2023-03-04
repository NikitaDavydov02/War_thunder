using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Gun {
    public float sensetivityvert = 9f;
	// Use this for initialization
	void Start () {
        Debug.Log("Up: " + transform.up + tankName);
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
        alive = false;
    }
	// Update is called once per frame
	void Update () {
        if (!alive)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TimeSinseFire = 0;
            MainManager.userInterfaseManager.CurbButton(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TimeSinseFire = 0;
            MainManager.userInterfaseManager.CurbButton(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TimeSinseFire = 0;
            MainManager.userInterfaseManager.CurbButton(2);
        }
        curbTypeIndex = MainManager.userInterfaseManager.currentCurbIndex;
        TimeSinseFire += Time.deltaTime;
        float rot = Input.GetAxis("Mouse Y")*sensetivityvert;
        Rot(rot);
        if (Input.GetKey(KeyCode.Space))
        {
            if (TimeSinseFire >= timeOfPer)
                MainManager.buttleResult.AddShoot();
            Fire();
        }
        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit))
        {
            currentDistance = hit.distance;
            MainManager.userInterfaseManager.UpdateDistance(currentDistance);
        }
    }

    //public void ChangeCurbIndex(int index)
    //{
    //    curbTypeIndex = index;
    //}
}
