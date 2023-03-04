using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankWanderingAI : MonoBehaviour {
    [SerializeField]
    public WanderingAIBank tower;
    [SerializeField]
    public WanderingAICorpus corpus;
    public bool stop { get; private set; }
    public bool dolgoStou { get; private set; }
    public ButtleWanderingAI buttleWanderingAI;
    public bool IWantNewRoad = false;
    // Use this for initialization
    void Start () {
        corpus.tankWanderingAI = this;
        corpus.bankWanderingAI = tower;
        tower.tankWanderingAI = this;
        stop = false;
        dolgoStou = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (stop && TimeOfStop == 0)
            TimeOfStop += Time.deltaTime;
        if (TimeOfStop > 5)
            dolgoStou = true;
        if (tower.target != null)
        {
            ModuleController mc = tower.target.GetComponent<ModuleController>();
            if (mc != null)
                if (!mc.alive)
                    buttleWanderingAI.NewTargetForTank(this);
        }
    }

    private float TimeOfStop = 0;
    public void Stop(bool newStop)
    {
        stop = newStop;
        if (newStop = false)
            TimeOfStop = 0;
    }
    public void SetTargetPointForCorpus(Vector3 targetPoint)
    {
        if (corpus != null)
        {
            corpus.NewPointTask(targetPoint);
        }
    }
    public void SetTargetGameObjectForCorpus(GameObject targetGameObject)
    {
        if (corpus != null)
        {
            corpus.NewGameObjectTask(targetGameObject);
        }
    }
    public void SetTargetForTower(Transform target)
    {
        if (tower != null)
        {
            if (tower.SetTargetForBank(target))
            {
                Debug.Log("New!");
                if (IWantNewRoad)
                {
                    Debug.Log("Всё таки дал!");
                    buttleWanderingAI.SelectRoadFor(this, target);
                    IWantNewRoad = false;
                }
            }
        }
    }
    private void SetRoadForCorpus(Road road, int startPoint, int endPoint)
    {
        if (corpus != null)
        {
            corpus.NewRoadTask(road, startPoint, endPoint);
        }
    }
    public IEnumerator SetRoad(Road road, int startPoint, int endPoint, float timePause)
    {
        yield return new WaitForSeconds(timePause);
        if (corpus != null)
        {
            corpus.NewRoadTask(road, startPoint, endPoint);
        }
    }
    public void NewRoad()
    {
        Debug.Log("New road!");
        IWantNewRoad = true;

    }
}
