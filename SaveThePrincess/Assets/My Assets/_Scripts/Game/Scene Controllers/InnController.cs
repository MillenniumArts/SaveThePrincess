using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InnController : MonoBehaviour {

	public PlayerController player;
	public int cost;
	public Button sleepForNight;

	// Use this for initialization
	void Start () {
		this.player = GetComponent<PlayerController> ();
		this.cost = 25;
	}

	public void SleepForNight(){
		if (player.PurchaseItem(cost)) {
			player.HealForAmount (player.totalHealth);
			player.GiveMana (player.totalMana);
		} else {
			//Debug.Log ("Not Enough money for that!");
		}

	}

	public void LeaveInn(){
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Town_LVP");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
