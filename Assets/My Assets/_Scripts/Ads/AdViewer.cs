using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class AdViewer : MonoBehaviour {
    [SerializeField]
    private int reward = 50;
    [SerializeField]
    private float cooldownTime = 3.0f;
    private bool canWatch = true;

    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void AskForAds()
    {
        if (canWatch)
        {
            // Open dialog for asking to show an ad.
        }
        else
        {
            // Show notification to wait before watching an ad.
        }
    }

    public void StartAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public void CancelAd()
    {
        // Close ad dialog.
    }

    private void CheckIfWatched()
    {
        
        StartCoroutine("RewardPlayer");
    }

    private IEnumerator RewardPlayer()
    {
        canWatch = false;
        player.dollarBalance += reward;
        // Show notification that player has got a reward.
        yield return new WaitForSeconds(cooldownTime);
        canWatch = true;
    }

}
