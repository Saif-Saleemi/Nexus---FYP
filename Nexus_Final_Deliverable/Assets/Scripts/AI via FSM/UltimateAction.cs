using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "FSM/Actions/Ultimate")]
public class UltimateAction : FSMAction
{
    


    public override void Execute(BaseStateMachine stateMachine, AI enemy)
    {
        if (enemy.UltimateMove.Active)
        {
            enemy.useUltimate = true;
        }
        else
        {
            enemy.useUltimate = false;
        }

    }
}
