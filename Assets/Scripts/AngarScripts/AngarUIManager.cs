using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngarUIManager : MonoBehaviour {
    //REFACTORED

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

    // Use this for initialization
    void Start () {
        buttleResultPanel.SetActive(false);
        Debug.Log("silverLabel" + silverResultLabel);
    }
	public void DisplayButtleResults(ButtleResult result)
    {
        Debug.Log("Show buttle result" + result.silver+" "+ result.expirience + " " + result.frags);
        Debug.Log("silverLabel" + silverResultLabel);
        buttleResultPanel.SetActive(true);
        silverResultLabel.text = result.silver.ToString();
        experienseResultLabel.text = result.expirience.ToString();
        fragsLabel.text = result.frags.ToString();
        //WayLabel.text = "";

    }
    public void DisplayUsersSavings(int gold, int silver, int experience)
    {
        goldLabel.text = gold.ToString();
        silverLabel.text = silver.ToString();
        experienseLabel.text = experience.ToString();
    }
	// Update is called once per frame
	void Update () {
	}

    public void Buttle()
    {
        AngarMainManager.Buttle();
    }
    public void CloseButtleResultPanel()
    {
        buttleResultPanel.SetActive(false);
    }
    public void ChangeTechnic(string newTechnickName)
    {
        AngarMainManager.ChangeTechnic(newTechnickName);
    }
}
