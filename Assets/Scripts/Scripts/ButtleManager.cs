using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleManager : MonoBehaviour {


    public List<GameObject> blue{ get; private set; }
    public List<GameObject> red { get; private set; }
    
    public List<GameObject> allblue { get; private set; }
    
    public List<GameObject> allred { get; private set; }

    private int howManyHumanDestroied = 0;
    public bool gameFinished = false;
    public float blueScore = 50;
    public float redScore = 50;
    public int blueScoreDelta = 0;
    public int redScoreDelta = 0;
    public float scoreSpeed = 0.01f;
    private bool redComandInicialized = false;
    private bool blueComandInicialized = false;
    private bool namePoslani = false;
    public List<string> redNames { get; private set; }
    public List<string> blueNames { get; private set; }
    private bool chekerWasAnabled = false;
    [SerializeField]
    public List<Vector3> redSpawns;
    [SerializeField]
    public List<Vector3> blueSpawns;
    public GameObject humanTank;
    public int redCount = 4;
    public int blueCount = 4;
    // Use this for initialization
    void Start () {
        allred = new List<GameObject>();
        allblue = new List<GameObject>();
        for(int i = 0; i < redCount-1; i++)
        {
            GameObject tank = Instantiate(MainManager.tankLibrary.GetRandomTank()) as GameObject;
            Debug.Log("Random tank: "+tank);
            if (redSpawns.Count <= i)
                continue;
            tank.transform.position = redSpawns[i];
            tank.transform.Rotate(0, 180, 0);
            tank.name = "player" + i;
            allred.Add(tank);
        }
        string humanTankName = ButtleStartSettings.humantankname;
        Debug.Log("Human tank name: "+ButtleStartSettings.humantankname);
        //GameObject humanTankPrefab = MainManager.tankLibrary.GetHumanTankByName("Tiger I");
        //GameObject humanTankPrefab = MainManager.tankLibrary.GetHumanTankByName("Т-34");
        GameObject humanTankPrefab = MainManager.tankLibrary.GetHumanTankByName(humanTankName);
        Debug.Log("Tank lybrary: " + MainManager.tankLibrary);
        Debug.Log("Tank lybary random tank: " + MainManager.tankLibrary.GetHumanTankByName(humanTankName));
        humanTank = Instantiate(humanTankPrefab)as GameObject;
        Debug.Log("humanTank: " + humanTank);
        humanTank.transform.position = new Vector3(1110, 15, 3900);
        humanTank.transform.Rotate(0, 180, 0);
        humanTank.name = "Т-34";
        allred.Add(humanTank);
        Debug.Log("All red: " + allred.Count);


        //allred.Add(humanTank);
        for (int i = 0; i < blueCount; i++)
        {
            GameObject tank;
            tank = Instantiate(MainManager.tankLibrary.GetRandomTank()) as GameObject;
            if (blueSpawns.Count <= i)
                continue;
            tank.transform.position = blueSpawns[i];
            tank.name = "playerr" + i;
            allblue.Add(tank);
        }
        Debug.Log("All blue: " + allblue.Count);

        List<string> redNames = new List<string>();
        List<string> blueNames = new List<string>();
        red = new List<GameObject>();
        blue = new List<GameObject>();
        foreach (GameObject tank in allred)
        {
            redNames.Add(tank.name);
            red.Add(tank);
        }
        foreach (GameObject tank in allblue)
        {
            blueNames.Add(tank.name);
            blue.Add(tank);
        }
        
        MainManager.userInterfaseManager.InicializeRedComand(redNames);
        MainManager.userInterfaseManager.InicializeBlueComand(blueNames);
    }

    // Update is called once per frame
    void Update () {
        if (gameFinished)
            return;
        while (!namePoslani)
        {
            MainManager.userInterfaseManager.InicializeBlueComand(blueNames);
            MainManager.userInterfaseManager.InicializeRedComand(redNames);
            namePoslani = true;
        }
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
        MainManager.userInterfaseManager.UpdateComandScore(redScore, blueScore);

        List<GameObject> mustbeRemovedB = new List<GameObject>();
        List<GameObject> mustbeRemovedR = new List<GameObject>();

        foreach (GameObject b in blue)
        {
            ModuleController mc = b.GetComponent<ModuleController>();
            if (mc.alive == false)
            {
                bool tyblue = false;
                foreach (GameObject bc in blue)
                {
                    if (bc.name == mc.fatalityName)
                        tyblue = true;
                }
                Debug.Log("Killer: " + mc.fatalityName);
                MainManager.userInterfaseManager.RemoveTank(b.name);
                if (!tyblue)
                {
                    MainManager.userInterfaseManager.AddFragTo(mc.fatalityName);
                    if (mc.fatalityName == MainManager.buttleManager.humanTank.name)
                    {
                        MainManager.buttleResult.AddFrag();
                        Debug.Log("Frag v kopilku");
                    }
                }
                else
                    MainManager.userInterfaseManager.AddBlue(mc.fatalityName);
                //blue.Remove(b);
                mustbeRemovedB.Add(b);
                redScoreDelta += 1;
            }
        }
        foreach (GameObject r in red)
        {
            ModuleController mc = r.GetComponent<ModuleController>();
            if (mc.alive == false)
            {
                bool tyblue = false;
                foreach (GameObject rc in red)
                {
                    if (rc.name == mc.fatalityName)
                        tyblue = true;
                }
                MainManager.userInterfaseManager.RemoveTank(r.name);
                if (!tyblue)
                    MainManager.userInterfaseManager.AddFragTo(mc.fatalityName);
                else
                    MainManager.userInterfaseManager.AddBlue(mc.fatalityName);
                //red.Remove(r);
                mustbeRemovedR.Add(r);
                blueScoreDelta += 1;
            }
        }
        foreach(GameObject r in mustbeRemovedR)
        {
            red.Remove(r);
        }
        foreach (GameObject b in mustbeRemovedB)
        {
            blue.Remove(b);
        }

        if (blue.Count == 0 || redScore==100 || blueScore==0)
        {
            MainManager.userInterfaseManager.EndOfButtle("Победа!");
            MainManager.musicManager.Win();
            MainManager.buttleResult.SetWin(true);
            gameFinished = true;
        }
        if (red.Count == 0 || redScore == 0 || blueScore == 100)
        {
            MainManager.userInterfaseManager.EndOfButtle("Поражение!");
            MainManager.buttleResult.SetWin(false);
            MainManager.musicManager.Fail();
            gameFinished = true;
        }
        if (gameFinished)
        {
            foreach(GameObject go in blue)
            {
                ModuleController mc = go.GetComponent<ModuleController>();
                if (mc != null)
                    mc.GameFinished();
            }
            foreach (GameObject go in red)
            {
                ModuleController mc = go.GetComponent<ModuleController>();
                if (mc != null)
                    mc.GameFinished();
            }
        }
    }

    public void Point(bool red)
    {
        if (red)
        {
            redScoreDelta += 2;
            blueScoreDelta -= 2;
        }
        else
        {
            redScoreDelta -= 2;
            blueScoreDelta += 2;
        }
    }
    public List<GameObject> ReturnCopyOfAllRed()
    {
        List<GameObject> output = new List<GameObject>();
        foreach (GameObject tank in allred)
            output.Add(tank);
        return output;
    }
    public List<GameObject> ReturnCopyOfAllBlue()
    {
        List<GameObject> output = new List<GameObject>();
        foreach (GameObject tank in allblue)
            output.Add(tank);
        return output;
    }
}
