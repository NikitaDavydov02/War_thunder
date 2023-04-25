using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpuse : MonoBehaviour {
    //REFACTORED_1
    [SerializeField]
    private CorpuseAudioManager audioManager;
    [SerializeField]
    private Rigidbody rigidbody;
    public float maxForce;
    public float rotForce;
    [SerializeField]
    protected ModuleController controller;
    //public float maxSpeed = 10f;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }
    public void Move(Vector3 input)
    {
        if (!controller.alive||!controller.canMove|| MainManager.GameStatus!=GameStatus.Playing)
            return;
        rigidbody.AddForce(input*maxForce);
        audioManager.ChangePitch(input.magnitude);
    }
    public void Turn(float turn)
    {
        if (!controller.alive|| !controller.canMove|| MainManager.GameStatus != GameStatus.Playing)
            return;
        rigidbody.AddTorque(transform.parent.transform.up * rotForce * turn);
    }
}
