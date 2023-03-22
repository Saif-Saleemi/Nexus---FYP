using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stance {REFLECT, IDLE, STUN}
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
    private Move baseMove;



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
    public Move BaseMove
    {
        get { return baseMove; }
        set { baseMove = value; }
    }


    public void setMoves()
    {
        this.classMove = new Move(3,move3, "Defend");
        this.ultimateMove = new Move(4,ultimate, "Attack");
        this.healMove = new Move(2,"Heal", "Defend");
        this.baseMove = new Move(1, "Strike", "Attack");
    }


    public void moveManager()
    {
        this.classMove.decreaseCooldown();
        this.ultimateMove.decreaseCooldown();
        this.healMove.decreaseCooldown();
        this.baseMove.decreaseCooldown();
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
        else if (playerType == "Water")
        {
            armor += 10;
            classMove.Active = false;
        }
        else if (playerType == "Electric")
        {
            healingCast += 25;
            Heal(50);
            classMove.Active = false;
        }
    }

    public void Ultimate(string playerName)
    {
        if (playerName == "Rex")
        {
            damage += 15;
            ultimateMove.Active = false;
        }
    }



}
