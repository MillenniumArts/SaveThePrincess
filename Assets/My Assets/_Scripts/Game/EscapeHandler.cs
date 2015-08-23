using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EscapeHandler : MonoBehaviour {

    public bool paused = false;
    public GameObject pausePanel;
    public Button resume;
    public Button exit;
    public Button[] buttons;

    #region Singleton
    private static EscapeHandler _instance;

    public static EscapeHandler instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<EscapeHandler>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
        }
    }
    #endregion Singleton

    #region Pause Behaviour
    /// <summary>
    /// Do what's needed in the Pause state
    /// </summary>
    public void OnPause()
    {
        // disable buttons
        foreach (Button b in buttons)
        {
            // if the button is not part of the pause panel
            if (!b.Equals(this.resume) || !b.Equals(this.exit))
            {
                b.gameObject.SetActive(false);
            }
        }
    }

    public void OnUnPause()
    {
        // re-enable buttons
        foreach (Button b in buttons)
        {
            // if the button is not part of the pause panel
            if (!b.Equals(this.resume) || !b.Equals(this.exit))
            {
                b.gameObject.SetActive(true);
            }
        }
    }

    public void ResumeGame()
    {
        // close panel
        this.pausePanel.gameObject.SetActive(false);
        // resume timescale
        Time.timeScale = 1.0f;
        paused = false;
        OnUnPause();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game. DOES NOT EXIT IN EDITOR!!!");
        // SAVE STATS NEEDED HERE!
        if (LevelLoadHandler.Instance.currentScene != "StartMenu_LVP"
         && LevelLoadHandler.Instance.currentScene != "CharacterSelect_LVP"
         && LevelLoadHandler.Instance.currentScene != "LoadSave_LVP")
        { 
            SaveSystemHandler.instance.SaveGame();
        }

        Application.Quit();
    }
    #endregion Pause Behaviour

    public void ClearButtons()
    {
        buttons = null;
    }

    public void GetButtons()
    {
        buttons = FindObjectsOfType<Button>();
    }
    
    
    #region monobehaviour
    void Start () {
        this.pausePanel.gameObject.SetActive(paused);
        GetButtons();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused) {
                if (Time.timeScale == 1.0f){
                    Time.timeScale = 0;
                    paused = true;
                    this.pausePanel.gameObject.SetActive(true);
                    OnPause();
                }
            }
            else
            {
                Time.timeScale = 1.0f;
                paused = false;
                this.pausePanel.gameObject.SetActive(false);
                OnUnPause();
            }
        }
        
	}
}
    #endregion monobehaviour
