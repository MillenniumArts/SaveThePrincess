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
        GetButtons();
        foreach (Button b in buttons)
        {
            if (!b.Equals(this.confirm))
            {
                b.gameObject.SetActive(false);
            }
        }
    }

    public void Notify()
    {
        MakeNotification("Title", "CONTENT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    public void Confirm()
    {
        foreach (Button b in buttons)
        {
            if (!b.Equals(this.confirm))
            {
                b.gameObject.SetActive(true);
            }
        }
        // clear content
        this.title.text = null;
        this.content.text = null;
        // disable panel
        this.gameObject.SetActive(false);

        ClearButtons();
    }
    public void ClearButtons()
    {
        buttons = null;
    }

    public void GetButtons()
    {
        buttons = FindObjectsOfType<Button>();
    }

	// Use this for initialization
	void Start () {
        this.gameObject.SetActive(false);
        GetButtons();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
