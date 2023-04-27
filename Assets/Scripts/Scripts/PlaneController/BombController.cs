using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField]
    List<FixedJoint> bombs;
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
                Destroy(bombs[bombs.Count - 1]);
                bombs.RemoveAt(bombs.Count - 1);
            }
        }
    }
}
