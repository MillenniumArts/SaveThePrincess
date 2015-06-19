using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TavernController : MonoBehaviour
{

    public PlayerController player;
    Vector3 prevPos;
    // UI
    public Button sleepForNight;
    public Text sleepText;
    public Button[] foodButtons;
    public Text[] buttonText;
    // stats
    public int[] prices, stats;
    public Text healthText, playerBalance;
    // Variables
    int BASE_MEAL_COST, NUM_MEALS;
    int totalHealthMissing;

    // Use this for initialization
    void Start()
    {
        BASE_MEAL_COST = 15;
        NUM_MEALS = 3;
        // get player
        this.player = FindObjectOfType<PlayerController>();
        // relocate player
        this.prevPos = this.player.gameObject.transform.localPosition;
        Vector3 newSpot = new Vector3(-4.5f, -2.5f);
        this.player.gameObject.transform.localPosition = newSpot;
        //init texts
        this.healthText.text = "";
        this.playerBalance.text = "";
        this.sleepText.text = "";

        prices = new int[NUM_MEALS];
        stats = new int[NUM_MEALS];

        //get total health missing on entry
        totalHealthMissing = (this.player.totalHealth - this.player.remainingHealth);

        // RANDOMIZE MEAL PRICING HERE
        for (int i = 0; i < buttonText.Length; i++)
        {
            prices[i] = BASE_MEAL_COST + (i * 5);
            stats[i] = BASE_MEAL_COST + (i * 5);
        }

    }

    private void UpdateText()
    {
        this.totalHealthMissing = (this.player.totalHealth - this.player.remainingHealth);
        this.healthText.text = "Health: " + this.player.remainingHealth + "/" + this.player.totalHealth;
        this.playerBalance.text = "Balance: $" + this.player.dollarBalance;

        for (int i = 0; i < buttonText.Length; i++)
        {
            buttonText[i].text = "Meal: $" + prices[i] + " ";
        }

        if (totalHealthMissing > 0)
        {
            this.sleepText.text = "Sleep: $" + totalHealthMissing;
        }
        else
        {
            this.sleepText.text = "";
            this.sleepForNight.gameObject.SetActive(false);

        }
    }

    public void PurchaseMeal(int index)
    {
        if (totalHealthMissing > 0)
        {
            if (player.PurchaseItem(prices[index]))
            {
                this.player.HealForAmount(stats[index]);
            }
        }
        else
        {
            Handheld.Vibrate();
            Debug.Log("You already have full stats! Go fight something!");
        }
    }

    public void SleepForNight()
    {
        // if player needs health OR mana
        if (totalHealthMissing > 0)
        {
            // if can afford, purchase
            if (player.PurchaseItem(totalHealthMissing))
            {
                Debug.Log(this.player.name + " is refreshed after a night of sleep!");
            }
        }
        else
        {
            Handheld.Vibrate();
            Debug.Log("You already have full stats! Go fight something!");
        }
    }


    public void LeaveInn()
    {
        this.player.gameObject.transform.localPosition = prevPos;
        DontDestroyOnLoad(this.player);
        Application.LoadLevel("Town_LVP");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }
}
