using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    [SerializeField]
    ToweAudioManager audioManager;
    public float rotSpeed = 2f;
    [SerializeField]
    private ModuleController controller;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    public void Rotate(float rotY)
    {
        if (!controller.alive)
            return;
        if (audioManager != null)
        {
            if (rotY == 0)
                audioManager.ChangeRot(false);
            else
                audioManager.ChangeRot(true);
        }
        transform.Rotate(0, rotY, 0, Space.Self);
    }
}
