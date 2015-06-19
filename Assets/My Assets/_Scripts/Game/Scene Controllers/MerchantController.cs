using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MerchantController : MonoBehaviour {

    // player reference
    private PlayerController player;

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
    public Text remainingBalanceText;

    // buttons for increase/decrease quantity
    public Button increaseOne, 
                  decreaseOne, 
                  increaseTwo, 
                  decreaseTwo, 
                  increaseThree, 
                  decreaseThree;

    // text to display amount of each item
    public Text quantityOneText, 
                quantityTwoText, 
                quantityThreeText;

   // public int q_1, q_2, q_3;

    // total number of items purchased 
    public int numItemsPurchased;

    // purchase button
    public Button purchaseButton;
    // cancel button
    public Button cancelButton;
    // exit button
    public Button exitButton;

	void Start () {
        // get player
        this.player = FindObjectOfType<PlayerController>();

        // set up items list
        this.items = new int[numItemsForSale];
        this.prices = new int[numItemsForSale];

        // add Item counts to list and set prices
        for (int i=0; i < items.Length; i++){
            items[i] = 0;
            prices[i] = 2 * i + 1;
        }
        // get palyer balance
        this.playerBalance = this.player.dollarBalance;

        // get item balance
        for (int i = 0; i < items.Length; i++)
        {
            this.purchaseBalance = items[i] * prices[i]; 
        }

        // calculate player's remaining balance
        this.remainingBalance = this.playerBalance - this.purchaseBalance;
	}

    public void LeaveMerchant()
    {
        DontDestroyOnLoad(this.player);
        Application.LoadLevel("Town_LVP");
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
        }

        // calculate player's remaining balance
        this.remainingBalance = this.playerBalance - this.purchaseBalance;
    }

    public void IncreaseAmount(int index)
    {
        if (this.player.inventory.HasRoomInInventoryFor("food", numItemsPurchased))
        {
            items[index]+=1;
            numItemsPurchased+=1;
        }
    }

    public void DecreaseAmount(int index) {

        if (items[index] - 1 <= 0){
            items[index] = 0;
            numItemsPurchased = 0;
        }
        else
        {
            items[index]-=1;
            numItemsPurchased--;
        }
    }

    void UpdateText()
    {
        this.playerBalanceText.text = this.playerBalance.ToString();
        this.remainingBalanceText.text = this.remainingBalance.ToString();
        this.purchaseBalanceText.text = this.purchaseBalance.ToString();
        
        this.quantityOneText.text = this.items[0].ToString();
        this.quantityTwoText.text = this.items[1].ToString();
        this.quantityThreeText.text = this.items[2].ToString();


    }

	void Update () {
        UpdateText();
        CalculateShop();
	}
}
