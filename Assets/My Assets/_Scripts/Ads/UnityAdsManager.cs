using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour {

    public bool testMode = true;
    [SerializeField]
    private string iOSGameID;
    [SerializeField]
    private string androidGameID;

    private AdRewardManager rManager;

    /*#if UNITY_EDITOR
        void Start()
        {
            // Initialize Unity Ads here.
            Advertisement.Initialize("69111", testMode);
            DontDestroyOnLoad(this.gameObject);
        }
    #endif*/

    #if UNITY_IOS
	    void Start () 
        {
            // Initialize Unity Ads here.
	        Advertisement.Initialize(iOSGameID, testMode);
            DontDestroyOnLoad(this.gameObject);
	    }
    #endif

    #if UNITY_ANDROID
	    void Start () 
        {
            // Initialize Unity Ads here.
	        Advertisement.Initialize(androidGameID, testMode);
            DontDestroyOnLoad(this.gameObject);
	    }
    #endif

    public void ShowAd(AdRewardManager rewardManager, string zoneID = "")
    {
        rManager = rewardManager;
        #if UNITY_EDITOR
            StartCoroutine(WaitForAd());
        #endif

        if (zoneID == "")
        {
            zoneID = null;
        }

        ShowOptions options = new ShowOptions();
        options.resultCallback = AdCallBackHandler;

        if (Advertisement.IsReady(zoneID))
        {
            Advertisement.Show(zoneID, options);
        }
        else if (Advertisement.IsReady())
        {
            Advertisement.Show(null, options);
        }
        else
        {
            Debug.Log("No ads are ready.");
        }
    }

    public bool CheckIfAdIsReady(string zoneID)
    {
        bool isReady = false;

        if (Advertisement.IsReady(zoneID))
        {
            isReady = true;
        }
        else if (Advertisement.IsReady())
        {
            isReady = true;
        }
        else
        {
            isReady = false;
        }

        return isReady;
    }

    private void AdCallBackHandler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                //Debug.Log("finished");
                rManager.RewardPlayer(1);
                //rManager.CancelAds();
                break;
            case ShowResult.Skipped:
                //Debug.Log("Skipped");
                rManager.RewardPlayer(2);
                //rManager.CancelAds();
                break;
            case ShowResult.Failed:
                Debug.Log("The Show Result of the Ad is 'Failed'");
                break;
        }
    }

    private IEnumerator WaitForAd()
    {
        float currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return null;

        while (Advertisement.isShowing)
        {
            yield return null;
        }

        Time.timeScale = currentTimeScale;
    }

}
