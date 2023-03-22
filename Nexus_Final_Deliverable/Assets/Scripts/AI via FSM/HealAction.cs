using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "FSM/Actions/Heal")]
public class HealAction : FSMAction
{



    public override void Execute(BaseStateMachine stateMachine, AI enemy)
    {
        if (enemy.HealMove.Active)
        {
            enemy.useHeal = true;
        }
        else
        {
            enemy.useHeal = false;
        }

    }
}
