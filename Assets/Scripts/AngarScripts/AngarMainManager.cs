using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AngarMainManager : MonoBehaviour
{
    //REFACTORED
    private static AngarAudioManager angarAudioManager;
    private static AngarUIManager angarUIManager;
    private static SaveManager saveManager;
    private static TechnicsManager technicsManager;
    private static AngarCamera angarCamera;

    private static ButtleStartSettings buttleStartSettings;
    private GameObject buttleMainManager;//This manager is received after buttle

    //public static ButtleManager buttleManager;
    // Use this for initialization
    void Awake()
    {
        buttleStartSettings = GameObject.FindGameObjectWithTag("StartSettings").GetComponent<ButtleStartSettings>();
        GameObject camera = GameObject.FindWithTag("MainCamera");

        
        buttleMainManager = GameObject.FindGameObjectWithTag("ButtleMainManager");
        
        
        if (camera != null)
            angarCamera = camera.GetComponent<AngarCamera>();
        
        angarAudioManager = GetComponent<AngarAudioManager>();
        angarUIManager = GetComponent<AngarUIManager>();
        technicsManager = GetComponent<TechnicsManager>();
        saveManager = GetComponent<SaveManager>();
        //buttleStartSettings = GetComponent<ButtleStartSettings>();
        saveManager.LoadGameState();
        
    }

    // Update is called once per frame
    void Start()
    {
        if (buttleMainManager != null)
        {


            ButtleResult result = buttleMainManager.GetComponent<ButtleManagerAgainstBots>().results["playerRed0"];
            Debug.Log("Find buttle manager " + buttleMainManager.GetComponent<ButtleManagerAgainstBots>());
            Debug.Log("Find buttle result " + result);
            foreach (string s in buttleMainManager.GetComponent<ButtleManagerAgainstBots>().results.Keys)
            {
                Debug.Log(s + "has result " + buttleMainManager.GetComponent<ButtleManagerAgainstBots>().results[s]);
            }
            //}
            //    if (s == "playerRed0")
            //        result = buttleMainManager.GetComponent<ButtleManagerAgainstBots>().results[s];
            Debug.Log("Find buttle result2 " + result);
            angarUIManager.DisplayButtleResults(result);
            angarAudioManager.PlayMusicAfterButtle(result.Win);
            saveManager.UpdateResources(result);

            Destroy(buttleMainManager.gameObject);
        }
        angarUIManager.DisplayUsersSavings(saveManager.gold, saveManager.silver, saveManager.experiense);
        

    }
    public static void Buttle()
    {
        saveManager.SaveGame();
        SceneManager.LoadScene("Конго");
    }
    public static void ChangeTechnic(string nameOfTechnic)
    {
        Debug.Log("Change technic");
        GameObject technic = technicsManager.InstantiateTechnic(nameOfTechnic);
        if (technic == null)
            return;
        buttleStartSettings.playerTechnicName = nameOfTechnic;
        if (angarCamera != null)
        {
            angarCamera.target = technic.transform;
            
        }
           


    }
}
