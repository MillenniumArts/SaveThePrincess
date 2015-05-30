using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public PlayerController player;
	public BaseEnemyController enemy;
	public GameObject gameObj;
	public GameController gameController;
	public bool enemyHasHealed, waiting, scoredThisRound;
	public int score, turn;
	public static float MONEY_TRANSFER_PCT = 0.2f, COOLDOWN_LENGTH = 1.5f;
	public float cooldownValue;

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
		if (!this.player.inventory.initialized)
			this.player.inventory.PopulateInventory ();
		InitInventoryButtons ();
	}

	/// <summary>
	/// Player Uses the item in inventory specified at index.
	/// </summary>
	/// <param name="index">Index.</param>
	public void UseItem(int index){
		this.player.UseItem (index);
		this.player.TriggerAnimation(this.player.inventory._items[index].GetItemClass());
		this.player.inventory.DisableButtonsIfUsed ();
	}

	/// <summary>
	/// When button is clicked
	/// </summary>
	/// <param name="attacked">Attacked.</param>
	public void OnActionUsed(PawnController attacked){
		if (turn == 0){
			waiting = true;
			this.leftPhysAttack.gameObject.SetActive(false);
			for (int i=0; i<this.player.inventory.clickables.Length; i++){
				if (!this.player.inventory._items[i].used)
					this.player.inventory.clickables[i].gameObject.SetActive(false);
			}
			this.player.PhysicalAttack(this.enemy);
			StartCooldown(waiting, COOLDOWN_LENGTH);
		}
	}

	/// <summary>
	/// called when animation is finished
	/// </summary>
	public void OnWaitComplete(){
		waiting = false;
			this.leftPhysAttack.gameObject.SetActive(true);
			for (int i=0; i<this.player.inventory.clickables.Length; i++){
				if (!this.player.inventory._items[i].used)
					this.player.inventory.clickables[i].gameObject.SetActive(true);
			}
			this.player.inventory.gameObject.SetActive(true);
			NextTurn();
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
		this.leftHealthText.text = "HEALTH: " + this.player.remainingHealth+"/"+this.player.totalHealth;
		this.rightHealthText.text = "HEALTH: " + this.enemy.remainingHealth+"/"+this.enemy.totalHealth;

		this.leftManaText.text = "MANA: " + this.player.remainingMana+"/"+this.player.totalMana;
		this.rightManaText.text = "MANA: " + this.enemy.remainingMana + "/" + this.enemy.totalMana;
	
		this.leftArmorText.text = "ARMOR: " + this.player.armor;
		this.rightArmorText.text = "ARMOR: " + this.enemy.armor;

		this.leftDamageText.text = "DAMAGE: " + this.player.physicalDamage;
		this.rightDamageText.text = "DAMAGE: " + this.enemy.physicalDamage;

		this.numEnemiesKilledText.text = "SCORE: " + score;
	}

	/// <summary>
	/// Does the enemy AI Behaviour.
	/// </summary>
	/// <returns>The enemy action.</returns>
	private void DoEnemyAction(){
		//yield return new WaitForSeconds (0.75f);
			// player alive and enemy turn
		if (!this.player.IsDead () && !this.enemy.IsDead () && turn == 1 && !waiting) {
			//StartCooldown (waiting, COOLDOWN_LENGTH);
			//enemy health < 25% , 50% chance of healing
			if (this.enemy.remainingHealth > (0.25 * this.enemy.totalHealth) && Random.Range (0, 1) == 0) {
				//50% chance of physical vs magic
				if (Random.Range (0, 1) == 1) {
					// physical
					Debug.Log (this.enemy.name + " attacks!");
					this.enemy.PhysicalAttack (this.player);
				} else {
					//Magical	
					Debug.Log (this.enemy.name + " uses a Magic Attack!");
					bool pass = this.enemy.MagicAttack (this.player);
					// if there is not enough mana to cast a magic attack
					if (!pass) {
						//50% chance of mana potion or physical attack instead
						if (Random.Range (0, 1) == 0) {
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
					Debug.Log (this.enemy.name + "Healed self for 25 hp");
					this.enemy.TriggerAnimation ("potion");
					this.enemy.remainingHealth += 25;
					this.enemyHasHealed = true;
				} else {
					// no potions to heal, LAST RESORT ATTACK!
					Debug.Log (this.enemy.name + " attacks!");
					this.enemy.PhysicalAttack (this.player);
				}
			}
			// end computer turn
			NextTurn ();
		} else {

			if (this.enemy.IsDead ()){
				Debug.Log ("enemy Dead");
				this.enemy.TriggerAnimation("death");
				//StartCooldown (waiting, COOLDOWN_LENGTH);
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
		if (this.player.IsDead() && !waiting){
			this.player.TriggerAnimation("death");
		}
		// check for high score
		if (PlayerPrefs.GetInt("score") > PlayerPrefs.GetInt("hiscore")){
			// set new HighScore
			PlayerPrefs.SetInt("hiscore", PlayerPrefs.GetInt("score"));
		}

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

		UpdateText ();
		// enemy dead, fight another and keep player on screen
		DontDestroyOnLoad(this.player);
		// reload battle scene
		Application.LoadLevel ("Battle_LVP");
	}

	void GoToStore(){
		UpdateText ();
		DontDestroyOnLoad(this.player);
		Application.LoadLevel ("Store_LVP");
	}
	/// <summary>
	/// Drops a random amount of money after enemy dies.
	/// </summary>
	private int DropMoney(){
		return Random.Range ((score+1),((score+1)*2));
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
				this.OnWaitComplete();
			}
		}
	}

	// Update is called once per frame
	void Update () {
		UpdateText ();
		UpdateScore();

		Cooldown ();
		if (!waiting) {
			DoEnemyAction();
			if (this.player.IsDead()) {
				// player dead
				EndGame ();
			} else if (this.enemy.IsDead ()) {
				// enemy dead
				//get enemy moneydrop
				int moneyDrop = DropMoney ();
				this.player.dollarBalance += moneyDrop;

				GoToStore ();
				//LoadNextLevel ();
			} else if (this.enemy.IsDead()) {
				// CHANGE THIS TO LOADING TOWN!

			}
		}
	}	
}
