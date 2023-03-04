using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtleWanderingAI : MonoBehaviour {
    //[SerializeField]
    public List<GameObject> friends;
    //[SerializeField]
    public List<GameObject> enemies;
    [SerializeField]
    List<Vector3> platsdarms;
    [SerializeField]
    ButtleManager buttleManager;
    public float TimePause = 10f;
    public float timeSinceStart = 0;
    public bool red;
    public bool poslalPervieRaz = false;

    public List<GameObject> dinamicfriends;
    public float distanceK;
    public List<int> enemyesOfFriends;

    // Use this for initialization
    void Start () {
        enemies = new List<GameObject>();
        friends = new List<GameObject>();
        //if (!red)
        //    return;
        //GameObject tank = Instantiate(tankPrefab) as GameObject;
        //tank.transform.position = new Vector3(1156, 31, 3800);
        //friends.Add(tank);

        //if (red)
        //    buttleManager.InizializeRedComand(friends);
        //else
         //   buttleManager.InizializeBlueComand(friends);
    }

    // Update is called once per frame
    public void UpdateRoad(TankWanderingAI script)
    {
        for (int i = 0; i < friends.Count; i++)
        {
            if (script != null)
            {
                int indexOfRoad = Random.Range(0, MainManager.roadManager.roads.Count);
                Road r = MainManager.roadManager.roads[indexOfRoad];
                float distance;
                float minDistanceToStart = 100000;
                int bestControlPoint = 0;
                for (int j = 0; j < r.controlPoints.Count; j++)
                {
                    distance = Mathf.Abs(Vector3.Magnitude(friends[i].transform.position - r.controlPoints[j]));
                    if (distance < minDistanceToStart)
                    {
                        minDistanceToStart = distance;
                        bestControlPoint = j;
                    }
                }
                int bestEndControlPoint = 0;
                float minDistanceToEnd = 10000;
                for (int j = 0; j < r.controlPoints.Count; j++)
                {
                    distance = Mathf.Abs(Vector3.Magnitude(enemies[enemyesOfFriends[i]].transform.position - r.controlPoints[j]));
                    if (distance < minDistanceToEnd)
                    {
                        minDistanceToEnd = distance;
                        bestEndControlPoint = j;
                    }
                }
                float distanceByRoad = minDistanceToStart + minDistanceToEnd + r.Distance(bestControlPoint, bestEndControlPoint);
                float ahead = Mathf.Abs(Vector3.Magnitude(enemies[enemyesOfFriends[i]].transform.position - transform.position));
                Debug.Log("By Road = " + distanceByRoad + " Ahead = " + ahead);
                if (distanceByRoad / ahead >= distanceK)
                    script.SetTargetGameObjectForCorpus(enemies[enemyesOfFriends[i]]);
                else
                    StartCoroutine(script.SetRoad(r, bestControlPoint, bestEndControlPoint, TimePause * i));
            }
        }
    }
	void Update () {
        if (buttleManager.gameFinished)
            return;
        while (friends.Count==0)
        {
            if (red)
            {
                enemies = buttleManager.ReturnCopyOfAllBlue();
                friends= buttleManager.ReturnCopyOfAllRed();
            }
            else
            {
                enemies = buttleManager.ReturnCopyOfAllRed();
                friends = buttleManager.ReturnCopyOfAllBlue();
            }

            return;
        }
        foreach(GameObject friend in friends)
        {
            TankWanderingAI script = friend.GetComponent<TankWanderingAI>();
            if (script != null)
            {
                script.buttleWanderingAI = this;
                Debug.Log("Script otdal.");
            }
            else
            {
                Debug.Log("Script = null");
            }
        }

        
        //enemyesOfFriends = new List<int>();

        //NewTargetForFriends();

        //timeSinceStart += Time.deltaTime;
        for(int i = 0; i < friends.Count; i++)
        {
            float minDistance = 100000;
            int nearestEnemy = 0;
            for(int x = 0; x < enemies.Count; x++)
            {
                float d = Mathf.Abs(Vector3.Magnitude(friends[i].transform.position - enemies[x].transform.position));
                if (d < minDistance && enemies[x].GetComponent<ModuleController>().alive)
                {
                    minDistance = d;
                    nearestEnemy = x;
                }
           }
            TankWanderingAI script = friends[i].GetComponent<TankWanderingAI>();
            if(script!=null)
                script.SetTargetForTower(enemies[nearestEnemy].transform);
            enemyesOfFriends.Add(nearestEnemy);
        }

        if (!poslalPervieRaz)
        {
            for (int i = 0; i < friends.Count; i++)
            {
                TankWanderingAI script = friends[i].GetComponent<TankWanderingAI>();
                //SelectRoadFor(script);
                if (script != null)
                {
                    int indexOfRoad = Random.Range(0, MainManager.roadManager.roads.Count);
                    Road r = MainManager.roadManager.roads[indexOfRoad];
                    float distance;
                    float minDistanceToStart = 100000;
                    int bestControlPoint = 0;
                    for (int j = 0; j < r.controlPoints.Count; j++)
                    {
                        distance = Mathf.Abs(Vector3.Magnitude(friends[i].transform.position - r.controlPoints[j]));
                        if (distance < minDistanceToStart)
                        {
                            minDistanceToStart = distance;
                            bestControlPoint = j;
                        }
                    }
                    int bestEndControlPoint = 0;
                    float minDistanceToEnd = 10000;
                    for (int j = 0; j < r.controlPoints.Count; j++)
                    {
                        distance = Mathf.Abs(Vector3.Magnitude(enemies[0].transform.position - r.controlPoints[j]));
                        if (distance < minDistanceToEnd)
                        {
                            minDistanceToEnd = distance;
                            bestEndControlPoint = j;
                        }
                    }
                    float distanceByRoad = minDistanceToStart + minDistanceToEnd + r.Distance(bestControlPoint, bestEndControlPoint);
                    float ahead = Mathf.Abs(Vector3.Magnitude(enemies[0].transform.position - friends[i].transform.position));
                    Debug.Log(gameObject.name+" By Road = " + distanceByRoad + " Ahead = " + ahead);
                    if (distanceByRoad / ahead >= distanceK)
                        script.SetTargetGameObjectForCorpus(enemies[enemyesOfFriends[i]]);
                    else
                        StartCoroutine(script.SetRoad(r, bestControlPoint, bestEndControlPoint, TimePause * i));
                    script.SetTargetForTower(enemies[Random.Range(0, enemies.Count - 1)].transform);
                }
            }
            poslalPervieRaz = true;
            return;
        }
    }
    public void NewTargetForTank(TankWanderingAI tankWanderingAI)
    {
        float minDistance = 100000;
        int nearestEnemy = 0;
        for (int x = 0; x < enemies.Count; x++)
        {
            float d = Mathf.Abs(Vector3.Magnitude(tankWanderingAI.gameObject.transform.position - enemies[x].transform.position));
            if (d < minDistance && enemies[x].GetComponent<ModuleController>().alive)
            {
                minDistance = d;
                nearestEnemy = x;
            }
        }
    }

    public void NewTargetForFriends()
    {
        enemyesOfFriends = new List<int>();
        for (int i = 0; i < friends.Count; i++)
        {
            float minDistance = 100000;
            int nearestEnemy = 0;
            for (int x = 0; x < enemies.Count; x++)
            {
                float d = Mathf.Abs(Vector3.Magnitude(friends[i].transform.position - enemies[x].transform.position));
                if (d < minDistance && enemies[x].GetComponent<ModuleController>().alive)
                {
                    minDistance = d;
                    nearestEnemy = x;
                }
            }
            TankWanderingAI script = friends[i].GetComponent<TankWanderingAI>();
            if (script != null)
                script.SetTargetForTower(enemies[nearestEnemy].transform);
            enemyesOfFriends.Add(nearestEnemy);
        }
    }
    public void SelectRoadFor(TankWanderingAI script, Transform newEnemy)
    {
        if (script == null)
            return;
        //int ind = 0;
        //for(int i = 0; i < friends.Count; i++)
        //{
        //    if (friends[i] == script.gameObject)
        //    {
        //        ind = i;
        //        Debug.Log("Found! " + script.gameObject.name+" was looking for: "+newEnemy.gameObject.name);
        //        break;
        //    }
        //}
        int indexOfRoad = Random.Range(0, MainManager.roadManager.roads.Count);
        Road r = MainManager.roadManager.roads[indexOfRoad];
        float distance;
        float minDistanceToStart = 100000;
        int bestControlPoint = 0;
        for (int j = 0; j < r.controlPoints.Count; j++)
        {
            distance = Mathf.Abs(Vector3.Magnitude(script.gameObject.transform.position - r.controlPoints[j]));
            if (distance < minDistanceToStart)
            {
                minDistanceToStart = distance;
                bestControlPoint = j;
            }
        }
        int bestEndControlPoint = 0;
        float minDistanceToEnd = 10000;
        for (int j = 0; j < r.controlPoints.Count; j++)
        {
            distance = Mathf.Abs(Vector3.Magnitude(newEnemy.transform.position - r.controlPoints[j]));
            if (distance < minDistanceToEnd)
            {
                minDistanceToEnd = distance;
                bestEndControlPoint = j;
            }
        }
        float distanceByRoad = minDistanceToStart + minDistanceToEnd + r.Distance(bestControlPoint, bestEndControlPoint);
        float ahead = Mathf.Abs(Vector3.Magnitude(newEnemy.transform.position - transform.position));
        Debug.Log(gameObject.name + " By Road = " + distanceByRoad + " Ahead = " + ahead);
        if (distanceByRoad / ahead >= distanceK)
            script.SetTargetGameObjectForCorpus(newEnemy.gameObject);
        else
            StartCoroutine(script.SetRoad(r, bestControlPoint, bestEndControlPoint,0));
    }
}
