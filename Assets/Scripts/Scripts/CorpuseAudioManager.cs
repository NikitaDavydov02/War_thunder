using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineAudioManager : MonoBehaviour {
    //REFACTORED_1
    public AudioSource engineAudioSource;
    [SerializeField]
    public AudioClip stopClip;
    [SerializeField]
    public AudioClip gazClip;
    public float k = 1;
    public float maxPitch = 10f;
    public float minPitch = 1f;
    // Use this for initialization
    void Start () {
        engineAudioSource.clip = gazClip;
        engineAudioSource.Play();
       
	}
	
	// Update is called once per frame
	void Update () {
        k = maxPitch - minPitch;
    }

    public void ChangePitch(float gaz)
    {
        engineAudioSource.pitch = k * Mathf.Abs(gaz)+minPitch;
        if (engineAudioSource.pitch < minPitch)
            engineAudioSource.pitch = minPitch;
        if (engineAudioSource.pitch > maxPitch)
            engineAudioSource.pitch = maxPitch;
    }
}
