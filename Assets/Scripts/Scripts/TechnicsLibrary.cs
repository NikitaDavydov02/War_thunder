using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnicsLibrary : MonoBehaviour {
    //REFACTORED_1
    //[SerializeField]
    //private List<GameObject> stSovetsSecondRang;
    [SerializeField]
    private List<Technic> humanTechnics;
    private List<Technic> humanTanks;
    private List<Technic> humanPlanes;
	// Use this for initialization
	void Awake () {
        humanPlanes = new List<Technic>();
        humanTanks = new List<Technic>();

		foreach(Technic technics in humanTechnics)
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
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject GetRandomHumanTank()
    {
        return humanTanks[Random.RandomRange(0, humanTanks.Count)].gameObject;
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