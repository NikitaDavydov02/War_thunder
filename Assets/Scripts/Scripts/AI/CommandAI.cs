using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAI : MonoBehaviour
{
    public bool IsRed;
    private List<TankAI> tanks;
    //float HeightTargetCorrection = 2f;
    // Start is called before the first frame update
    void Start()
    {
        tanks = new List<TankAI>();
        if (IsRed)
        {
            foreach (GameObject g in MainManager.buttleManager.allred)
                if(g.GetComponent<Technic>().Type == TechnicsType.Tank && g.GetComponent<TankAI>() != null)
                    tanks.Add(g.GetComponent<TankAI>());
        }
        else
            foreach (GameObject g in MainManager.buttleManager.allblue)
                if (g.GetComponent<Technic>().Type == TechnicsType.Tank && g.GetComponent<TankAI>()!=null)
                    tanks.Add(g.GetComponent<TankAI>());
        int n = tanks.Count;
        int active = n / 2;
        List<TankAI> activeTanks = new List<TankAI>();
        foreach(TankAI g in tanks)
        {
            g.IsRed = IsRed;
            if (active > 0)
            {
                g.IsActive = true;
                active--;
            }
        }
            
        foreach (TankAI g in tanks)
        {
            g.GoToPosition(MainManager.mapAIManager.GetRandomPositionFor(IsRed));
        }
        Debug.Log("Targets allocated");
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!activeOrNotDictionary.ContainsValue(true))
        {
            //No one active but must be one

        }*/
        /*foreach(TankAI g in activeOrNotDictionary.Keys)
        {
            if(activeOrNotDictionary[g] && g.folowingTarget==null)
            {
                float maxDistance = 100000f;
                GameObject target = null;
                if (IsRed)
                {
                    foreach(GameObject enemy in MainManager.buttleManager.allblue)
                    {
                        float distance = (g.gameObject.transform.position - enemy.gameObject.transform.position).magnitude;
                        if (distance < maxDistance)
                        {
                            maxDistance = distance;
                            target = enemy;
                        }
                    }
                }
                else
                {
                    foreach (GameObject enemy in MainManager.buttleManager.allred)
                    {
                        float distance = (g.gameObject.transform.position - enemy.gameObject.transform.position).magnitude;
                        if (distance < maxDistance)
                        {
                            maxDistance = distance;
                            target = enemy;
                        }
                    }
                }
                g.FolowTarget(target);
                Debug.Log("Comand AI folow target " + g.name);
            }
        }*/
    }
    
}
