using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public BaseEnemyController enemy;
    public GameObject gameObj;
    public GameController gameController;
    public CombatController combatController;

    public bool enemyHasHealed, 
                waiting, 
                scoredThisRound, 
                enemyHasAttacked,
                playerHasAttacked,
                attackBarMoving;

    public int score, 
               turn,
               ENERGY_REGEN_AMT = 10;

    public float MONEY_TRANSFER_PCT = 0.2f,
                 COOLDOWN_LENGTH = 1.75f;

    public float cooldownValue;

    public Slider playerHealth, 
                  playerMana, 
                  enemyHealth, 
                  enemyMana;
    public Slider attackMeter;

    public float attackAmount;

    private Vector3 prevPos;

    // GUI/HUD
    public Text leftHealthText,
                rightHealthText,
                leftManaText,
                rightManaText,
                leftArmorText,
                rightArmorText,
                leftDamageText,
                rightDamageText,
                numEnemiesKilledText;

    public Button leftPhysAttack = null;

    private Image background;

    // Use this for initialization
    void Awake()
    {
        // Combat AI Controller reference
        this.combatController = FindObjectOfType<CombatController>();
        // INITIALIZE BATTLE SCENE
        combatController.setState(CombatController.BattleStates.START);

        this.turn = 0;
        PlayerPrefs.SetInt("turn", turn);
        this.scoredThisRound = false;
        this.score = PlayerPrefs.GetInt("score");
        this.cooldownValue = 0.0f;

        // enemy has not healed or attacked yet
        enemyHasHealed = false;
        enemyHasAttacked = false;

        // get game Controller Object
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameControllerObject == null)
        {
            Debug.Log("Cannot find GameObject!");
            return;
        }



        // get playerController 
        this.player = FindObjectOfType<PlayerController>();
        
        // carry over previous balance
        this.player.dollarBalance += PlayerPrefs.GetInt("carryover");
        PlayerPrefs.SetInt("carryover", 0);
        // reposition player
        this.prevPos = this.player.transform.localPosition;
        Vector3 newSpot = new Vector3(-2.5f, -2.5f);
        this.player.gameObject.transform.localPosition = newSpot;

        // get enemy reference
        this.enemy = FindObjectOfType<BaseEnemyController>();

        // Set Attack Meter Amount
        this.attackMeter.maxValue = 100;
        this.attackMeter.value = (float)Random.Range(0, this.attackMeter.maxValue);
        
      // ENEMY SET UP

        // ENEMY ARMOR

        // ENEMY WEAPON

        // ENEMY STATS

        // ENEMY INVENTORY


        // combat starts after initialization is finished
        combatController.setState(CombatController.BattleStates.PLAYERCHOICE);
    }



    /// <summary>
    /// Disables the buttons.
    /// </summary>
    public void DisableButtons()
    {
        this.leftPhysAttack.gameObject.SetActive(false);
        this.attackMeter.gameObject.SetActive(false);
    }

    /// <summary>
    /// Enables the buttons.
    /// </summary>
    public void EnableButtons()
    {
        this.leftPhysAttack.gameObject.SetActive(true);
    }

    /// <summary>
    /// Player Uses the item in inventory specified at index.
    /// </summary>
    /// <param name="index">Index.</param>
    public void UseItem(int index)
    {
        /*if (combatController.currentState == CombatController.BattleStates.PLAYERCHOICE)
        {
            //bool pass = this.player.UseItem(index);
            if (pass)
            {
                // start animation
                combatController.setState(CombatController.BattleStates.PLAYERANIMATE);
                waiting = true;
                StartCooldown(waiting, COOLDOWN_LENGTH);
                //this.player.inventory._items[index].used = true;
            }
        }*/
    }

    /// <summary>
    /// When button is clicked
    /// </summary>
    /// <param name="attacked">Attacked Player.</param>
    public void OnActionUsed(PawnController attacked)
    {
        // toggle movement on click
        if (!attackBarMoving)
        {
            // starts attackBar movement in updateAttackBar
            attackBarMoving = true;
            this.attackMeter.gameObject.SetActive(true);
        }
        else
        {
            // second click
            attackBarMoving = false;
            playerHasAttacked = true;
        }
        // After second click
        if (!attackBarMoving && playerHasAttacked)
        {
            // start animation
            combatController.setState(CombatController.BattleStates.PLAYERANIMATE);
            waiting = true;
            StartCooldown(waiting, COOLDOWN_LENGTH);
            this.player.Attack(attacked, attackAmount);
        }  
    }

    /// <summary>
    /// called when player animation is finished
    /// </summary>
    public void OnWaitComplete()
    {
        this.enemy.TakeDamage();
        combatController.setState(CombatController.BattleStates.ENEMYTAKEDAMAGE);
        this.enemy.GiveEnergy(ENERGY_REGEN_AMT);
        // ENEMY TURN
        combatController.setState(CombatController.BattleStates.ENEMYCHOICE);
    }

    /// <summary>
    /// Does the enemy AI Behaviour.
    /// </summary>
    /// <returns>The enemy action.</returns>
    private void DoEnemyAction()
    {
        if (!waiting && !enemyHasAttacked 
            && combatController.currentState == CombatController.BattleStates.ENEMYCHOICE)
        {
            // determine crit chance for enemy
            float attackAmount = (Random.Range(25, 75) + (Random.Range(25, 75)) / 2);

            // player/enemy alive and enemy turn
            if (!this.player.IsDead() && !this.enemy.IsDead())
            {
                int r = Random.Range(0, 10);
                //enemy health < 25% 
                if (this.enemy.remainingHealth >= (0.25 * this.enemy.totalHealth))
                {
                    //50% chance of physical vs magic
                    r = Random.Range(0, 10);
                    if (r > 5)
                    {
                        // physical
                        Debug.Log(this.enemy.name + " attacks!");
                        this.enemy.Attack(this.player, attackAmount);
                    }
                    else if (r <= 5)
                    {
                        //Magical	
                        Debug.Log(this.enemy.name + " uses a Magic Attack!");
                        bool pass = this.enemy.MagicAttack(this.player, attackAmount);
                        // if there is not enough mana to cast a magic attack
                        if (!pass)
                        {
                            //50% chance of mana potion or physical attack instead
                            r = Random.Range(0, 10);
                            if (r > 5)
                            {
                                Debug.Log("Instead of casting Magic, " + this.enemy.name + " attacks!");
                                this.enemy.Attack(this.player, attackAmount);
                            }
                            else
                            {
                                Debug.Log("Mana restored by 10");
                                this.enemy.remainingEnergy += 10;
                            }
                        }
                    }
                }
                else
                {
                    // chance to heal if enemy has potion
                    if (!enemyHasHealed)
                    {
                        Debug.Log(this.enemy.name + " healed for 25 hp");
                        this.enemy.TriggerAnimation("potion");
                        this.enemy.HealForAmount(25);
                        this.enemyHasHealed = true;
                    }
                    else
                    {
                        // no potions to heal, LAST RESORT ATTACK!
                        Debug.Log(this.enemy.name + " attacks!");
                        this.enemy.Attack(this.player, attackAmount);
                    }
                }
                this.enemy.UseEnergy(30);
                OnEnemyActionUsed(this.player);
            }
        }
    }

    /// <summary>
    /// called after AI sequence has finished.
    /// </summary>
    /// <param name="attacked">Attacked.</param>
    public void OnEnemyActionUsed(PawnController attacked)
    {
        if (combatController.currentState == CombatController.BattleStates.ENEMYCHOICE)
        {
            StartCooldown(waiting, COOLDOWN_LENGTH);
            enemyHasAttacked = true;
            
            waiting = true;
            // animate enemy
            combatController.setState(CombatController.BattleStates.ENEMYANIMATE);
        }
    }

    /// <summary>
    ///  the enemy wait complete event.
    /// </summary>
    public void OnEnemyWaitComplete()
    {
        this.player.TakeDamage();
        waiting = false;
        enemyHasAttacked = false;
        playerHasAttacked = false;
        this.player.GiveEnergy(ENERGY_REGEN_AMT);
        combatController.setState(CombatController.BattleStates.PLAYERCHOICE);
    }

    /// <summary>
    /// Starts the cooldown for specified boolean.
    /// </summary>
    /// <param name="toggle"><c>true</c>if boolean is ON cooldown.</param>
    /// <param name="time">Time.</param>
    public void StartCooldown(bool toggle, float time)
    {
        cooldownValue = time;
        toggle = !toggle;
    }

    /// <summary>
    /// Cooldown counter.
    /// </summary>
    private void Cooldown()
    {
        //Debug.Log (waiting);
        if (waiting)
        {
            this.cooldownValue -= 0.01f;
            if (this.cooldownValue <= 0)
            {
                this.cooldownValue = 0.0f;
                this.waiting = false;
                if (combatController.currentState == CombatController.BattleStates.PLAYERANIMATE){
                    combatController.setState(CombatController.BattleStates.ENEMYTAKEDAMAGE);
                    this.OnWaitComplete();
                }
                else if (combatController.currentState == CombatController.BattleStates.ENEMYANIMATE)
                {
                    combatController.setState(CombatController.BattleStates.PLAYERTAKEDAMAGE);
                    this.OnEnemyWaitComplete();
                }
            }
        }
    }


    /// <summary>
    /// Updates the UI Health/mana bars.
    /// </summary>
    public void UpdateBars()
    {
        this.playerHealth.maxValue = this.player.totalHealth;
        this.playerHealth.value = this.player.remainingHealth;
        this.playerMana.maxValue = this.player.totalEnergy;
        this.playerMana.value = this.player.remainingEnergy;
        this.enemyHealth.maxValue = this.enemy.totalHealth;
        this.enemyHealth.value = this.enemy.remainingHealth;
        this.enemyMana.maxValue = this.enemy.totalEnergy;
        this.enemyMana.value = this.enemy.remainingEnergy;
    }

        bool increasing = true;
    public void UpdateAttackBar()
    {       
        int counter = 0;
        if (attackBarMoving)
        {
            if (increasing && counter % 60 == 0)
            {
                this.attackMeter.value++;
            }
            else if (!increasing && counter % 60 == 0)
            {
                this.attackMeter.value--;
            }
            else
            {
                if (counter >= 60)
                    counter = 0;   
            }
                counter++;

            // limit the values
            if (this.attackMeter.value >= this.attackMeter.maxValue)
            {
                this.attackMeter.value = this.attackMeter.maxValue;
                increasing = false;
            }
            else if (this.attackMeter.value <= 0 )
            {
                this.attackMeter.value = 0;
                increasing = true;
            } 
            // set final value
            this.attackAmount = this.attackMeter.value;
        }
    }

    // increment counter and set local storage
    private void UpdateScore()
    {
        if (this.enemy.IsDead() && !scoredThisRound)
        {
            this.score++;
            scoredThisRound = true;
            PlayerPrefs.SetInt("score", score);
        }
    }
    /// <summary>
    /// Updates the text for HUD in battle.
    /// </summary>
    private void UpdateText()
    {
        this.leftHealthText.text = this.player.remainingHealth + "/" + this.player.totalHealth;
        this.rightHealthText.text = this.enemy.remainingHealth + "/" + this.enemy.totalHealth;

        this.leftManaText.text = this.player.remainingEnergy + "/" + this.player.totalEnergy;
        this.rightManaText.text = this.enemy.remainingEnergy + "/" + this.enemy.totalEnergy;

        this.leftArmorText.text = "AMR: " + this.player.GetTotalArmor();
        this.rightArmorText.text = "AMR: " + this.enemy.GetTotalArmor();

        this.leftDamageText.text = "DMG: " + this.player.GetTotalDamage();
        this.rightDamageText.text = "DMG: " + this.enemy.GetTotalDamage();

        this.numEnemiesKilledText.text = "SCORE: " + score;
    }



    /// <summary>
    /// Ends the game.
    /// </summary>
    private void EndGame()
    {
        // player dead
        UpdateText();
        TransferGold();
        this.player.transform.localPosition = this.prevPos;
        // animate death
        if (this.player.IsDead() && !waiting)
        {
            this.player.TriggerAnimation("death");
        }
        // check for high score
        if (PlayerPrefs.GetInt("score") > PlayerPrefs.GetInt("hiscore"))
        {
            // set new HighScore
            PlayerPrefs.SetInt("hiscore", PlayerPrefs.GetInt("score"));
        }
        // reset score/turn
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetInt("turn", 0);
        // reset difficulty
        DifficultyLevel.GetInstance().ResetDifficulty();

        if (!waiting)
        {
            // load death scene
            Application.LoadLevel("DeathScene_LVP");
        }
    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
    private void LoadNextLevel()
    {
        UpdateDisplay();
        turn = 0;
        // player turn stored in local, 0 for playerTurn
        PlayerPrefs.SetInt("turn", turn);
        // increase difficulty
        DifficultyLevel.GetInstance().IncreaseDifficulty();
        // return to prev pos
        this.player.transform.localPosition = this.prevPos;

        // VICTORY ANIMATION


        // enemy dead, fight another and keep player on screen
        DontDestroyOnLoad(this.player);
        if (!waiting)
        {
            // reload battle scene
            Application.LoadLevel("Battle_LVP");
        }
    }
    /// <summary>
    /// Loads town scene
    /// </summary>
    void GoToTown()
    {
        UpdateDisplay();
        this.player.TriggerAnimation("victory");
        this.enemy.TriggerAnimation("death");
        // VICTORY ANIMATION

        // reset position
        this.player.transform.localPosition = this.prevPos;
        // transfer money
        this.enemy.DropMoney();
        this.player.dollarBalance += this.enemy.DropMoney();
        DifficultyLevel.GetInstance().IncreaseDifficulty();

        DontDestroyOnLoad(this.player);
        if (!waiting)
        {
            Application.LoadLevel("Town_LVP");
        }
    }


    /// <summary>
    /// Transfers a portion of player's gold to next game.
    /// </summary>
    private void TransferGold()
    {
        int goldAmount = Mathf.FloorToInt((this.player.dollarBalance * MONEY_TRANSFER_PCT));

        if (goldAmount > 100)
        {
            goldAmount = 100;
        }
        PlayerPrefs.SetInt("carryover", goldAmount);
    }

    /// <summary>
    /// Updates display for user
    /// </summary>
    void UpdateDisplay()
    {
        UpdateText();
        UpdateScore();
        UpdateBars();
        UpdateAttackBar();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
        Cooldown();

        if (!waiting)
        {
            DoEnemyAction();
            if (this.player.IsDead())
            {
                // player dead
                EndGame();
            }
            else if (this.enemy.IsDead())
            {
                // enemy dead
                StartCooldown(waiting, COOLDOWN_LENGTH);
                this.enemy.TriggerAnimation("death");
                if (!waiting)
                    GoToTown();
            }
        }
    }
}
