using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    //UI Elements
    [SerializeField]
    private Canvas canvas;
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
    
    [SerializeField]
    private Text moduleDameged;
    [SerializeField]
    private Text moduleCrit;
    [SerializeField]
    private Text moduleDestroied;
    [SerializeField]
    private Dictionary<GameObject, Text> redComandText;
    [SerializeField]
    private Dictionary<GameObject, Text> blueComandText;
    [SerializeField]
    private Dictionary<GameObject,Button> redComandButtons;
    [SerializeField]
    private Dictionary<GameObject,Button> blueComandButtons;
    [SerializeField]
    AudioSource UIAudioSource;
    [SerializeField]
    AudioClip curbButtonClip;
    [SerializeField]
    Slider redScoreComand;
    [SerializeField]
    Slider blueScoreComand;
    [SerializeField]
    private Dictionary<GameObject, Text> redComandFragText;
    [SerializeField]
    private Dictionary<GameObject, Text> blueComandFragText;
    [SerializeField]
    public Button tankButton;
    [SerializeField]
    private Text distanceText;
    [SerializeField]
    private Text RepairTimeText;

    //Prefabs
    [SerializeField]
    private Button redComandButtonPrefab;
    [SerializeField]
    private Button blueComandButtonPrefab;
    //public List<int> redFrags;
    //public List<int> blueFrags;
    public int currentCurbIndex = 0;
    
    //public float distance;
    // Use this for initialization
    void Start () {
        redComandText = new Dictionary<GameObject, Text>();
        blueComandText = new Dictionary<GameObject, Text>();

        foreach (GameObject o in pricel)
            o.gameObject.SetActive(false);
        centralText.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
        angar.gameObject.SetActive(false);
        humanDestroy.gameObject.SetActive(false);
        RepairTimeText.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        redScoreComand.value = (MainManager.buttleManager.redScore / 100) * redScoreComand.maxValue;
        blueScoreComand.value = (MainManager.buttleManager.blueScore / 100) * blueScoreComand.maxValue;
    }
    public void ThisPlayerIsDestroied()
    {
        StartCoroutine(SomeoneDestroied("Танк уничтожен!"));
        MainManager.Camera.ZoomOut();
        

    }
    private IEnumerator SomeoneDestroied(string messenge)
    {
        centralText.text = messenge;
        centralText.gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        centralText.gameObject.SetActive(false);
    }
    public void Pricel(bool activate)
    {
        foreach (GameObject o in pricel)
            o.gameObject.SetActive(activate);
    }
    public void EndOfButtle(string result)
    {
        resultText.text = result;
        resultText.gameObject.SetActive(true);
        angar.gameObject.SetActive(true);
    }
    public void Angar()
    {
        MainManager.GoToAngar();
    }
    //Modules or thecnics that human destriyed
    public IEnumerator HumanDestroy()
    {
        humanDestroy.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        humanDestroy.gameObject.SetActive(false);
    }
    public void ModuleDamaged(string name)
    {
        StartCoroutine(ModuleDamagedCoroutine(name));
    }
    private IEnumerator ModuleDamagedCoroutine(string name)
    {
        moduleDameged.text += ("\n" + name);
        yield return new WaitForSeconds(3f);
        moduleDameged.text = "";
    }
    public void ModuleCrit(string name)
    {
        StartCoroutine(ModuleCritCoroutine(name));
    }
    private IEnumerator ModuleCritCoroutine(string name)
    {
        moduleCrit.text += ("\n" + name);
        yield return new WaitForSeconds(3f);
        moduleCrit.text = "";
    }
    public void ModuleDestroied(string name)
    {
        StartCoroutine(ModuleDestroiedCoroutine(name));
    }
    private IEnumerator ModuleDestroiedCoroutine(string name)
    {
        moduleDestroied.text += ("\n" + name);
        yield return new WaitForSeconds(3f);
        moduleDestroied.text = "";
    }


    public void InicializeRedComand(List<GameObject> red)
    {
        if (red == null)
            return;
        redComandText = new Dictionary<GameObject, Text>();
        redComandButtons = new Dictionary<GameObject, Button>();
        redComandFragText = new Dictionary<GameObject, Text>();
        for (int i = 0; i < red.Count; i++)
        {
            Button button = Instantiate(CanvasScaler.Instantiate(redComandButtonPrefab)) as Button;
            button.GetComponent<RectTransform>().SetParent(canvas.transform);
            //button.transform.position = new Vector3(100, 100 * i, 0);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(80, -15-25 * i);
            redComandButtons.Add(red[i], button);

            Text playerName = button.transform.GetChild(0).GetComponent<Text>();
            playerName.text = red[i].name;
            redComandText.Add(red[i], playerName);

            Text playerFrags = button.transform.GetChild(1).GetComponent<Text>();
            playerFrags.text = "0";
            redComandFragText.Add(red[i], playerFrags);
            //redComandButtons[i].image.color = new Color(128, 128, 128, 255);
            //button.GetComponent<RectTransform>().localScale = playerHPSlider.GetComponent<RectTransform>().localScale;
        }
    }
    public void InicializeBlueComand(List<GameObject> blue)
    {
        if (blue == null)
            return;
        blueComandText = new Dictionary<GameObject, Text>();
        blueComandButtons = new Dictionary<GameObject, Button>();
        blueComandFragText = new Dictionary<GameObject, Text>();
        for (int i = 0; i < blue.Count; i++)
        {
            Button button = Instantiate(CanvasScaler.Instantiate(blueComandButtonPrefab)) as Button;
            button.GetComponent<RectTransform>().SetParent(canvas.transform);
            //button.transform.position = new Vector3(100, 100 * i, 0);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-80, -15 - 25 * i);
            blueComandButtons.Add(blue[i], button);

            Text playerName = button.transform.GetChild(0).GetComponent<Text>();
            playerName.text = blue[i].name;
            blueComandText.Add(blue[i], playerName);

            Text playerFrags = button.transform.GetChild(1).GetComponent<Text>();
            playerFrags.text = "0";
            blueComandFragText.Add(blue[i], playerFrags);
            //redComandButtons[i].image.color = new Color(128, 128, 128, 255);
            //button.GetComponent<RectTransform>().localScale = playerHPSlider.GetComponent<RectTransform>().localScale;
        }
    }
    public void RemoveTank(GameObject player)
    {
        Debug.Log("Blue buttons: "+blueComandButtons.Count);
        if (redComandButtons.ContainsKey(player))
            redComandButtons[player].GetComponent<Image>().color = new Color(0, 0, 1, 1);
        if (blueComandButtons.ContainsKey(player))
        {
            blueComandButtons[player].GetComponent<Image>().color = new Color(0, 0, 1, 0.5f);
            Debug.Log("Color changed");
        }
       //Text
    }

    public void SwitchCurb()
    {
        UIAudioSource.PlayOneShot(curbButtonClip);
    }
    public void UpdateFrag(GameObject player, int fragCount)
    {
        if (redComandFragText.ContainsKey(player))
            redComandFragText[player].text = fragCount.ToString();
        if (blueComandFragText.ContainsKey(player))
            blueComandFragText[player].text = fragCount.ToString();
        
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
