using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InnController : MonoBehaviour {

	public PlayerController player;
	public int cost;
	public Button sleepForNight;
	public Text healthText, manaText, playerBalance, sleepText;

	// Use this for initialization
	void Start () {
		this.player = FindObjectOfType<PlayerController>();
		this.cost = 25;
		this.healthText.text = "";
		this.manaText.text = "";
		this.playerBalance.text = "";
		this.sleepText.text = "";
	}

	public void SleepForNight(){
		if (player.PurchaseItem(cost)) {
			player.HealForAmount (player.totalHealth);
			player.GiveMana (player.totalMana);
		} else {
			//Debug.Log ("Not Enough money for that!");
		}
	}

	private void UpdateText(){
		this.healthText.text = "Mana: " + this.player.remainingHealth + "/" + this.player.totalHealth;
		this.manaText.text = "Health: " + this.player.remainingMana + "/" + this.player.totalMana;
		this.playerBalance.text = "Balance: $" + this.player.dollarBalance;
		this.sleepText.text = "Sleep For Night: $" + this.cost;
	}

	public void LeaveInn(){
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Town_LVP");
	}

	// Update is called once per frame
	void Update () {
		UpdateText ();
	}
}
