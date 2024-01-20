using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleManager : MonoBehaviour {
    System.Random r = new System.Random();

    public List<GameObject> allblue;

    public List<GameObject> allred;

    public float blueScore { get; private set; } = 50;
    public float redScore { get; private set; } = 50;

    public int blueScoreDelta { get; private set; } = 0;
    public int redScoreDelta { get; private set; } = 0;
    private int redPointCaptureScore = 0;
    private int bluePointCaptureScore = 0;
    [SerializeField]
    private float scoreSpeed = 0.05f;


    [SerializeField]
    public List<Vector3> redSpawnsForTanks;
    [SerializeField]
    public List<Vector3> blueSpawnsForTanks;
    [SerializeField]
    public List<Vector3> redSpawnsForTanksOrientation;
    [SerializeField]
    public List<Vector3> blueSpawnsForTanksOrientation;

    [SerializeField]
    public List<Vector3> redSpawnsForPlanes;
    [SerializeField]
    public List<Vector3> blueSpawnsForPlanes;
    [SerializeField]
    public List<Vector3> redSpawnsForPlanesOrientation;
    [SerializeField]
    public List<Vector3> blueSpawnsForPlanesOrientation;

    //Frags
    [SerializeField]
    public Dictionary<GameObject, int> redFrags;
    [SerializeField]
    public Dictionary<GameObject, int> blueFrags;
    public GameObject clientTank { get; protected set; } = null;

    //public Dictionary<string, ButtleResult> results { get; protected set; }
    [SerializeField]
    public Dictionary<string, ButtleResult> results;

    public Dictionary<string, GameObject> TechnicsOfPlayers;

    protected ButtleStartSettings startSettings;


    //public GameObject humanTank;
    public int redCurrentCount =2;
    public int blueCurrentCount = 2;

    //For buttles with points
    [SerializeField]
    private List<Vector3> pointsCoordinates;
    private List<Point> points = new List<Point>();
    [SerializeField]
    private GameObject pointPrefab;


    // Use this for initialization
    protected virtual void Awake () {
        MainManager.technicsLibrary.Inicialize();
        DontDestroyOnLoad(this.gameObject);
        startSettings = GameObject.FindGameObjectWithTag("StartSettings").GetComponent<ButtleStartSettings>();

        if (startSettings.regime == Regime.OnePoint)
        {
            GameObject p = Instantiate(pointPrefab) as GameObject;
            p.transform.position = pointsCoordinates[0];
            points.Add(p.GetComponent<Point>());
        }

        allred = new List<GameObject>();
        allblue = new List<GameObject>();

        redFrags = new Dictionary<GameObject, int>();
        blueFrags = new Dictionary<GameObject, int>();

        //Technics, players and results
        results = new Dictionary<string, ButtleResult>();
        TechnicsOfPlayers = new Dictionary<string, GameObject>();

        SpawnTechnics();
        foreach (string s in results.Keys)
            Debug.Log(s + "has result" + results[s]);
        Debug.Log("End of buttle mnager output");

        Destroy(startSettings.gameObject);
        
        MainManager.userInterfaseManager.InicializeRedComand(allred);
        MainManager.userInterfaseManager.InicializeBlueComand(allblue);
    }
    protected virtual void Start() { }
    protected virtual void SpawnTechnics()
    {
        
    }
    public Transform GetTargetForBlue()
    {
        List<GameObject> aliveRed = new List<GameObject>();
        foreach (GameObject g in allred)
            if (g.GetComponent<ModuleController>().alive)
                    aliveRed.Add(g);
        if (aliveRed.Count == 0)
            return null;
        int index = r.Next(aliveRed.Count);
        return aliveRed[index].transform;
    }
    public Transform GetTargetForRed()
    {
        List<GameObject> aliveBlue = new List<GameObject>();
        foreach (GameObject g in allblue)
            if (g.GetComponent<ModuleController>().alive)
                aliveBlue.Add(g);
        if (aliveBlue.Count == 0)
            return null;
        int index = r.Next(aliveBlue.Count);
        return aliveBlue[index].transform;
    }
    public GameObject GetGroundTagretFotBlue()
    {

        foreach (GameObject g in allred)
            if (g.GetComponent<ModuleController>().alive && g.GetComponent<Technic>().Type != TechnicsType.Plane)
                return g;
        
        return null;
    }
    public Vector3 GetBlueAirport()
    {
        return blueSpawnsForPlanes[0];
    }
    // Update is called once per frame
    protected virtual void Update () {
        if (MainManager.GameStatus!=GameStatus.Playing)
            return;
        foreach(Point p in points)
        {
            redPointCaptureScore = 0;
            bluePointCaptureScore = 0;
            if (p.state == PointState.CapturedByRed)
                redPointCaptureScore += 1;
            if (p.state == PointState.CapturedByBlue)
                bluePointCaptureScore += 1;
        }
        redScore += (redScoreDelta+redPointCaptureScore) * Time.deltaTime * scoreSpeed;
        blueScore += (blueScoreDelta+bluePointCaptureScore) * Time.deltaTime * scoreSpeed;
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
                results[go.name].Win = true;
            foreach (GameObject go in allblue)
                results[go.name].Win = false;
        }
        if (blueScore == 100 || redCurrentCount==0)
        {
            MainManager.FinishTheGame(false);
            foreach (GameObject go in allred)
                results[go.name].Win = false;
            foreach (GameObject go in allblue)
                results[go.name].Win = true;
        }
    }
    public void AddShotToPlayerResults(GameObject player)
    {
        if (results == null)
            return;
        if (results.ContainsKey(player.name))
            results[player.name].AddShoot();
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
        if (!player.GetComponent<ModuleController>().alive)
            return;
        Debug.Log("Buttle manager player died: " + player.name + " killer " + killerName);
        GameObject killerTechnics = null;
        
        if (TechnicsOfPlayers.ContainsKey(killerName))
            killerTechnics = TechnicsOfPlayers[killerName];
        Debug.Log("Killer technics: " + killerTechnics);
        if((allred.Contains(killerTechnics) && allred.Contains(player)))
        {
            redCurrentCount--;
            MainManager.userInterfaseManager.RemoveTank(player);
        }
        else if ((allblue.Contains(killerTechnics) && allblue.Contains(player)))
        {
            blueCurrentCount--;
            MainManager.userInterfaseManager.RemoveTank(player);
        }
        else if (allred.Contains(player))
        {
            foreach (string s in TechnicsOfPlayers.Keys)
                Debug.Log("Technics of " + s + "is " + TechnicsOfPlayers[s]);
            //Victim
            redCurrentCount--;
            blueScoreDelta += 1;
            MainManager.userInterfaseManager.RemoveTank(player);
            //Killer
            blueFrags[killerTechnics]++;
            results[killerName].AddFrag();
            Debug.Log("Frags");
            
            MainManager.userInterfaseManager.UpdateFrag(killerTechnics, blueFrags[killerTechnics]);
        }
        else if (allblue.Contains(player))
        {
            //Victim
            blueCurrentCount--;
            redScoreDelta += 1;
            MainManager.userInterfaseManager.RemoveTank(player);
            //Killer
            redFrags[killerTechnics]++;
            results[killerName].AddFrag();
            MainManager.userInterfaseManager.UpdateFrag(killerTechnics, redFrags[killerTechnics]);
        }
        if (player == clientTank)
            MainManager.userInterfaseManager.ThisPlayerIsDestroied();
        if (killerName == clientTank.name)
            StartCoroutine(MainManager.userInterfaseManager.HumanDestroy());
    }
}
public enum ButtleType
{
    AgainstBots,
}
public enum Regime
{
    WithoutPoints,
    OnePoint,
}
