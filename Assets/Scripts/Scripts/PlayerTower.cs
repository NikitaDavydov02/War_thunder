using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : Tower {
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!alive)
            return;
        float rotY = Input.GetAxis("Mouse X");
        Rotate(rotY);
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
}
