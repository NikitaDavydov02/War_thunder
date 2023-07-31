using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudioManager : MonoBehaviour {
    //REFACTORED_1
    [SerializeField]
    AudioSource gunAudioSource;
    [SerializeField]
    AudioClip shootClip;
    [SerializeField]
    AudioClip zatvorClip;
    public float volume = 0.5f;
	// Use this for initialization
	void Start () {
        gunAudioSource.volume = volume;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Shoot()
    {
        gunAudioSource.PlayOneShot(shootClip);
    }
    public void EndOfRecharge()
    {
        gunAudioSource.PlayOneShot(zatvorClip);
    }
}
