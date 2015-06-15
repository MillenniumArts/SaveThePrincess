using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EscapeHandler : MonoBehaviour {

    bool paused = false;
    public Button resume;

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

    }

    #endregion Pause Behaviour

    // Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused) {
                if (Time.timeScale == 1.0f){
                    Time.timeScale = 0;
                    Debug.Log("Pausing game");
                    DoPauseBehaviour();
                }
                else
                {
                    Time.timeScale = 1.0f;
                    Debug.Log("Unpausing game");

                }
            
            }
        }
	}
}
