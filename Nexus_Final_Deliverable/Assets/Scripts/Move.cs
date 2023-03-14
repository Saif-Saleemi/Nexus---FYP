using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private int cooldown;
    public string moveName;
    private bool active = true;
    private int counter = 0;

    public Move(int cooldown, string name)
    {
        this.cooldown = cooldown;
        this.moveName = name;
    }
    
    public void decreaseCooldown()
    {
        if (active == false)
        {
        if (counter == this.cooldown)
        {
            counter = 0;
            active = true;
        }
        else
        {
            counter += 1;
        }
        }
    }

    public bool Active
    {
        get { return active; }
        set { active = value; }
    }


}
