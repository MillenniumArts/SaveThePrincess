using UnityEngine;
using System.Collections;

public class CharacterSelectController : MonoBehaviour {

    public PlayerController player;

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
    }

    /// <summary>
    /// Previous skin button call
    /// </summary>
    public void PrevSkin()
    {
        AudioManager.Instance.PlaySFX("Button1");
    }

    public void Confirm()
    {
        AudioManager.Instance.PlaySFX("Button1");
        // do something here before next load if needed
        LevelLoadHandler.Instance.LoadLevel("StatSelect_LVP");
    }
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
