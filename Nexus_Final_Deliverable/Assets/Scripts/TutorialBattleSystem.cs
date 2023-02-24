using System.Collections;
using System.Collections.Generic;
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

    Unit playerUnit;
    Unit enemyUnit;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
       StartCoroutine(SetupBattle());
  
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(1f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
        
    }

    void PlayerTurn()
    {
        playerDialogue.text = "Choose an action";
    }

    public void OnStrikeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(Strike());
    }
    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(Heal());
    }
    IEnumerator Heal()
    {
        playerUnit.Heal(15);
        state = BattleState.ENEMYTURN;
        playerHUD.SetHP(playerUnit.currentHP);
        playerDialogue.text = "Player healed for " + "15 Hit Points";
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(EnemyTurn());
    }
    IEnumerator Strike()
    {

        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
      
        
        if (isDead)
        {
            state = BattleState.WON;
            playerDialogue.text = playerUnit.unitName + " Used Strike for " + playerUnit.damage + " damage";
            yield return new WaitForSeconds(1f);
            enemyHUD.SetHP(enemyUnit.currentHP = 0);
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            enemyHUD.SetHP(enemyUnit.currentHP);
            playerDialogue.text = playerUnit.unitName + " Used Strike for " + playerUnit.damage + " damage";
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }
    }
    IEnumerator EnemyTurn()
    {
  

        playerDialogue.text = enemyUnit.unitName + " attacks for " + enemyUnit.damage + " damage";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
           StartCoroutine( EndBattle());

        }
        else
        {
            state = BattleState.PLAYERTURN;
                PlayerTurn();
        }
    }
    IEnumerator EndBattle()
    {
        if(state == BattleState.WON)
        {
            playerDialogue.text = "You won the battle!";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else if(state == BattleState.LOST)
        {
            playerDialogue.text = "You were defeated!";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

}
