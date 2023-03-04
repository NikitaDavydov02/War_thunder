using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private List<GameObject> pricel;
    [SerializeField]
    private Text centralText;
    [SerializeField]
    private Text resultText;
    [SerializeField]
    private Button angar;
    [SerializeField]
    private Text humanDestroy;
    private static  bool pricelIsActive = false;
    [SerializeField]
    private Text moduleDameged;
    [SerializeField]
    private Text moduleCrit;
    [SerializeField]
    private Text moduleDestroied;
    [SerializeField]
    private List<Text> redComandText;
    [SerializeField]
    private List<Text> blueComandText;
    [SerializeField]
    private List<Button> redComandButton;
    [SerializeField]
    private List<Button> blueComandButton;
    [SerializeField]
    AudioSource UIAudioSource;
    [SerializeField]
    AudioClip curbButtonClip;
    [SerializeField]
    Slider redScoreComand;
    [SerializeField]
    Slider blueScoreComand;
    [SerializeField]
    private List<Text> redComandFragsText;
    [SerializeField]
    private List<Text> blueComandFragsText;
    public List<int> redFrags;
    public List<int> blueFrags;
    public int currentCurbIndex = 0;
    [SerializeField]
    public Button tankButton;
    [SerializeField]
    private Text distanceText;
    [SerializeField]
    private Text RepairTimeText;
    public float distance;
    // Use this for initialization
    void Start () {
        foreach (GameObject o in pricel)
            o.gameObject.SetActive(false);
        centralText.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
        angar.gameObject.SetActive(false);
        humanDestroy.gameObject.SetActive(false);
        RepairTimeText.gameObject.SetActive(false);
        for (int i = 0; i < redComandText.Count; i++)
        {
            redFrags.Add(0);
            redComandFragsText[i].text = "0";
        }
        for (int i = 0; i < blueComandText.Count; i++)
        {
            blueFrags.Add(0);
            blueComandFragsText[i].text = "0";
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (pricelIsActive)
            distanceText.text = distance.ToString();
        else
            distanceText.text = "";
	}

    void Awake()
    {
        Messenger.AddListener(GameEvent.PRICEL, Pricel);
        Messenger.AddListener(GameEvent.HUMANTANKDESTROIED, HumanDestroied);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PRICEL, Pricel);
        Messenger.RemoveListener(GameEvent.HUMANTANKDESTROIED, HumanDestroied);
    }
    private void HumanDestroied()
    {
        StartCoroutine(SomeoneDestroied("Танк уничтожен!"));
    }
    private IEnumerator SomeoneDestroied(string messenge)
    {
        centralText.text = messenge;
        centralText.gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        centralText.gameObject.SetActive(false);
    }
    private void Pricel()
    {
        if (pricelIsActive)
        {
            foreach (GameObject o in pricel)
                o.gameObject.SetActive(false);
        }
        else
        {
            foreach (GameObject o in pricel)
                o.gameObject.SetActive(true);
        }
        pricelIsActive = !pricelIsActive;
    }
    public void EndOfButtle(string result)
    {
        resultText.text = result;
        resultText.gameObject.SetActive(true);
        angar.gameObject.SetActive(true);
    }
    public void Angar()
    {
        Application.LoadLevel("Angar");
    }
    public IEnumerator HumanDestroy()
    {
        humanDestroy.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        humanDestroy.gameObject.SetActive(false);
    }
    public IEnumerator ModuleDamaged(string name)
    {
        moduleDameged.text += ("\n" + name);
        yield return new WaitForSeconds(3f);
        moduleDameged.text = "";
    }
    public IEnumerator ModuleCrit(string name)
    {
        moduleCrit.text += ("\n" + name);
        yield return new WaitForSeconds(3f);
        moduleCrit.text = "";
    }
    public IEnumerator ModuleDestroied(string name)
    {
        moduleDestroied.text += ("\n" + name);
        yield return new WaitForSeconds(3f);
        moduleDestroied.text = "";
    }

    //public void InicializeComands(List<string>red, List<string> blue)
    //{
    //    for(int i = 0; i < redComandText.Count; i++)
    //    {
    //        if (i < red.Count)
    //            redComandText[i].text = red[i];
    //        else
    //        {
    //            redComandButton[i].image.color= new Color(128, 128, 128, 255);
    //        }
    //    }
    //    for (int i = 0; i < blueComandText.Count; i++)
    //    {
    //        if (i < blue.Count)
    //            blueComandText[i].text = blue[i];
    //        else
    //        {
    //            blueComandButton[i].GetComponent<Image>().color= new Color(128, 128, 128, 255);
    //            //blueComandImage[i].color = new Color(128, 128, 128, 255);
    //        }
    //    }
    //}
    public void InicializeRedComand(List<string> red)
    {
        if (red == null)
            return;
        //Button b = Canvas.Instantiate(tankButton) as Button;
        //b.GetComponent<RectTransform>().position = new Vector3(10, 10, 10);
        for (int i = 0; i < redComandText.Count; i++)
        {
            if (i < red.Count)
                redComandText[i].text = red[i];
            else
            {
                redComandButton[i].image.color = new Color(128, 128, 128, 255);
            }
        }
    }
    public void InicializeBlueComand(List<string> blue)
    {
        if (blue == null)
            return;
        for (int i = 0; i < blueComandText.Count; i++)
        {
            if (i < blue.Count)
                blueComandText[i].text = blue[i];
            else
            {
                blueComandButton[i].GetComponent<Image>().color = new Color(128, 128, 128, 255);
                //blueComandImage[i].color = new Color(128, 128, 128, 255);
            }
        }
    }

    public void RemoveTank(string name)
    {
        for (int i = 0; i < redComandText.Count; i++)
        {
            if (redComandText[i].text == name)
            {
                //redComandText[i].color = new Color(300, 300, 300, 255);
                redComandButton[i].image.color = new Color(255,0,0);
                redComandText[i].text = "";
            }
        }
        for(int i = 0; i < blueComandText.Count; i++)
        {
            if (blueComandText[i].text == name)
            {
                //blueComandText[i].color = new Color(300, 300, 300, 255);
                blueComandButton[i].image.color = new Color(62,0,255);
                blueComandText[i].text = "";
            }
        }
    }

    public void CurbButton(int index)
    {
        currentCurbIndex = index;
        UIAudioSource.PlayOneShot(curbButtonClip);
    }
    public void UpdateComandScore(float redScore, float blueScore)
    {
        redScoreComand.value = (redScore / 100) * redScoreComand.maxValue;
        blueScoreComand.value = (blueScore / 100) * blueScoreComand.maxValue;
    }
    public void AddFragTo(string fragMaker)
    {
        Debug.Log("FragMaker: " + fragMaker);
        for(int i = 0; i < redComandText.Count; i++)
        {
            if (redComandText[i].text == fragMaker)
            {
                redFrags[i]++;
                redComandFragsText[i].text = redFrags[i].ToString();
            }
        }
        for (int i = 0; i < blueComandText.Count; i++)
        {
            if (blueComandText[i].text == fragMaker)
            {
                blueFrags[i]++;
                blueComandFragsText[i].text = blueFrags[i].ToString();
            }
        }
    }
    public void AddBlue(string blueTank)
    {
        for (int i = 0; i < redComandText.Count; i++)
        {
            if (redComandText[i].text == blueTank)
                redComandText[i].color = new Color(0, 100, 100);
        }
        for (int i = 0; i < blueComandText.Count; i++)
        {
            if (blueComandText[i].text == blueTank)
                blueComandText[i].color = new Color(0, 100, 100);
        }
    }
    public void UpdateDistance(float distance)
    {
        this.distance = distance;
    }
    public void UpdateRepairTime(float time)
    {
        if (time == 0)
            RepairTimeText.gameObject.SetActive(false);
        else
        {
            RepairTimeText.gameObject.SetActive(true);
            RepairTimeText.text = "Время ремонта "+ Mathf.Round(time).ToString();
        }
    }
}
