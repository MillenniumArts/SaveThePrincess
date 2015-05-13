using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public PlayerController leftPlayer, rightPlayer;
	public GameObject gameObj;
	public GameController gameController;

	// GUI/HUD
	public GUIText leftHealthText, 
				   rightHealthText,
				   leftManaText,
				   rightManaText,
				   leftArmorText,
				   rightArmorText,
				   leftSpeedText,
				   rightSpeedText,
				   leftDamageIndText,
				   rightDamageIndText;
	[SerializeField] 
	private Button leftPhysAttack = null,
				   rightPhysAttack = null,
				   leftMagAttack = null,
				   rightMagAttack = null;

	// Use this for initialization
	void Start () {
		this.leftHealthText.text = "";
		this.rightHealthText.text = "";

		leftPhysAttack.onClick.AddListener (()=>{PlayerPhysicalAttack(this.leftPlayer, this.rightPlayer);});
		leftMagAttack.onClick.AddListener (()=>{PlayerMagicAttack(this.leftPlayer, this.rightPlayer);});
		rightPhysAttack.onClick.AddListener (()=>{PlayerPhysicalAttack(this.rightPlayer, this.leftPlayer);});
		rightMagAttack.onClick.AddListener (()=>{PlayerMagicAttack(this.rightPlayer, this.leftPlayer);});

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		} 
		if (gameControllerObject == null) {
			Debug.Log("Cannot find GameObject!");
			return;
		}


		if (this.gameObj != null) {
			this.leftPlayer = this.gameObj.AddComponent<PlayerController> ();
			this.rightPlayer = this.gameObj.AddComponent<PlayerController> ();

			this.leftPlayer.totalHealth = 100;
			this.rightPlayer.totalHealth = 100;

			//this.leftPlayer.body.random = true;
			//this.rightPlayer.body.random = true;

			UpdateText ();
		} 

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
	}

	// Update is called once per frame
	void Update () {
		UpdateText ();
	}

	// deal damage to a player (makeshift game call)
	public void PlayerPhysicalAttack(PlayerController attackingPlayer, PlayerController attackedPlayer){
		// call player take damage to handle armor etc on the player object

		// animate sprites

		// apply damage to player
		attackedPlayer.TakeDamage (attackingPlayer.physicalDamage);
		
		//UpdateHealth ();
	}

	public void PlayerMagicAttack(PlayerController attackingPlayer, PlayerController attackedPlayer){
		// call player take damage to handle armor etc on the player object
		
		// animate sprites
		
		// apply damage to player
		attackedPlayer.TakeDamage (attackingPlayer.magicalDamage);
		//UpdateHealth ();
	}
}
