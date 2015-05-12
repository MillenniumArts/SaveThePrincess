using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public PlayerController leftPlayer, rightPlayer;
	public GameObject gameObj;
	public GameController gameController;

	// GUI
	public GUIText leftHealthText, rightHealthText;
	[SerializeField] 
	private Button leftAttack = null, rightAttack = null; // assign in the editor

	// Use this for initialization
	void Start () {
		this.leftHealthText.text = "";
		this.rightHealthText.text = "";

		leftAttack.onClick.AddListener (()=>{PlayerAttack(this.rightPlayer, 10);});
		rightAttack.onClick.AddListener (()=>{PlayerAttack(this.leftPlayer, 10);});

		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		} 
		if (gameControllerObject == null) {
			Debug.Log("Cannot find GameObject!");
		}


		if (this.gameObj != null) {
			this.leftPlayer = this.gameObj.AddComponent<PlayerController> ();
			this.rightPlayer = this.gameObj.AddComponent<PlayerController> ();

			this.leftPlayer.totalHealth = 100;
			this.rightPlayer.totalHealth = 100;

			UpdateHealth ();
		} 

	}

	void UpdateHealth(){
		this.leftHealthText.text = "HEALTH: " + this.leftPlayer.remainingHealth;
		this.rightHealthText.text = "HEALTH: " + this.rightPlayer.remainingHealth;
	}

	// Update is called once per frame
	void Update () {
		UpdateHealth ();
	}

	// deal damage to a player (makeshift game call)
	public void PlayerAttack(PlayerController attackedPlayer, int attackDamage){
		// call player take damage to handle armor etc on the player object
		//Debug.Log ("DAMAGING!");
		attackedPlayer.TakeDamage (attackDamage);
		//UpdateHealth ();
	}
}
