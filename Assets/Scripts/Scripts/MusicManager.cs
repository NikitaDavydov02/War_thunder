using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private string winClipName;
    [SerializeField]
    private string failClipName;
    [SerializeField]
    public AudioSource sourcePrefab;
    // Use this for initialization
    void Start () {
        audioSource.volume = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Win()
    {
        PlayMusic(Resources.Load("Music/" + winClipName) as AudioClip);
        Debug.Log("Music/" + winClipName);
    }
    public void Fail()
    {
        PlayMusic(Resources.Load("Music/" + failClipName) as AudioClip);
        Debug.Log("Music/" + winClipName);
    }
    private void PlayMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
