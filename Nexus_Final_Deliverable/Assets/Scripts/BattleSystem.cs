using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleSystem : TutorialBattleSystem
{
    public TextMeshProUGUI move3Name;
    public TextMeshProUGUI ultimateName;


    void Start()
    {
        state = BattleState.START;


        StartCoroutine(SetupBattle());
    }


    protected override IEnumerator SetupBattle()
    {


        
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        move3Name.text = playerUnit.move3;
        ultimateName.text = playerUnit.ultimate;
        playerUnit.setMoves();
        enemyUnit.setMoves();
        yield return new WaitForSeconds(1f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }

    public void OnMove3Button()
    {

        if (state != BattleState.PLAYERTURN)
            return;
        if (playerUnit.ClassMove.Active)
        {
            StartCoroutine(Move3());
        }
        else
        {
            playerDialogue.text = playerUnit.ClassMove.moveName + " is on Cooldown!!";
            return;
        }
        
    }

    IEnumerator Move3()
    {
        playerUnit.TypeMove(playerUnit.type);
        playerDialogue.text = playerUnit.unitName + " Used Counter, Armor increased by 10";
        yield return new WaitForSeconds(2f);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }


    protected new  IEnumerator EnemyTurn()
    {
        StartCoroutine(Strike(enemyUnit, playerUnit, enemyHUD, playerHUD));
        yield return new WaitForSeconds(2f);
        if (playerUnit.unitStance == Stance.REFLECT)
        {
            playerDialogue.text = playerUnit.unitName + " Reflected Strike for " + enemyUnit.damage / 2 + " damage";
            enemyUnit.TakeDamage(enemyUnit.damage / 2);
            enemyHUD.SetHP(enemyUnit.currentHP);
            playerUnit.unitStance = Stance.IDLE;
            //add this to strike method instead
            //add AI pattern
            
        }
       state = BattleState.PLAYERTURN; 
       yield return new WaitForSeconds(2f);
       PlayerTurn();
       

    }


}



    














