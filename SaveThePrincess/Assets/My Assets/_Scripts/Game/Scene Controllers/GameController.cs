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
    public bool enemyHasHealed, waiting, scoredThisRound, enemyHasAttacked;
    public int score, turn;
    public float MONEY_TRANSFER_PCT = 0.2f, COOLDOWN_LENGTH = 1.75f;
    public float cooldownValue;

    public Slider playerHealth, playerMana, enemyHealth, enemyMana;
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

    //[SerializeField] 
    public Button leftPhysAttack = null;

    private Image background;

    // Use this for initialization
    void Awake()
    {
        this.turn = 0;
        this.scoredThisRound = false;
        this.cooldownValue = 0.0f;
        PlayerPrefs.SetInt("turn", turn);
        // enemy has not healed yet
        enemyHasHealed = false;
        enemyHasAttacked = false;
        this.score = PlayerPrefs.GetInt("score");

        // get game Object
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

        this.combatController = FindObjectOfType<CombatController>();
        // get playerController 
        this.player = FindObjectOfType<PlayerController>();
        this.player.inventory = GameObject.FindObjectOfType<InventoryGUIController>();
        this.player.dollarBalance += PlayerPrefs.GetInt("carryover");
        PlayerPrefs.SetInt("carryover", 0);
        this.prevPos = this.player.transform.localPosition;
        Vector3 newSpot = new Vector3(-2.5f, -2.5f);
        this.player.gameObject.transform.localPosition = newSpot;

        // get enemy
        this.enemy = FindObjectOfType<BaseEnemyController>();

        // initate player inventory
        InventoryInit();
        // combat control start
        combatController.setState(CombatController.BattleStates.PLAYERCHOICE);

    }

    /// <summary>
    /// Initiates Player Inventory.
    /// </summary>
    public void InventoryInit()
    {
        if (!this.player.inventory.isActiveAndEnabled)
            this.player.gameObject.SetActive(true);
        if (!this.player.inventory.initialized)
            this.player.inventory.PopulateInventory();

        InitInventoryButtons();
    }

    /// <summary>
    /// Initiates Inventory buttons.
    /// </summary>
    void InitInventoryButtons()
    {
        GameObject a = GameObject.FindWithTag("InventoryGUI");
        Button[] but = a.GetComponentsInChildren<Button>();
        Text[] t = a.GetComponentsInChildren<Text>();
        for (int i = 0; i < this.player.inventory._items.Length; i++)
        {
            this.player.inventory.clickables[i] = but[i];
            this.player.inventory.buttonText[i] = t[i];
            if (this.player.inventory._items[i].used || this.player.inventory._items[i] == null)
            {
                this.player.inventory.clickables[i].gameObject.SetActive(false);
                this.player.inventory.buttonText[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Disables the buttons.
    /// </summary>
    public void DisableButtons()
    {
        this.leftPhysAttack.gameObject.SetActive(false);
        for (int i = 0; i < this.player.inventory.clickables.Length; i++)
        {
            this.player.inventory.clickables[i].gameObject.SetActive(false);
            this.player.inventory.buttonText[i].gameObject.SetActive(false);
        }
        this.player.inventory.gameObject.SetActive(false);
    }

    /// <summary>
    /// Enables the buttons.
    /// </summary>
    public void EnableButtons()
    {
        this.leftPhysAttack.gameObject.SetActive(true);
        for (int i = 0; i < this.player.inventory.clickables.Length; i++)
        {
            if (!this.player.inventory._items[i].used)
            {
                this.player.inventory.clickables[i].gameObject.SetActive(true);
                this.player.inventory.buttonText[i].gameObject.SetActive(true);
            }
            else
            {
                if (this.player.inventory._items[i].used || this.player.inventory._items[i] == null)
                {
                    this.player.inventory.clickables[i].gameObject.SetActive(false);
                    this.player.inventory.buttonText[i].gameObject.SetActive(false);
                }
            }
        }
        this.player.inventory.gameObject.SetActive(true);
    }

    /// <summary>
    /// Player Uses the item in inventory specified at index.
    /// </summary>
    /// <param name="index">Index.</param>
    public void UseItem(int index)
    {
        if (combatController.currentState == CombatController.BattleStates.PLAYERCHOICE)
        {
            bool pass = this.player.UseItem(index);
            if (pass)
            {
                // start animation
                combatController.setState(CombatController.BattleStates.PLAYERANIMATE);
                waiting = true;
                StartCooldown(waiting, COOLDOWN_LENGTH);
                //this.player.inventory._items[index].used = true;
            }
        }
    }

    /// <summary>
    /// When button is clicked
    /// </summary>
    /// <param name="attacked">Attacked Player.</param>
    public void OnActionUsed(PawnController attacked)
    {
        if (combatController.currentState == CombatController.BattleStates.PLAYERCHOICE)
        {
            // start animation
            combatController.setState(CombatController.BattleStates.PLAYERANIMATE);
            waiting = true;
            StartCooldown(waiting, COOLDOWN_LENGTH);
            this.player.PhysicalAttack(attacked);
        }
    }

    /// <summary>
    /// called when player animation is finished
    /// </summary>
    public void OnWaitComplete()
    {
        this.enemy.TakeDamage();
        combatController.setState(CombatController.BattleStates.ENEMYTAKEDAMAGE);

        waiting = false;
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
                        this.enemy.PhysicalAttack(this.player);
                    }
                    else if (r <= 5)
                    {
                        //Magical	
                        Debug.Log(this.enemy.name + " uses a Magic Attack!");
                        bool pass = this.enemy.MagicAttack(this.player);
                        // if there is not enough mana to cast a magic attack
                        if (!pass)
                        {
                            //50% chance of mana potion or physical attack instead
                            r = Random.Range(0, 10);
                            if (r > 5)
                            {
                                Debug.Log("Instead of casting Magic, " + this.enemy.name + " attacks!");
                                this.enemy.PhysicalAttack(this.player);
                            }
                            else
                            {
                                Debug.Log("Mana restored by 10");
                                this.enemy.remainingMana += 10;
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
                        this.enemy.PhysicalAttack(this.player);
                    }
                }
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
    /// Updates the button text.
    /// </summary>
    void UpdateButtonText()
    {
        for (int i = 0; i < this.player.inventory._items.Length; i++)
        {

            if (this.player.inventory._items[i].GetItemSubClass() == "AttackMagic")
            {
                this.player.inventory.buttonText[i].text = this.player.inventory._items[i].GetAtkMod() + " DMG";
            }
            else if (this.player.inventory._items[i].GetItemSubClass() == "HealPotion"
                   || this.player.inventory._items[i].GetItemSubClass() == "HealMagic")
            {
                this.player.inventory.buttonText[i].text = "+" + this.player.inventory._items[i].GetHealEffect() + " HP";
            }
            else if (this.player.inventory._items[i].GetItemSubClass() == "ManaPotion")
            {
                this.player.inventory.buttonText[i].text = "+" + this.player.inventory._items[i].GetManaMod() + " MN";
            }
            else
            {
                this.player.inventory.buttonText[i].text = "";
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
        this.playerMana.maxValue = this.player.totalMana;
        this.playerMana.value = this.player.remainingMana;
        this.enemyHealth.maxValue = this.enemy.totalHealth;
        this.enemyHealth.value = this.enemy.remainingHealth;
        this.enemyMana.maxValue = this.enemy.totalMana;
        this.enemyMana.value = this.enemy.remainingMana;
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

        this.leftManaText.text = this.player.remainingMana + "/" + this.player.totalMana;
        this.rightManaText.text = this.enemy.remainingMana + "/" + this.enemy.totalMana;

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
        // disable inventory
        this.player.inventory.gameObject.SetActive(false);
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
        UpdateButtonText();
        UpdateBars();
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
