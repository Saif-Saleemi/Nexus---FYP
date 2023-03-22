using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "FSM/Actions/TypeMove")]
public class TypeMoveAction : FSMAction
{



    public override void Execute(BaseStateMachine stateMachine, AI enemy)
    {
        if (enemy.ClassMove.Active)
        {
            enemy.useTypeMove = true;
        }
        else
        {
            enemy.useTypeMove = false;
        }

    }
}
