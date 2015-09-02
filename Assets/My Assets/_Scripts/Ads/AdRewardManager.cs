using UnityEngine;
using System.Collections;

public class AdRewardManager : MonoBehaviour {
    [SerializeField]
    private int reward = 100;
    [SerializeField]
    private GameObject notification;
    [SerializeField]
    private GameObject showButton;

    private PlayerController player;
    private UnityAdsManager adManager;

    void Start()
    {
        adManager = FindObjectOfType<UnityAdsManager>();
        player = FindObjectOfType<PlayerController>();
    }

    public void OpenAdDialog()
    {
        notification.SetActive(true);
        showButton.SetActive(false);
    }

    public void AcceptAds(string zoneID = "")
    {
        adManager.ShowAd(this, zoneID);
    }

    public void RewardPlayer(int typeOfReward)
    {
        switch (typeOfReward)
        {
            case 1:
                player.dollarBalance += reward;
                NotificationHandler.instance.MakeNotification("Thank You !", "You have earned " + reward + " coins!");
                break;
            case 2:
                player.dollarBalance += (reward / 4);
                NotificationHandler.instance.MakeNotification("Thank You !", "You have earned " + (reward/4) + " coins for watching part of the ad.");
                break;
            default:
                Debug.Log("Reward switch error.");
                break;
        }
    }

    public void CancelAds()
    {
        notification.SetActive(false);
        showButton.SetActive(true);
    }
}
