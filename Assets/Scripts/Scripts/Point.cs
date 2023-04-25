﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
    //REFACTORED_1
    //public bool pointIsRed = true;
    public List<GameObject> blue;
    public List<GameObject> red;
    public float scoreInFavourOfRed = 0;
    public float speed = 1f;

    [SerializeField]
    private AudioSource pointAudioSource;
    [SerializeField]
    private AudioClip zahvatClip;
    private bool audioIsPlaying=false;


    [SerializeField]
    Material normalMaterial;
    [SerializeField]
    Material blueMaterial;
    [SerializeField]
    Material redMaterial;
    private PointState state = PointState.Free;
    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material = normalMaterial;

    }
	
    private void ChangePointState(PointState state)
    {
        if (GetComponent<Renderer>().material == redMaterial)
            return;
        if (state == PointState.CapturedByRed)
        {
            GetComponent<Renderer>().material = redMaterial;
            MainManager.buttleManager.PointIsCaptured(true);
        }
        if (state == PointState.CapturedByBlue)
        {
            GetComponent<Renderer>().material = blueMaterial;
            MainManager.buttleManager.PointIsCaptured(false);
        }
        pointAudioSource.Stop();
        this.state = state;
    }
	// Update is called once per frame
	void Update () {
        if (MainManager.GameStatus!=GameStatus.Playing)
            return;
        float delta = red.Count - blue.Count;
        scoreInFavourOfRed += Time.deltaTime * speed * delta;
        if (scoreInFavourOfRed > 100)
        {
            scoreInFavourOfRed = 100;
            if (state != PointState.CapturedByRed)
                ChangePointState(PointState.CapturedByRed);
        }
        if (scoreInFavourOfRed < -100)
        {
            scoreInFavourOfRed = -100;
            if (state != PointState.CapturedByBlue)
                ChangePointState(PointState.CapturedByBlue);
        }

        if (delta != 0)
        {
            if (!audioIsPlaying)
            {
                pointAudioSource.clip = zahvatClip;
                pointAudioSource.Play();
                audioIsPlaying = true;
            }
        }
        else
        {
            pointAudioSource.Stop();
            audioIsPlaying = false;
        }

    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Point entered" + other.gameObject.name);
    }

}
public enum PointState {
 Free,
 CapturedByRed,
 CapturedByBlue,
}

