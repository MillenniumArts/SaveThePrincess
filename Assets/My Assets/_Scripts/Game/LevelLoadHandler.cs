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
    /// Loads the specified level
    /// </summary>
    public void LoadLevel(string level)
    {
        DontDestroyOnLoad(this.player);
        EscapeHandler.instance.ClearButtons();
        Application.LoadLevel(level);
        EscapeHandler.instance.GetButtons();
    }

    // Use this for initialization
    void Start()
    {
        this.player = GameObject.FindObjectOfType<PlayerController>();
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
