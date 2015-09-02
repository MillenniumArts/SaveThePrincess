using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {
    [SerializeField]
    private int reward = 100;

    public void ShowAd(string zoneID = "")
    {
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


    private void AdCallBackHandler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                FindObjectOfType<PlayerController>().dollarBalance += reward;
                break;
            case ShowResult.Skipped:
                FindObjectOfType<PlayerController>().dollarBalance += (reward/4);
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
