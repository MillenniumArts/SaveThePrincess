using UnityEngine;
using System.Collections;

public class CharacterSelectController : MonoBehaviour {

    public PlayerController player;
    public int numOfChars;
    public int currentChar = 0;

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
        AudioManager.Instance.PlaySFX("Button1");
        // do something here before next load if needed
        LevelLoadHandler.Instance.LoadLevel("StatSelect_LVP");
    }
    
    // Use this for initialization
	void Start () {
        numOfChars -= 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
