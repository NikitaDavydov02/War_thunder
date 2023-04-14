using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngarUIManager : MonoBehaviour {

    //private int serebro;
    //private int golda;
    //private int exp;
    //public static int frags;
    [SerializeField]
    private Text silverLabel;
    [SerializeField]
    private Text goldLabel;
    [SerializeField]
    private Text experienseLabel;
    [SerializeField]
    private Text fragsLabel;

    [SerializeField]
    private Text silverResultLabel;
    [SerializeField]
    private Text experienseResultLabel;
    [SerializeField]
    private Text ShootsLabel;
    [SerializeField]
    private Text WayLabel;
    [SerializeField]
    private GameObject buttleResultPanel;
    [SerializeField]
    private List<Button> tanksButtons;

    //private AngarAudioManager audioManager;
    //private AngarTankManager angarTankManager;

    // Use this for initialization
    void Start () {
        //audioManager = this.gameObject.GetComponent<AngarAudioManager>();
        //angarTankManager = this.gameObject.GetComponent<AngarTankManager>();
       // SaveManager.LoadGameState();
        //if (MainManager.buttleResult == null)
        //{
        //    buttleResultPanel.gameObject.SetActive(false);
        //    //silverLabel.text = SaveManager.serebro;
        //    //exp = SaveManager.exp;
        //}
        //else
        //{
        //    audioManager.PlayWin(MainManager.buttleResult.win);
        //    buttleResultPanel.gameObject.SetActive(true);
        //    serebroResultLabel.text = MainManager.buttleResult.serebro.ToString();
        //    expResultLabel.text = MainManager.buttleResult.expirience.ToString();
        //    frags = MainManager.buttleResult.GetFrags();
        //    ShootsLabel.text = MainManager.buttleResult.shoots.ToString();
        //    PutLabel.text = MainManager.buttleResult.puti.ToString();

        //    serebro = SaveManager.serebro + MainManager.buttleResult.serebro;
        //    exp = SaveManager.exp + MainManager.buttleResult.expirience;
        //}
        //golda = SaveManager.golda;
        //SaveManager.UpdateResources(serebro, exp);
        //serebroLabel.text = serebro.ToString();
        //goldaLabel.text = golda.ToString();
        //expLabel.text = exp.ToString();
        //fragsLabel.text = frags.ToString();
    }
	public void DisplayButtleResults(int silver, int experience, int frags, float way)
    {
        buttleResultPanel.gameObject.SetActive(true);
        silverResultLabel.text = silver.ToString();
        experienseResultLabel.text = experience.ToString();
        fragsLabel.text = frags.ToString();
        WayLabel.text = way.ToString();

    }
    public void DisplayUsersSavings(int gold, int silver, int experience)
    {
        goldLabel.text = gold.ToString();
        silverLabel.text = silver.ToString();
        experienseLabel.text = experience.ToString();
    }
	// Update is called once per frame
	void Update () {
        //silverLabel.text=AngarMainManager.saveManager.
        //fragsLabel.text=frags.ToString();
	}

    public void Buttle()
    {
        AngarMainManager.Buttle();
        //SaveManager.SaveGame();
        //Application.LoadLevel("Конго");
    }
    void OnDestroy()
    {
        
        //SaveManager.SaveGame();
    }
    public void CloseButtleResultPanel()
    {
        buttleResultPanel.gameObject.SetActive(false);
    }
    public void ChangeTechnic(string newTechnickName)
    {
        AngarMainManager.ChangeTechnic(newTechnickName);
    }
}
