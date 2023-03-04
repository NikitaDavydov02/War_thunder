using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankLibrary : MonoBehaviour {
    [SerializeField]
    private List<GameObject> stSovetsSecondRang;
    [SerializeField]
    private List<GameObject> humanTanks;
    [SerializeField]
    private List<string> humanTankNames;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject GetRandomTank()
    {
        Debug.Log("Tank LIbrary " + stSovetsSecondRang.Count);
        Debug.Log("TL "+Random.Range(0, stSovetsSecondRang.Count - 1));
        return stSovetsSecondRang[Random.Range(0, stSovetsSecondRang.Count)];
    }
    public GameObject GetTiger()
    {
        return stSovetsSecondRang[2];
    }
    public GameObject GetHumanTankByName(string tankName)
    {
        Debug.Log("Tank library:"+tankName);
        for(int i = 0; i < humanTankNames.Count; i++)
        {
            if (tankName == humanTankNames[i])
                return humanTanks[i];
        }
        Debug.Log("A ya vernul null");
        return null;
    }
}
