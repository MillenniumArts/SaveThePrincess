using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationHandler : MonoBehaviour {

    public GameObject notificationPanel;
    public Button confirm;
    public Button[] buttons;
    public Text title, content;

    #region Singleton
    private static NotificationHandler _instance;

    public static NotificationHandler instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<NotificationHandler>();
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

    public void MakeNotification(string nTitle, string nContent)
    {
        // enable panel
        this.gameObject.SetActive(true);
        // set up content/title
        this.title.text = nTitle;
        this.content.text = nContent;
    }

    public void Notify()
    {
        MakeNotification("Title", "CONTENT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    public void Confirm()
    {
        // clear content
        this.title.text = null;
        this.content.text = null;
        // disable panel
        this.gameObject.SetActive(false);
    }

	// Use this for initialization
	void Start () {
        this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
