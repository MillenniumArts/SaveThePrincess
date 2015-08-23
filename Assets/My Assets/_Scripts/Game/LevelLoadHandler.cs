using UnityEngine;
using System.Collections;

public class LevelLoadHandler : MonoBehaviour
{
    /// <summary>
    /// This class will handle level loading throughout the game
    /// </summary>

    /// <summary>
    /// Player Reference
    /// </summary>
    public PlayerController player;

    /// <summary>
    /// The Current scene
    /// </summary>
    public string currentScene;

    /// <summary>
    /// Loads the specified level
    /// </summary>
    public void LoadLevel(string level,bool deletePlayer)
    {
        this.currentScene = level;
        EscapeHandler.instance.ClearButtons();
        SceneFadeHandler.Instance.levelStarting = false;
        if (!deletePlayer)
            DontDestroyOnLoad(this.player);
        else
            Destroy(this.player);
        Resources.UnloadUnusedAssets();
        Application.LoadLevel(level);
        SceneFadeHandler.Instance.alpha = 1.0f;
        SceneFadeHandler.Instance.levelStarting = true;
        Invoke("GetButtons", 0.02f);
    }

    /// <summary>
    ///  called to return to start menu. will destroy player
    /// </summary>
    public void LoadStartGame()
    {
        SceneFadeHandler.Instance.levelStarting = true;
        EscapeHandler.instance.ClearButtons();
        Application.LoadLevel("StartMenu_LVP");
        Invoke("GetButtons", 0.02f);
    }

    public void GetButtons()
    {
        EscapeHandler.instance.GetButtons();
    }

    // Use this for initialization
    void Start()
    {
        this.player = GameObject.FindObjectOfType<PlayerController>();
        this.currentScene = "StartMenu_LVP";
    }

    // Update is called once per frame
    void Update()
    {
        if (this.player == null)
            this.player = GameObject.FindObjectOfType<PlayerController>();
    }
    #region Singleton
    private static LevelLoadHandler _instance;

    public static LevelLoadHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LevelLoadHandler>();
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
}
