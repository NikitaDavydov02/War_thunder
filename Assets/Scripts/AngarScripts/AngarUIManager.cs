using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngarUIManager : MonoBehaviour {

    private int serebro;
    private int golda;
    private int exp;
    public static int frags;
    [SerializeField]
    private Text serebroLabel;
    [SerializeField]
    private Text goldaLabel;
    [SerializeField]
    private Text expLabel;
    [SerializeField]
    private Text fragsLabel;

    [SerializeField]
    private Text serebroResultLabel;
    [SerializeField]
    private Text expResultLabel;
    [SerializeField]
    private Text ShootsLabel;
    [SerializeField]
    private Text PutLabel;
    [SerializeField]
    private GameObject buttleResultPanel;
    [SerializeField]
    private List<Button> tanksButtons;

    private AngarAudioManager audioManager;
    private AngarTankManager angarTankManager;

    // Use this for initialization
    void Start () {
        audioManager = this.gameObject.GetComponent<AngarAudioManager>();
        angarTankManager = this.gameObject.GetComponent<AngarTankManager>();
        SaveManager.LoadGameState();
        if (MainManager.buttleResult == null)
        {
            buttleResultPanel.gameObject.SetActive(false);
            serebro = SaveManager.serebro;
            exp = SaveManager.exp;
        }
        else
        {
            audioManager.PlayWin(MainManager.buttleResult.win);
            buttleResultPanel.gameObject.SetActive(true);
            serebroResultLabel.text = MainManager.buttleResult.serebro.ToString();
            expResultLabel.text = MainManager.buttleResult.expirience.ToString();
            frags = MainManager.buttleResult.GetFrags();
            ShootsLabel.text = MainManager.buttleResult.shoots.ToString();
            PutLabel.text = MainManager.buttleResult.puti.ToString();

            serebro = SaveManager.serebro + MainManager.buttleResult.serebro;
            exp = SaveManager.exp + MainManager.buttleResult.expirience;
        }
        golda = SaveManager.golda;
        SaveManager.UpdateResources(serebro, exp);
        serebroLabel.text = serebro.ToString();
        goldaLabel.text = golda.ToString();
        expLabel.text = exp.ToString();
        fragsLabel.text = frags.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        //fragsLabel.text=frags.ToString();
	}

    public void VBoy()
    {
        SaveManager.SaveGame();
        Application.LoadLevel("Конго");
    }
    void OnDestroy()
    {
        SaveManager.SaveGame();
    }
    public void CloseButtleResultPanel()
    {
        buttleResultPanel.gameObject.SetActive(false);
    }
    public void ChangeTank(string newTankName)
    {
        ButtleStartSettings.humantankname = newTankName;
        angarTankManager.InstantiateTank(newTankName);
        Debug.Log("New tank: "+ ButtleStartSettings.humantankname);
    }
}
