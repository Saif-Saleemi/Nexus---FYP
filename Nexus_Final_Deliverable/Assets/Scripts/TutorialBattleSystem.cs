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

    protected Unit playerUnit;
    protected Unit enemyUnit;

   
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
        enemyUnit = enemyGO.GetComponent<Unit>();

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
        playerDialogue.text = "Choose an action";
    }

    public void OnStrikeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(Strike(playerUnit,enemyUnit,playerHUD,enemyHUD));
    }
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        if (playerUnit.HealMove.Active)
        {
          StartCoroutine(Heal());
        }
        else
        {
            playerDialogue.text = playerUnit.HealMove.moveName + " is on Cooldown!!";
            return;
        }
        
    }


    protected IEnumerator Heal()
    {
        playerUnit.Heal(playerUnit.healingCast);
        state = BattleState.ENEMYTURN;
        playerHUD.SetHP(playerUnit.currentHP);
        playerDialogue.text = playerUnit.unitName + " healed for " + playerUnit.healingCast + " Hit Points";
        playerUnit.HealMove.Active = false;
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(EnemyTurn());
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


                playerDialogue.text = attacker.unitName + " Used Strike for " + trueDamageInt + " damage";
                defenderHUD.SetHP(defender.currentHP = 0);
                yield return new WaitForSeconds(1f);
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.ENEMYTURN;

                defenderHUD.SetHP(defender.currentHP);
                playerDialogue.text = attacker.unitName + " Used Strike for " + trueDamageInt + " damage";
                yield return new WaitForSeconds(2f);
                StartCoroutine(EnemyTurn());
            }
        }
        else if (state == BattleState.ENEMYTURN)
        {
             if (isDead)
            {
                state = BattleState.LOST;
                playerDialogue.text = attacker.unitName + " Used Strike for " + trueDamageInt + " damage";
                defenderHUD.SetHP(defender.currentHP = 0);
                yield return new WaitForSeconds(1f);
                StartCoroutine(EndBattle());
            }
            else
            {
                state = BattleState.PLAYERTURN;
                defenderHUD.SetHP(defender.currentHP);
                playerDialogue.text = attacker.unitName + " Used Strike for " + trueDamageInt + " damage";
                yield return new WaitForSeconds(2f);
                PlayerTurn();
            }
        }
        
  
    }
    protected  IEnumerator EnemyTurn()
    {
        StartCoroutine(Strike(enemyUnit, playerUnit, enemyHUD, playerHUD));
        yield return new WaitForSeconds(1f);
    }



    IEnumerator EndBattle()
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
