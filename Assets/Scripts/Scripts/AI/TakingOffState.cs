using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakingOffState : BaseState
{
    private float treshholdAltitude;
    private PlaneStateMachine planeStateMachine;
    public TakingOffState(PlaneStateMachine planeStateMachine,PlaneController planeController, float treshholdAltitude=300f) : base("TakingOff", planeStateMachine)
    {
        this.treshholdAltitude = treshholdAltitude;
    }
    public override void Eneter()
    {
        base.Eneter();
        //planeController.generalLevel = 1f;
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //planeController.StabalizePitch(10f);
        //planeController.StabilazeRoll(0f);
    }
}
