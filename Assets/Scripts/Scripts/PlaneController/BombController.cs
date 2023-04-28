using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField]
    List<Bomb> bombs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (bombs.Count != 0)
            {
                bombs[bombs.Count - 1].Release(GetComponent<Rigidbody>().velocity);
                Debug.Log("Initial velocity" + GetComponent<Rigidbody>().velocity);
                MainManager.PlayerFired(gameObject, bombs[bombs.Count - 1].gameObject);
                bombs.RemoveAt(bombs.Count - 1);
            }
        }
    }
}
