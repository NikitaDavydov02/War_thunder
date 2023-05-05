﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {
    //REFACTORED_1
    public static UIManager userInterfaseManager;
    public static MusicManager musicManager;
    public static ButtleManager buttleManager;
    public static TechnicsLibrary technicsLibrary;
    public static SceneCamera Camera;
    public static GameStatus GameStatus { get; private set; } = GameStatus.Loading;

    void Awake () {
        
        

        technicsLibrary = GetComponent<TechnicsLibrary>();
        userInterfaseManager = GetComponent<UIManager>();
        musicManager = GetComponent<MusicManager>();
        //buttleResult = GetComponent<ButtleResult>();
        buttleManager = GetComponent<ButtleManager>();
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SceneCamera>();
        //roadManager = GetComponent<RoadManager>();
        
	}
    private void Start()
    {
        //Find tank to camera
        GameObject tank = buttleManager.clientTank;
        ModuleController controller = tank.GetComponent<ModuleController>();
        Technic technic = tank.GetComponent<Technic>();
        if (technic.Type == TechnicsType.Tank)
        {
            //Setting camera tergets
            GameObject[] guns = GameObject.FindGameObjectsWithTag("Gun");
            GameObject gun = null;
            foreach (GameObject g in guns)
                if (g.transform.IsChildOf(tank.transform))
                {
                    gun = g;
                    break;
                }
            GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
            GameObject tower = null;
            foreach (GameObject g in towers)
                if (g.transform.IsChildOf(tank.transform))
                {
                    tower = g;
                    break;
                }
            Camera.SetTargetForCamera(TechnicsType.Tank, tower.transform, gun.transform, controller);
        }
        if (technic.Type == TechnicsType.Plane)
            Camera.SetTargetForCamera(TechnicsType.Plane, tank.transform, null, controller);
        GameStatus = GameStatus.Playing;
    }
    public static void GoToAngar()
    {
        Destroy(userInterfaseManager);
        Destroy(musicManager);
        Destroy(technicsLibrary);
        Application.LoadLevel("Angar");
    }
    // Update is called once per frame
    void Update () {
		
	}
    public static void FinishTheGame(bool redWin)
    {
        GameStatus = GameStatus.Finished;
        if (redWin)
        {
            MainManager.userInterfaseManager.EndOfButtle("Победа!");
            MainManager.musicManager.Win();
        }
        else
        {
            MainManager.userInterfaseManager.EndOfButtle("Поражение!");
            MainManager.musicManager.Fail();
        }
    }
    public static void PlayerFired(GameObject player, GameObject curb)
    {
        buttleManager.AddShotToPlayerResults(player);
        if(player==buttleManager.clientTank)
            Camera.AddFiredCurb(curb.transform);
    }
}
public enum GameStatus
{
    Loading,
    Playing,
    Finished,
}