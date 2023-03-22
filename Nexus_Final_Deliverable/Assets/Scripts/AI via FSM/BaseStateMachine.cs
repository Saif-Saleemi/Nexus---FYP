using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    [SerializeField] private BaseState initialState;
    
    private void Awake()
    {
        currentState = initialState;
        
    }
    public BaseState currentState
    {
        get;
        set;
    }
  



}
