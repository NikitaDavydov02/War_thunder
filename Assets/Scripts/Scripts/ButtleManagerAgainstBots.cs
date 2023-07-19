using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleManagerAgainstBots : ButtleManager
{
    [SerializeField]
    private CommandAI redAI;
    [SerializeField]
    private CommandAI blueAI;

    protected override void Awake()
    {
        base.Awake();
        /*redAI = new CommandAI();
        blueAI = new CommandAI();
        Debug.Log("Comands AI crated");*/
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
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
                //if(i==1)
                 //   technic = Instantiate(MainManager.technicsLibrary.GetRandomBotsPlane()) as GameObject;
                //else
                    technic = Instantiate(MainManager.technicsLibrary.GetRandomBotsTank()) as GameObject;
                technic.GetComponent<TankAI>().IsRed = true;
            }

            if (redSpawnsForTanks.Count <= i)
                continue;
            switch (technic.GetComponent<Technic>().Type)
            {
                case TechnicsType.Tank:
                    technic.transform.position = redSpawnsForTanks[i];
                    technic.transform.localEulerAngles = redSpawnsForTanksOrientation[i];
                    break;
                case TechnicsType.Plane:
                    technic.transform.position = redSpawnsForPlanes[i];
                    technic.transform.localEulerAngles = redSpawnsForPlanesOrientation[i];
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
            GameObject technic;
            if (i == 0)
                technic = Instantiate(MainManager.technicsLibrary.GetRandomBotsPlane()) as GameObject;
            else
            {
                technic = Instantiate(MainManager.technicsLibrary.GetRandomBotsTank()) as GameObject;
                technic.GetComponent<TankAI>().IsRed = false;
            }
                

            switch (technic.GetComponent<Technic>().Type)
            {
                case TechnicsType.Tank:
                    technic.transform.position = blueSpawnsForTanks[i];
                    technic.transform.localEulerAngles = blueSpawnsForTanksOrientation[i];
                    break;
                case TechnicsType.Plane:
                    technic.transform.position = blueSpawnsForPlanes[i];
                    technic.transform.localEulerAngles = blueSpawnsForPlanesOrientation[i];
                    break;
            }


            technic.name = "playerBlue" + i;
            allblue.Add(technic);
            blueFrags.Add(technic, 0);

            TechnicsOfPlayers.Add(technic.name, technic);
            results.Add(technic.name, new ButtleResult());
            
        }
        foreach (string s in results.Keys)
            Debug.Log(s + "has result" + results[s]);
        Debug.Log("End of buttle against bot mnager output");
        clientTank = allred[0];
    }
}
