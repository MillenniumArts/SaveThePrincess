using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TownController : MonoBehaviour {
	public PlayerController player;

	public Text playerBalance;

	// Use this for initialization
	void Start () {
        this.player = GameObject.FindObjectOfType<PlayerController>();
	}

	public void GoToBattle(){
        DontDestroyOnLoad(this.player);
		Application.LoadLevel ("Battle_LVP");
	}

	public void GoToStore(){
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Store_LVP");
	}

	public void GoToInn(){
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Tavern_LVP");
	}

    public void GoToMerchant()
    {
        DontDestroyOnLoad(this.player);
        Application.LoadLevel("Merchant_LVP");
    }

	// Update is called once per frame
	void Update () {
		this.playerBalance.text = "Balance: $" + this.player.dollarBalance;
	}
}
