using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class InitializeUnityAds : MonoBehaviour {

    public bool testMode = true;
    [SerializeField]
    private string iOSGameID;
    [SerializeField]
    private string androidGameID;

    /*#if UNITY_EDITOR
        void Start()
        {
            // Initialize Unity Ads here.
            Advertisement.Initialize("69111", testMode);
            //DontDestroyOnLoad(this.gameObject);
        }
    #endif*/

    #if UNITY_IOS
	    void Start () 
        {
            // Initialize Unity Ads here.
	        Advertisement.Initialize(iOSGameID, testMode);
            //DontDestroyOnLoad(this.gameObject);
	    }
    #endif

    #if UNITY_ANDROID
	    void Start () 
        {
            // Initialize Unity Ads here.
	        Advertisement.Initialize(androidGameID, testMode);
            //DontDestroyOnLoad(this.gameObject);
	    }
    #endif

}
