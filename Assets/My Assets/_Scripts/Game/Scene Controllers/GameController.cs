using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public BaseEnemyController enemy;
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
    private bool increasing = true,
                 playerHasEatenFoodThisTurn = false;

    public int score,
               turn;
               
    public float barDelta, BAR_SPEED = 4.0f;

    private int PLAYER_ENERGY_REGEN_AMT,
                PLAYER_ENERGY_COST_AMT,
                ENEMY_ENERGY_REGEN_AMT,
                ENEMY_ENERGY_COST_AMT,
                currentBattle,
                remainingBattles;

    private float MONEY_TRANSFER_PCT = 0.2f,
                  COOLDOWN_LENGTH = 2.0f;

    private float attackAmount,
                  startTime,
                  endTime,
                  curTime,
                  timeVal = 2.0f;

    public Slider playerHealth,
                  playerMana,
                  enemyHealth,
                  enemyMana;

    public Slider attackMeter;
    public Button cancelAttack;

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
                healTurnsRemainText;
    // Inventory Text
    public Text apples, bread, cheese, hPots, ePots;

    // battle counter text
    public Text battleText;

    public Button leftPhysAttack = null,
                  retreatButton = null,
                  confirmButton = null,
                  cancelButton = null,
                  inventoryToggleButton = null;

    public GameObject confirmPanel = null;

    public GameObject[] pulsingButtons;

    private BackgroundManager _backgroundManager;

    #region Retreat
    /// <summary>
    /// when Retreat button is clicked
    /// </summary>
    public void OnRetreat()
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        // open confirm panel
        this.confirmPanel.gameObject.SetActive(true);
    }
    /// <summary>
    /// Called from ConfirmPanel
    /// </summary>
    public void Confirm()
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        confirmed = true;
        hasSelected = true;
    }
    /// <summary>
    /// Called from ConfirmPanel
    /// </summary>
    public void Cancel()
    {
        AudioManager.Instance.PlaySFX("Return");
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
        AudioManager.Instance.PlaySFX("SelectLarge");
        // close inv if open
        if (invAnim.open)
            invAnim.OpenClose();

        // toggle movement on click
        if (!attackBarMoving)
        {
            // starts attackBar movement in updateAttackBar
            attackBarMoving = true;
            this.attackMeter.gameObject.SetActive(true);
            this.attackMeter.interactable = false;
            this.cancelAttack.gameObject.SetActive(true);
        }
        else
        {
            // second click
            attackBarMoving = false;
            playerHasAttacked = true;
            for (int i = 0; i < 2; i++)
            {
                if (pulsingButtons[i].GetComponent<ImagePulse>() == true)
                {
                    pulsingButtons[i].GetComponent<ImagePulse>().PulseOff();
                }
                else if (pulsingButtons[i].GetComponent<SpritePulse>() == true)
                {
                    pulsingButtons[i].GetComponent<SpritePulse>().PulseOff();
                }
            }

        }
        // After second click
        if (!attackBarMoving && playerHasAttacked)
        {
            // apply damage amount from attack meter
            attackAmount = attackMeter.value;

            // (remainingNRG/totalNRG) is the factor of damage reduction to be applied 
            // when there is less than the required amount of energy
            float NRGReductionFactor = (float)this.player.remainingEnergy / (float)this.player.ATTACK_ENERGY_COST;

            NRGReductionFactor = Mathf.Clamp(NRGReductionFactor, 0.0f, 1.0f);
            // apply the percent damage from the bar (value/max) * NRGFactor
            float damageToApply = NRGReductionFactor * (attackAmount / attackMeter.maxValue) * (float)this.player.GetTotalDamage();

            // regenerate the reciprocal ((max - value) / max) of the energy used on attack
            PLAYER_ENERGY_REGEN_AMT = Mathf.RoundToInt(
                                    ((attackMeter.maxValue - attackMeter.value) / attackMeter.maxValue) * this.player.ATTACK_ENERGY_COST
                                    ) ;
            // energy cost is the remainder of the total cost - regen (divided by 2, because damage is limited to 50% output)
            PLAYER_ENERGY_COST_AMT = Mathf.RoundToInt(
                                    (this.player.ATTACK_ENERGY_COST - PLAYER_ENERGY_REGEN_AMT)
                                    ) / 2;


            // start animation
            combatController.setState(CombatController.BattleStates.PLAYERANIMATE);
            StartCooldown(COOLDOWN_LENGTH);
			if (damageToApply > 0)
				this.player.Attack(attacked, Mathf.RoundToInt(damageToApply), PLAYER_ENERGY_COST_AMT);
			else
				this.player.TriggerAnimation("healmagic");


            // set bar to a random position for the next attack
            attackMeter.value = Random.Range(0, attackMeter.maxValue);
            this.turn = 1;
        }
    }

    public void CancelAttack()
    {
        attackBarMoving = false;
        attackMeter.value = Random.Range(0, attackMeter.maxValue);
        this.attackMeter.gameObject.SetActive(false);
        this.cancelAttack.gameObject.SetActive(false);
        for (int i = 0; i < 2; i++)
        {
            if (pulsingButtons[i].GetComponent<ImagePulse>() == true)
            {
                pulsingButtons[i].GetComponent<ImagePulse>().PulseOn();
            }
            else if (pulsingButtons[i].GetComponent<SpritePulse>() == true)
            {
                pulsingButtons[i].GetComponent<SpritePulse>().PulseOn();
            }
        }
    }

    /// <summary>
    /// called when player animation is finished
    /// </summary>
    public void OnWaitComplete()
    {
        this.enemy.TakeDamage();
        Invoke("EnemyRegen", timeVal);
        // ENEMY TURN
    }
    /// <summary>
    /// Invokable call to delay stat regen
    /// </summary>
    public void PlayerRegen()
    {
        // energy regen
        this.player.GiveEnergyAmount(PLAYER_ENERGY_REGEN_AMT);

        // turn-based heal for food
        if (this.player.numTurnsLeftToHeal > 0)
        {
            this.player.GiveEnergyPercent(this.player.inventory.PercentToRegenPerTurn);
            this.player.GiveHealthPercent(this.player.inventory.PercentToRegenPerTurn);
            if (this.player.numTurnsLeftToHeal <= 1)
            {
                this.player.numTurnsLeftToHeal = 0;
            }
            else
            {
                this.player.numTurnsLeftToHeal--;
            }
        }
    }

    #endregion Player Choice
    #region Enemy Choice
    /// <summary>
    /// Does the enemy AI Behaviour.
    /// </summary>
    /// <returns>The enemy action.</returns>
    private void DoEnemyAction()
    {
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
            ENEMY_ENERGY_COST_AMT = this.enemy.ATTACK_ENERGY_COST - ENEMY_ENERGY_REGEN_AMT;
            // multiply damage by reduction factor
            int damageToApply = Mathf.RoundToInt(attackAmount * this.enemy.GetTotalDamage());

            // player/enemy alive and enemy turn
            if (!this.player.IsDead() && !this.enemy.IsDead())
            {
                int r = Random.Range(0, 10);
                if (r > 1)  // 90%chance to attack
                {
                    // physical
                    //cdReq = COOLDOWN_LENGTH * ATTACK_LENGTH;
                    this.enemy.Attack(this.player, damageToApply, ENEMY_ENERGY_COST_AMT);
                }
                else
                {   // 10%chance to heal, only if nrg/hp <30%
                    if ((this.enemy.remainingEnergy < Mathf.RoundToInt((0.3f * this.enemy.totalEnergy))      // <30%nrg
                     || this.enemy.remainingHealth < Mathf.RoundToInt((0.3f * this.enemy.totalHealth)))      // <30%hp
                        && !enemyHasHealed)                                                                 // hasn't healed yet this round
                    {
                        r = Random.Range(0, 2);
                        if (r == 0)
                        {
                            Debug.Log("Enemy uses a Health Potion. Heals 50% Health");
                            // Healthregen (r=0)
                            this.enemy.TriggerAnimation("HealPotion");
                            this.enemy.GiveHealthPercent(50);
                            this.enemyHasHealed = true;
                        }
                        else if (r == 1)
                        {
                            Debug.Log("Enemy uses a Energy Potion. Heals 50% Energy.");
                            // Energy Regen (r=1)
                            this.enemy.TriggerAnimation("HealPotion");
                            this.enemy.GiveEnergyPercent(50);
                            this.enemyHasHealed = true;
                        }
                        else if (r == 2)
                        {
                            Debug.Log("Enemy uses a Health Potion. Heals 25% Health and Energy");
                            // Health and energy regen (r=2)
                            this.enemy.TriggerAnimation("HealPotion");
                            this.enemy.GiveHealthPercent(25);
                            this.enemy.GiveEnergyPercent(25);
                            this.enemyHasHealed = true;
                        }
                    }
                    else
                    {
                        this.enemy.Attack(this.player, damageToApply, ENEMY_ENERGY_COST_AMT);
                    }
                }
                OnEnemyActionUsed(this.player, COOLDOWN_LENGTH);
            }
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
            enemyHasAttacked = true;
            StartCooldown(requiredCooldownLength);
            combatController.setState(CombatController.BattleStates.ENEMYANIMATE);
        }
    }

    /// <summary>
    ///  the enemy wait complete event.
    /// </summary>
    public void OnEnemyWaitComplete()
    {
        this.player.TakeDamage();
        combatController.setState(CombatController.BattleStates.PLAYERANIMATE);

        if (this.player.remainingHealth > 0){
            Invoke("PlayerRegen", (timeVal));
            this.combatController.setState(CombatController.BattleStates.PLAYERCHOICE);
            this.turn = 0;
            this.playerHasEatenFoodThisTurn = false;
            this.enemyHasAttacked = false;
            this.playerHasAttacked = false;
        }
        else
        {
            this.combatController.setState(CombatController.BattleStates.LOSE);
            if (!this.enemy.IsDead())
                waiting = false;
        }
        for (int i = 0; i < 2; i++)
        {
            if (pulsingButtons[i].GetComponent<ImagePulse>() == true)
            {
                pulsingButtons[i].GetComponent<ImagePulse>().PulseOn();
            }
            else if (pulsingButtons[i].GetComponent<SpritePulse>() == true)
            {
                Invoke("PulseNow",2f);
            }
        }
    }

    private void PulseNow()
    {
        pulsingButtons[0].GetComponent<SpritePulse>().PulseOn();
    }

    /// <summary>
    /// Invokable call to delay Enemy Regen
    /// </summary>
    public void EnemyRegen()
    {
        this.enemy.GiveEnergyAmount(ENEMY_ENERGY_REGEN_AMT);
        combatController.setState(CombatController.BattleStates.ENEMYCHOICE);
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
                if (!this.playerHasEatenFoodThisTurn)
                {
                    pass = this.player.inventory.EatFood("Apple");
                    this.playerHasEatenFoodThisTurn = true;
                }
                break;
            case 1:
                if (!this.playerHasEatenFoodThisTurn)
                {
                    pass = this.player.inventory.EatFood("Bread");
                    this.playerHasEatenFoodThisTurn = true;
                }
                break;
            case 2:
                if (!this.playerHasEatenFoodThisTurn)
                {
                    pass = this.player.inventory.EatFood("Cheese");
                    this.playerHasEatenFoodThisTurn = true;
                }
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

        // toggle inventoryAnim after each use
        invAnim.OpenClose();

        if (pass && index >= 3) // only potions will consume a turn
        {
            combatController.setState(CombatController.BattleStates.PLAYERANIMATE);
            StartCooldown(COOLDOWN_LENGTH);
        }
    }

    #endregion Item Use
    #region buttons 
    /// <summary>
    /// Disables the buttons.
    /// </summary>
    public void DisableButtons()
    {
        this.leftPhysAttack.gameObject.SetActive(false);
        this.attackMeter.gameObject.SetActive(false);
        this.cancelAttack.gameObject.SetActive(false);
        this.retreatButton.gameObject.SetActive(false);
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
    }
    #endregion buttons
    #region CalledOnTheTick
    /// <summary>
    /// Updates the UI Health/mana bars.
    /// </summary>
    public void UpdateBars()
    {
        this.playerHealth.value = this.player.remainingHealth;
        if (((float)this.player.remainingHealth / (float)this.player.totalHealth) <= 0.25f)
        {
            pulsingButtons[2].GetComponent<ImagePulse>().PulseOn();
        }
        else
        {
            pulsingButtons[2].GetComponent<ImagePulse>().PulseOff();
        }
        this.playerMana.value = this.player.remainingEnergy;
        if (((float)this.player.remainingEnergy / (float)this.player.totalEnergy) <= 0.25f)
        {
            pulsingButtons[3].GetComponent<ImagePulse>().PulseOn();
        }
        else
        {
            pulsingButtons[3].GetComponent<ImagePulse>().PulseOff();
        }
        this.enemyHealth.value = this.enemy.remainingHealth;
        this.enemyMana.value = this.enemy.remainingEnergy;
    }
    /// <summary>
    /// Handles logic for attack Bar.
    /// </summary>
    public void UpdateAttackBar()
    {
        if (attackBarMoving)
        {
            if (increasing )
            {
                this.attackMeter.value += barDelta;
            }
            else if (!increasing )
            {
                this.attackMeter.value -= barDelta;
            }
            
            // limit the values
            if (this.attackMeter.value >= this.attackMeter.maxValue)
            {
                this.attackMeter.value = this.attackMeter.maxValue;
                increasing = false;
            }
            else if (this.attackMeter.value <= attackMeter.minValue)
            {
                this.attackMeter.value = attackMeter.minValue;
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
        // Battle stats (Top UI)
        this.leftHealthText.text = this.player.remainingHealth + " / " + this.player.totalHealth;
        this.rightHealthText.text = this.enemy.remainingHealth + " / " + this.enemy.totalHealth;
        this.leftManaText.text = this.player.remainingEnergy + " / " + this.player.totalEnergy;
        this.rightManaText.text = this.enemy.remainingEnergy + " / " + this.enemy.totalEnergy;
        this.leftArmorText.text = this.player.GetTotalArmor().ToString();
        this.rightArmorText.text = this.enemy.GetTotalArmor().ToString();
        this.leftDamageText.text = this.player.GetTotalDamage().ToString();
        this.rightDamageText.text = this.enemy.GetTotalDamage().ToString();
        this.healTurnsRemainText.text = this.player.numTurnsLeftToHeal.ToString();

        // score stat
        //this.numEnemiesKilledText.text = score.ToString();

        // battles stat
        this.battleText.text = (currentBattle + 1) + " / " + DifficultyLevel.GetInstance().GetDifficultyMultiplier();
        if (this.inventoryToggleButton.isActiveAndEnabled)
        {
            // inventory text
            this.apples.text = this.player.inventory.Apples.ToString();
            this.bread.text = this.player.inventory.Bread.ToString();
            this.cheese.text = this.player.inventory.Cheese.ToString();
            this.hPots.text = this.player.inventory.HealthPotions.ToString();
            this.ePots.text = this.player.inventory.EnergyPotions.ToString();
            //this.campKits.text = this.player.inventory.CampKits.ToString();
        }
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

    private void UpdateInventory()
    {
        if (this.turn == 0)
        {
            this.inventoryToggleButton.gameObject.SetActive(true);
        }
        else
        {
            this.inventoryToggleButton.gameObject.SetActive(false);
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
        UpdateInventory();
    }

    /// <summary>
    /// runs a test for a dead player/enemy
    /// </summary>
    void CheckDeath()
    {
        if (this.player.IsDead() || this.enemy.IsDead())
        {
            someoneIsDead = true;
            if (this.player.IsDead())
            {
                //Debug.Log("Player Dead!!");
                this.player.numTurnsLeftToHeal = 0;
                DisableButtons();
                foreach (SpriteRenderer renderer in this.player.playerWeapon.gameObject.GetComponentsInChildren<SpriteRenderer>())
                {
                    renderer.gameObject.SetActive(false);
                }
            }
        }
        else
            someoneIsDead = false;
    }

    #endregion CalledOnTheTick
    #region level loading
    /// <summary>
    /// Ends the game.
    /// </summary>
    private void EndGame()
    {
        // player dead
        TransferGoldOnDeath();
      //  this.player.transform.localPosition = this.prevPos;

        // check for high score
        if (PlayerPrefs.GetInt("score") > PlayerPrefs.GetInt("hiscore"))
        {
            // set new HighScore
            PlayerPrefs.SetInt("hiscore", PlayerPrefs.GetInt("score"));
        }
        // reset score/turn
        PlayerPrefs.SetInt("turn", 0);
        // reset difficulty
        DifficultyLevel.GetInstance().ResetDifficulty();
        BattleCounter.GetInstance().ResetCurrentBattleCount();
        BattleCounter.GetInstance().ResetBattlesNeeded();
        // Set the idle animation to town idle
        player.playerAnimator.SetBool("InBattle", false);

        if (!waiting)
        {
            // load death scene
            LevelLoadHandler.Instance.LoadLevel("DeathScene_LVP", false);
        }
    }

    /// <summary>
    /// Loads the next level.
    /// </summary>
    private void LoadNextBattle()
    {
        // enemy dead, fight another and keep player on screen
        //DontDestroyOnLoad(this.player);
        this.player.dollarBalance += this.enemy.DropMoney();
        if (!waiting)
        {
            // restore player mana after battle
            this.player.remainingEnergy = this.player.totalEnergy;
            // reload battle scene
            LevelLoadHandler.Instance.LoadLevel("Battle_LVP", false);
        }
    }

    /// <summary>
    /// Player chooses to retreat, no rewards from battle. Reload Town
    /// </summary>
    private void RetreatFromBattle()
    {
        //this.player.transform.localPosition = this.prevPos;
        //BattleCounter.GetInstance().ResetCurrentBattleCount(); /// Commented out by Carlo, I think this is to keep the count even at a retreat.
        // Set the idle animation to town idle
        player.InBattle(false);
        PlayerPrefs.SetInt("retreated", 1);
        if (!waiting)
        {
            Debug.Log("Enemy Stats need to be reset to full");
            SaveSystemHandler.instance.enemyLoadedFromFile = false;
            //EnemyStats.GetInstance().GetEnemyBaseStats(this.enemy, this.player);
            EnemyStats.GetInstance().ResetCheckpoint();
            EnemyStats.GetInstance().SetLastFoughtEnemyStatString(this.enemy.GetEnemyStatString());
            // restore player mana after battle
            this.player.remainingEnergy = this.player.totalEnergy;
            this.player.inBattle = false;
            LevelLoadHandler.Instance.LoadLevel("Town_LVP", false);
        }
    }

    /// <summary>
    /// Loads town scene on victory
    /// </summary>
    private void GoToTown()
    {
        // reset position
        //this.player.transform.localPosition = this.prevPos;
        // transfer money
        // this.enemy.DropMoney();
        this.player.dollarBalance += this.enemy.DropMoney();
        if (!waiting)
        {
            Debug.Log("Loading Town Scene");
            // restore player energy after battle
            this.player.remainingEnergy = this.player.totalEnergy;
            this.player.inBattle = false;
            LevelLoadHandler.Instance.LoadLevel("Town_LVP", false);
        }
    }

    /// <summary>
    /// Loads Stat Select Scene
    /// </summary>
    private void LoadStatSelect()
    {
        if (!waiting)
        {
            //Debug.Log("Loading Stat Scene");
            // restore player mana after battle
            this.player.remainingEnergy = this.player.totalEnergy;
            //Give player extra 10 health
            this.player.totalHealth += 10;
            this.player.remainingHealth += 10;
            this.player.dollarBalance += this.enemy.DropMoney();
          //  this.player.transform.localPosition = this.prevPos;
            this.player.posController.MovePlayer(-30, -37);
            this.player.inBattle = false;
            PlayerPrefs.SetInt("midgame", 1);
            LevelLoadHandler.Instance.LoadLevel("MidGameStatSelect_LVP", false);
        }
    }

    /// <summary>
    /// transfers player gold after death
    /// </summary>
    private void TransferGoldOnDeath()
    {
        int goldAmount = Mathf.FloorToInt((this.player.dollarBalance * MONEY_TRANSFER_PCT));

        if (goldAmount > 100)
        {
            goldAmount = 100;
        }
        PlayerPrefs.SetInt("carryover", goldAmount);
    }

    /// <summary>
    /// Called on player victory to decide what comes next
    /// </summary>
    void PlayerVictory()
    {
        //Debug.Log("Winner");
        // increase number of battles until level up
        BattleCounter.GetInstance().IncreaseCurrentBattleCount();
        // if no more battles
        if (BattleCounter.GetInstance().GetRemainingBattles() <= 0)
        {
            //Debug.Log("Increasing Difficulty");
            // increase difficulty
            DifficultyLevel.GetInstance().IncreaseDifficulty();
            // increase number of battles to difficulty level
            BattleCounter.GetInstance().SetBattlesNeeded(DifficultyLevel.GetInstance().GetDifficultyMultiplier());
            // start back from 0 battles
            BattleCounter.GetInstance().ResetCurrentBattleCount();
            // set a checkpoint for enemy stat creation
            EnemyStats.GetInstance().SetCheckpoint();
            // go to town, no more battles to be fought this round
            LoadStatSelect();
            _backgroundManager.BackgroundChange();
        }
        else
        {
            //Debug.Log("On to the next battle!");
            // check to see if player wants to use a camp kit (only if they have one!)
            if (this.player.inventory.CampKits > 0)
            {
                // check if player wants to camp out before next battle
                // load camp kit check scene?
            }
            // No more resetting stats on retreat.  So this is to save the stats after each battle.
            EnemyStats.GetInstance().SetCheckpoint();
            LoadNextBattle();
        }
    }
    #endregion LevelLoading
    #region monobehaviour
    void Awake()
    {
        PlayerPrefs.SetInt("retreated", 0);
        SceneFadeHandler.Instance.levelStarting = true;
        EscapeHandler.instance.GetButtons();
        // Combat AI Controller reference
        this.combatController = FindObjectOfType<CombatController>();
        // inventory animator
        this.invAnim = FindObjectOfType<InventoryAnimation>();

        this.player = FindObjectOfType<PlayerController>();

        this.enemy = FindObjectOfType<BaseEnemyController>();

        currentBattle = BattleCounter.GetInstance().GetCurrentBattleCount();
        remainingBattles = BattleCounter.GetInstance().GetRemainingBattles();

        _backgroundManager = FindObjectOfType<BackgroundManager>();
        _backgroundManager.SetBackground();
    }

    // Use this for initialization
    void Start()
    {
        // INITIALIZE BATTLE SCENE
        combatController.setState(CombatController.BattleStates.START);
        AudioManager.Instance.PlayNewSong("ForestBattle");

        this.confirmPanel.gameObject.SetActive(false);

        this.turn = 0;

        this.scoredThisRound = false;
        this.score = PlayerPrefs.GetInt("score");

        COOLDOWN_LENGTH = 2.0f;

        someoneIsDead = false;
        finalFrame = false;
        hasSelected = false;

        // enemy has not healed or attacked yet
        enemyHasHealed = false;
        enemyHasAttacked = false;

        // get playerController 
        //this.player = FindObjectOfType<PlayerController>();
        this.player.InBattle(true);
        this.enemy.InBattle(true);

        // carry over previous balance
        this.player.dollarBalance += PlayerPrefs.GetInt("carryover");
        PlayerPrefs.SetInt("carryover", 0);
        // reposition player
    //    this.prevPos = this.player.transform.localPosition;
    //    Vector3 newSpot = new Vector3(-2.5f, -2f); // New Position
    //    this.player.gameObject.transform.localPosition = newSpot;
        this.player.posController.MovePlayer(25, 37);
        this.enemy.posController.MovePlayer(25, 87);

        // get enemy reference
        //this.enemy = FindObjectOfType<BaseEnemyController>();

        // Set Attack Meter Amount
        this.attackMeter.maxValue = 100;
        
		//this.attackMeter.value = (float)Random.Range(0, this.attackMeter.maxValue);
        this.attackMeter.gameObject.SetActive(false);
        // set cancel button to invis
        this.cancelAttack.gameObject.SetActive(false);

        this.playerHealth.maxValue = this.player.totalHealth;
        this.playerMana.maxValue = this.player.totalEnergy;
        this.enemyHealth.maxValue = this.enemy.totalHealth;
        this.enemyMana.maxValue = this.enemy.totalEnergy;

        // combat starts after initialization is finished
        // Set the idle animation to battle idle and trigger the entry animations.
        //player.InBattle(true);

        if(currentBattle == 0)
            player.TriggerAnimation("enterbattle");

        combatController.setState(CombatController.BattleStates.PLAYERCHOICE);
    }
    /// <summary>
    /// Temporary fix to make the animations fit a bit better.  Better fix would be to change the animations.
    /// </summary>
    /// <returns></returns>
    /*IEnumerator smalldelay()
    {
        yield return new WaitForSeconds(0.5f);
        enemy.TriggerAnimation("enterbattle");
        Vector3 newSpot = new Vector3(5.5f, -2.2f); // New Position
        yield return new WaitForSeconds(0.6f);
        this.enemy.gameObject.transform.localPosition = newSpot;
    }*/

    // Update is called once per frame
    void FixedUpdate()
    {
        curTime = Time.time;
        UpdateDisplay();
        Cooldown();
        CheckDeath();
        this.barDelta = this.attackMeter.maxValue * Time.fixedDeltaTime * BAR_SPEED;

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
