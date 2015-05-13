using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public PlayerController leftPlayer;
	public PlayerController rightPlayer;
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
		this.leftHealthText.text = "";
		this.rightHealthText.text = "";
		this.leftManaText.text = "";
		this.rightManaText.text = "";
		this.leftArmorText.text = "";
		this.rightArmorText.text = "";
		this.leftSpeedText.text = "";
		this.rightSpeedText.text = "";
		this.leftDamageIndText.text = "";
		this.rightDamageIndText.text = "";
		// BUTTON LISTENERS
		this.leftPhysAttack.onClick.AddListener (()=>{PlayerPhysicalAttack(this.leftPlayer, this.rightPlayer);});
		this.leftMagAttack.onClick.AddListener (()=>{PlayerMagicAttack(this.leftPlayer, this.rightPlayer);});

		this.rightPhysAttack.onClick.AddListener (()=>{PlayerPhysicalAttack(this.rightPlayer, this.leftPlayer);});
		this.rightMagAttack.onClick.AddListener (()=>{PlayerMagicAttack(this.rightPlayer, this.leftPlayer);});

		/** HELP HERE!!*/
		// The body is not set to an instance of an object, and I can't figure out why!

		// grab all CreateCombinations
		CreateCombination[] bodies = GameObject.FindObjectsOfType<CreateCombination> ();

		// iterate through each, assigning left to left, right to right
		foreach (CreateCombination bod in bodies) {

			Debug.Log (bod.GetComponentInParent<PlayerController> ().name);
			// get playerController 
			PlayerController pc = bod.GetComponentInParent<PlayerController> ();
			// check for left or Right
			if (pc.name == "Left") {
				// assign PC
				this.leftPlayer = pc;
				this.leftPlayer.body = bod;
				//Debug.Log(this.leftPlayer);
			} else if (pc.name == "Right") {
				this.rightPlayer = pc;
				this.rightPlayer.body = bod;
				//Debug.Log(this.rightPlayer);
			}
		}

		this.leftPlayer.body.random = true;

		UpdateText ();

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

		Debug.Log (attackingPlayer + " Physically Attacked " + attackedPlayer);

		// animate sprites

		// apply damage to player
		attackedPlayer.TakeDamage (attackingPlayer.physicalDamage);
		
		//UpdateHealth ();
	}

	public void PlayerMagicAttack(PlayerController attackingPlayer, PlayerController attackedPlayer){
		// call player take damage to handle armor etc on the player object
		Debug.Log (attackingPlayer + " Magically Attacked " + attackedPlayer);
		// animate sprites
		
		// apply damage to player
		attackedPlayer.TakeDamage (attackingPlayer.magicalDamage);
		//UpdateHealth ();
	}
}
