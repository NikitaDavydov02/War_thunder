using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandedState : BaseState
{
    // Start is called before the first frame update
    public LandedState(PlaneStateMachine planeStateMachine, Transform transform) : base("Landed", planeStateMachine)
    {

    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //stateMachine.ChangeState();
    }
}
