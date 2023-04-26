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

    //public static ButtleManager buttleManager;
    // Use this for initialization
    void Awake()
    {
        buttleStartSettings = GameObject.FindGameObjectWithTag("StartSettings").GetComponent<ButtleStartSettings>();
        GameObject camera = GameObject.FindWithTag("MainCamera");

        
        GameObject buttleMainManager = GameObject.FindGameObjectWithTag("ButtleMainManager");
        if (buttleMainManager != null)
        {
            
            
            ButtleResult result = buttleMainManager.GetComponent<ButtleManager>().results["playerRed0"];
            Debug.Log("Find buttle manager " + buttleMainManager.GetComponent<ButtleManagerAgainstBots>());
            foreach (string s in buttleMainManager.GetComponent<ButtleManager>().results.Keys)
                if (s == "playerRed0")
                    result = buttleMainManager.GetComponent<ButtleManager>().results[s];
            Debug.Log("Find buttle result " + result);
            angarUIManager.DisplayButtleResults(result);
            angarAudioManager.PlayMusicAfterButtle(result.Win);
            saveManager.UpdateResources(result);

            Destroy(buttleMainManager.gameObject);
        }
        
        
        if (camera != null)
            angarCamera = camera.GetComponent<AngarCamera>();
        
        angarAudioManager = GetComponent<AngarAudioManager>();
        angarUIManager = GetComponent<AngarUIManager>();
        technicsManager = GetComponent<TechnicsManager>();
        saveManager = GetComponent<SaveManager>();
        //buttleStartSettings = GetComponent<ButtleStartSettings>();
        saveManager.LoadGameState();
        angarUIManager.DisplayUsersSavings(saveManager.gold, saveManager.silver, saveManager.experiense);
    }

    // Update is called once per frame
    void Start()
    {
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
