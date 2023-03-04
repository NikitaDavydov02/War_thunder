using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToweAudioManager : MonoBehaviour {
    [SerializeField]
    public AudioSource towerAudioSource;
    [SerializeField]
    public AudioClip rotateClip;
    private bool rotating = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeRot(bool rot)
    {
        if (rotating != rot)
        {
            rotating = rot;
            if (rotating == true)
            {
                towerAudioSource.clip = rotateClip;
                towerAudioSource.Play();
            }
            else
            {
                towerAudioSource.Stop();
            }
        }
    }
}
