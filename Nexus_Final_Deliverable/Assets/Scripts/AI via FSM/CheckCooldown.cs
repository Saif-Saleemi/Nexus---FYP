using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "FSM/Decisions/CheckCooldown")]
public class CheckCooldown : Decision
{
    public override bool Decide(BaseStateMachine stateMachine, AI enemy)
    {
        if (!(enemy.BaseMove.Active && enemy.UltimateMove.Active))
        {
            return true;
        }
        else if(!(enemy.HealMove.Active && enemy.ClassMove.Active))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
