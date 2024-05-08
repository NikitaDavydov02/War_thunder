using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField]
    List<Curb> bombs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Release()
    {
        if (bombs.Count != 0)
        {
            bombs[bombs.Count - 1].Release(gameObject.name, GetComponent<Rigidbody>().velocity);
            bombs[bombs.Count - 1].name = gameObject.name + "_curb";
            bombs[bombs.Count - 1].transform.parent = null;
            Debug.Log("Initial velocity" + GetComponent<Rigidbody>().velocity);
            MainManager.PlayerFired(gameObject, bombs[bombs.Count - 1].gameObject);
            bombs.RemoveAt(bombs.Count - 1);
        }
    }
}
