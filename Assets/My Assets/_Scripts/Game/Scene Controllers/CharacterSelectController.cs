using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelectController : MonoBehaviour {

    public PlayerController player;
    public int numOfChars;
    public int currentChar = 0;
    public InputField playerName;

    void Awake()
    {
        this.player = FindObjectOfType<PlayerController>();
    }

    /// <summary>
    ///  Next Skin Button Call
    /// </summary>
    public void NextSkin()
    {
        AudioManager.Instance.PlaySFX("Button1");
        if (currentChar < numOfChars)
        {
            currentChar++;
        }
        else if (currentChar >= numOfChars)
        {
            currentChar = 0;
        }
        player.GetComponentInChildren<CreateCombination>().NewSpriteSheet(currentChar);
    }

    public void GoBack()
    {
        LevelLoadHandler.Instance.LoadLevel("LoadSave_LVP", true);
    }

    /// <summary>
    /// Previous skin button call
    /// </summary>
    public void PrevSkin()
    {
        AudioManager.Instance.PlaySFX("Button1");
        if (currentChar > 0)
        {
            currentChar--;
        }
        else if (currentChar <= 0)
        {
            currentChar = numOfChars;
        }
        player.GetComponentInChildren<CreateCombination>().NewSpriteSheet(currentChar);
    }

    public void Confirm()
    {
        if (playerName.text != "")
        {
            AudioManager.Instance.PlaySFX("Button1");
            this.player.playerName= this.playerName.text;
            // do something here before next load if needed
            LevelLoadHandler.Instance.LoadLevel("StatSelect_LVP", false);
        }
        else {
            NotificationHandler.instance.MakeNotification("Error!", "You need to enter a player name!");
        }
    }
    
    // Use this for initialization
	void Start () {
        numOfChars -= 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
