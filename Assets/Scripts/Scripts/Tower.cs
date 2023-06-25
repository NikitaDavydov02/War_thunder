using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    //REFACTORED_1

    [SerializeField]
    ToweAudioManager audioManager;
    public float rotSpeed = 2f;
    [SerializeField]
    protected ModuleController controller;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    public void Rotate(float input)
    {
        if (!controller.alive|| MainManager.GameStatus != GameStatus.Playing)
            return;
        if (audioManager != null)
        {
            if (input == 0)
                audioManager.PlayRotationSound(false);
            else
                audioManager.PlayRotationSound(true);
        }
        transform.Rotate(0, input*rotSpeed*Time.deltaTime, 0, Space.Self);
    }
}
