using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public BaseEnemyController enemy;
    public GameController gameController;
    public CombatController combatController;
    public InventoryAnimation invAnim;

    private bool enemyHasHealed,
                waiting,
                scoredThisRound,
                enemyHasAttacked,
                playerHasAttacked,
                attackBarMoving,
                someoneIsDead,
                finalFrame,
                hasSelected,
                confirmed;
    private bool increasing = true;

    public int score,
               turn;
    private int PLAYER_ENERGY_REGEN_AMT,
               ENEMY_ENERGY_REGEN_AMT,
               currentBattle,
               remainingBattles;

    private float MONEY_TRANSFER_PCT = 0.2f,
                 COOLDOWN_LENGTH = 0.0f,
                 ATTACK_LENGTH,
                 MAGIC_LENGTH;

    private float attackAmount;

    private float startTime, endTime, curTime;

    public Slider playerHealth,
                  playerMana,
                  enemyHealth,
                  enemyMana;

    public Slider attackMeter;

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
    // Inventory Text
    public Text apples, bread, cheese, hPots, ePots, campKits;

    // battle counter text
    public Text battleText;

    public Button leftPhysAttack = null,
                  retreatButton = null,
                  confirmButton = null,
                  cancelButton = null,
                  inventoryToggleButton = null,
                  inventoryHandle = null;

    private Image background;

    public GameObject confirmPanel = null;

    #region Retreat
    /// <summary>
    /// when Retreat button is clicked
    /// </summary>
    public void OnRetreat()
    {
        AudioManager.Instance.PlaySFX("Select");
        // open confirm panel
        this.confirmPanel.gameObject.SetActive(true);
    }
    /// <summary>
    /// Called from ConfirmPanel
    /// </summary>
    public void Confirm()
    {
        AudioManager.Instance.PlaySFX("Select");
        confirmed = true;
        hasSelected = true;
    }
    /// <summary>
    /// Called from ConfirmPanel
    /// </summary>
    public void Cancel()
    {
        AudioManager.Instance.PlaySFX("Select");
        confirmed = false;
        hasSelected = true;
    }
    #endregion retreat
    #region Battle
    #region Player Choice
    /// <summary>
    /// When button is clicked
    /// </summary>
    /// <param name="attacked">Attacked Player.</param>
    public void OnActionUsed(PawnController attacked)
    {
        AudioManager.Instance.PlaySFX("Select");
        // close inv if open
        if (invAnim.open)
            invAnim.OpenClose();

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
            // apply damage amount from attack meter
            attackAmount = attackMeter.value;
            // apply the percent damage from the bar (value/max)
            int damageToApply = Mathf.RoundToInt((attackMeter.value / attackMeter.maxValue) * this.player.physicalDamage);
            // regenerate the reciprocal ((max - value) / max) of the energy used on attack
            PLAYER_ENERGY_REGEN_AMT = Mathf.RoundToInt( ((attackMeter.maxValue - attackMeter.value) / attackMeter.maxValue) * this.player.ATTACK_ENERGY_COST );
            // start animation
            combatController.setState(CombatController.BattleStates.PLAYERANIMATE);
            StartCooldown(COOLDOWN_LENGTH);
            this.player.Attack(attacked, damageToApply);
            //attackMeter.value = Random.Range(0, attackMeter.maxValue);
        }
    }

    /// <summary>
    /// called when player animation is finished
    /// </summary>
    public void OnWaitComplete()
    {
        this.enemy.TakeDamage();
        this.enemy.GiveEnergyAmount(ENEMY_ENERGY_REGEN_AMT);
        // ENEMY TURN
        combatController.setState(CombatController.BattleStates.ENEMYCHOICE);
    }
    #endregion Player Choice
    #region Enemy Choice
    /// <summary>
    /// Does the enemy AI Behaviour.
    /// </summary>
    /// <returns>The enemy action.</returns>
    private void DoEnemyAction()
    {
        float cdReq = COOLDOWN_LENGTH * MAGIC_LENGTH;

        if (!waiting && !enemyHasAttacked
            && combatController.currentState == CombatController.BattleStates.ENEMYCHOICE)
        {
            // get enemy % (int)
            float attackAmount = (Random.Range(25, 75) + (Random.Range(25, 75)) / 2);
            // reciprocal for energy regen
            float regenAmt = 100f - attackAmount;
            //attack %
            attackAmount /= 100f;
            //Energy Amount regen
            regenAmt = (regenAmt / 100f) * this.enemy.ATTACK_ENERGY_COST;
            ENEMY_ENERGY_REGEN_AMT = Mathf.RoundToInt(regenAmt);

            Debug.Log("regen enemy: " + regenAmt);
            // multiply damage by reduction factor
            int damageToApply = Mathf.RoundToInt(attackAmount * this.enemy.physicalDamage);
            Debug.Log("Enemy bar at " + attackAmount + " Damage to apply: " + damageToApply);

            // player/enemy alive and enemy turn
            if (!this.player.IsDead() && !this.enemy.IsDead())
            {
                //50% chance of physical vs magic
                int r = Random.Range(0, 10);
                if (r > 1)
                {
                    // physical
                    Debug.Log(this.enemy.name + " attacks!");
                    cdReq = COOLDOWN_LENGTH * ATTACK_LENGTH;
                    this.enemy.Attack(this.player, damageToApply);
                }
                else
                { //enemy health < 25% 
                    // chance to heal if enemy has potion
                    if (!enemyHasHealed)
                    {
                        Debug.Log(this.enemy.name + " healed for 25 hp");
                        this.enemy.TriggerAnimation("HealPotion");
                        this.enemy.HealForAmount(25);
                        this.enemyHasHealed = true;
                    }
                    else
                    {
                        // no potions to heal, LAST RESORT ATTACK!
                        Debug.Log(this.enemy.name + " attacks!");
                        cdReq = COOLDOWN_LENGTH * ATTACK_LENGTH;
                        this.enemy.Attack(this.player, damageToApply);
                    }
                }
                OnEnemyActionUsed(this.player, cdReq);
            }// end if someone is dead            
        }
    }

    /// <summary>
    /// called after AI sequence has finished.
    /// </summary>
    /// <param name="attacked">Attacked.</param>
    /// <param name="requiredCooldownLength">Cooldown length needed for animation.</param>
    public void OnEnemyActionUsed(PawnController attacked, float requiredCooldownLength)
    {
        if (combatController.currentState == CombatController.BattleStates.ENEMYCHOICE)
        {
            // waiting = true;
            enemyHasAttacked = true;
            StartCooldown(requiredCooldownLength);
            //this.enemy.UseEnergy(30);
            // animate enemy
            combatController.setState(CombatController.BattleStates.ENEMYANIMATE);
        }
    }

    /// <summary>
    ///  the enemy wait complete event.
    /// </summary>
    public void OnEnemyWaitComplete()
    {
        combatController.setState(CombatController.BattleStates.PLAYERANIMATE);
        this.player.TakeDamage();
        this.player.GiveEnergyAmount(PLAYER_ENERGY_REGEN_AMT);
        enemyHasAttacked = false;
        playerHasAttacked = false;
        combatController.setState(CombatController.BattleStates.PLAYERCHOICE);
        if (!this.enemy.IsDead())
            waiting = false;
    }

    #endregion Enemy Choice
    #region Cooldown logic
    /// <summary>
    /// Starts the cooldown for specified boolean.
    /// </summary>
    /// <param name="toggle"><c>true</c>if boolean is ON cooldown.</param>
    /// <param name="time">Time.</param>
    public void StartCooldown(float amount)
    {
        startTime = curTime;
        endTime = startTime + amount;
        waiting = true;
    }

    /// <summary>
    /// Cooldown counter.
    /// </summary>
    private void Cooldown()
    {
        if (waiting)
        {
            if (curTime >= endTime)
            {
                this.endTime = curTime;
                this.waiting = false;

                if (!someoneIsDead)
                {
                    if (combatController.currentState == CombatController.BattleStates.PLAYERANIMATE)
                    {
                        combatController.setState(CombatController.BattleStates.ENEMYTAKEDAMAGE);
                        this.OnWaitComplete();
                    }
                    else if (combatController.currentState == CombatController.BattleStates.ENEMYANIMATE)
                    {
                        combatController.setState(CombatController.BattleStates.PLAYERTAKEDAMAGE);
                        this.OnEnemyWaitComplete();
                    }
                }
                else
                {
                    // do when someone is dead
                    if (!finalFrame)
                    {
                        finalFrame = true;
                        if (this.enemy.IsDead())
                        {
                            this.player.TriggerAnimation("victory");
                            this.enemy.TriggerAnimation("death");
                            StartCooldown(COOLDOWN_LENGTH);
                            DisableButtons();
                            //PlayerVictory();
                        }

                        if (this.player.IsDead())
                        {
                            this.enemy.TriggerAnimation("victory");
                            this.player.TriggerAnimation("death");
                            DisableButtons();
                            StartCooldown(COOLDOWN_LENGTH);
                            //EndGame();
                        }
                    }
                    else
                    {
                        if (this.player.IsDead())
                        {
                            EndGame();
                        }
                        else if (this.enemy.IsDead())
                        {
                            PlayerVictory();
                        }
                    }
                }
            }
        }
    }
    #endregion Cooldown Logic
    #endregion Battle
    #region Item Use

    /// <summary>
    /// Player Uses the item in inventory specified at index, ends turn.
    /// </summary>
    /// <param name="index">Index.</param>
    public void UseItem(int index)
    {
        bool pass = false;
        switch (index)
        {
            case 0:
                pass = this.player.inventory.EatFood("Apple");
                break;
            case 1:
                pass = this.player.inventory.EatFood("Bread");
                break;
            case 2:
                pass = this.player.inventory.EatFood("Cheese");
                break;
            case 3:
                pass = this.player.inventory.UsePotion("Health");
                break;
            case 4:
                pass = this.player.inventory.UsePotion("Energy");
                break;
            case 5:
                //this.player.inventory.UseCampKit();
                Debug.Log("Now is not the time to use that!");
                break;
            default:
                break;
        }
        if (pass)
        {
            invAnim.OpenClose();
            combatController.setState(CombatController.BattleStates.PLAYERANIMATE);
            StartCooldown(COOLDOWN_LENGTH);
        }
    }

    #endregion Item Use
    #region UI
    /// <summary>
    /// Disables the buttons.
    /// </summary>
    public void DisableButtons()
    {
        this.leftPhysAttack.gameObject.SetActive(false);
        this.attackMeter.gameObject.SetActive(false);
        this.retreatButton.gameObject.SetActive(false);
        this.inventoryToggleButton.gameObject.SetActive(false);
        this.inventoryHandle.gameObject.SetActive(false);
        this.inventoryToggleButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// Enables the buttons.
    /// </summary>
    public void EnableButtons()
    {
        this.leftPhysAttack.gameObject.SetActive(true);
        this.retreatButton.gameObject.SetActive(true);
        this.inventoryToggleButton.gameObject.SetActive(true);
        this.inventoryHandle.gameObject.SetActive(true);
        this.inventoryToggleButton.gameObject.SetActive(true);
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
            else if (this.attackMeter.value <= 0)
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
            PlayerPrefs.SetInt("score", score);
            scoredThisRound = true;
        }
    }
    /// <summary>
    /// Updates the text for HUD in battle.
    /// </summary>
    private void UpdateText()
    {
        // Battle stats
        this.leftHealthText.text = this.player.remainingHealth + "/" + this.player.totalHealth;
        this.rightHealthText.text = this.enemy.remainingHealth + "/" + this.enemy.totalHealth;
        this.leftManaText.text = this.player.remainingEnergy + "/" + this.player.totalEnergy;
        this.rightManaText.text = this.enemy.remainingEnergy + "/" + this.enemy.totalEnergy;
        this.leftArmorText.text = this.player.GetTotalArmor().ToString();
        this.rightArmorText.text = this.enemy.GetTotalArmor().ToString();
        this.leftDamageText.text = this.player.GetTotalDamage().ToString();
        this.rightDamageText.text = this.enemy.GetTotalDamage().ToString();

        // score stat
        this.numEnemiesKilledText.text = score.ToString();

        // battles stat
        this.battleText.text = currentBattle + "/" + DifficultyLevel.GetInstance().GetDifficultyMultiplier() + " Battles";
        // inventory text
        this.apples.text = this.player.inventory.Apples.ToString();
        this.bread.text = this.player.inventory.Bread.ToString();
        this.cheese.text = this.player.inventory.Cheese.ToString();
        this.hPots.text = this.player.inventory.HealthPotions.ToString();
        this.ePots.text = this.player.inventory.EnergyPotions.ToString();
        this.campKits.text = this.player.inventory.CampKits.ToString();
    }
    /// <summary>
    /// Handles Retreat UI ("confirm Panel")
    /// </summary>
    private void UpdateConfirmPanel()
    {
        if (hasSelected && confirmed)
        {
            RetreatFromBattle();
        }
        else if (hasSelected && !confirmed)
        {
            // close panel
            this.confirmPanel.gameObject.SetActive(false);
            hasSelected = false;
        }
    }

    /// <summary>
    /// Updates UI
    /// </summary>
    void UpdateDisplay()
    {
        UpdateText();
        UpdateScore();
        UpdateBars();
        UpdateAttackBar();
        UpdateConfirmPanel();
    }

    #endregion UI
    #region level loading
    /// <summary>
    /// Ends the game.
    /// </summary>
    private void EndGame()
    {
        // player dead
        TransferGold();
        this.player.transform.localPosition = this.prevPos;

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
        BattleCounter.GetInstance().ResetCurrentBattleCount();
        BattleCounter.GetInstance().ResetBattlesNeeded();

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
        // VICTORY ANIMATION
        turn = 0;
        // player turn stored in local, 0 for playerTurn
        PlayerPrefs.SetInt("turn", turn);
        // enemy dead, fight another and keep player on screen
        DontDestroyOnLoad(this.player);
        this.player.dollarBalance += this.enemy.DropMoney();
        if (!waiting)
        {
            // restore player mana after battle
            this.player.remainingEnergy = this.player.totalEnergy;
            // reload battle scene
            Application.LoadLevel("Battle_LVP");
        }
    }

    /// <summary>
    /// Player chooses to retreat, no rewards from battle. Reload Town
    /// </summary>
    private void RetreatFromBattle()
    {
        this.player.transform.localPosition = this.prevPos;
        DontDestroyOnLoad(this.player);
        BattleCounter.GetInstance().ResetCurrentBattleCount();
        if (!waiting)
        {
            // restore player mana after battle
            this.player.remainingEnergy = this.player.totalEnergy;
            Application.LoadLevel("Town_LVP");
        }
    }

    /// <summary>
    /// Loads town scene
    /// </summary>
    private void GoToTown()
    {
        // reset position
        this.player.transform.localPosition = this.prevPos;
        // transfer money
        // this.enemy.DropMoney();
        this.player.dollarBalance += this.enemy.DropMoney();

        DontDestroyOnLoad(this.player);
        if (!waiting)
        {
            Debug.Log("Loading Town Scene");
            // restore player energy after battle
            this.player.remainingEnergy = this.player.totalEnergy;
            Application.LoadLevel("Town_LVP");
        }
    }

    private void LoadStatSelect()
    {
        DontDestroyOnLoad(this.player);
        if (!waiting)
        {
            Debug.Log("Loading Stat Scene");
            // restore player mana after battle
            this.player.remainingEnergy = this.player.totalEnergy;
            //Give player extra 10 health
            this.player.totalHealth += 10;
            this.player.remainingHealth += 10;
            this.player.dollarBalance += this.enemy.DropMoney();
            this.player.transform.localPosition = this.prevPos;
            PlayerPrefs.SetInt("midgame", 1);
            Application.LoadLevel("MidGameStatSelect_LVP");
        }
    }

    private void TransferGold()
    {
        int goldAmount = Mathf.FloorToInt((this.player.dollarBalance * MONEY_TRANSFER_PCT));

        if (goldAmount > 100)
        {
            goldAmount = 100;
        }
        PlayerPrefs.SetInt("carryover", goldAmount);
    }

    void PlayerVictory()
    {
        Debug.Log("Winner");
        // increase number of battles until level up
        BattleCounter.GetInstance().IncreaseCurrentBattleCount();
        // if no more battles
        if (BattleCounter.GetInstance().GetRemainingBattles() <= 0)
        {
            Debug.Log("Increasing Difficulty");
            // increase difficulty
            DifficultyLevel.GetInstance().IncreaseDifficulty();
            // increase number of battles to difficulty level
            BattleCounter.GetInstance().SetBattlesNeeded(DifficultyLevel.GetInstance().GetDifficultyMultiplier());
            // start back from 0 battles
            BattleCounter.GetInstance().ResetCurrentBattleCount();

            //StartCooldown(COOLDOWN_LENGTH);

            // go to town, no more battles to be fought this round
            //GoToTown();
            LoadStatSelect();
            // Stat Select?


        }
        else
        {
            Debug.Log("On to the next battle!");
            // check to see if player wants to use a camp kit (only if they have one!)
            if (this.player.inventory.CampKits > 0)
            {
                // check if player wants to camp out before next battle
                // load camp kit check scene?
            }
            LoadNextLevel();
        }
    }

    void CheckDeath()
    {
        if (this.player.IsDead() || this.enemy.IsDead())
            someoneIsDead = true;
        else
            someoneIsDead = false;
    }

    #endregion Misc. Game functionality methods
    #region monobehaviour
    // Use this for initialization
    void Start()
    {
        AudioManager.Instance.PlayNewSong("ForestBattleMusic");
        EscapeHandler.instance.GetButtons();
        // Combat AI Controller reference
        this.combatController = FindObjectOfType<CombatController>();
        // inventory animator
        this.invAnim = FindObjectOfType<InventoryAnimation>();
        // INITIALIZE BATTLE SCENE
        combatController.setState(CombatController.BattleStates.START);

        this.confirmPanel.gameObject.SetActive(false);

        this.turn = 0;
        currentBattle = BattleCounter.GetInstance().GetCurrentBattleCount();
        remainingBattles = BattleCounter.GetInstance().GetRemainingBattles();
        PlayerPrefs.SetInt("turn", turn);
        this.scoredThisRound = false;
        this.score = PlayerPrefs.GetInt("score");

        COOLDOWN_LENGTH = 2.0f;
        ATTACK_LENGTH = 1.85f;
        MAGIC_LENGTH = 1.5f;

        someoneIsDead = false;
        finalFrame = false;
        hasSelected = false;

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
        Vector3 newSpot = new Vector3(-4.5f, -2.5f);
        this.player.gameObject.transform.localPosition = newSpot;

        // get enemy reference
        this.enemy = FindObjectOfType<BaseEnemyController>();

        // Set Attack Meter Amount
        this.attackMeter.maxValue = 100;
        //this.attackMeter.value = (float)Random.Range(0, this.attackMeter.maxValue);
        this.attackMeter.gameObject.SetActive(false);

        // combat starts after initialization is finished
        combatController.setState(CombatController.BattleStates.PLAYERCHOICE);
    }

    // Update is called once per frame
    void Update()
    {
        curTime = Time.time;
        UpdateDisplay();
        Cooldown();
        CheckDeath();

        if (!waiting)
        {
            DoEnemyAction();
            if (someoneIsDead && !finalFrame)
            {
                StartCooldown(COOLDOWN_LENGTH * 1.5f);
            }
        }
    }
}
    #endregion monoBehaviour
