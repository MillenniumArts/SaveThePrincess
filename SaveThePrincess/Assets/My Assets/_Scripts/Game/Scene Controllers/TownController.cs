using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TownController : MonoBehaviour {
	public PlayerController player;

	public Text playerBalance;

	// Use this for initialization
	void Start () {
		this.player = GameObject.FindObjectOfType<PlayerController> ();
		if (this.player.inventory != null)
			this.player.inventory.gameObject.SetActive (false);
	}

	public void GoToBattle(){
		DontDestroyOnLoad (this.player);
		if (this.player.inventory != null)
			this.player.inventory.gameObject.SetActive (true);
		Application.LoadLevel ("Battle_LVP");

		
	}

	public void GoToStore(){
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Store_LVP");


	}

	public void GoToInn(){
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Inn_LVP");


	}

	// Update is called once per frame
	void Update () {
		this.playerBalance.text = "Balance: $" + this.player.dollarBalance;
	}
}
