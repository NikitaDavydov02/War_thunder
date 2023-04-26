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
    protected override void SpawnTechnics()
    {
        
        string humanTankName = startSettings.playerTechnicName;
        Debug.Log("human technics: " + humanTankName);
        GameObject humanTechnic = MainManager.technicsLibrary.GetHumanTechnicByName(humanTankName);
        d

        for (int i = 1; i < redCurrentCount; i++)
        {
            GameObject tank;
            tank = Instantiate(MainManager.technicsLibrary.GetRandomBotsTank()) as GameObject;
            if (redSpawnsForTanks.Count <= i)
                continue;
            tank.transform.position = redSpawnsForTanks[i];
            tank.transform.Rotate(0, 180, 0);
            tank.name = "playerRed" + i;
            allred.Add(tank);
            redFrags.Add(tank, 0);

            TechnicsOfPlayers.Add(tank.name, tank);
            results.Add(tank.name, new ButtleResult());
        }
        for (int i = 0; i < blueCurrentCount; i++)
        {
            GameObject tank;
            tank = Instantiate(MainManager.technicsLibrary.GetRandomBotsTank()) as GameObject;
            if (blueSpawnsForTanks.Count <= i)
                continue;
            tank.transform.position = blueSpawnsForTanks[i];
            tank.name = "playerBlue" + i;
            allblue.Add(tank);
            blueFrags.Add(tank, 0);

            TechnicsOfPlayers.Add(tank.name, tank);
            results.Add(tank.name, new ButtleResult());
        }
        clientTank = allred[0];
    }
}
