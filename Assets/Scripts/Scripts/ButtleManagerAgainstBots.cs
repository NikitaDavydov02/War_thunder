using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleManagerAgainstBots : ButtleManager
{

    private void Awake()
    {
        base.Awake();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    protected override void SpawnTanks()
    {
        startSettings = GameObject.FindGameObjectWithTag("StartSettings").GetComponent<ButtleStartSettings>();
        string humanTankName = startSettings.playerTechnicName;
        Debug.Log("human technics: " + humanTankName);

        for (int i = 0; i < redCurrentCount; i++)
        {
            GameObject tank;
            if (i==0)
                tank = Instantiate(MainManager.technicsLibrary.GetHumanTechnicByName(humanTankName)) as GameObject;
            else
                tank = Instantiate(MainManager.technicsLibrary.GetRandomBotsTank()) as GameObject;
            if (redSpawns.Count <= i)
                continue;
            tank.transform.position = redSpawns[i];
            tank.transform.Rotate(0, 180, 0);
            tank.name = "playerRed" + i;
            allred.Add(tank);
            redFrags.Add(tank, 0);
            results.Add(tank, new ButtleResult());
        }
        for (int i = 0; i < blueCurrentCount; i++)
        {
            GameObject tank;
            tank = Instantiate(MainManager.technicsLibrary.GetRandomBotsTank()) as GameObject;
            if (blueSpawns.Count <= i)
                continue;
            tank.transform.position = blueSpawns[i];
            tank.name = "playerBlue" + i;
            allblue.Add(tank);
            blueFrags.Add(tank, 0);
            results.Add(tank, new ButtleResult());
        }
        clientTank = allred[0];
    }
}
