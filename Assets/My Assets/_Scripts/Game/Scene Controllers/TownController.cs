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
        AudioManager.Instance.PlaySFX("Select");
        DontDestroyOnLoad(this.player);
        AudioManager.Instance.PlayNewSong("ForestBattle");
		Application.LoadLevel ("Battle_LVP");
	}

	public void GoToStore(){
        AudioManager.Instance.PlaySFX("Select");
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Store_LVP");
	}

	public void GoToInn(){
        AudioManager.Instance.PlaySFX("Select");
		DontDestroyOnLoad (this.player);
		Application.LoadLevel ("Tavern_LVP");
	}

    public void GoToMerchant()
    {
        AudioManager.Instance.PlaySFX("Select");
        DontDestroyOnLoad(this.player);
        Application.LoadLevel("Merchant_LVP");
    }

	// Update is called once per frame
	void Update () {
		this.playerBalance.text = "Balance: $" + this.player.dollarBalance;
	}
}
