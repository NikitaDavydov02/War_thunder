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
        

        for (int i = 0; i < redCurrentCount; i++)
        {
            GameObject technic;
            if (i == 0)
            {
                technic = Instantiate( MainManager.technicsLibrary.GetHumanTechnicByName(humanTankName))as GameObject;
                
            }
            else
            {
                technic = Instantiate(MainManager.technicsLibrary.GetRandomBotsTank()) as GameObject;
            }
            if (redSpawnsForTanks.Count <= i)
                continue;
            switch (technic.GetComponent<Technic>().Type)
            {
                case TechnicsType.Tank:
                    technic.transform.position = redSpawnsForTanks[i];
                    break;
                case TechnicsType.Plane:
                    technic.transform.position = redSpawnsForPlanes[i];
                    break;
            }
            technic.transform.Rotate(0, 180, 0);
            technic.name = "playerRed" + i;
            allred.Add(technic);
            redFrags.Add(technic, 0);

            TechnicsOfPlayers.Add(technic.name, technic);
            results.Add(technic.name, new ButtleResult());
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
