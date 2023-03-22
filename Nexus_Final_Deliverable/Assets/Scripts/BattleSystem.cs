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
    public BaseStateMachine aiFSM;

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
        enemyUnit = enemyGO.GetComponent<AI>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        move3Name.text = playerUnit.move3;
        ultimateName.text = playerUnit.ultimate;
        playerUnit.setMoves();
        enemyUnit.setMoves();
        aiFSM = enemyGO.GetComponent<BaseStateMachine>();
        
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
            StartCoroutine(Move3(playerUnit.unitName));
        }
        else
        {
            playerDialogue.text = playerUnit.ClassMove.moveName + " is on Cooldown!!";
            return;
        }
        
    }


    public void OnUltimateButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        if (playerUnit.UltimateMove.Active)
        {
            StartCoroutine(Ultimate(playerUnit.unitName));
        }
        else
        {
            playerDialogue.text = playerUnit.UltimateMove.moveName + " is on Cooldown!!";
            return;
        }
    }

    IEnumerator  Cripple()
    {
        
        bool isDead = enemyUnit.TakeDamage(100);
        playerDialogue.text = playerUnit.unitName + " Used Cripple";
        if (isDead)
        {
            state = BattleState.WON;
            playerLog.text = "damage increased by 15";
            yield return new WaitForSeconds(2f);
            enemyLog.text = enemyUnit.unitName + " took 100 damage";
            enemyHUD.SetHP(enemyUnit.currentHP);
            yield return new WaitForSeconds(2f);
            StartCoroutine(EndBattle());
        }
        else
        {
        yield return new WaitForSeconds(2f);
        playerLog.text = "damage increased by 20";
        yield return new WaitForSeconds(2f);
        enemyLog.text = enemyUnit.unitName + " took 100 damage";
        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(2f);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator Thunderstrike()
    {
        bool isDead = playerUnit.TakeDamage(50);
        playerDialogue.text = enemyUnit.unitName + " Used Thunderstrike";
        if (isDead)
        {
            state = BattleState.LOST;
            playerLog.text = playerUnit.unitName + " took 50 damage and is stunned";
            yield return new WaitForSeconds(2f);
            playerHUD.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(2f);
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.LOST;
            playerLog.text = playerUnit.unitName + " took 50 damage and is stunned";
            yield return new WaitForSeconds(2f);
            playerLog.text = playerUnit.unitName + " Cannot make a move!";
            playerHUD.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;
            PlayerTurn();

        }
    }

    IEnumerator Ultimate(string userName)
    {
        if (userName == "Rex")
        {
            playerUnit.Ultimate(userName);
            StartCoroutine(Cripple());
        }
        else if (userName == "Sharky")
        {
            playerUnit.armor -= 10.0;
            enemyUnit.UltimateMove.Active = false;
            StartCoroutine(Drown());
        }
        else if (userName == "Thunderblade")
        {
            playerUnit.unitStance = Stance.STUN;
            enemyUnit.UltimateMove.Active = false;
            StartCoroutine(Thunderstrike());
        }
        yield return new WaitForSeconds(2f);
    }
    IEnumerator Counter()
    {
        playerDialogue.text = playerUnit.unitName + " Used Counter";
        yield return new WaitForSeconds(2f);
        playerLog.text = "Armor increased by 10, Reflect Active";
        yield return new WaitForSeconds(2f);
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
    IEnumerator Hydroburst()
    {
        playerDialogue.text = enemyUnit.unitName + " Used Hydroburst";
        
        enemyLog.text = "Armor increased by 10";
        yield return new WaitForSeconds(2f);

        playerUnit.healingCast = playerUnit.healingCast - 10;
        playerLog.text = "Healing decreased by 10";
        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator Drown()
    {
        playerDialogue.text = enemyUnit.unitName + " Used Drown";
        
        playerLog.text = "Armor decreased by 10";
        yield return new WaitForSeconds(2f);

        bool isDead = playerUnit.TakeDamage(80);
        if (isDead)
        {
            state = BattleState.LOST;
            
            
            playerLog.text = playerUnit.unitName + " took 80 damage";
            playerHUD.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(2f);
            StartCoroutine(EndBattle());
        }
        else
        {
            
            
            
            playerLog.text = playerUnit.unitName + " took 80 damage";
            playerHUD.SetHP(playerUnit.currentHP);
            yield return new WaitForSeconds(2f);
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator Charge()
    {
        playerDialogue.text = enemyUnit.unitName + " Used Charge";
        enemyLog.text = "Healing Cast increased by 25";
        yield return new WaitForSeconds(2f);
        enemyLog.text = enemyUnit.unitName + " Healed for 50 Hitpoints";
        enemyHUD.SetHP(enemyUnit.currentHP);
        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }
    IEnumerator Move3(string userName)
    {
        
        if (userName == "Rex")
        {
            playerUnit.TypeMove(playerUnit.type);
            StartCoroutine(Counter());
        }
        else if (userName == "Sharky")
        {
            enemyUnit.TypeMove(enemyUnit.type);
            StartCoroutine(Hydroburst());
        }
        else if (userName == "Thunderblade")
        {
            enemyUnit.TypeMove(enemyUnit.type);
            StartCoroutine(Charge());
        }

        yield return new WaitForSeconds(2f);


    }

    


    protected override  IEnumerator EnemyTurn()
    {
        
        enemyUnit.moveManager();
        aiFSM.currentState.Execute(aiFSM, enemyUnit);
        
        aiFSM.currentState.Execute(aiFSM, enemyUnit);
        if ((enemyUnit.useStrike && enemyUnit.useUltimate)
            || (enemyUnit.useStrike && !enemyUnit.useUltimate))
        {
            StartCoroutine(Strike(enemyUnit, playerUnit, enemyHUD, playerHUD));
        }
       else if (!enemyUnit.useStrike && enemyUnit.useUltimate)
        {
            StartCoroutine(Ultimate(enemyUnit.unitName)); //Use Ultimate
        }
        else if ((enemyUnit.useHeal && enemyUnit.useTypeMove)
            || (enemyUnit.useHeal && !enemyUnit.useTypeMove))
        {
            StartCoroutine(Heal(enemyUnit)); //Use heal
        }
        else if (!enemyUnit.useHeal && enemyUnit.useTypeMove)
        {
            StartCoroutine(Move3(enemyUnit.unitName)); //Use TypeMove
        }









        aiFSM.currentState.Execute(aiFSM, enemyUnit);
        yield return new WaitForSeconds(2f);
       
       
       

    }


}



    














