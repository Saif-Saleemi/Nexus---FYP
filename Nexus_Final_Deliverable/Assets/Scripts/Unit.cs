using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stance {REFLECT, IDLE}
public class Unit : MonoBehaviour
{
    public string unitName;
    public int damage;
    public string type;
    public int maxHP;
    public int currentHP;
    public double armor;
    public int healingCast;
    public string move3;
    public string ultimate;
    public Stance unitStance;
    private Move classMove;
    private Move ultimateMove;
    private Move healMove;



    public Move ClassMove
    {
        get { return classMove; }
        set { classMove = value; }
    }
    public Move UltimateMove
    {
        get { return ultimateMove; }
        set { ultimateMove = value; }
    }
    public Move HealMove
    {
        get { return healMove; }
        set { healMove = value; }
    }


    public void setMoves()
    {
        this.classMove = new Move(3,move3);
        this.ultimateMove = new Move(4,ultimate);
        this.healMove = new Move(2,"Heal");
    }


    public void moveManager()
    {
        this.classMove.decreaseCooldown();
        this.ultimateMove.decreaseCooldown();
        this.healMove.decreaseCooldown();
    }
    public bool TakeDamage(int trueDmg)
    {
    
        currentHP -= trueDmg;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int n)
    {
        currentHP += n;
        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }

    }

    public void TypeMove(string playerType)
    {
        if (playerType == "Fighter")
        {
            armor += 10;

            unitStance = Stance.REFLECT;
            classMove.Active = false;
        }
    }



}
