using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AngarMainManager : MonoBehaviour
{
    //REFACTORED
    private static AngarAudioManager angarAudioManager;
    private static AngarUIManager angarUIManager;
    private static ButtleResult buttleResult;
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

        ///Place where you should know tank of player
        ButtleResult results = buttleMainManager.GetComponent<ButtleManager>().results[0];
        
        if (camera != null)
            angarCamera = camera.GetComponent<AngarCamera>();
        
        angarAudioManager = GetComponent<AngarAudioManager>();
        angarUIManager = GetComponent<AngarUIManager>();
        buttleResult = GetComponent<ButtleResult>();
        technicsManager = GetComponent<TechnicsManager>();
        saveManager = GetComponent<SaveManager>();
        //buttleStartSettings = GetComponent<ButtleStartSettings>();
        saveManager.LoadGameState();
        angarUIManager.DisplayUsersSavings(saveManager.gold, saveManager.silver, saveManager.experiense);
    }

    // Update is called once per frame
    void Start()
    {
        if (buttleResult != null)
        {
            //Coming back from buttle
            angarAudioManager.PlayMusicAfterButtle(buttleResult.Win);
            angarUIManager.DisplayButtleResults(buttleResult.silver, buttleResult.expirience, buttleResult.frags);
            saveManager.UpdateResources(buttleResult.silver, buttleResult.expirience);
        }
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
