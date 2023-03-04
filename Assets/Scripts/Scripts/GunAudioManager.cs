using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAudioManager : MonoBehaviour {
    [SerializeField]
    AudioSource gunAudioSource;
    [SerializeField]
    AudioClip shootClip;
    [SerializeField]
    AudioClip zatvorClip;
	// Use this for initialization
	void Start () {
        gunAudioSource.volume = 0.5f;
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
