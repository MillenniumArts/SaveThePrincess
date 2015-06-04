using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public PlayerController player;
	public BaseEnemyController enemy;
	public GameObject gameObj;
	public GameController gameController;
	public bool enemyHasHealed, waiting, scoredThisRound, enemyHasAttacked;
	public int score, turn;
	public static float MONEY_TRANSFER_PCT = 0.2f, COOLDOWN_LENGTH = 1.25f;
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

	[SerializeField] 
	public Button leftPhysAttack = null;

	private Image background;

	// Use this for initialization
	void Start () {
		this.turn = 0;
		this.scoredThisRound = false;
		this.cooldownValue = 0.0f;
		PlayerPrefs.SetInt("turn",turn);
		// enemy has not healed yet
		enemyHasHealed = false;
		enemyHasAttacked = false;
		this.score = PlayerPrefs.GetInt ("score");
			
		// get game Object
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		} 
		if (gameControllerObject == null) {
			Debug.Log("Cannot find GameObject!");
			return;
		}

		// get playerController 
		this.player = FindObjectOfType<PlayerController>();
		this.player.inventory = GameObject.FindObjectOfType<InventoryGUIController> ();
		this.player.dollarBalance += PlayerPrefs.GetInt ("carryover");
		PlayerPrefs.SetInt ("carryover", 0);
		this.prevPos = this.player.transform.localPosition;
		Vector3 newSpot = new Vector3 (-2.5f, -2.5f);
		this.player.gameObject.transform.localPosition = newSpot;

		// get enemy
		this.enemy = FindObjectOfType<BaseEnemyController>();

		// initate player inventory
		InventoryInit ();
	}

	// END START
	
	/// <summary>
	/// Starts next turn.
	/// </summary>
	public void NextTurn(){
		if (turn == 0) {
			PlayerPrefs.SetInt("turn" , 1);
			turn = 1;
		} else if (turn == 1) {
			PlayerPrefs.SetInt("turn" , 0);
			turn = 0;
			waiting=false;
		}
	}

	/// <summary>
	/// Initiates Player Inventory.
	/// </summary>
	public void InventoryInit(){
		if (!this.player.inventory.isActiveAndEnabled)
			this.player.gameObject.SetActive (true);
		if (!this.player.inventory.initialized)
			this.player.inventory.PopulateInventory ();

		InitInventoryButtons ();
	}

	/// <summary>
	/// Initiates Inventory buttons.
	/// </summary>
	void InitInventoryButtons(){
		GameObject a = GameObject.FindWithTag ("InventoryGUI");
		Button[] but = a.GetComponentsInChildren<Button> ();
		Text[] t = a.GetComponentsInChildren<Text> ();
		for (int i=0; i < this.player.inventory._items.Length; i++) {
			this.player.inventory.clickables [i] = but [i];
			this.player.inventory.buttonText [i] = t [i];
			this.player.inventory.buttonText [i].text = "+" + this.player.inventory._items[i].GetHealEffect()+ "HP";
			
			if (this.player.inventory._items[i].used){
				this.player.inventory.clickables[i].gameObject.SetActive(false);
				this.player.inventory.drawLocations[i].gameObject.SetActive(false);
			}
		}
	}
	
	/// <summary>
	/// Disables the buttons.
	/// </summary>
	private void DisableButtons(){
		this.leftPhysAttack.gameObject.SetActive(false);
		for (int i=0; i<this.player.inventory.clickables.Length; i++){
			this.player.inventory.clickables[i].gameObject.SetActive(false);
			this.player.inventory.buttonText[i].gameObject.SetActive(false);
		}
		this.player.inventory.gameObject.SetActive(false);
	}

	/// <summary>
	/// Enables the buttons.
	/// </summary>
	private void EnableButtons(){
		this.leftPhysAttack.gameObject.SetActive(true);
		for (int i=0; i<this.player.inventory.clickables.Length; i++){
			if (!this.player.inventory._items[i].used){
				this.player.inventory.clickables[i].gameObject.SetActive(true);
				this.player.inventory.buttonText[i].gameObject.SetActive(true);
			}
		}
		this.player.inventory.gameObject.SetActive(true);

	}

	/// <summary>
	/// Player Uses the item in inventory specified at index.
	/// </summary>
	/// <param name="index">Index.</param>
	public void UseItem(int index){
		if (turn == 0) {
			waiting = true;
			DisableButtons();
			this.player.UseItem (index);
			StartCooldown (waiting, COOLDOWN_LENGTH);
		}
	}

	/// <summary>
	/// When button is clicked
	/// </summary>
	/// <param name="attacked">Attacked.</param>
	public void OnActionUsed(PawnController attacked){
		if (turn == 0){
			waiting = true;
			DisableButtons();
			this.player.PhysicalAttack(this.enemy);
			StartCooldown(waiting, COOLDOWN_LENGTH);
		}
	}

	/// <summary>
	/// called when animation is finished
	/// </summary>
	public void OnWaitComplete(){
		waiting = false;
		EnableButtons ();
		NextTurn();
	}

	/// <summary>
	/// the enemy action used event.
	/// </summary>
	/// <param name="attacked">Attacked.</param>
	public void OnEnemyActionUsed(PawnController attacked){
		if (turn == 1){
			enemyHasAttacked = true;
			waiting = true;
			DisableButtons();
			//this.player.PhysicalAttack(this.enemy);
			StartCooldown(waiting, COOLDOWN_LENGTH);
		}
	}

	/// <summary>
	///  the enemy wait complete event.
	/// </summary>
	public void OnEnemyWaitComplete(){
		waiting = false;
		enemyHasAttacked = false;
		EnableButtons ();
		NextTurn ();
	}

	/// <summary>
	/// Starts the cooldown for specified boolean.
	/// </summary>
	/// <param name="toggle"><c>true</c>if boolean is ON cooldown.</param>
	/// <param name="time">Time.</param>
	public void StartCooldown(bool toggle, float time){
		cooldownValue = time;
		toggle = !toggle;
	}
	
	/// <summary>
	/// Cooldown counter.
	/// </summary>
	private void Cooldown(){
		//Debug.Log (waiting);
		if (waiting) {
			this.cooldownValue -= 0.01f;
			if (this.cooldownValue <= 0){
				this.cooldownValue = 0.0f;
				this.waiting = false;
				if (turn == 0)
					this.OnWaitComplete();
				else if (turn == 1)
					this.OnEnemyWaitComplete();
			}
		}
	}


	void UpdateButtonText(){
		for (int i=0; i < this.player.inventory._items.Length; i++) {
			this.player.inventory.buttonText [i].text = "+" + this.player.inventory._items [i].GetHealEffect () + "HP";
			if (this.player.inventory._items [i].GetHealEffect () == 0)
				this.player.inventory.buttonText [i].text = this.player.inventory._items[i].GetAtkMod() + " DMG";
		}
	}
	/// <summary>
	/// Updates the UI Health/mana bars.
	/// </summary>
	public void UpdateBars (){
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
	private void UpdateScore(){
		if (this.enemy.IsDead() && !scoredThisRound) {
			this.score++;
			scoredThisRound = true;
			PlayerPrefs.SetInt ("score", score );
		}
	}
	/// <summary>
	/// Updates the text for HUD in battle.
	/// </summary>
	private void UpdateText(){
		this.leftHealthText.text = this.player.remainingHealth+"/"+this.player.totalHealth;
		this.rightHealthText.text = this.enemy.remainingHealth+"/"+this.enemy.totalHealth;

		this.leftManaText.text = this.player.remainingMana+"/"+this.player.totalMana;
		this.rightManaText.text = this.enemy.remainingMana + "/" + this.enemy.totalMana;
	
		this.leftArmorText.text = "AMR: " + this.player.GetTotalArmor();
		this.rightArmorText.text = "AMR: " + this.enemy.GetTotalArmor();

		this.leftDamageText.text = "DMG: " + this.player.GetTotalDamage();
		this.rightDamageText.text = "DMG: " + this.enemy.GetTotalDamage();

		this.numEnemiesKilledText.text = "SCORE: " + score;
	}

	/// <summary>
	/// Does the enemy AI Behaviour.
	/// </summary>
	/// <returns>The enemy action.</returns>
	private void DoEnemyAction(){
		if (!enemyHasAttacked && !waiting) {
			// player alive and enemy turn
			if (!this.player.IsDead () && !this.enemy.IsDead () && turn == 1) {
				//enemy health < 25% 
				int r = Random.Range (0, 10);
				if (this.enemy.remainingHealth >= (0.25 * this.enemy.totalHealth)) {
					//50% chance of physical vs magic
					r = Random.Range (0, 10);
					if (r > 5) {
						// physical
						Debug.Log (this.enemy.name + " attacks!");
						this.enemy.PhysicalAttack (this.player);
					} else if (r <= 5) {
						//Magical	
						Debug.Log (this.enemy.name + " uses a Magic Attack!");
						bool pass = this.enemy.MagicAttack (this.player);
						// if there is not enough mana to cast a magic attack
						if (!pass) {
							//50% chance of mana potion or physical attack instead
							r = Random.Range (0, 10);
							if (r > 5) {
								Debug.Log ("Instead of casting Magic, " + this.enemy.name + " attacks!");
								this.enemy.PhysicalAttack (this.player);
							} else {
								Debug.Log ("Mana restored by 10");
								this.enemy.remainingMana += 10;
							}
						}
					}
				} else {
					// chance to heal if enemy has potion
					if (!enemyHasHealed) {
						Debug.Log (this.enemy.name + " healed for 25 hp");
						this.enemy.TriggerAnimation ("potion");
						this.enemy.HealForAmount (25);
						this.enemyHasHealed = true;
					} else {
						// no potions to heal, LAST RESORT ATTACK!
						Debug.Log (this.enemy.name + " attacks!");
						this.enemy.PhysicalAttack (this.player);
					}
				}
				OnEnemyActionUsed (this.player);
			} else{
				if (this.enemy.IsDead()){
					this.enemy.TriggerAnimation("death");
				}

			}
		}
	}

	/// <summary>
	/// Ends the game.
	/// </summary>
	private void EndGame(){
		// player dead
		UpdateText ();
		TransferGold ();
		this.player.transform.localPosition = this.prevPos;

		if (this.player.IsDead() && !waiting){
			this.player.TriggerAnimation("death");
		}
		// check for high score
		if (PlayerPrefs.GetInt("score") > PlayerPrefs.GetInt("hiscore")){
			// set new HighScore
			PlayerPrefs.SetInt("hiscore", PlayerPrefs.GetInt("score"));
		}
		DifficultyLevel.GetInstance ().ResetDifficulty ();
		this.player.inventory.gameObject.SetActive (false);
		// reset score
		PlayerPrefs.SetInt("score", 0);
		PlayerPrefs.SetInt("turn", 0);
		// load death scene
		Application.LoadLevel ("DeathScene_LVP");
	}

	/// <summary>
	/// Loads the next level.
	/// </summary>
	private void LoadNextLevel(){
		turn = 0;
		// player turn stored in local, 0 for playerTurn
		PlayerPrefs.SetInt ("turn", turn);
		this.player.transform.localPosition = this.prevPos;
		DifficultyLevel.GetInstance ().IncreaseDifficulty ();

		UpdateText ();
		// enemy dead, fight another and keep player on screen
		DontDestroyOnLoad(this.player);
		// reload battle scene
		Application.LoadLevel ("Battle_LVP");
	}

	void GoToTown(){
		UpdateText ();
		this.enemy.TriggerAnimation("death");
		this.player.transform.localPosition = this.prevPos;
		this.enemy.DropMoney ();
		this.player.dollarBalance += this.enemy.DropMoney ();
		DontDestroyOnLoad(this.player);
		Application.LoadLevel ("Town_LVP");
	}


	/// <summary>
	/// Transfers a portion of player's gold to next game.
	/// </summary>
	private void TransferGold (){
		int goldAmount = Mathf.FloorToInt((this.player.dollarBalance * MONEY_TRANSFER_PCT));

		if(goldAmount > 100){
			goldAmount = 100;
		}
		PlayerPrefs.SetInt ("carryover" , goldAmount);
	}

	// Update is called once per frame
	void Update () {
		UpdateText ();
		UpdateScore();
		UpdateButtonText ();
		UpdateBars ();
		Cooldown ();

		if (!waiting) {
			DoEnemyAction();
			if (this.player.IsDead()) {
				// player dead
				this.player.TriggerAnimation("death");
				EndGame ();
			} else if (this.enemy.IsDead () && !waiting) {
				// enemy dead

				GoToTown ();
			} else if (this.enemy.IsDead()) {
				// CHANGE THIS TO LOADING TOWN!

			}
		}
	}	
}
