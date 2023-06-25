using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToweAudioManager : MonoBehaviour {
    //REFACTORED_1
    [SerializeField]
    private AudioSource towerAudioSource;
    [SerializeField]
    private AudioClip rotateClip;
    //private bool rotating = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayRotationSound(bool rotationStarted)
    {
        if (!rotationStarted)
        {
            towerAudioSource.Pause();
            towerAudioSource.clip = null;
        }
        if(rotationStarted&&towerAudioSource.clip==null)
        {
            towerAudioSource.clip = rotateClip;
            towerAudioSource.Play();
        }
    }
}
