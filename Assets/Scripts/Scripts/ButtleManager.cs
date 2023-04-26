using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleManager : MonoBehaviour {

    public List<GameObject> allblue { get; private set; }
    
    public List<GameObject> allred { get; private set; }
    public float blueScore { get; private set; } = 50;
    public float redScore { get; private set; } = 50;
    public int blueScoreDelta { get; private set; } = 0;
    public int redScoreDelta { get; private set; } = 0;
    private float scoreSpeed = 0.01f;
    [SerializeField]
    public List<Vector3> redSpawns;
    [SerializeField]
    public List<Vector3> blueSpawns;

    //Frags
    protected Dictionary<GameObject, int> redFrags;
    protected Dictionary<GameObject, int> blueFrags;
    public GameObject clientTank { get; protected set; } = null;

    public Dictionary<GameObject, ButtleResult> results { get; protected set; }
    protected ButtleStartSettings startSettings;


    //public GameObject humanTank;
    public int redCurrentCount =2;
    public int blueCurrentCount = 2;
    // Use this for initialization
    protected void Awake () {
        MainManager.technicsLibrary.Inicialize();
        allred = new List<GameObject>();
        allblue = new List<GameObject>();

        redFrags = new Dictionary<GameObject, int>();
        blueFrags = new Dictionary<GameObject, int>();

        results = new Dictionary<GameObject, ButtleResult>();

        SpawnTanks();
        Destroy(startSettings.gameObject);
        
        MainManager.userInterfaseManager.InicializeRedComand(allred);
        MainManager.userInterfaseManager.InicializeBlueComand(allblue);
    }
    protected virtual void SpawnTanks()
    {
        
    }
    // Update is called once per frame
    protected void Update () {
        if (MainManager.GameStatus!=GameStatus.Playing)
            return;
        redScore += redScoreDelta * Time.deltaTime * scoreSpeed;
        blueScore += blueScoreDelta * Time.deltaTime * scoreSpeed;
        if (redScore > 100)
            redScore = 100;
        if (redScore < 0)
            redScore = 0;
        if (blueScore > 100)
            blueScore = 100;
        if (blueScore < 0)
            blueScore = 0;

        if (redScore==100 || blueCurrentCount==0)
        {
            MainManager.FinishTheGame(true);
            foreach (GameObject go in allred)
                results[go].Win = true;
            foreach (GameObject go in allblue)
                results[go].Win = false;
        }
        if (blueScore == 100 || redCurrentCount==0)
        {
            MainManager.FinishTheGame(false);
            foreach (GameObject go in allred)
                results[go].Win = false;
            foreach (GameObject go in allblue)
                results[go].Win = true;
        }
    }
    public void AddShotToPlayerResults(GameObject player)
    {
        if (results.ContainsKey(player))
            results[player].AddShoot();
    }

    public void PointIsCaptured(bool red)
    {
        if (red)
            redScoreDelta += 2;
        else
            blueScoreDelta += 2;
    }
    public void PointIsDecaptured(bool byred)
    {
        if (byred)
            blueScoreDelta -= 2;
        else
            redScoreDelta -= 2;
    }
    public void PlayerDied(GameObject player, string killerName)
    {
        Debug.Log("ButtleManager: Died"+ player.name);
        foreach(GameObject go in allred)
        {
            if (go == player)
            {
                redCurrentCount--;
                blueScoreDelta += 1;
                MainManager.userInterfaseManager.RemoveTank(player);
            }
            if (go.name == killerName)
            {
                redFrags[go]++;
                results[go].AddFrag();
                MainManager.userInterfaseManager.UpdateFrag(go, redFrags[go]);
            }
                
        }
        foreach (GameObject go in allblue)
        {
            if (go == player)
            {
                blueCurrentCount--;
                redScoreDelta += 1;
                MainManager.userInterfaseManager.RemoveTank(player);
            }
            if (go.name == killerName)
            {
                blueFrags[go]++;
                results[go].AddFrag();
                MainManager.userInterfaseManager.UpdateFrag(go, blueFrags[go]);
            }
        }
        if (player == clientTank)
            MainManager.userInterfaseManager.ThisPlayerIsDestroied();
        if (killerName == clientTank.name)
            StartCoroutine(MainManager.userInterfaseManager.HumanDestroy());
    }
}
public enum ButtleType
{
    AgainstBots
}
