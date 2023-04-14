using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AngarMainManager : MonoBehaviour
{
    private static AngarAudioManager angarAudioManager;
    private static AngarUIManager angarUIManager;
    private static ButtleResult buttleResult;
    public static SaveManager saveManager { get; private set; }
    private static TechnicsManager technicsManager;
    private static AngarCamera angarCamera;
    //public static ButtleManager buttleManager;
    // Use this for initialization
    void Start()
    {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        if (camera != null)
            angarCamera = camera.GetComponent<AngarCamera>();
        angarAudioManager = GetComponent<AngarAudioManager>();
        angarUIManager = GetComponent<AngarUIManager>();
        buttleResult = GetComponent<ButtleResult>();
        technicsManager = GetComponent<TechnicsManager>();
        saveManager = GetComponent<SaveManager>();
        saveManager.LoadGameState();
        angarUIManager.DisplayUsersSavings(saveManager.gold, saveManager.silver, saveManager.experiense);
    }

    // Update is called once per frame
    void Update()
    {
        if (buttleResult != null)
        {
            //Coming back from buttle
            angarAudioManager.PlayMusicAfterButtle(buttleResult.Win);
            angarUIManager.DisplayButtleResults(buttleResult.silver, buttleResult.expirience, buttleResult.frags, buttleResult.Way);
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
        GameObject technic = technicsManager.InstantiateTechnic(nameOfTechnic);
        if (technic == null)
            return;
        ButtleStartSettings.playerTechnicName = nameOfTechnic;
        if (angarCamera != null)
            angarCamera.target = technic.transform;


    }
}
