using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour {
    public int silver { get; set; } = 0;
    public int gold { get; set; } = 0;
    public int experiense { get; set; } = 0;
    public int fragsInButtle { get; set; } = 0;
    private string filename;
    //public static Dictionary<string, List<string>> tanks = new Dictionary<string, List<string>>();
    public Dictionary<string, GameObject> availableTechnics = new Dictionary<string, GameObject>();
    //private static List<string> tankNames = new List<string>() { "Т-34" };
    //[SerializeField]
    //public GameObject Botst34Prefab;
    // Use this for initialization
    void Start () {
        filename = Path.Combine(Application.persistentDataPath, "game.dat");
        //if (BotsstSecondRang.ContainsKey("Т-34"))
        //{
         //   BotsstSecondRang.Add("Т-34", Botst34Prefab);
        //}/
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void SaveGame()
    {
        Debug.Log("Saving:");
        filename = Path.Combine(Application.persistentDataPath, "game.dat");
        Debug.Log(filename);
        Dictionary<string, object> gamestate = new Dictionary<string, object>();
        gamestate.Add("silver", silver);
        gamestate.Add("gold", gold);
        gamestate.Add("experiense", experiense);
        //Debug.Log(serebro + " " + golda + " " + exp);
        gamestate.Add("availableTechnics", availableTechnics);
        //foreach (string s in gamestate.Keys)
        //{
        //    Debug.Log(s + "," + gamestate[s]);
        //}
        FileStream stream = File.Create(filename);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, gamestate);
        stream.Close();
    }

    public  void LoadGameState()
    {
        filename = Path.Combine(Application.persistentDataPath, "game.dat");
        //filename = "C:/Users/nikit/AppData/LocalLow/DefaultCompany/War Thunder/game.dat";
        Debug.Log("Readin from: " + filename);
        if (!File.Exists(filename))
            return;
        //Debug.Log("Loading");
        Dictionary<string, object> gamestate;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(filename, FileMode.Open);
        gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();
        silver = (int)gamestate["silver"];
        gold = (int)gamestate["gold"];
        experiense = (int)gamestate["experiense"];
        availableTechnics = gamestate["availableTechnics"] as Dictionary<string, GameObject>;
    }
    public void UpdateResources(int additionalSilver, int additionalExperiense)
    {
        silver += additionalSilver;
        experiense += additionalExperiense;
    }
}
