using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpuseAudioManager : MonoBehaviour {
    public AudioSource engineAudioSource;
    [SerializeField]
    public AudioClip stopClip;
    [SerializeField]
    public AudioClip gazClip;
    private bool gaz = false;
    private bool trekZapushen = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!gaz)
        {
            if ((!trekZapushen && !engineAudioSource.isPlaying) || engineAudioSource.clip == gazClip)
            {
                engineAudioSource.clip = stopClip;
                engineAudioSource.Play();
                trekZapushen = true;
            }
            else
            {
                trekZapushen = false;
            }
        }
        else
        {
            if ((!trekZapushen && !engineAudioSource.isPlaying) || engineAudioSource.clip == stopClip)
            {
                engineAudioSource.clip = gazClip;
                engineAudioSource.Play();
                trekZapushen = true;
            }
            else
            {
                trekZapushen = false;
            }
        }
    }

    public void ChangeGaz(bool newGaz)
    {
        gaz = newGaz;
    }
}
