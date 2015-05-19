using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public PlayerController leftPlayer;
	public PlayerController rightPlayer;
	public GameObject gameObj;
	public GameController gameController;
	private bool playerTurn ;
	public int numEnemiesKilled;

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
				   rightPhysAttack = null,
				   leftMagAttack = null,
				   rightMagAttack = null;

	// Use this for initialization
	void Start () {
		playerTurn = true;
		// get game Objects to associate with code
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");


		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		} 
		if (gameControllerObject == null) {
			Debug.Log("Cannot find GameObject!");
			return;
		}
		// HUD INIT
		this.numEnemiesKilled = PlayerPrefs.GetInt("score");
		this.leftHealthText.text = "";
		this.rightHealthText.text = "";
		this.leftManaText.text = "";
		this.rightManaText.text = "";
		this.leftArmorText.text = "";
		this.rightArmorText.text = "";
		this.leftSpeedText.text = "";
		this.rightSpeedText.text = "";
		this.numEnemiesKilledText.text = "";
		// BUTTON LISTENERS
		this.leftPhysAttack.onClick.AddListener (()=>{PlayerPhysicalAttack(this.leftPlayer, this.rightPlayer);});
		this.leftMagAttack.onClick.AddListener (()=>{PlayerMagicAttack(this.leftPlayer, this.rightPlayer);});

		this.rightPhysAttack.onClick.AddListener (()=>{PlayerPhysicalAttack(this.rightPlayer, this.leftPlayer);});
		this.rightMagAttack.onClick.AddListener (()=>{PlayerMagicAttack(this.rightPlayer, this.leftPlayer);});

		// grab all CreateCombinations
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
	}

	// increment counter and set local storage

	void UpdateScore(){
		if (this.rightPlayer.remainingHealth <=0)
			this.numEnemiesKilled++;
		PlayerPrefs.SetInt ("score", this.numEnemiesKilled);
	}

	void UpdateText(){
		this.leftHealthText.text = "HEALTH: " + this.leftPlayer.remainingHealth+"/"+this.leftPlayer.totalHealth;
		this.rightHealthText.text = "HEALTH: " + this.rightPlayer.remainingHealth+"/"+this.rightPlayer.totalHealth;

		this.leftManaText.text = "MANA: " + this.leftPlayer.remainingMana+"/"+this.leftPlayer.totalMana;
		this.rightManaText.text = "MANA: " + this.rightPlayer.remainingMana + "/" + this.rightPlayer.totalMana;
	
		this.leftArmorText.text = "ARMOR: " + this.leftPlayer.armor;
		this.rightArmorText.text = "ARMOR: " + this.rightPlayer.armor;

		this.leftSpeedText.text = "SPEED: " + this.leftPlayer.speed;
		this.rightSpeedText.text = "SPEED: " + this.rightPlayer.speed;

		this.numEnemiesKilledText.text = "SCORE: " + this.numEnemiesKilled;
	}


	// Update is called once per frame
	void Update () {
		UpdateText ();
		UpdateScore();

		if (this.leftPlayer.remainingHealth > 0 && !playerTurn) {
			if (Random.Range(0,1) == 1)
				PlayerPhysicalAttack(this.rightPlayer,this.leftPlayer);
			else
				PlayerMagicAttack(this.rightPlayer,this.leftPlayer);
		}


		if (this.leftPlayer.remainingHealth == 0) {
			// player dead
			Destroy (this.leftPlayer);

			// check for high score
			if (PlayerPrefs.GetInt("score") > PlayerPrefs.GetInt("hiscore")){
				// set new HighScore
				PlayerPrefs.SetInt("hiscore", this.numEnemiesKilled);
			}

			// reset score
			PlayerPrefs.SetInt("score", 0);
			// load 
			Application.LoadLevel ("DeathScene_LVP");
		} else if (this.rightPlayer.remainingHealth <= 0) {
			this.numEnemiesKilled++;
			// enemy dead, fight another
			this.leftPlayer.remainingHealth = this.leftPlayer.totalHealth;
			this.leftPlayer.remainingMana = this.leftPlayer.totalMana;

			DontDestroyOnLoad(this.leftPlayer);

			Application.LoadLevel ("Battle_LVP");
		}
	}

	// deal damage to a player (makeshift game call)
	public void PlayerPhysicalAttack(PlayerController attackingPlayer, PlayerController attackedPlayer){
		// call player take damage to handle armor etc on the player object
		// animate sprites
		// apply damage to player
		attackingPlayer.PhysicalAttack (attackedPlayer);
		if (attackingPlayer == this.leftPlayer) {
			playerTurn = false;
		}else {
			playerTurn = true;
		}
	}

	public void PlayerMagicAttack(PlayerController attackingPlayer, PlayerController attackedPlayer){
		// call player take damage to handle armor etc on the player object
		// animate sprites
		// apply damage to player

		bool pass = attackingPlayer.MagicAttack (attackedPlayer);

	if (attackingPlayer == this.leftPlayer && pass) {
			playerTurn = false;
		} else {
			playerTurn = true;
		}
	} 
}
