using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "FSM/Decisions/CheckHealth")]
public class CheckHealth : Decision
{
    public override bool Decide(BaseStateMachine stateMachine, AI enemy)
    {
        if (enemy.currentHP <= enemy.maxHP / 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
