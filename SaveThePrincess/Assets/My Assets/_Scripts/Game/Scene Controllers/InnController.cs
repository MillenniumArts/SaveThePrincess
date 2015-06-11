using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InnController : MonoBehaviour {

	public PlayerController player;
	public int[] prices, stats;
	public Button[] sleepButtons;
	public Text[] buttonText;
	public Text healthText, manaText, playerBalance;
	protected int BASE_ROOM_COST;
	private Vector3 prevPos;

	// Use this for initialization
	void Start () {
		this.player = FindObjectOfType<PlayerController>();
		this.prevPos = this.player.gameObject.transform.localPosition;
		
		// relocate player
		Vector3 newSpot = new Vector3 (-5.5f, -3.7f);
		this.player.gameObject.transform.localPosition = newSpot;



		BASE_ROOM_COST = 15;
		this.healthText.text = "";
		this.manaText.text = "";
		this.playerBalance.text = "";
		prices = new int[buttonText.Length];
		stats = new int[buttonText.Length];

		for (int i=0; i<buttonText.Length; i++) {
			prices [i] = BASE_ROOM_COST + (i * 5);
			buttonText [i].text = "SLEEP FOR NIGHT: $" + prices [i] + "";
		}
	}

	private void UpdateText(){
		this.healthText.text = "Health: " + this.player.remainingHealth + "/" + this.player.totalHealth;
		this.manaText.text = "Mana: " + this.player.remainingMana + "/" + this.player.totalMana;
		this.playerBalance.text = "Balance: $" + this.player.dollarBalance;
		for (int i=0; i<buttonText.Length; i++) {
			buttonText[i].text = "SLEEP FOR NIGHT: $" + prices[i] + " ";
		}
	}

	public void SleepForNight(int index){
		// if player needs health OR mana
		if (player.remainingMana < player.totalMana || player.remainingHealth < player.totalHealth) {
			// if can afford, purchase
			if (player.PurchaseItem (prices [index])) {
				// effects administered here
				// USING SIMPLE MATH HERE FOR NOW, DOLLAR PER POINT OF MANA/HEALTH
				player.GiveMana (prices [index]);
				player.HealForAmount (prices [index]);
			}
		} else {
			Debug.Log ("You already have full stats! Go fight something!");
		}
	}


	public void LeaveInn(){
		this.player.gameObject.transform.localPosition = prevPos;
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Town_LVP");
	}

	// Update is called once per frame
	void Update () {
		UpdateText ();
	}
}
