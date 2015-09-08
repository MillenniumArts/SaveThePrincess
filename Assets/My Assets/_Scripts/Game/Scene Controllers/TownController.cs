using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TownController : MonoBehaviour {
	public PlayerController player;

	public Text playerBalance, 
                playerHealth,
                playerScore;
    public Text apples, bread, cheese, nrgPot, hPot, score;

	// Use this for initialization
	void Start () {
        SceneFadeHandler.Instance.levelStarting = true;
        AudioManager.Instance.PlayNewSong("ForestOverworld");
        //EscapeHandler.instance.GetButtons();
        this.player = GameObject.FindObjectOfType<PlayerController>();

        //Vector3 newPos = new Vector3(-7.5f, -2.5f);
        //this.player.gameObject.transform.localPosition = newPos;
        this.player.posController.MovePlayer(25, 25);
        
        //animate if player won battles
        if (PlayerPrefs.GetInt("retreated") == 0 && PlayerPrefs.GetInt("score") > 0)
        {
            this.player.TriggerAnimation("victory");
            // only trigger once
            PlayerPrefs.SetInt("retreated", 1); 
        }
        
        this.apples.text = this.player.inventory.Apples.ToString() + "/3";
        this.bread.text = this.player.inventory.Bread.ToString() + "/3";
        this.cheese.text = this.player.inventory.Cheese.ToString() + "/3";

        this.nrgPot.text = this.player.inventory.EnergyPotions.ToString() + "/2";
        this.hPot.text = this.player.inventory.HealthPotions.ToString() + "/2";
        this.score.text = PlayerPrefs.GetInt("score").ToString();
	}

	public void GoToBattle(){
        AudioManager.Instance.PlaySFX("SelectLarge");
        DontDestroyOnLoad(this.player);
        //EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("Battle_LVP", false);
	}

	public void GoToStore(){
        AudioManager.Instance.PlaySFX("SelectSmall");
		//DontDestroyOnLoad (this.player);
        //EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("Store_LVP", false);
	}

	public void GoToInn(){
        AudioManager.Instance.PlaySFX("SelectSmall");
		//DontDestroyOnLoad (this.player);
        //EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("Tavern_LVP", false);
	}

    public void GoToMerchant()
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        //DontDestroyOnLoad(this.player);
        //EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("Merchant_LVP", false);
    }

	// Update is called once per frame
	void Update () {
        this.playerBalance.text = this.player.dollarBalance.ToString();
        this.playerHealth.text = this.player.remainingHealth.ToString() + " / " + this.player.totalHealth.ToString();
        this.playerScore.text = PlayerPrefs.GetInt("score").ToString();
	}
}
