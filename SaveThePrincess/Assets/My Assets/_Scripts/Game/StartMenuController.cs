using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenuController : MonoBehaviour {

	public Button startButton = null,
				  resetHiScore;
	public Text hiScoreText;

	// Use this for initialization
	void Start () {

	}

	public void StartGame(){
		Application.LoadLevel("CharacterSelect_LVP");
	}

	public void ResetHiScore(){
		PlayerPrefs.SetInt ("hiscore", 0);
	}

	private void GetScore(){
		this.hiScoreText.text = "High Score: " + PlayerPrefs.GetInt("hiscore").ToString ();
	}

	// Update is called once per frame
	void Update () {
		GetScore ();
	}
}
