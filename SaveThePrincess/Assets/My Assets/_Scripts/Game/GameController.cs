using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public PlayerController leftPlayer;
	public PlayerController rightPlayer;
	public GameObject gameObj;
	public GameController gameController;
	public bool inventoryOn, enemyHasHealed;
	public int score, turn;
	public static float TRANSFER_PCT = 0.2f;


	// GUI/HUD
	public GUIText leftHealthText, 
				   rightHealthText,
				   leftManaText,
				   rightManaText,
				   leftArmorText,
				   rightArmorText,
				   leftSpeedText,
				   rightSpeedText,
				   numEnemiesKilledText;
	[SerializeField] 
	private Button leftPhysAttack = null,
				   playerInventory = null,
				   useSlot1 = null,
				   useSlot2 = null,
				   useSlot3 = null,
				   cancelInventory=null;

	// Use this for initialization
	void Start () {
		turn = 0;
		PlayerPrefs.SetInt("turn",turn);
		// enemy has not healed yet
		enemyHasHealed = false;
		this.score = PlayerPrefs.GetInt ("score");
		// not showing inventory
		inventoryOn = false;

		// get game Object
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		} 
		if (gameControllerObject == null) {
			Debug.Log("Cannot find GameObject!");
			return;
		}

		// NOT SURE THIS IS THE BEST WAY... ******
		// grab all CreateCombinations to get player/enemy
		CreateCombination[] bodies = GameObject.FindObjectsOfType<CreateCombination> ();
		// iterate through each, assigning left to left, right to right
		foreach (CreateCombination bod in bodies) {
			// get playerController 
			PlayerController pc = bod.GetComponentInParent<PlayerController> ();
			// check for left or Right
			if (pc.tag == "Player") {
				// assign PC
				this.leftPlayer = pc;
				this.leftPlayer.body = bod;
			} else if (pc.tag == "Enemy") {
				this.rightPlayer = pc;
				this.rightPlayer.body = bod;
			}
		}
		// initiate buttons
		ButtonInit ();
		// get previous carryover Gold
		this.leftPlayer.dollarBalance += PlayerPrefs.GetInt ("carryover");
	}
	/// <summary>
	/// Toggles the inventory.
	/// </summary>
	public void ToggleInventory(){
		if (!inventoryOn) {
			ShowInventory ();
			inventoryOn = true;
		} else {
			HideInventory ();
			inventoryOn = false;
		}
	}
	/// <summary>
	/// Starts next turn.
	/// </summary>
	public void NextTurn(){
		if (PlayerPrefs.GetInt ("turn") == 0) {
			PlayerPrefs.SetInt("turn" , 1);
			turn = 1;
		} else if (PlayerPrefs.GetInt ("turn") == 1) {
			PlayerPrefs.SetInt("turn" , 0);
			turn = 0;
		}
	}
	/// <summary>
	/// Initiates all buttons needed.
	/// </summary>
	void ButtonInit(){
		this.leftPhysAttack.onClick.AddListener (()=>{
			if (PlayerPrefs.GetInt("turn") == 0){
				this.leftPlayer.PhysicalAttack(this.rightPlayer);
				NextTurn();
			}
		});

		this.playerInventory.onClick.AddListener (()=>{
			// MENU TOGGLE ON
			ToggleInventory();
		});
		
		this.useSlot1.onClick.AddListener (()=>{

			// HARD CODED HEALTH POTION

			if (this.leftPlayer.remainingHealth + 25 <= this.leftPlayer.totalHealth)
				this.leftPlayer.remainingHealth += 25;
			else
				this.leftPlayer.remainingHealth = this.leftPlayer.totalHealth;

			NextTurn();
			ToggleInventory ();
		});
		
		this.useSlot2.onClick.AddListener (()=>{
			// HARD CODED MANA POTION
			
			if (this.leftPlayer.remainingMana + 10 <= this.leftPlayer.totalMana)
				this.leftPlayer.remainingMana += 10;
			else
				this.leftPlayer.remainingMana = this.leftPlayer.totalMana;

			NextTurn();
			ToggleInventory ();
		});
		
		this.useSlot3.onClick.AddListener (()=>{

			//HARD CODED MAGIC SCROLL USE
			bool pass = this.leftPlayer.MagicAttack(this.rightPlayer);
			if (pass){
				ToggleInventory ();
				NextTurn();
			}

		});
		
		this.cancelInventory.onClick.AddListener (()=>{
			ToggleInventory();
		});
	}
	/// <summary>
	/// Shows the inventory.
	/// </summary>
	public void ShowInventory(){
		// disable battle HUD
		//this.leftHealthText.gameObject.SetActive (false);
		this.rightHealthText.gameObject.SetActive (false);

		//this.leftManaText.gameObject.SetActive (false);
		this.rightManaText.gameObject.SetActive (false);

		//this.leftArmorText.gameObject.SetActive (false);
		this.rightArmorText.gameObject.SetActive (false);

		//this.leftSpeedText.gameObject.SetActive (false);
		this.rightSpeedText.gameObject.SetActive (false);
		this.numEnemiesKilledText.gameObject.SetActive (false);

		// disable enemy
		this.rightPlayer.gameObject.SetActive (false);
		// disable buttons
		this.leftPhysAttack.gameObject.SetActive (false);
		this.playerInventory.gameObject.SetActive (false);

		//ENABLE BATTLE INVENTORY
		//this.battleInventory.gameObject.SetActive (true);
		this.useSlot1.gameObject.SetActive (true);
		this.useSlot2.gameObject.SetActive (true);
		this.useSlot3.gameObject.SetActive (true);
		this.cancelInventory.gameObject.SetActive (true);
	}

	/// <summary>
	/// Hides the inventory.
	/// </summary>
	public void HideInventory(){
		// reenable Battle HUD
		this.rightHealthText.gameObject.SetActive (true);
		this.rightManaText.gameObject.SetActive (true);
		this.rightArmorText.gameObject.SetActive (true);
		this.rightSpeedText.gameObject.SetActive (true);
		this.numEnemiesKilledText.gameObject.SetActive (true);

		// reenable enemy
		this.rightPlayer.gameObject.SetActive (true);
		// reenable buttons
		this.leftPhysAttack.gameObject.SetActive (true);
		this.playerInventory.gameObject.SetActive (true);

		// button deactivation
		this.useSlot1.gameObject.SetActive (false);
		this.useSlot3.gameObject.SetActive (false);
		this.useSlot2.gameObject.SetActive (false);
		this.cancelInventory.gameObject.SetActive (false);
	}

	// increment counter and set local storage
	private void UpdateScore(){
		if (this.rightPlayer.remainingHealth <= 0 ) {
			Debug.Log ("Score " + score);
			this.score++;
			PlayerPrefs.SetInt ("score", score );
		}
	}
	/// <summary>
	/// Updates the text for HUD in battle.
	/// </summary>
	private void UpdateText(){
		this.leftHealthText.text = "HEALTH: " + this.leftPlayer.remainingHealth+"/"+this.leftPlayer.totalHealth;
		this.rightHealthText.text = "HEALTH: " + this.rightPlayer.remainingHealth+"/"+this.rightPlayer.totalHealth;

		this.leftManaText.text = "MANA: " + this.leftPlayer.remainingMana+"/"+this.leftPlayer.totalMana;
		this.rightManaText.text = "MANA: " + this.rightPlayer.remainingMana + "/" + this.rightPlayer.totalMana;
	
		this.leftArmorText.text = "ARMOR: " + this.leftPlayer.armor;
		this.rightArmorText.text = "ARMOR: " + this.rightPlayer.armor;

		this.leftSpeedText.text = "SPEED: " + this.leftPlayer.speed;
		this.rightSpeedText.text = "SPEED: " + this.rightPlayer.speed;

		this.numEnemiesKilledText.text = "SCORE: " + score;
	}

	/// <summary>
	/// Does the enemy AI Behaviour.
	/// </summary>
	/// <returns>The enemy action.</returns>
	private IEnumerator DoEnemyAction(){
		yield return new WaitForSeconds (1.95f);
			// player alive and enemy turn
		if (this.leftPlayer.remainingHealth > 0 && this.rightPlayer.remainingHealth > 0 && turn == 1) {
				//enemy health < 40% , 50% chance of healing
				if (this.rightPlayer.remainingHealth > (0.4 * this.rightPlayer.totalHealth) && Random.Range (0, 1) == 0) {
					//50% chance of physical vs magic
					if (Random.Range (0, 1) == 1) {
						// physical
						this.rightPlayer.PhysicalAttack (this.leftPlayer);
					} else {
						//Magical	
						bool pass = this.rightPlayer.MagicAttack (this.leftPlayer);
						// if there is not enough mana to cast a magic attack
						if (!pass) {
							//50% chance of mana potion or physical attack instead
							if (Random.Range (0, 1) == 0) {
								Debug.Log ("Instead of casting Magic, " + this.rightPlayer.name + " attacks!");
								this.rightPlayer.PhysicalAttack (this.leftPlayer);
							} else {
								Debug.Log ("Mana restored by 10");
								this.rightPlayer.remainingMana += 10;
							}
						}
					}
				} else {
					// 40% chance to heal if enemy has potion
					if (!enemyHasHealed) {
						Debug.Log (this.rightPlayer.name + "Healed self for 25 hp");
						this.rightPlayer.remainingHealth += 25;
						this.enemyHasHealed = true;
					} else {
						// no potions to heal, LAST RESORT ATTACK!
						this.rightPlayer.PhysicalAttack (this.leftPlayer);
					}
				}
				// end computer turn
				NextTurn ();
			}
		}

	/// <summary>
	/// Ends the game.
	/// </summary>
	private void EndGame(){
		// player dead
		UpdateText ();
		TransferGold ();
		
		// check for high score
		if (PlayerPrefs.GetInt("score") > PlayerPrefs.GetInt("hiscore")){
			// set new HighScore
			PlayerPrefs.SetInt("hiscore", PlayerPrefs.GetInt("score"));
		}
		
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
		DontDestroyOnLoad(this.leftPlayer);
		// reload battle scene
		Application.LoadLevel ("Battle_LVP");
	}

	void GoToStore(){
		UpdateText ();
		DontDestroyOnLoad(this.leftPlayer);
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
		int goldAmount = Mathf.FloorToInt((this.leftPlayer.dollarBalance * TRANSFER_PCT));

		if(goldAmount > 100){
			goldAmount = 100;
		}
		PlayerPrefs.SetInt ("carryover" , goldAmount);
	}

	// Update is called once per frame
	void Update () {
		UpdateText ();
		UpdateScore();
		// ENEMY BEHAVIOUR FUNCTION HERE
		StartCoroutine("DoEnemyAction");

		if (this.leftPlayer.remainingHealth <= 0) {
			// player dead
			EndGame ();
		} else if (this.rightPlayer.remainingHealth <= 0/* && (PlayerPrefs.GetInt("score") % 5 != 0) */) {
			// enemy dead
			//get enemy moneydrop
			int moneyDrop = DropMoney ();
			Debug.Log ("Dropped " + moneyDrop + " gold");
			this.leftPlayer.dollarBalance += moneyDrop;
			// this.leftPlayer.dollarBalance += DropMoney();

			GoToStore();
			//LoadNextLevel ();
		} else if (this.rightPlayer.remainingHealth <= 0 ){
			//Debug.Log ("STORE!?");

			// CHANGE THIS TO LOADING TOWN!

		}
	}	
}
