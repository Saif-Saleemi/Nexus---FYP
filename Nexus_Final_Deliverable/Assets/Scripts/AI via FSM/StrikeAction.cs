using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeAction : FSMAction
{



    public override void Execute(BaseStateMachine stateMachine, AI enemy)
    {
        if (enemy.BaseMove.Active)
        {
            enemy.useStrike = true;
        }
        else
        {
            enemy.useStrike = false;
        }

    }




}
