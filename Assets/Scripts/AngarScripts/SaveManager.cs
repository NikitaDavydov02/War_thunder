using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour {
    public static int serebro = 0;
    public static int golda = 0;
    public static int exp = 0;
    public static int fragsInButtle=0;
    private static string filename;
    //public static Dictionary<string, List<string>> tanks = new Dictionary<string, List<string>>();
    public static Dictionary<string, GameObject> BotsstSecondRang = new Dictionary<string, GameObject>();
    private static List<string> tankNames = new List<string>() { "Т-34" };
    [SerializeField]
    public GameObject Botst34Prefab;
    // Use this for initialization
    void Start () {
        filename = Path.Combine(Application.persistentDataPath, "game.dat");
        if (BotsstSecondRang.ContainsKey("Т-34"))
        {
            BotsstSecondRang.Add("Т-34", Botst34Prefab);
        }
    }
	
	// Update is called once per frame
	void Update () {
    }

    public static void SaveGame()
    {
        Debug.Log("Saving:");
        filename = Path.Combine(Application.persistentDataPath, "game.dat");
        Debug.Log(filename);
        Dictionary<string, object> gamestate = new Dictionary<string, object>();
        gamestate.Add("serebro", serebro);
        gamestate.Add("golda", golda);
        gamestate.Add("exp", exp);
        Debug.Log(serebro + " " + golda + " " + exp);
        gamestate.Add("BotsstSecondRang", BotsstSecondRang);
        foreach (string s in gamestate.Keys)
        {
            Debug.Log(s + "," + gamestate[s]);
        }
        FileStream stream = File.Create(filename);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, gamestate);
        stream.Close();
    }

    public static void LoadGameState()
    {
        filename = Path.Combine(Application.persistentDataPath, "game.dat");
        //filename = "C:/Users/nikit/AppData/LocalLow/DefaultCompany/War Thunder/game.dat";
        Debug.Log("Ya smotrel: " + filename);
        if (!File.Exists(filename))
        {
            Debug.Log("No save game.");
            return;
        }
        Debug.Log("Loading");
        Dictionary<string, object> gamestate;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(filename, FileMode.Open);
        gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();
        serebro = (int)gamestate["serebro"];
        golda = (int)gamestate["golda"];
        exp = (int)gamestate["exp"];
        BotsstSecondRang = gamestate["BotsstSecondRang"] as Dictionary<string, GameObject>;
    }
    public static void UpdateResources(int newSerebro, int newExp)
    {
        serebro = newSerebro;
        exp = newExp;
    }
}
