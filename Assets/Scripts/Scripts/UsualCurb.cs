using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsualCurb : Curb
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (stop || MainManager.GameStatus != GameStatus.Playing)
            return;
        speedVector.y += g * Time.deltaTime;
        transform.Translate(speedVector * Time.deltaTime, Space.World);
    }
}
