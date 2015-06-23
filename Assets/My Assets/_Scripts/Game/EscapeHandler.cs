using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EscapeHandler : MonoBehaviour {

    public bool paused = false;
    public GameObject pausePanel;
    public Button resume;
    public Button exit;

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
    void DoPauseBehaviour()
    {
        if (paused)
        {
        }
        else
        {
        }
    }

    public void ResumeGame()
    {
        // close panel
        this.pausePanel.gameObject.SetActive(false);
        // resume timescale
        Time.timeScale = 1.0f;
        paused = false;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
    }
    #endregion Pause Behaviour

    // Use this for initialization
	void Start () {
        this.pausePanel.gameObject.SetActive(false);
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
                }
            }
            else
            {
                Time.timeScale = 1.0f;
                paused = false;
                this.pausePanel.gameObject.SetActive(false);

            }
            DoPauseBehaviour();
        }
	}
}
