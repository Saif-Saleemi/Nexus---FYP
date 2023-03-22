using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "FSM/Transition")]
public sealed class Transition : ScriptableObject
{
    public Decision Decision1;
    public Decision Decision2;
    public BaseState TrueState;
    public BaseState FalseState;

    public void Execute(BaseStateMachine stateMachine, AI enemy)
    {
        if ((Decision1.Decide(stateMachine, enemy) || Decision2.Decide(stateMachine, enemy)) && !(TrueState is RemainInState))
        {
            stateMachine.currentState = TrueState;
        }
        else if (!(FalseState is RemainInState))
        {
            stateMachine.currentState = FalseState;
        }
    }
}
