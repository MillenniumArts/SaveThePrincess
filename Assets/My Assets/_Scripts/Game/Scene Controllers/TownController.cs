using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TownController : MonoBehaviour {
	public PlayerController player;

	public Text playerBalance, 
                playerHealth,
                playerScore;

	// Use this for initialization
	void Start () {
        SceneFadeHandler.Instance.levelStarting = true;
        AudioManager.Instance.PlayNewSong("ForestOverworld");
        //EscapeHandler.instance.GetButtons();
        this.player = GameObject.FindObjectOfType<PlayerController>();
        this.fadingIn = true;

        this.inn.image.CrossFadeAlpha(0f, 0f, false);
        this.store.image.CrossFadeAlpha(0f, 0f, false);
        this.merchant.image.CrossFadeAlpha(0f, 0f, false);

        //Vector3 newPos = new Vector3(-7.5f, -2.5f);
        //this.player.gameObject.transform.localPosition = newPos;
        player.posController.MovePlayer(25, 25);

	}

	public void GoToBattle(){
        AudioManager.Instance.PlaySFX("Button1");
        DontDestroyOnLoad(this.player);
        //EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("Battle_LVP", false);
	}

	public void GoToStore(){
        AudioManager.Instance.PlaySFX("Button1");
		//DontDestroyOnLoad (this.player);
        //EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("Store_LVP", false);
	}

	public void GoToInn(){
        AudioManager.Instance.PlaySFX("Button1");
		//DontDestroyOnLoad (this.player);
        //EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("Tavern_LVP", false);
	}

    public void GoToMerchant()
    {
        AudioManager.Instance.PlaySFX("Button1");
        //DontDestroyOnLoad(this.player);
        //EscapeHandler.instance.ClearButtons();
        LevelLoadHandler.Instance.LoadLevel("Merchant_LVP", false);
    }

	// Update is called once per frame
	void Update () {
        this.playerBalance.text = this.player.dollarBalance.ToString();
        this.playerHealth.text = this.player.remainingHealth.ToString() + "/" + this.player.totalHealth.ToString();
        this.playerScore.text = PlayerPrefs.GetInt("score").ToString();

       
	}
}
