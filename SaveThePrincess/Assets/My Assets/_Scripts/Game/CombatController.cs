using UnityEngine;
using System.Collections;

public class CombatController : MonoBehaviour {

    public enum BattleStates{
        START,
        PLAYERCHOICE,
        PLAYERANIMATE,
        ENEMYANIMATE,
        ENEMYCHOICE,
        PLAYERTAKEDAMAGE,
        ENEMYTAKEDAMAGE,
        LOSE,
        WIN
    }

    public  BattleStates currentState;
    public GameController gc;

	// Use this for initialization
	void start () {
        // begin in start state
        currentState = BattleStates.START;
	}

    void Awake()
    {
        this.gc = FindObjectOfType<GameController>();

    }

    public void setState(BattleStates state)
    {
        this.currentState = state;
    }

	// Update is called once per frame
	void Update () {
        //Debug.Log(currentState);
        switch (currentState)
        {
            case BattleStates.START:
                // initialize for battle if needed

                break;
            case BattleStates.PLAYERCHOICE:
                gc.EnableButtons();
                // wait for choice. will change on click
                break;
            case BattleStates.PLAYERANIMATE:
                // no need for selection after turn
                gc.DisableButtons();
                // player animates
                //gc.player.PhysicalAttack(gc.enemy);

                break;
            case BattleStates.ENEMYTAKEDAMAGE:
                //gc.enemy.TakeDamage();
                //enemy turn
                break;
            case BattleStates.ENEMYCHOICE:
                //enemy turn handled in gc ai
                break;
            case BattleStates.ENEMYANIMATE:
                // enemy animates, player takes damage

                break;
            case BattleStates.PLAYERTAKEDAMAGE:
                //gc.player.TakeDamage();
                //enemy turn
                break;
            case BattleStates.WIN:
                
                // animation/next screen
                break;
            case BattleStates.LOSE:
                
                // animation/endscreen
                break;
        }
	}
}
