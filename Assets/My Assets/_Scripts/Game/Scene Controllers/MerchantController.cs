using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MerchantController : MonoBehaviour {

    // player reference
    private PlayerController player;

    public GameObject confirmPanel;
    // items for sale 
    public int[] items;
    
    // prices of items
    public int[] prices;

    //number of items for sale
    public int numItemsForSale;

    private int purchaseBalance, 
                playerBalance, 
                remainingBalance;

    // text to display player balance
    public Text playerBalanceText;

    // text to show total amount being purchased
    public Text purchaseBalanceText;

    // text to show remaining balance after purchase
    public Text remainingBalanceText,
                playerApples,
                playerBread,
                playerCheese,
                playerHP,
                playerNRG;

    public Text[] labelText;

    // buttons for increase/decrease quantity
    public Button increaseOne, 
                  decreaseOne, 
                  increaseTwo, 
                  decreaseTwo, 
                  increaseThree, 
                  decreaseThree,
                  increaseFour,
                  decreaseFour,
                  increaseFive,
                  decreaseFive;



    // total number of items purchased 
    public int numFoodItemsPurchased, numPotionsPurchased;

    // purchase button
    public Button purchaseButton;
    // cancel button
    public Button cancelButton;
    // exit button
    public Button exitButton;

    /// <summary>
    /// on Cancel Button Click
    /// </summary>
    public void CancelPurchase()
    {
        AudioManager.Instance.PlaySFX("Button1");
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = 0;
        }
        numFoodItemsPurchased = 0;
        numPotionsPurchased = 0;
    }

    /// <summary>
    /// on Confirm Button Click
    /// </summary>
    public void ConfirmPurchase()
    {
        AudioManager.Instance.PlaySFX("Button1");
        if (this.player.PurchaseItem(purchaseBalance)){
            this.player.inventory.Apples += this.items[0];
            this.player.inventory.Bread += this.items[1];
            this.player.inventory.Cheese += this.items[2];
            this.player.inventory.HealthPotions += this.items[3];
            this.player.inventory.EnergyPotions += this.items[4];

            // clear prev. stats
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = 0;
            }
            numFoodItemsPurchased = 0;
            numPotionsPurchased = 0;
        }
    }

    /// <summary>
    ///  on exit button click
    /// </summary>
    public void LeaveMerchant()
    {
        bool unPurchased = false;
        AudioManager.Instance.PlaySFX("Button1");
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] > 0)
            {
                unPurchased = true;
                break;
            }
            else
            {
                ConfirmPanel();
            }
        }
        if (unPurchased)
        {
            this.confirmPanel.gameObject.SetActive(true);
        }
    }

    public void CancelPanel()
    {
        AudioManager.Instance.PlaySFX("Button1");
        this.confirmPanel.gameObject.SetActive(false);
    }

    public void ConfirmPanel()
    {
        AudioManager.Instance.PlaySFX("Button1");
        LevelLoadHandler.Instance.LoadLevel("Town_LVP", false);
    }

    /// <summary>
    ///  on '+' button click
    /// </summary>
    /// <param name="index"></param>
    public void IncreaseAmount(int index)
    {
        AudioManager.Instance.PlaySFX("Button1");
        if (index <= 2)
        {
            if (this.player.inventory.HasRoomInInventoryFor("food", numFoodItemsPurchased))
            {
                items[index]+=1;
                numFoodItemsPurchased+=1;
            }
        }else if (index > 2 && index <= 4){
            if (this.player.inventory.HasRoomInInventoryFor("potion", numPotionsPurchased))
            {
                items[index] += 1;
                numPotionsPurchased += 1;
            }
        }
    }

    /// <summary>
    /// on '-' button click
    /// </summary>
    /// <param name="index"></param>
    public void DecreaseAmount(int index) {
        AudioManager.Instance.PlaySFX("Button1");
        if (index <= 2)
        {
            if (items[index] - 1 <= 0)
            {
                items[index] = 0;
                numFoodItemsPurchased = 0;
            }
            else
            {
                items[index] -= 1;
                numFoodItemsPurchased--;
            }
        }
        else if (index > 2 && index <= 4)
        {
            if (items[index] - 1 <= 0)
            {
                items[index] = 0;
                numPotionsPurchased= 0;
            }
            else
            {
                items[index] -= 1;
                numPotionsPurchased--;
            }
        }
    }

    void UpdateText()
    {
        this.playerBalanceText.text = this.playerBalance.ToString();
        this.remainingBalanceText.text = this.remainingBalance.ToString();
        this.purchaseBalanceText.text = this.purchaseBalance.ToString();

        // update stock
        this.playerApples.text = (this.player.inventory.Apples + this.items[0]).ToString();
        this.playerBread.text = (this.player.inventory.Bread + this.items[1]).ToString();
        this.playerCheese.text = (this.player.inventory.Cheese + this.items[2]).ToString();
        this.playerHP.text = (this.player.inventory.HealthPotions + this.items[3]).ToString();
        this.playerNRG.text = (this.player.inventory.EnergyPotions + this.items[4]).ToString();

        for (int i = 0; i < this.labelText.Length; i++ )
        {
            this.labelText[i].text = this.prices[i].ToString();
        }
    }

    /// <summary>
    /// Calculate Shop balances to be updated on tick.
    /// </summary>
    void CalculateShop()
    {
        // get palyer balance
        this.playerBalance = this.player.dollarBalance;

        this.purchaseBalance = 0;
        // get item balance
        for (int i = 0; i < items.Length; i++)
        {
            this.purchaseBalance += items[i] * prices[i];
            //if (!this.player.PurchaseItem(prices[i]))
            //{
            //    // if can't afford item
            //}
        }

        // calculate player's remaining balance
        this.remainingBalance = this.playerBalance - this.purchaseBalance;
    }
   
    /// <summary>
   /// sets active of buttons to false when not enough money
   /// </summary>
    void UpdateButtons()
    {
        if (this.purchaseBalance > this.player.dollarBalance 
         || (this.numPotionsPurchased == 0 && this.numFoodItemsPurchased == 0) )
        {
            this.purchaseButton.gameObject.SetActive(false);
            this.cancelButton.gameObject.SetActive(false);
        }
        else
        {
            this.purchaseButton.gameObject.SetActive(true);
            this.cancelButton.gameObject.SetActive(true);
        }
    }

    void Start()
    {
        SceneFadeHandler.Instance.levelStarting = true;
        AudioManager.Instance.PlayNewSong("ForestOverworld");
        EscapeHandler.instance.GetButtons();
        NotificationHandler.instance.MakeNotification("Merchant", "Welcome to the merchant! Feel free to buy yourself some food or potions to help sustain through battles!");
        this.confirmPanel.gameObject.SetActive(false);

        // get player
        this.player = FindObjectOfType<PlayerController>();

        //Vector3 newPos = new Vector3(-12f, -2.5f);
        //this.player.gameObject.transform.localPosition = newPos;
        this.player.posController.MovePlayer(-30, -37);

        // set up items list
        this.items = new int[numItemsForSale];
        this.prices = new int[numItemsForSale];

        // food
        for (int i = 0; i < 3; i++)
        {
            items[i] = 0;
            prices[i] = 5 * i + 10;
        }
        // potions
        for (int i = 3; i < items.Length; i++)
        {
            items[i] = 0;
            prices[i] = 15;
        }
        // get player balance
        this.playerBalance = this.player.dollarBalance;

        // get item balance
        for (int i = 0; i < items.Length; i++)
        {
            this.purchaseBalance = items[i] * prices[i];
        }

        // calculate player's remaining balance
        this.remainingBalance = this.playerBalance - this.purchaseBalance;
    }
	void Update () {
        UpdateText();
        UpdateButtons();
        CalculateShop();
	}
}
