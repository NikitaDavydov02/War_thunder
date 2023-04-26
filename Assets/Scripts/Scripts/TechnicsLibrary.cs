using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnicsLibrary : MonoBehaviour {
    //REFACTORED_1
    //[SerializeField]
    //private List<GameObject> stSovetsSecondRang;
    [SerializeField]
    private List<Technic> humanTechnics;
    [SerializeField]
    private List<Technic> botsTechnics;
    private List<Technic> humanTanks;
    private List<Technic> humanPlanes;
    private List<Technic> botsTanks;
    private List<Technic> botsPlanes;
    // Use this for initialization
    void Awake () {
        
    }
    public void Inicialize()
    {
        humanPlanes = new List<Technic>();
        humanTanks = new List<Technic>();

        foreach (Technic technics in humanTechnics)
        {
            switch (technics.Type)
            {
                case TechnicsType.Tank:
                    humanTanks.Add(technics);
                    break;
                case TechnicsType.Plane:
                    humanPlanes.Add(technics);
                    break;
            }
        }
        //bots
        botsPlanes = new List<Technic>();
        botsTanks = new List<Technic>();

        foreach (Technic technics in botsTechnics)
        {
            switch (technics.Type)
            {
                case TechnicsType.Tank:
                    botsTanks.Add(technics);
                    break;
                case TechnicsType.Plane:
                    botsPlanes.Add(technics);
                    break;
            }
        }
        Debug.Log("Technics library inicialized");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject GetRandomHumanTank()
    {
        Debug.Log("Humank tanks: " + humanTanks);
        int index = Random.Range(0, humanTanks.Count - 1);
        Debug.Log("Get tank: " + index);
        return humanTanks[index].gameObject;
     }
    public GameObject GetRandomBotsTank()
    {
        return botsTanks[Random.Range(0, botsTanks.Count-1)].gameObject;
    }
    public GameObject GetHumanTechnicByName(string name)
    {
        foreach (Technic technics in humanTechnics)
            if (technics.Name == name)
                return technics.gameObject;
        return null;
    }
}

public enum TechnicsType
{
    Tank,
    Plane,
}