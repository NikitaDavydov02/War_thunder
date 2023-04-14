using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngarAudioManager : MonoBehaviour {
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private List<string> music;
    [SerializeField]
    private AudioClip winClip;
    [SerializeField]
    private AudioClip defeatClip;
    // Use this for initialization
    void Start () {
        if (music!=null && music.Count>0)
        {
            PlayMusic(Resources.Load("Music/" + music[0]) as AudioClip);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (audioSource.clip==null)
        {
            int i = Random.Range(0, music.Count);
            PlayMusic(Resources.Load("Music/" + music[i]) as AudioClip);
        }
    }
    public void PlayMusicAfterButtle(bool win)
    {
        if (win)
            PlayMusic(winClip);
        else
            PlayMusic(defeatClip);
    }
    private void PlayMusic(AudioClip clip)
    {
        //if (audioSource.isPlaying)
        //    return;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
