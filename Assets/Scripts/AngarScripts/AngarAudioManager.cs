using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngarAudioManager : MonoBehaviour {
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    List<string> music;
    [SerializeField]
    AudioClip winClip;
    [SerializeField]
    AudioClip failClip;
    // Use this for initialization
    void Start () {
        if (music[0]!=null)
        {
            PlayMusic(Resources.Load("Music/" + music[0]) as AudioClip);
        }
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("BR=0: " + (MainManager.buttleResult == null));
        if (audioSource.clip==null&&MainManager.buttleResult==null)
        {
            int i = Random.RandomRange(0, music.Count - 1);
            PlayMusic(Resources.Load("Music/" + music[i]) as AudioClip);
        }
	}

    private void PlayMusic(AudioClip clip)
    {
        if (audioSource.isPlaying)
            return;
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void PlayWin(bool winb)
    {
        //Debug.Log("Play Win");
        if (winb)
            //PlayMusic(Resources.Load("Music/" + win) as AudioClip);
            PlayMusic(winClip);
        else
            PlayMusic(failClip);
    }
}
