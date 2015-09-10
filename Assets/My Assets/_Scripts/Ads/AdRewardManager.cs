using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdRewardManager : MonoBehaviour {
    [SerializeField]
    private int reward = 100;
    [SerializeField]
    private GameObject rewardPanel;
    [SerializeField]
    private Text rewardMessageText;
    [SerializeField]
    private GameObject notification;
    [SerializeField]
    private GameObject showButton;
    [SerializeField]
    private GameObject UICanvas;

    private PlayerController player;
    private UnityAdsManager adManager;

    [SerializeField]
    public string zoneId = "";

    private bool displayButton = true;

    void Start()
    {
        adManager = FindObjectOfType<UnityAdsManager>();
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (adManager != null)
        {
            if (adManager.CheckIfAdIsReady(zoneId) == true)
            {
                if(displayButton)
                    showButton.SetActive(true);
            }
            else
            {
                showButton.SetActive(false);
            }
        }
    }

    public void OpenAdDialog()
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        notification.SetActive(true);
        showButton.SetActive(false);
        displayButton = false;
        UICanvas.SetActive(false);
    }

    public void AcceptAds(string zoneID = "")
    {
        AudioManager.Instance.PlaySFX("SelectSmall");
        adManager.ShowAd(this, zoneID);
    }

    public void RewardPlayer(int typeOfReward)
    {
        switch (typeOfReward)
        {
            case 1:
                player.dollarBalance += reward;
                notification.SetActive(false);
                showButton.SetActive(false);
                displayButton = false;
                rewardMessageText.text = "You have earned " + reward + " coins!";
                rewardPanel.SetActive(true);
                break;
            case 2:
                player.dollarBalance += (reward / 4);
                notification.SetActive(false);
                showButton.SetActive(false);
                displayButton = false;
                rewardMessageText.text = "You have earned " + (reward/4) + " coins for watching part of the ad.";
                rewardPanel.SetActive(true);
                break;
            default:
                Debug.Log("Reward switch error.");
                CancelAds();
                break;
        }
    }

    public void CancelAds()
    {
        AudioManager.Instance.PlaySFX("Return");
        notification.SetActive(false);
        rewardPanel.SetActive(false);
        displayButton = true;
        UICanvas.SetActive(true);
    }
}
