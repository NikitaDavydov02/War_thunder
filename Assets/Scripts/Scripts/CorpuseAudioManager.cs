using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpuseAudioManager : MonoBehaviour {
    //REFACTORED_1
    public AudioSource engineAudioSource;
    [SerializeField]
    public AudioClip stopClip;
    [SerializeField]
    public AudioClip gazClip;
    public float k = 1;
    // Use this for initialization
    void Start () {
        engineAudioSource.clip = stopClip;
        engineAudioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void ChangePitch(float pitch)
    {
        engineAudioSource.pitch = 1 - pitch/2;
    }
}
