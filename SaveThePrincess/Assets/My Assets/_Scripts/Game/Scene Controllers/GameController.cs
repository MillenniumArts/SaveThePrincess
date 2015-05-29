using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public PlayerController leftPlayer;
	public BaseEnemyController rightPlayer;
	public GameObject gameObj;
	public GameController gameController;
	public bool enemyHasHealed;
	public int score, turn;
	public static float MONEY_TRANSFER_PCT = 0.2f;

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
		turn = 0;
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
		this.leftPlayer = FindObjectOfType<PlayerController>();
		this.leftPlayer.inventory = GameObject.FindObjectOfType<InventoryGUIController> ();
		this.leftPlayer.dollarBalance += PlayerPrefs.GetInt ("carryover");
		PlayerPrefs.SetInt ("carryover", 0);
		// get enemy
		this.rightPlayer = FindObjectOfType<BaseEnemyController>();

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
		}
	}

	/// <summary>
	/// Initiates Player Inventory.
	/// </summary>
	public void InventoryInit(){
		if (!this.leftPlayer.inventory.initialized)
			this.leftPlayer.inventory.PopulateInventory ();

		InitInventoryButtons ();

	}

	/// <summary>
	/// Player Uses the item in inventory specified at index.
	/// </summary>
	/// <param name="index">Index.</param>
	public void UseItem(int index){
		this.leftPlayer.UseItem (index);
		this.leftPlayer.inventory.DisableButtonsIfUsed ();

	}



	public void AttackButton(PawnController attacked){
		if (turn == 0){
			this.leftPlayer.PhysicalAttack(this.rightPlayer);
			NextTurn();
		}
	}


	/// <summary>
	/// Initiates Inventory buttons.
	/// </summary>
	void InitInventoryButtons(){
		GameObject a = GameObject.FindWithTag ("InventoryGUI");
		Button[] but = a.GetComponentsInChildren<Button> ();
		Text[] t = a.GetComponentsInChildren<Text> ();
		for (int i=0; i < this.leftPlayer.inventory._items.Length; i++) {
				this.leftPlayer.inventory.clickables [i] = but [i];
				this.leftPlayer.inventory.buttonText [i] = t [i];
				this.leftPlayer.inventory.buttonText [i].text = "USE";
			if (this.leftPlayer.inventory._items[i].used){
				this.leftPlayer.inventory.clickables[i].gameObject.SetActive(false);
				this.leftPlayer.inventory.drawLocations[i].gameObject.SetActive(false);
			}

		}
		//this.leftPlayer.inventory.DisableButtonsIfUsed ();
	}

	// increment counter and set local storage
	private void UpdateScore(){
		if (this.rightPlayer.remainingHealth <= 0 ) {
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

		this.leftDamageText.text = "DAMAGE: " + this.leftPlayer.physicalDamage;
		this.rightDamageText.text = "DAMAGE: " + this.rightPlayer.physicalDamage;

		this.numEnemiesKilledText.text = "SCORE: " + score;
	}

	/// <summary>
	/// Does the enemy AI Behaviour.
	/// </summary>
	/// <returns>The enemy action.</returns>
	private IEnumerator DoEnemyAction(){
		yield return new WaitForSeconds (0.5f);
			// player alive and enemy turn
		if (this.leftPlayer.remainingHealth > 0 && this.rightPlayer.remainingHealth > 0 && turn == 1) {
				//enemy health < 25% , 33% chance of healing
				if (this.rightPlayer.remainingHealth > (0.25 * this.rightPlayer.totalHealth) && Random.Range (0, 2) == 0) {
					//50% chance of physical vs magic
					if (Random.Range (0, 1) == 1) {
						// physical
						Debug.Log (this.rightPlayer.name +" attacks!");
						this.rightPlayer.PhysicalAttack (this.leftPlayer);
					} else {
						//Magical	
						Debug.Log (this.rightPlayer.name +" uses a Magic Attack!");
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
					// chance to heal if enemy has potion
					if (!enemyHasHealed) {
						Debug.Log (this.rightPlayer.name + "Healed self for 25 hp");
						this.rightPlayer.remainingHealth += 25;
						this.enemyHasHealed = true;
					} else {
					// no potions to heal, LAST RESORT ATTACK!
						Debug.Log (this.rightPlayer.name +" attacks!");
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

		this.leftPlayer.inventory.gameObject.SetActive (false);
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
		int goldAmount = Mathf.FloorToInt((this.leftPlayer.dollarBalance * MONEY_TRANSFER_PCT));

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
			// CHANGE THIS TO LOADING TOWN!

		}
	}	
}
