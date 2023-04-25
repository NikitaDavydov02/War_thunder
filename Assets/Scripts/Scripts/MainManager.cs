using System.Collections;
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
        ButtleStartSettings startSettings = GameObject.FindGameObjectWithTag("StartSettings").GetComponent<ButtleStartSettings>();
        

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
        GameObject[] guns = GameObject.FindGameObjectsWithTag("Gun");
        GameObject gun = null;
        foreach(GameObject g in guns)
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

        Camera.SetTargetForCamera(tower.transform, gun.transform, controller);
        GameStatus = GameStatus.Playing;
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
}
public enum GameStatus
{
    Loading,
    Playing,
    Finished,
}