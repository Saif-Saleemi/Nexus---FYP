using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class TutorialBattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public TextMeshProUGUI playerDialogue;
    public TextMeshProUGUI playerLog;
    public TextMeshProUGUI enemyLog;

    protected Unit playerUnit;
    protected AI enemyUnit;

   
    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
       StartCoroutine(SetupBattle());
  
    }



    protected virtual IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<AI>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        playerUnit.setMoves();
        enemyUnit.setMoves();
        yield return new WaitForSeconds(1f);
        state = BattleState.PLAYERTURN;

        PlayerTurn();
        
    }

    protected void PlayerTurn()
    {
        playerUnit.moveManager();
        if (playerUnit.unitStance == Stance.STUN)
        {
            
            playerUnit.unitStance = Stance.IDLE;
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        else
        {
        playerDialogue.text = "Choose an action";
        ResetLogs();
        }

    }
    protected void ResetLogs()
    {
        playerLog.text = "";
        enemyLog.text = "";
    }

    public void OnStrikeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        if (playerUnit.BaseMove.Active || playerUnit.unitName == "Player")
        {
           StartCoroutine(Strike(playerUnit,enemyUnit,playerHUD,enemyHUD));
        }
        else
        {
            playerDialogue.text = playerUnit.BaseMove.moveName + " is on Cooldown!!";
            return;
        }
        
    }
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        if (playerUnit.HealMove.Active)
        {
          StartCoroutine(Heal(playerUnit));
        }
        else
        {
            playerDialogue.text = playerUnit.HealMove.moveName + " is on Cooldown!!";
            return;
        }
        
    }


    protected IEnumerator Heal(Unit unitToHeal)
    {
        unitToHeal.Heal(unitToHeal.healingCast);
        if (state == BattleState.PLAYERTURN)
        {
          playerHUD.SetHP(unitToHeal.currentHP);
        }
        else if (state == BattleState.ENEMYTURN)
        {
            enemyHUD.SetHP(unitToHeal.currentHP);
        }
        

       
        playerDialogue.text = unitToHeal.unitName + " used Heal for " + unitToHeal.healingCast + " hitpoints";

        unitToHeal.HealMove.Active = false;
        yield return new WaitForSeconds(2f);
        if (state == BattleState.PLAYERTURN)
        {
            
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        else
        {
        state = BattleState.PLAYERTURN;
        PlayerTurn();
        }

        
    }
    protected IEnumerator Strike(Unit attacker, Unit defender, BattleHUD attackerHUD, BattleHUD defenderHUD)
    {
        double offset = 1.0 - (defender.armor / 100.0);
        double trueDamageDouble = attacker.damage * offset;
        int trueDamageInt = Convert.ToInt32(trueDamageDouble);
        bool isDead = defender.TakeDamage(trueDamageInt);



        if (state == BattleState.PLAYERTURN)
        {
            if (isDead)
            {
                state = BattleState.WON;


                playerDialogue.text = attacker.unitName + " Used Strike!";
                enemyLog.text = defender.unitName + " took " + trueDamageInt + " damage";
                defenderHUD.SetHP(defender.currentHP = 0);
                yield return new WaitForSeconds(1f);
                attacker.BaseMove.Active = false;
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.ENEMYTURN;

                defenderHUD.SetHP(defender.currentHP);
                playerDialogue.text = attacker.unitName + " Used Strike!";
                enemyLog.text = defender.unitName + " took " + trueDamageInt + " damage";
                yield return new WaitForSeconds(2f);
                attacker.BaseMove.Active = false;
                StartCoroutine(EnemyTurn());
            }
        }
        else if (state == BattleState.ENEMYTURN)
        {

            if (isDead)
            {
                state = BattleState.LOST;
                playerDialogue.text = attacker.unitName + " Used Strike!";
                playerLog.text = defender.unitName + " took " + trueDamageInt + " damage";
                defenderHUD.SetHP(defender.currentHP = 0);
                yield return new WaitForSeconds(1f);
                attacker.BaseMove.Active = false;
                StartCoroutine(EndBattle());
            }
            else
            {
                
                defenderHUD.SetHP(defender.currentHP);
                playerDialogue.text = attacker.unitName + " Used Strike!";
                playerLog.text = defender.unitName + " took " + trueDamageInt + " damage";
                attacker.BaseMove.Active = false;

                if (playerUnit.unitStance == Stance.REFLECT)
                {
                    
                    
                    enemyLog.text = playerUnit.unitName + " Reflected Strike for " + enemyUnit.damage / 2 + " damage";
                    bool isDead2 = enemyUnit.TakeDamage(enemyUnit.damage / 2);

                    if (isDead2)
                    {
                        state = BattleState.WON;
                        enemyHUD.SetHP(enemyUnit.currentHP);
                        StartCoroutine(EndBattle());
                    }
                    enemyHUD.SetHP(enemyUnit.currentHP);
                    playerUnit.unitStance = Stance.IDLE;


                }
                yield return new WaitForSeconds(2f);
                state = BattleState.PLAYERTURN;
                PlayerTurn();
            }

        }
        
  
    }
    protected virtual  IEnumerator EnemyTurn()
    {
        //ResetLogs();
        StartCoroutine(Strike(enemyUnit, playerUnit, enemyHUD, playerHUD));
        yield return new WaitForSeconds(2f);
    }



    protected IEnumerator EndBattle()
    {
        if(state == BattleState.WON)
        {
            playerDialogue.text = "You won the battle!";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(0);
        }
        else if(state == BattleState.LOST)
        {
            playerDialogue.text = "You were defeated!";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(0);
        }
    }

}
