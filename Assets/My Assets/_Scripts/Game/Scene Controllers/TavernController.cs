using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TavernController : MonoBehaviour {

	public PlayerController player;
	public int[] prices, stats;

	public Button[] foodButtons;
    public Button sleepForNight;
	public Text[] buttonText;
	public Text healthText, energyText, playerBalance;
	int BASE_MEAL_COST;
	Vector3 prevPos;

	// Use this for initialization
	void Start () {
		this.player = FindObjectOfType<PlayerController>();
		this.prevPos = this.player.gameObject.transform.localPosition;
		
		// relocate player
		Vector3 newSpot = new Vector3 (-5.5f, -3.7f);
		this.player.gameObject.transform.localPosition = newSpot;
        
		BASE_MEAL_COST = 15;
		this.healthText.text = "";
		this.energyText.text = "";
		this.playerBalance.text = "";
		prices = new int[buttonText.Length];
		stats = new int[buttonText.Length];
        
        // RANDOMIZE PRICING HERE
		for (int i=0; i<buttonText.Length; i++) {
			prices [i] = BASE_MEAL_COST + (i * 5);
			buttonText [i].text = "SLEEP FOR NIGHT: $" + prices [i] + "";
		}
	}

	private void UpdateText(){
		this.healthText.text = "Health: " + this.player.remainingHealth + "/" + this.player.totalHealth;
		this.energyText.text = "Mana: " + this.player.remainingEnergy + "/" + this.player.totalEnergy;
		this.playerBalance.text = "Balance: $" + this.player.dollarBalance;
		for (int i=0; i<buttonText.Length; i++) {
			buttonText[i].text = "SLEEP FOR NIGHT: $" + prices[i] + " ";
		}
	}

	public void SleepForNight(int index){
		// if player needs health OR mana
		if (player.remainingEnergy < player.totalEnergy || player.remainingHealth < player.totalHealth) {
			// if can afford, purchase
			if (player.PurchaseItem (prices [index])) {
				// effects administered here
				// USING SIMPLE MATH HERE FOR NOW, DOLLAR PER 2 POINT OF MANA/HEALTH
				player.GiveEnergy (prices [index]*2);
				player.HealForAmount (prices [index]*2);
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
