using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenuController : MonoBehaviour {

	public Button startButton = null;
	public GUIText hiScoreText;

	// Use this for initialization
	void Start () {
		this.hiScoreText.text = "";
		this.startButton.onClick.AddListener (()=>{
			Application.LoadLevel("CharacterSelect_LVP");
		});
		GetScore ();
	}

	void GetScore(){
		this.hiScoreText.text = "High Score: " + PlayerPrefs.GetInt("hiscore").ToString ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
