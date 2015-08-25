using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TavernController : MonoBehaviour
{

    public PlayerController player;
    Vector3 prevPos;
    // UI
    public Button sleepForNight;
    public Button leaveInn;
    public Button[] foodButtons;
    public Text sleepText, sleepCost;
    public Text[] buttonText;
    public Text[] healthLabelText;
    public Text[] moneyLabelText;

    // stats
    public int[] prices, stats;
    public Text healthText, playerBalance;
    // Variables
    int BASE_MEAL_COST, NUM_MEALS;
    int totalHealthMissing;

    #region button clicks
    /// <summary>
    /// called on exit tavern click
    /// </summary>
    public void LeaveInn()
    {
        AudioManager.Instance.PlaySFX("Button1");
      //  this.player.gameObject.transform.localPosition = prevPos;
        EscapeHandler.instance.ClearButtons();
        //DontDestroyOnLoad(this.player);
        //Application.LoadLevel("Town_LVP");
        LevelLoadHandler.Instance.LoadLevel("Town_LVP", false);
    }
    /// <summary>
    /// called on meal purchase button click
    /// </summary>
    /// <param name="index"></param>
    public void PurchaseMeal(int index)
    {
        AudioManager.Instance.PlaySFX("Button1");
        if (totalHealthMissing > 0)
        {
            if (player.PurchaseItem(prices[index]))
            {
                NotifyStatIncrease(false,index);
                this.player.GiveHealthAmount(stats[index]);
            }
        }
        else
        {
            Debug.Log("You already have full stats! Go fight something!");
            NotifyFullStats();
        }
    }

    /// <summary>
    /// called on sleep for night button click
    /// </summary>
    public void SleepForNight()
    {
        AudioManager.Instance.PlaySFX("Button1");
        // if player needs health OR mana
        if (totalHealthMissing > 0)
        {
            // if can afford, purchase
            if (player.PurchaseItem((totalHealthMissing/2)))
            {
                Debug.Log(this.player.name + " is refreshed after a night of sleep!");
                NotifyStatIncrease(true);
                this.player.GiveHealthAmount(totalHealthMissing);
            }
        }
        else
        {
            Debug.Log("You already have full stats! Go fight something!");
            NotifyFullStats();
        }
    }
   
#endregion button clicks

    #region Notifications
    /// <summary>
    /// called when stats are increased, boolean indicates if the player slept over night or bought a meal
    /// </summary>
    /// <param name="sleptForNight">true if player restored all stats by sleeping the night</param>
    /// <param name="index">only used if player purchased a meal</param>
    void NotifyStatIncrease(bool sleptForNight, int index = -1)
    {
        string notTitle = "";
        string notString = "";
        if (sleptForNight){
            notTitle = "Slept for Night!";
            notString = "Your remaining balance: " + this.player.dollarBalance +" \nHealth is fully restored.";
        }
        else
        {
            notTitle = "A Hearty Meal!";
            notString = "Your remaining balance: " + this.player.dollarBalance + " \nHealth is restored by " + this.stats[index] + "%!" ;
        }
        NotificationHandler.instance.MakeNotification(notTitle, notString);
    }
    /// <summary>
    /// called if player tries to increase stats when already full
    /// </summary>
    void NotifyFullStats()
    {
        string notTitle = "Full Stats!";
        string notString = "You already have full stats! \nGo fight someone!";
        NotificationHandler.instance.MakeNotification(notTitle, notString);
    }
    /// <summary>
    /// called to show welcome message to player if health is not full
    /// </summary>
    void NotifyWelcomeToTavern()
    {
        string notTitle = "Welcome to the Tavern!";
        string notString = "Click on the pictures to purchase a meal, or the stairs to sleep for the night!";
        NotificationHandler.instance.MakeNotification(notTitle, notString);
    }
   
    #endregion Notifications

    #region on updates
    /// <summary>
    /// Updates text on buttons, disables buttons if player is full health
    /// </summary>
    private void UpdateText()
    {
        this.totalHealthMissing = (this.player.totalHealth - this.player.remainingHealth);
        this.healthText.text = this.player.remainingHealth + "/" + this.player.totalHealth;
        this.playerBalance.text = this.player.dollarBalance.ToString();

        for (int i = 0; i < this.healthLabelText.Length; i++)
        {
            this.healthLabelText[i].text = this.stats[i].ToString()+"%";
            this.moneyLabelText[i].text = this.prices[i].ToString();
        }

            // disable buttons if no health needed
            if (totalHealthMissing > 0)
            {
                this.sleepText.text = totalHealthMissing.ToString();
                this.sleepCost.text = (totalHealthMissing/2).ToString();
                
                for (int i = 0; i < buttonText.Length; i++)
                {
                    foodButtons[i].gameObject.SetActive(true);
                }
                this.sleepForNight.gameObject.SetActive(true);
            }
            else
            {
                for (int i = 0; i < buttonText.Length; i++)
                {
                    foodButtons[i].gameObject.SetActive(false);
                }
                this.sleepForNight.gameObject.SetActive(false);
            }
    }

    #endregion on updates

    #region monobehaviour
    // Use this for initialization
    void Start()
    {
        SceneFadeHandler.Instance.levelStarting = true;
        AudioManager.Instance.PlayNewSong("ForestOverworld");
        EscapeHandler.instance.GetButtons();
        BASE_MEAL_COST = 5;
        NUM_MEALS = 3;
        // get player
        this.player = FindObjectOfType<PlayerController>();
        // relocate player
      //  this.prevPos = this.player.gameObject.transform.localPosition;
      //  Vector3 newSpot = new Vector3(-4.5f, -2.5f);
      //  this.player.gameObject.transform.localPosition = newSpot;
        this.player.posController.MovePlayer(23, 40);
        //init texts
        this.healthText.text = "";
        this.playerBalance.text = "";
        this.sleepText.text = "";

        prices = new int[NUM_MEALS];
        stats = new int[NUM_MEALS];
        this.leaveInn.gameObject.SetActive(true);

        //get total health missing on entry
        totalHealthMissing = (this.player.totalHealth - this.player.remainingHealth);
        if (totalHealthMissing == 0){
            NotifyFullStats();
        }
        else
        {
            NotifyWelcomeToTavern();
        }

        // RANDOMIZE MEAL PRICING HERE
        for (int i = 0; i < buttonText.Length; i++)
        {
            float tempNum = BASE_MEAL_COST + (i * 7.5f);
            prices[i] = Mathf.FloorToInt(tempNum);
            stats[i] = (int)(tempNum * 2);
        }

    }


    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }
    #endregion monobehaviour
}
