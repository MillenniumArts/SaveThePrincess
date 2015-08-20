using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelectController : MonoBehaviour {

    public PlayerController player;
    public int numOfChars;
    public int currentChar = 0;
    public InputField playerName;

    public int unlockCharsMinIndex;
    public int[] unlockLevels;
    public bool[] skinUnlock;

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
        if (currentChar >= unlockCharsMinIndex)
        {
            if (skinUnlock[currentChar - 2] == false)
            {
                // Darken all the sprites.
                ChangeSpriteColour(0.1f);
            }
        }
        else
        {
            // Lighten all the sprites.
            ChangeSpriteColour(1.0f);
        }
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


    public void GoBack()
    {
        LevelLoadHandler.Instance.LoadLevel("StatSelect_LVP", true);
    }
    public void CheckUnlock()
    {
        if (currentChar >= unlockCharsMinIndex)
        {
            int charUnlock = PlayerPrefs.GetInt("CharUnlock");
            for (int i = 0; i < unlockLevels.Length; i++)
            {
                if (charUnlock >= unlockLevels[i])
                {
                    skinUnlock[i] = true;
                }
            }
        }
    }

    public void ChangeSpriteColour(float colourNum)
    {
        //Debug.Log("Called Change Sprite Colour.");
        Color newColour = new Color(colourNum, colourNum, colourNum);
        //Debug.Log(newColour);
        foreach (SpriteRenderer renderer in player.gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = newColour;
        }
    }
    
    // Use this for initialization
	void Start () {
        numOfChars -= 1;
        skinUnlock = new bool[unlockLevels.Length];
        CheckUnlock();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
